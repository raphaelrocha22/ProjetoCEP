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
        public ActionResult Index(HomeViewModelLogin model)
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

                        return RedirectToAction("Index", "CEP", new {area = "AreaRestrita" });
                    }
                    else
                    {
                        throw new Exception("Acesso negado, usuário ou senha incorretos");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Mensagem = e.Message;
                }
            }
            return View();
        }

        public ActionResult Cadastro()
        {           
            return PartialView();
        }

        [HttpPost]
        public ActionResult Cadastro(HomeViewModelCadastro model)
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
                    ViewBag.Mensagem = $"Usuario {u.Login}, cadastrado com sucesso.";
                }
                catch (Exception e)
                {
                    ViewBag.Mensagem = e.Message;
                }   
            }
            return PartialView("Cadastro");
        }
    }
}