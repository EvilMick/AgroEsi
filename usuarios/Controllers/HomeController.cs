using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using usuarios.Models;

namespace usuarios.Controllers
{
    public class HomeController : Controller
    {
        ElasticClient elasticClient = Broker.EsClient();
        public ActionResult Index()
        {
            List<Noticia> noticiasGN = new List<Noticia>();
            List<Noticia> noticiasAG = new List<Noticia>();
            Busqueda b = new Busqueda();

            var newsgn = elasticClient.Search<Noticia>(n => n
                                                .Index("rss")
                                                .Type("noticia")
                                                .Size(5)
                                                .Sort(ss => ss.Descending(p => p.fecha))
                                                .Query(q => q
                                                .Bool(qb => qb
                                                .Must(
                                                    bs => bs.Term(ns => ns.codigo, "gn"),
                                                    bs => bs.Term(ns => ns.año, 2017)))));
            foreach(var hit in newsgn.Hits)
            {
                Noticia n = new Noticia();
                n.titulo = hit.Source.titulo;
                n.link = hit.Source.link;
                noticiasGN.Add(n);
            }
            b.noticiasGN = noticiasGN;

            var newsag = elasticClient.Search<Noticia>(n => n
                                                .Index("rss")
                                                .Type("noticia")
                                                .Size(5)
                                                .Sort(ss => ss.Descending(p => p.fecha))
                                                .Query(q => q
                                                .Bool(qb => qb
                                                .Must(
                                                    bs => bs.Term(ns => ns.codigo, "ag"),
                                                    bs => bs.Term(ns => ns.año, 2017)))));
            foreach (var hit in newsag.Hits)
            {
                Noticia n = new Noticia();
                n.titulo = hit.Source.titulo;
                n.link = hit.Source.link;
                noticiasAG.Add(n);
            }
            b.noticiasAG = noticiasAG;

            return View(b);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Acerca de";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contacto";

            return View();
        }
    }
}