using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Projeto.WEB.Areas.AreaRestrita.Models.Operador;
using Projeto.DAL.Persistencias;
using Projeto.Entidades;
using Projeto.Util;

namespace Projeto.WEB.Areas.AreaRestrita.Controllers
{
    public class OperadorController : Controller
    {
        // GET: AreaRestrita/Operador
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(OperadorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var d = new OperadorDAL();

                    if (!d.ObterOperador(model.Nome))
                    {
                        var o = new Operador();
                        o.Nome = model.Nome;
                        o.DataCadastro = DateTime.Now;
                        o.Status = true;

                        d.Cadastrar(o);

                        ModelState.Clear();
                        ViewBag.Resultado = true;
                        ViewBag.Mensagem = "Operador Cadastrado com sucesso";
                    }
                    else
                    {
                        ViewBag.Resultado = false;
                        ViewBag.Mensagem = "Já existe um operador cadastrado com esse nome";
                    }

                }
                catch (Exception e)
                {
                    ViewBag.Resultado = false;
                    ViewBag.Mensagem = "Erro não esperado. Tente novamente, se o erro persistir entre em contato com o administrador do sistema";
                    Logger.LogErro(HttpContext.Server.MapPath("/bin/Logs/"), e);
                }
            }

            return View();
        }

        public ActionResult Consultar()
        {
            var lista = new List<OperadorViewModel>();

            try
            {
                var d = new OperadorDAL();

                foreach (var item in d.ListaOperador())
                {
                    var model = new OperadorViewModel();
                    model.IdOperador = item.IdOperador;
                    model.Nome = item.Nome;
                    model.DataCadastro = item.DataCadastro;
                    
                    lista.Add(model);
                }
            }
            catch (Exception e)
            {
                ViewBag.Mensagem = "Erro não esperado, por favor entre em contato com o administrador do sistema";
                Logger.LogErro(HttpContext.Server.MapPath("/bin/Logs/"), e);
            }

            return View(lista);
        }

        public ActionResult Excluir(int id)
        {
            try
            {
                var d = new OperadorDAL();
                Operador o = d.ObterPorId(id);

                var model = new OperadorViewModel();
                model.IdOperador = o.IdOperador;
                model.Nome = o.Nome;
                model.DataCadastro = o.DataCadastro;

                return View(model);
            }
            catch (Exception e)
            {
                ViewBag.Mensagem = "Erro não esperado, por favor entre em contato com o administrador do sistema";
                Logger.LogErro(HttpContext.Server.MapPath("/bin/Logs/"), e);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Excluir(OperadorViewModel model)
        {
            try
            {
                var d = new OperadorDAL();
                d.Excluir(model.IdOperador);

                TempData["MensagemExclusao"] = $"Operador {model.Nome}, excluido com sucesso.";

                return RedirectToAction("Consultar", "Operador", new { area = "AreaRestrita" });

            }
            catch (Exception e)
            {
                ViewBag.Mensagem = "Erro não esperado, por favor entre em contato com o administrador do sistema";
                Logger.LogErro(HttpContext.Server.MapPath("/bin/Logs/"), e);
            }
            return View();
        }
    }
}