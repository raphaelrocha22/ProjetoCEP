using Projeto.DAL.Persistencias;
using Projeto.Entidades;
using Projeto.Entidades.Tipos;
using Projeto.Util;
using Projeto.WEB.Areas.AreaRestrita.Models.CEP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Projeto.WEB.Areas.AreaRestrita.Controllers
{
    [Authorize]
    public class CEPController : Controller
    {
        // GET: AreaRestrita/CEP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CalcularLimites()
        {
            return View(new CalcularLimitesViewModel());
        }

        [HttpPost]
        public ActionResult CalcularLimites(CalcularLimitesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.TotaLentes >= model.QtdNaoConforme)
                {
                    try
                    {
                        var l = new Lote();
                        l.OperadorAnalise = new Operador();

                        l.NumeroLote = model.Lote;
                        l.DataAnalise = model.DataAnalise;
                        l.OperadorAnalise.IdOperador = model.IdOperadorAnalise;
                        l.TotalLentes = Convert.ToInt32(model.TotaLentes);
                        l.QtdNaoConforme = Convert.ToInt32(model.QtdNaoConforme);
                        l.Percentual = model.QtdNaoConforme/model.TotaLentes;
                        if (model.Observacao == null)
                            model.Observacao = string.Empty;
                        l.Observacao = model.Observacao;
                        
                        var d = new CepDAL();
                        d.CadastrarAmostras(l);

                        ModelState.Clear();
                        TempData["MensagemCadastro"] = $"Lote {model.Lote}, incluído com sucesso.";
                    }
                    catch (Exception e)
                    {
                        if (e.HResult == -2146232060)
                        {
                            //TempData["MensagemErro"] = e.Message;
                            TempData["MensagemErro"] = "Um lote com essa numeração já foi inserido no sistema";
                        }
                        else
                        {
                            //TempData["MensagemErro"] = e.Message;
                            TempData["MensagemErro"] = "Erro não esperado. Tente novamente, se o erro persistir entre em contato com o administrador do sistema";
                            Logger.LogErro(HttpContext.Server.MapPath("/bin/Logs/"), e);
                        }
                    }
                }
                else
                {
                    TempData["MensagemErro"] = "Por favor, verifique se a quantidade não conforme não está maior que o total de lentes";
                }
            }

            return View(new CalcularLimitesViewModel());
        }

        public ActionResult Consultar()
        {
            var lista = new List<CalcularLimitesViewModel>();
            var d = new CepDAL();

            foreach (var item in d.ObterAmostras())
            {
                var l = new CalcularLimitesViewModel();

                l.idLote = item.IdLote;
                l.Lote = item.NumeroLote;
                l.DataAnalise = item.DataAnalise;
                l.OperadorAnaliseNome = item.OperadorAnalise.Nome;
                l.TotaLentes = item.TotalLentes;
                l.QtdNaoConforme = item.QtdNaoConforme;
                l.Percentual = item.Percentual;
                l.Observacao = item.Observacao;

                lista.Add(l);
            }
            return PartialView("_ConsultarAmostras", lista);
        }

        
        public JsonResult Excluir(int id)
        {
            try
            {
                var d = new CepDAL();
                d.Excluir(id);

                return Json($"Amostra excluida com sucesso.");
            }
            catch (Exception e)
            {

                return Json(e.Message);
            }
        }


        public ActionResult ResultadoCalculoLimites()
        {
            var listaModel = new List<CalcularLimitesViewModel>();

            try
            {
                var d = new CepDAL();
                List<Lote> lista = d.ObterAmostras();
                LimitesControle limites = d.CalcularLimites(lista);

                foreach (var item in lista)
                {
                    var l = new CalcularLimitesViewModel();

                    l.idLote = item.IdLote;
                    l.Lote = item.NumeroLote;
                    l.DataAnalise = item.DataAnalise;
                    l.OperadorAnaliseNome = item.OperadorAnalise.Nome;
                    l.TotaLentes = item.TotalLentes;
                    l.QtdNaoConforme = item.QtdNaoConforme;
                    l.Percentual = item.Percentual;
                    l.LSC = limites.LSC;
                    l.LC = limites.LC;
                    l.LIC = limites.LIC;
                    l.Observacao = item.Observacao;

                    if (l.Percentual > limites.LSC)
                    {
                        l.Status = "Reprovado";
                    }
                    else
                    {
                        l.Status = "Aprovado";
                    }

                    listaModel.Add(l);
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return View(listaModel);
        }

        [HttpPost]
        public JsonResult GraficoCalculoLimitesData()
        {
            var d = new CepDAL();
            List<Lote> lista = d.ObterAmostras();
            LimitesControle limites = d.CalcularLimites(lista);

            var list = new List<CalcularLimitesViewModel>();

            foreach (var item in lista)
            {
                var l = new CalcularLimitesViewModel();
                l.DataAnaliseGrafico = item.DataAnalise.ToString("dd/MM/yyyy hh:mm");
                l.LSC = limites.LSC;
                l.LC = limites.LC;
                l.LIC = limites.LIC;
                l.Percentual = item.Percentual;

                list.Add(l);
            }

            return Json(list);
        }

        public ActionResult HistoricoLimites()
        {
            return View();
        }

    }
}
