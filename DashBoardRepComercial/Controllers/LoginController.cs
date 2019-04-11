using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardRepComercial.Models;

namespace DashBoardRepComercial.Controllers
{
    public class LoginController : Controller
    {
        JUNDSOFTEntities jund = new JUNDSOFTEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(T_SY_42 usuario)
        {
            jund.Database.Connection.Open();
            var consultaUsuario = jund.T_SY_42.Where(u => u.SY42_USUARIO == usuario.SY42_USUARIO && u.SY42_SENHA == usuario.SY42_SENHA).FirstOrDefault();
            
            if (consultaUsuario != null)
            {
                var consultaGrupo = jund.T_SY_43.Where(g => g.SY42_USUARIO == consultaUsuario.SY42_USUARIO).FirstOrDefault();
                if (consultaGrupo.SY40_GRUPO == "GER COMERC")
                {
                    Session.Add("usuario", consultaUsuario.SY42_USUARIO);
                    Session.Add("grupo", "Administrador");
                    return Redirect("/Dashboard/Index");
                }
                else if (consultaGrupo.SY40_GRUPO == consultaGrupo.SY42_USUARIO)
                {
                    var consultaGerReg = jund.T_SY_40.Where(gReg => gReg.CD34_GERENTE != null && gReg.SY01_EMPRESA == "11" 
                        && consultaGrupo.SY40_GRUPO == gReg.SY40_GRUPO).FirstOrDefault();
                    Session.Add("usuario", consultaUsuario.SY42_USUARIO);
                    if(consultaGerReg != null)
                    {
                        Session.Add("grupo", "Gerente Regional");
                        Session.Add("gerRegional", consultaGerReg.CD34_GERENTE);
                    }
                    else
                    {
                        Session.Add("grupo", "Representante");
                    }
                    return Redirect("/Dashboard/Index");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Desculpe,este usuário não possui acesso a este sistema.Por favor, tente novamente com outro usuário.');window.location = '/Home/Index';</script>");
                }
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Nome de usuário ou senha incorretos / inválidos.Tente novamente.');window.location = '/Home/Index';</script>");
            }
        }

        public ActionResult FinalizarSessao()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}