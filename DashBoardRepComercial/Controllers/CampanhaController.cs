using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardRepComercial.Models;
using System.IO;

namespace DashBoardRepComercial.Controllers
{
    public class CampanhaController : Controller
    {
        dashboardEntities ctx = new dashboardEntities();
        // GET: Campanha
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
        public ActionResult CadastrarCampanha()
        {
            return View("~/Views/Campanha/Create.cshtml");
        }

        [HttpPost]
        public ActionResult CadastrarCampanha(campanhas campanha, HttpPostedFileBase file)
        {
            string[] allowedFiletypes = { ".jpeg", ".jpg", ".png", ".gif" };
            string ext = Path.GetExtension(file.FileName);
            if (file.ContentLength > 0 && allowedFiletypes.Contains(ext))
            {
                try
                {
                    //Obtençao das propriedades do arquivo
                    string name = Path.GetFileNameWithoutExtension(file.FileName);
                    byte[] bin = new byte[file.ContentLength];
                    using (BinaryReader binReader = new BinaryReader(file.InputStream))
                    {
                        bin = binReader.ReadBytes(file.ContentLength);
                    }
                    string array = Convert.ToBase64String(bin);
                    byte[] arquivo = Convert.FromBase64String(array);

                    //Instância do objeto/entidade campanha
                    campanhas clsCamp = new campanhas()
                    {
                        NomeCampanha = campanha.NomeCampanha,
                        MsgCampanha = campanha.MsgCampanha,
                        ImgCampanha = arquivo,
                        NomeImgCampanha = name,
                        ExtImgCampanha = ext,
                        DataCampanha = DateTime.Now
                    };
                    //Gravação no BD
                    ctx.campanhas.Add(clsCamp);
                    ctx.SaveChanges();
                    ViewBag.Message = "Campanha enviada com sucesso!!";
                    return View("~/Views/Campanha/Create.cshtml");
                }
                catch
                {
                    ViewBag.Message = "Ocorreu um erro no envio da campanha.";
                    return View("~/Views/Campanha/Create.cshtml");
                }
            }
            else
            {
                ViewBag.Message = "Por favor, selecione um tipo de imagem válido.";
                return View("~/Views/Campanha/Create.cshtml");
            }
        }

        [HttpGet]
        public ActionResult Show(int IdCampanha)
        {
            var usuarioLogado = HttpContext.Session["usuario"];
            if (usuarioLogado == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Você precisa estar logado para visualizar essa página. Clique em OK para ser redirecionado para a Área de login.');window.location = '/Home/Index';</script>");
            }
            else
            {
                var camps = ctx.campanhas.Where(i => i.IdCampanha == IdCampanha).FirstOrDefault();
                return View(camps);
            }
        }
        public ActionResult ShowImage(int IdCampanha)
        {
            var img = ctx.campanhas.Where(i => i.IdCampanha == IdCampanha).FirstOrDefault();
            return File(img.ImgCampanha, "?");
        }
    }
}