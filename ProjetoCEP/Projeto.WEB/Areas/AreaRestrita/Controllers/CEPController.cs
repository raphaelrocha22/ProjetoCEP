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
                        l.NumeroLote = model.Lote;
                        l.DataCriacao = model.DataCriacao;
                        l.IdOperadorCriacao = model.IdOperadorCriacao;
                        l.DataAnalise = model.DataAnalise;
                        l.IdOperadorAnalise = model.IdOperadorAnalise;
                        l.TotalLentes = model.TotaLentes;
                        l.QtdNaoConforme = model.QtdNaoConforme;
                        l.Percentual = (model.QtdNaoConforme / model.TotaLentes);

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

        public ActionResult HistoricoLimites()
        {
            return View();
        }
    }
}
