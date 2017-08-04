using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Projeto.WEB.Models;
using Projeto.WEB.Models.Home;
using Projeto.Entidades;
using Projeto.DAL.Persistencias;
using System.Web.Security;

namespace Projeto.WEB.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(HomeViewModelLogin model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var d = new UsuarioDAL();
                    Usuario u = d.ObterPorLoginSenha(model.Login, model.Senha);

                    if (u != null)
                    {
                        var ticket = new FormsAuthenticationTicket(u.Nome,false,60);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                        Response.Cookies.Add(cookie);

                        Session.Add("usuario", u);

                        return Json("/AreaRestrita/CEP/Index/", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ViewBag.Mensagem = "Acesso negado, usuário ou senha incorretos";
                    }


                }
                catch (Exception e)
                {
                    ViewBag.Mensagem = $"Erro: {e.Message}";
                }
            }


            return Json("Dados inválidos");
        }

        public ActionResult Cadastro()
        {           
            return View();
        }

        [HttpPost]
        public JsonResult Cadastro(HomeViewModelCadastro model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var u = new Usuario();
                    u.Nome = model.Nome;
                    u.Login = model.Login;
                    u.Senha = model.Senha;
                    u.DataCadastro = DateTime.Now;
                    u.Status = true;

                    var d = new UsuarioDAL();
                    d.Inserir(u);

                    ModelState.Clear();
                    return Json($"Usuario {u.Login}, cadastrado com sucesso.");

                }
                catch (Exception e)
                {
                    return Json(e.Message);
                }   
            }
            return Json("Dados inválidos");
        }
    }
}