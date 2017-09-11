using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using usuarios;
using usuarios.Models;

namespace WebAgroEsi.Controllers
{
    public class VacunoController : Controller
    {
        ElasticClient elasticClient = Broker.EsClient();
        [HttpGet]
 
        public ActionResult Index(Busqueda busqueda)
        {
            List<Precio> precios = new List<Precio>();
            List<double> numeros = new List<double>();
            List<Noticia> noticias = new List<Noticia>();
            List<string> fechas = new List<string>();
            string prod = null;
            string var = null;
            if (!string.IsNullOrEmpty(busqueda.botonbuscar))
            {
                prod = busqueda.prodyvar(busqueda.producto)[0];
                var = busqueda.prodyvar(busqueda.producto)[1];
                int año = System.Convert.ToInt16(busqueda.campaña);
                ISearchResponse<Precio> result = null;
                if (año>=2002 && año<=2016)
                {
                    result= elasticClient.Search<Precio>(s => s
                                                         .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(12)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, busqueda.producto.ToLower()),
                                                                bs => bs.Term(p => p.año, año)
                                                                ))));
                }
                else
                {
                    result = elasticClient.Search<Precio>(s => s
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

                }

                if (result.Hits.Count == 0)
                {
                    return RedirectToAction("NoResult");
                }


                var estadisticas = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.precio))
                                                        .Query(q => q
                                                        .Bool(qb => qb
                                                        .Must(
                                                                bs => bs.Term(p => p.codigo, busqueda.producto.ToLower()),
                                                                bs => bs.Range(c => c.Field(p => p.precio).GreaterThan(0).Relation(RangeRelation.Within))
                                                         ))));

                var ultimo_precio = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.fecha))
                                                        .Query(q => q
                                                        .Bool(qb => qb
                                                        .Must(
                                                                bs => bs.Term(p => p.codigo, busqueda.producto.ToLower()),
                                                                bs => bs.Range(c => c.Field(p => p.precio).GreaterThan(0).Relation(RangeRelation.Within))
                                                         ))));

                foreach (var hit in result.Hits)
                {

                    Precio p = new Precio();
                    p.producto = prod;
                    p.variedad = var;
                    p.precio = Convert.ToDouble(hit.Source.precio);
                    p.medida = hit.Source.medida;
                    p.fecha = hit.Source.fecha;
                    p.tipoPrecio = hit.Source.tipoPrecio;
                    precios.Add(p);
                    numeros.Add(p.precio);
                    fechas.Add(p.fecha);



                }
                busqueda.precios = numeros;
                busqueda.resultados = precios;
                busqueda.maxHistorico = estadisticas.Hits.First().Source;
                busqueda.minHistorico = estadisticas.Hits.Last().Source;
                busqueda.actual = ultimo_precio.Hits.First().Source;
                busqueda.fechas = fechas;

            }
            else
            {
                var estadisticas = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.precio))
                                                        .Query(q => q
                                                        .Bool(qb => qb
                                                        .Must(
                                                                bs => bs.Term(p => p.codigo, "TO200C1".ToLower()),
                                                                bs => bs.Range(c => c.Field(p => p.precio).GreaterThan(0).Relation(RangeRelation.Within))
                                                         ))));

                var ultimo_precio = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.fecha))
                                                        .Query(q => q
                                                        .Bool(qb => qb
                                                        .Must(
                                                                bs => bs.Term(p => p.codigo, "TO200C1".ToLower()),
                                                                bs => bs.Range(c => c.Field(p => p.precio).GreaterThan(0).Relation(RangeRelation.Within))
                                                         ))));

                var cargaIncial = elasticClient.Search<Precio>(s => s
                                                         .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(12)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, "TO200C1".ToLower()),
                                                                bs => bs.Term(p => p.año, 2017),
                                                                bs => bs.Term(p => p.nomMes, "agosto"),
                                                                bs => bs.Range(c => c.Field(p => p.precio).GreaterThan(0).Relation(RangeRelation.Within))
                                                                ))));

                foreach (var hit in cargaIncial.Hits)
                {
                    Precio p = new Precio();
                    p.producto = "Ternero cruzado";
                    p.variedad = "1ª Calidad (Base 200kg)";
                    p.precio = Convert.ToDouble(hit.Source.precio);
                    p.medida = hit.Source.medida;
                    p.fecha = hit.Source.fecha;
                    p.tipoPrecio = hit.Source.tipoPrecio;
                    precios.Add(p);
                    numeros.Add(p.precio);
                    fechas.Add(p.fecha);


                }
                busqueda.maxHistorico = estadisticas.Hits.First().Source;
                busqueda.minHistorico = estadisticas.Hits.Last().Source;
                busqueda.actual = ultimo_precio.Hits.First().Source;
                busqueda.precios = numeros;
                busqueda.resultados = precios;
                busqueda.fechas = fechas;
            }




            return View(busqueda);
        }
        public ActionResult NoResult()
        {
            return View();
        }
    }
}