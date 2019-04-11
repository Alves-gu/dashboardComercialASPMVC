using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardRepComercial.Models;

namespace DashBoardRepComercial.Controllers
{
    public class AvisoController : Controller
    {
        dashboardEntities ctx = new dashboardEntities();
        // GET: Aviso
        public ActionResult Create()
        {
            var usuarioLogado = HttpContext.Session["usuario"];
            var grupoUsuario = HttpContext.Session["grupo"];

            if (usuarioLogado == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Você precisa estar logado para visualizar essa página. Clique em OK para ser redirecionado para a Área de login.');window.location = '/Home/Index';</script>");
            }
            else
            {
                if ((string)grupoUsuario == "Representante")
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Acesso permitido somente aos administradores do sistema.');window.location = '/Dashboard/Index';</script>");
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult CadastrarAviso()
        {
            return View("~/Views/Aviso/Create.cshtml");
        }

        [HttpPost]
        public ActionResult CadastrarAviso(avisos aviso)
        {
            try
            {
                //Instância do objeto/entidade aviso
                avisos clsAvi = new avisos()
                {
                    NomeAviso = aviso.NomeAviso,
                    MsgAviso = aviso.MsgAviso,
                    DataAviso = DateTime.Now
                };
                //Gravação no BD
                ctx.avisos.Add(clsAvi);
                ctx.SaveChanges();
                ViewBag.Message = "Aviso enviado com sucesso.";
                return View("~/Views/Aviso/Create.cshtml");
            }
            catch
            {
                ViewBag.Message = "Erro ao enviar aviso.";
                return View("~/Views/Aviso/Create.cshtml");
            }     
        }
        [HttpGet]
        public ActionResult Show(int IdAviso)
        {
            var usuarioLogado = HttpContext.Session["usuario"];
            if (usuarioLogado == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Você precisa estar logado para visualizar essa página. Clique em OK para ser redirecionado para a Área de login.');window.location = '/Home/Index';</script>");
            }
            else
            {
                var avi = ctx.avisos.Where(i => i.IdAviso == IdAviso).FirstOrDefault();
                return View(avi);
            }
        }
    }
}