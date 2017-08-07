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
using Projeto.Util;
using Projeto.WEB.Areas.AreaRestrita.Models.Operador;

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
                        ViewBag.Mensagem = "Acesso negado, usuário ou senha incorretos";
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Mensagem = "Erro não esperado, por favor entre em contato com o administrador do sistema";
                    Logger.LogErro(HttpContext.Server.MapPath("/bin/Logs/"), e);
                }
            }
            return View();
        }

        public ActionResult Cadastro()
        {
            return View();
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
                    ViewBag.Resultado = true;
                    ViewBag.Mensagem = $"Usuario {u.Login}, cadastrado com sucesso.";

                }
                catch (Exception e)
                {
                    ViewBag.resultado = false;
                    if (e.HResult == -2146232060)
                    {
                        ViewBag.Mensagem = "Este usuário já existe, por favor escolha outro";
                    }
                    else
                    {
                        ViewBag.Mensagem = "Erro não esperado, por favor entre em contato com o administrador do sistema";
                        Logger.LogErro(HttpContext.Server.MapPath("/bin/Logs/"), e);
                    }                
                }   
            }
            return View();
        }        
    }
}