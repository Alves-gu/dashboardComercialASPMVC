using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoardRepComercial.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var usuarioLogado = HttpContext.Session["usuario"];

            if (usuarioLogado != null)
            {
                return Redirect("/Dashboard/Index");
            }
            else
            {
                return View();
            }
        }
    }
}