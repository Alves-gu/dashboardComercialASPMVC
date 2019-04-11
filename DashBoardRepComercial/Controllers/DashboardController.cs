using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardRepComercial.Models;
using System.Diagnostics;
using DashBoardRepComercial.Controllers;

namespace DashBoardRepComercial.Controllers
{
    public class DashboardController : Controller
    {
        dashboardEntities dash = new dashboardEntities();
        // GET: Dashboard
        public ActionResult Index()
        {
            var usuarioLogado = HttpContext.Session["usuario"];
            if (usuarioLogado == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Você precisa estar logado para visualizar essa página. Clique em OK para ser redirecionado para a Área de login.');window.location = '/Home/Index';</script>");
            }
            else
            {
                ViewData["camps"] = dash.campanhas.OrderByDescending(c => c.DataCampanha).Take(5).ToList();
                ViewData["avi"] = dash.avisos.OrderByDescending(a=> a.DataAviso).Take(5).ToList();
                if((string)HttpContext.Session["grupo"] == "Administrador")
                {

                }
                else if ((string)HttpContext.Session["grupo"] == "Representante")
                {
                    var vend = JundDataController.GetVendedor((string)usuarioLogado);

                    ViewBag.Produtos = JundDataController.GetProdutos(vend.CD25_VENDEDOR);
                    ViewBag.Faturamentos = JundDataController.GetFaturamentos(vend.CD25_VENDEDOR);
                }
                else if((string)HttpContext.Session["grupo"] == "Gerente Regional")
                {
                    ViewBag.Faturamentos = JundDataController.GetFaturamentosGerRegional((string)HttpContext.Session["gerRegional"]);
                }
                
                return View();
            }
        }

        public ActionResult Campanhas()
        {
            var usuarioLogado = HttpContext.Session["usuario"];
            if (usuarioLogado == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Você precisa estar logado para visualizar essa página. Clique em OK para ser redirecionado para a Área de login.');window.location = '/Home/Index';</script>");
            }
            else
            {
                var camps = dash.campanhas.Where(c => c.DataCampanha != null).OrderByDescending(c => c.DataCampanha).ToList();
                return View("~/Views/Dashboard/Campanhas.cshtml", camps);
            }
        }
        public ActionResult Avisos()
        {
            var usuarioLogado = HttpContext.Session["usuario"];
            if (usuarioLogado == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Você precisa estar logado para visualizar essa página. Clique em OK para ser redirecionado para a Área de login.');window.location = '/Home/Index';</script>");
            }
            else
            {
                var avi = dash.avisos.Where(c => c.DataAviso != null).OrderByDescending(c => c.DataAviso).ToList();
                return View("~/Views/Dashboard/Avisos.cshtml",avi);
            }
        }
    }
}