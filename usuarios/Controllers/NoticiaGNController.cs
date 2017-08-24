using Nest;
using System.Collections.Generic;
using System.Web.Mvc;
using usuarios.Models;
using PagedList;
using usuarios;

namespace usuarios.Controllers
{
    public class NoticiaGNController : Controller
    {
        ElasticClient elasticClient = Broker.EsClient();
        // GET: Noticia
        public ActionResult Index(int? page, Busqueda busqueda)
        {
            List<Noticia> noticias = new List<Noticia>();
            if (busqueda.campaña==null || busqueda.mes==null)
            {
                var news = elasticClient.Search<Noticia>(n => n
                                                                .Index("rss")
                                                                .Type("noticia")
                                                                .Size(60)
                                                                .Sort(s => s.Descending(sn => sn.fecha))
                                                                .Query(q => q
                                                                .Bool(qb => qb
                                                                .Must(
                                                                    bs => bs.Term(p => p.codigo, "gn"),
                                                                    bs => bs.Term(p => p.año, 2017)
                                                                ))));
                foreach (var hit in news.Hits)
                {
                    Noticia n = new Noticia();
                    n.etiquetas = hit.Source.etiquetas;
                    n.titulo = hit.Source.titulo;
                    n.descripcion = hit.Source.descripcion;
                    n.link = hit.Source.link;
                    n.imagen = hit.Source.imagen;
                    n.fecha = hit.Source.fecha;
                    n.año = hit.Source.año;
                    n.numMes = hit.Source.numMes;
                    noticias.Add(n);
                }

            }
            if (busqueda.campaña != null && busqueda.mes != null)
            {
                var news = elasticClient.Search<Noticia>(n => n
                                                                .Index("rss")
                                                                .Type("noticia")
                                                                .Size(12)
                                                                .Sort(s => s.Ascending(sn => sn.fecha))
                                                                .Query(q => q
                                                                .Bool(qb => qb
                                                                .Must(
                                                                    bs => bs.Term(p => p.codigo, "gn"),
                                                                    bs => bs.Term(p => p.año, busqueda.campaña),
                                                                    bs => bs.Term(p => p.nomMes, busqueda.mes)
                                                                ))));
                foreach (var hit in news.Hits)
                {
                    Noticia n = new Noticia();
                    n.etiquetas = hit.Source.etiquetas;
                    n.titulo = hit.Source.titulo;
                    n.descripcion = hit.Source.descripcion;
                    n.link = hit.Source.link;
                    n.imagen = hit.Source.imagen;
                    n.fecha = hit.Source.fecha;
                    n.año = hit.Source.año;
                    n.numMes = hit.Source.numMes;
                    noticias.Add(n);
                }


            }

            if (noticias.Count == 0)
            {
                return RedirectToAction("NoResult");
            }


            int pageSize = 12;
            int pageNumber = (page ?? 1);
            IPagedList<Noticia> listn = noticias.ToPagedList(pageNumber, pageSize);
            busqueda.listn = listn;
            return View(busqueda);
        }
        public ActionResult Buscar(string etiqueta)
        {
            List<Noticia> noticias = new List<Noticia>();
            string aux = null;
            if (etiqueta.Contains("-"))
            {
                aux = etiqueta.Split('-')[0];

            }if (etiqueta.Contains(" ")){

                aux = etiqueta.Split(' ')[0];
            }
            else
            {
                aux = etiqueta;
            }
            
            var news = elasticClient.Search<Noticia>(n => n
                                                                .Index("rss")
                                                                .Type("noticia")
                                                                .Size(12)
                                                                .Sort(s => s.Ascending(sn => sn.fecha))
                                                                .Query(q => q
                                                                .Bool(qb => qb
                                                                .Must(
                                                                    bs => bs.Term(p => p.etiquetas, aux.ToLower()),
                                                                    bs => bs.Term(p => p.codigo,"GN".ToLower())
                                                                ))));
            if (news.Hits.Count == 0)
            {
                return RedirectToAction("NoResult");
            }


            foreach (var hit in news.Hits)
            {
                Noticia n = new Noticia();
                n.link = hit.Source.link;
                n.titulo = hit.Source.titulo;
                n.imagen = hit.Source.imagen;
                n.etiquetas = hit.Source.etiquetas;
                n.fecha = hit.Source.fecha;
                n.numMes = hit.Source.numMes;
                n.año = hit.Source.año;
                noticias.Add(n);
            }
            return View(noticias);
        }
        public ActionResult BuscarF(int mes, int año)
        {
            List<Noticia> noticias = new List<Noticia>();
            var news = elasticClient.Search<Noticia>(n => n
                                                                .Index("rss")
                                                                .Type("noticia")
                                                                .Size(36)
                                                                .Sort(s => s.Ascending(sn => sn.fecha))
                                                                .Query(q => q
                                                                .Bool(qb => qb
                                                                .Must(
                                                                    bs => bs.Term(p => p.codigo, "GN".ToLower()),
                                                                     bs => bs.Term(p => p.año, año),
                                                                      bs => bs.Term(p => p.numMes, mes)


                                                                ))));
            if (news.Hits.Count == 0)
            {
                return RedirectToAction("NoResult");
            }

            foreach (var hit in news.Hits)
            {
                Noticia n = new Noticia();
                n.etiquetas = hit.Source.etiquetas;
                n.titulo = hit.Source.titulo;
                n.descripcion = hit.Source.descripcion;
                n.link = hit.Source.link;
                n.imagen = hit.Source.imagen;
                n.fecha = hit.Source.fecha;
                n.año = hit.Source.año;
                n.numMes = hit.Source.numMes;
                noticias.Add(n);
            }
            return View(noticias);
        }

        public ActionResult NoResult()
        {
            return View();
        }
    }
}