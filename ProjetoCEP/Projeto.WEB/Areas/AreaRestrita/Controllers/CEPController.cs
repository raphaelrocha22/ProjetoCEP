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
            return View(new CadastrarLoteViewModel());
        }
        
        [HttpPost]
        public ActionResult Index(CadastrarLoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.TotaLentes >= model.QtdNaoConforme)
                {
                    try
                    {
                        var d = new CepDAL();
                        LimitesControle limite = d.ObterLimiteAtivo();

                        var l = new Lote();
                        l.OperadorAnalise = new Operador();
                        l.Limites = new LimitesControle();

                        l.NumeroLote = model.Lote;
                        l.DataAnalise = model.DataAnalise;
                        l.OperadorAnalise.IdOperador = model.IdOperadorAnalise;
                        l.TotalLentes = Convert.ToInt32(model.TotaLentes);
                        l.QtdNaoConforme = Convert.ToInt32(model.QtdNaoConforme);
                        l.Percentual = model.QtdNaoConforme / model.TotaLentes;
                        if (model.Observacao == null)
                            model.Observacao = string.Empty;
                        l.Observacao = model.Observacao;
                        if (l.Percentual <= limite.LSC)
                        {
                            l.Status = "Aprovado";
                        }
                        else
                        {
                            l.Status = "Reprovado";
                        }
                        l.TipoLote = "Producao";
                        l.Limites.IdLimites = limite.IdLimites;
                        
                        d.CadastrarLote(l);

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

            return View(new CadastrarLoteViewModel());
        }

        public JsonResult LimiteAtivo()
        {
            var d = new CepDAL();
            LimitesControle l = d.ObterLimiteAtivo();

            return Json(l);
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
                        l.Percentual = model.QtdNaoConforme / model.TotaLentes;
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
            return View();
        }

        public JsonResult ResultadoCalculoLimitesData()
        {
            try
            {
                var d = new CepDAL();
                LimitesControle limites = d.CalcularLimites(d.ObterAmostras());

                var lista = new List<CalcularLimitesViewModel>();

                foreach (Lote item in d.ObterAmostras())
                {
                    var l = new CalcularLimitesViewModel();
                    l.idLote = item.IdLote;
                    l.Lote = item.NumeroLote;
                    l.DataAnaliseGrafico = item.DataAnalise.ToString("dd/MM HH:mm");
                    l.OperadorAnaliseNome = item.OperadorAnalise.Nome;
                    l.TotaLentes = item.TotalLentes;
                    l.QtdNaoConforme = item.QtdNaoConforme;
                    l.LSC = limites.LSC;
                    l.LC = limites.LC;
                    l.LIC = limites.LIC;
                    l.Percentual = item.Percentual;
                    l.DataCalculo = DateTime.Now.ToString();
                    if (l.Percentual > l.LSC)
                    {
                        l.Status = "Reprovado";
                    }
                    else
                    {
                        l.Status = "Aprovado";
                    }

                    l.Observacao = item.Observacao;

                    lista.Add(l);
                }

                return Json(lista);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }


        public JsonResult CadastrarLimites(CadastrarLimitesViewModel model)
        {
            try
            {
                var l = new LimitesControle();
                l.TipoCarta = new TipoCarta();

                l.DataCalculo = model.DataCalculo;
                l.LSC = model.LSC;
                l.LC = model.LC;
                l.LIC = model.LIC;
                l.Status = true;
                l.TipoCarta.IdTipoCarta = 2;

                var d = new CepDAL();
                d.CadastrarLimites(l);

                var lista = new List<Lote>();
                lista = d.ObterAmostras();

                d.TransferirAmostrasParaLote(lista, d.ObterLimiteAtivo());
                d.Excluir(lista);

                return Json("Limite Cadastrado com sucesso");

            }
            catch (Exception e)
            {
                return Json($"Erro: {e.Message}");
            }
        }

        public ActionResult HistoricoLimites()
        {
            try
            {
                var d = new CepDAL();
                var model = new List<ConsultarLimitesViewModel>();

                foreach (var item in d.ObterLimites())
                {
                    var l = new ConsultarLimitesViewModel();
                    
                    l.IdLimites = item.IdLimites;
                    l.DataCalculo = item.DataCalculo;
                    l.LSC = item.LSC;
                    l.LC = item.LC;
                    l.LIC = item.LIC;
                    l.Modelo = item.TipoCarta.Modelo;
                    l.Carta = item.TipoCarta.Carta;
                    if (item.Status)
                    {
                        l.Status = "Ativo";
                    }
                    else
                    {
                        l.Status = "Inativo";
                    }

                    model.Add(l);
                }

                return View(model);
            }
            catch (Exception e)
            {
                return View(new ConsultarLimitesViewModel());
            }
                        
        }

        public JsonResult AtivarLimite(int id)
        {
            try
            {
                var d = new CepDAL();
                d.Ativarlimite(id);

                return Json("Limite padrão alterado com sucesso");
            }
            catch (Exception e)
            {

                return Json("Erro: e");
            }
        }

        public JsonResult ResultadoLotes(ConsultarLotesViewModel model)
        {            
            try
            {
                var d = new CepDAL();
                var lista = new List<ConsultarLotesViewModel>();

                foreach (var item in d.ObterLotesProducao(model.DataInicio, model.DataFim))
                {
                    var l = new ConsultarLotesViewModel();

                    l.IdLote = item.IdLote;
                    l.Lote = item.NumeroLote;
                    l.DataAnalise = item.DataAnalise.ToString("dd/MM HH:mm");
                    l.OperadorAnaliseNome = item.OperadorAnalise.Nome;
                    l.TotalLentes = item.TotalLentes;
                    l.QtdNaoConforme = item.QtdNaoConforme;
                    l.Percentual = item.Percentual;
                    l.Status = item.Status;
                    l.LSC = item.Limites.LSC;
                    l.LC = item.Limites.LC;
                    l.LIC = item.Limites.LIC;
                    l.Observacao = item.Observacao;

                    lista.Add(l);
                }

                return Json(lista);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

    }
}
