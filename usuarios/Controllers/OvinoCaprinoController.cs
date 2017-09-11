using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using usuarios;
using usuarios.Models;

namespace WebAgroEsi.Controllers
{
    public class OvinoCaprinoController : Controller
    {
        ElasticClient elasticClient = Broker.EsClient();
        [HttpGet]
        public ActionResult Index(Busqueda busqueda)
        {
            List<Precio> precios = new List<Precio>();
            List<double> numerosMax = new List<double>();
            List<double> numerosMin = new List<double>();
            List<Noticia> noticias = new List<Noticia>();
            List<string> fechasMax = new List<string>();
            List<string> fechasMin = new List<string>();
            string prod = null;
            string var = null;
            if (!string.IsNullOrEmpty(busqueda.botonbuscar))
            {
                prod = busqueda.prodyvar(busqueda.producto)[0];
                var = busqueda.prodyvar(busqueda.producto)[1];

                var filtro = elasticClient.Search<Precio>(s => s
                                                         .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(12)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, busqueda.producto.ToLower()),
                                                                bs => bs.Term(p => p.año, busqueda.campaña),
                                                                bs => bs.Term(p => p.nomMes, busqueda.mes)
                                                                ))));
                if (filtro.Hits.Count==0)
                {
                    return RedirectToAction("NoResult");
                }

                var estadisticas = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.precio))
                                                        .Query(q => q.Term(p => p.codigo, busqueda.producto.ToLower())));

                var ultimo_precio = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.fecha))
                                                        .Query(q => q.Term(p => p.codigo, busqueda.producto.ToLower())));

                var news = elasticClient.Search<Noticia>(n => n
                                                .Index("rss")
                                                .Type("noticia")
                                                .Size(5)
                                                .Sort(ss => ss.Descending(p => p.fecha))
                                                .Query(q => q
                                                .Bool(qb => qb
                                                .Must(
                                                    bs => bs.Term(ns => ns.codigo, "gn"),
                                                    bs => bs.Term(ns => ns.año, 2017)))));

                foreach (var hit in news.Hits)
                {
                    Noticia n = new Noticia();
                    n.titulo = hit.Source.titulo;
                    n.link = hit.Source.link;
                    n.descripcion = hit.Source.descripcion;
                    n.fecha = hit.Source.fecha;
                    noticias.Add(n);
                }
                busqueda.noticias = noticias;


                foreach (var hit in filtro.Hits)
                {

                    Precio p = new Precio();
                    p.producto = prod;
                    p.variedad = var;
                    p.precio = Convert.ToDouble(hit.Source.precio);
                    p.medida = hit.Source.medida;
                    p.fecha = hit.Source.fecha;
                    p.tipoPrecio = hit.Source.tipoPrecio;
                    precios.Add(p);
                    if (p.tipoPrecio == "max")
                    {
                        numerosMax.Add(p.precio);
                        fechasMax.Add(p.fecha);
                        
                    }else
                    {
                        numerosMin.Add(p.precio);
                        fechasMin.Add(p.fecha);
                    }

                }
                busqueda.preciosMax = numerosMax;
                busqueda.preciosMin = numerosMin;
                busqueda.resultados = precios;
                busqueda.maxHistorico = estadisticas.Hits.First().Source;
                busqueda.minHistorico = estadisticas.Hits.Last().Source;
                busqueda.actual = ultimo_precio.Hits.First().Source;
                busqueda.fechasMax = fechasMax;
                busqueda.fechasMin = fechasMin;

            }
            else
            {
                var estadisticas = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.precio))
                                                        .Query(q => q.Term(p => p.codigo, "LC".ToLower())));

                var ultimo_precio = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.fecha))
                                                        .Query(q => q.Term(p => p.codigo, "LC".ToLower())));

                var cargaIncial = elasticClient.Search<Precio>(s => s
                                                         .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(12)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, "LC".ToLower()),
                                                                bs => bs.Term(p => p.año, 2017),
                                                                bs => bs.Term(p => p.nomMes, "julio")
                                                                ))));

                var news = elasticClient.Search<Noticia>(n => n
                                                .Index("rss")
                                                .Type("noticia")
                                                .Size(5)
                                                .Sort(ss => ss.Descending(p => p.fecha))
                                                .Query(q => q
                                                .Bool(qb => qb
                                                .Must(
                                                    bs => bs.Term(ns => ns.codigo, "gn"),
                                                    bs => bs.Term(ns => ns.año, 2017)))));


                foreach (var hit in news.Hits)
                {
                    Noticia n = new Noticia();
                    n.titulo = hit.Source.titulo;
                    n.link = hit.Source.link;
                    n.descripcion = hit.Source.descripcion;
                    n.fecha = hit.Source.fecha;
                    noticias.Add(n);
                }
                busqueda.noticias = noticias;
                foreach (var hit in cargaIncial.Hits)
                {
                    Precio p = new Precio();
                    p.producto = "Leche";
                    p.variedad = "de cabra";
                    p.precio = Convert.ToDouble(hit.Source.precio);
                    p.medida = hit.Source.medida;
                    p.fecha = hit.Source.fecha;
                    p.tipoPrecio = hit.Source.tipoPrecio;
                    precios.Add(p);
                    if (p.tipoPrecio == "max")
                    {
                        numerosMax.Add(p.precio);
                        fechasMax.Add(p.fecha);

                    }
                    else
                    {
                        numerosMin.Add(p.precio);
                        fechasMin.Add(p.fecha);
                    }


                }
                busqueda.maxHistorico = estadisticas.Hits.First().Source;
                busqueda.minHistorico = estadisticas.Hits.Last().Source;
                busqueda.actual = ultimo_precio.Hits.First().Source;
                busqueda.preciosMax = numerosMax;
                busqueda.preciosMin = numerosMin;
                busqueda.resultados = precios;
                busqueda.fechasMax = fechasMax;
                busqueda.fechasMin = fechasMin;
            }




            return View(busqueda);
        }

        public ActionResult NoResult()
        {
            return View();
        }
    }
}