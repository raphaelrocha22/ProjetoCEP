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
    }
}
