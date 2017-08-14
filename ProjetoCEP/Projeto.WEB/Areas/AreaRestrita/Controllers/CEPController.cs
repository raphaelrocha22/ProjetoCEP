using Projeto.DAL.Persistencias;
using Projeto.Entidades;
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
                if (model.DataAnalise >= model.DataCriacao && model.TotaLentes >= model.QtdNaoConforme)
                {
                    try
                    {
                        var l = new Lote();
                        l.OperadorCriacao = new Operador();
                        l.OperadorAnalise = new Operador();

                        l.NumeroLote = model.Lote;
                        l.DataCriacao = model.DataCriacao;
                        l.OperadorCriacao.IdOperador = model.IdOperadorCriacao;
                        l.DataAnalise = model.DataAnalise;
                        l.OperadorAnalise.IdOperador = model.IdOperadorAnalise;
                        l.TotalLentes = Convert.ToInt32(model.TotaLentes);
                        l.QtdNaoConforme = Convert.ToInt32(model.QtdNaoConforme);
                        l.Percentual = model.QtdNaoConforme/model.TotaLentes;
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
                            TempData["MensagemErro"] = "Um lote com essa numeração já foi inserido no sistema";
                        }
                        else
                        {
                            TempData["MensagemErro"] = "Erro não esperado. Tente novamente, se o erro persistir entre em contato com o administrador do sistema";
                            Logger.LogErro(HttpContext.Server.MapPath("/bin/Logs/"), e);
                        }
                    }
                }
                else
                {
                    TempData["MensagemErro"] = "Por favor, verifique se a data de análise não está inferior a data de criação " +
                                                "ou se a quantidade não conforme não está maior que o total de lentes";

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
                l.DataCriacao = item.DataCriacao;
                l.OperadorCriacaoNome = item.OperadorCriacao.Nome;
                l.DataAnalise = item.DataAnalise;
                l.OperadorAnaliseNome = item.OperadorAnalise.Nome;
                l.TotaLentes = item.TotalLentes;
                l.QtdNaoConforme = item.QtdNaoConforme;
                l.Percentual = item.Percentual;
                
                lista.Add(l);
            }
            
            return PartialView("_ConsultarAmostras",lista);
        }

        public JsonResult ObterPorId(int id)
        {
            try
            {
                var d = new CepDAL();
                Lote l = d.ObterPorId(id);

                var model = new CalcularLimitesViewModel();
                model.idLote = l.IdLote;
                model.Lote = l.NumeroLote;
                model.DataCriacao = l.DataCriacao;
                model.OperadorCriacaoNome = l.OperadorCriacao.Nome;
                model.DataAnalise = l.DataAnalise;
                model.OperadorAnaliseNome = l.OperadorAnalise.Nome;
                model.TotaLentes = l.TotalLentes;
                model.QtdNaoConforme = l.QtdNaoConforme;
                model.Percentual = l.Percentual;
                model.Observacao = l.Observacao;

                return Json(model);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
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











        public ActionResult HistoricoLimites()
        {
            return View();
        }
    }
}
