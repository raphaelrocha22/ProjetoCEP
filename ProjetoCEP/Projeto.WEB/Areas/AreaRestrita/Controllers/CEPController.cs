using Projeto.Entidades;
using Projeto.Util;
using Projeto.WEB.Areas.AreaRestrita.Models.CEP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


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
                }
                catch (Exception e)
                {
                    ViewBag.Mensagem = "Erro não esperado. Tente novamente, se o erro persistir entre em contato com o administrador do sistema";
                    Logger.LogErro(HttpContext.Server.MapPath("/bin/Logs/"), e);
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
