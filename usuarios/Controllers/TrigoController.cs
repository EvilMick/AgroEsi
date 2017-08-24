using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using usuarios.Models;

namespace usuarios.Controllers
{
    public class TrigoController : Controller
    {
        ElasticClient elasticClient = Broker.EsClient();
        [HttpGet]
        public ActionResult Index(Busqueda busqueda)
        {
            List<Precio> precios = new List<Precio>();
            List<double> numeros = new List<double>();
            List<double> numerosD = new List<double>();
            List<string> fechas = new List<string>();
            List<Estadistica> estadisticas = new List<Estadistica>();

            if (!string.IsNullOrEmpty(busqueda.botonbuscar))
            {
                string code = "tb";
                if (busqueda.variedad == "Blando")
                {
                    code = "tb";
                }
                if (busqueda.variedad == "Duro")
                {
                    code = "td";
                }

                var filtro = elasticClient.Search<Precio>(s => s
                                                         .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(26)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, code),
                                                                bs => bs.Term(p => p.año, System.Convert.ToInt16(busqueda.campaña)),
                                                                bs => bs.Term(p => p.tipoPrecio, "med"),
                                                                bs => bs.Term(p => p.nomMes,busqueda.mes)
                                                                ))));

                if (filtro.Hits.Count == 0)
                {
                    return RedirectToAction("NoResult");
                }
                var estats = elasticClient.Search<Estadistica>(s => s
                                                        .Index("cereales")
                                                        .Type("estadistica")
                                                        .Query(q => q.Term(p => p.codigo, "tb")));

                var maxymin = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.precio))
                                                        .Query(q => q
                                                        .Bool(qb => qb
                                                        .Must(
                                                                bs => bs.Term(p => p.codigo, "tb"),
                                                                bs => bs.Range(c => c.Field(p => p.precio).GreaterThan(0).Relation(RangeRelation.Within))
                                                         ))));
                if (code == "td")
                {
                    var blando = elasticClient.Search<Precio>(s => s
                                                         .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(26)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, "tb"),
                                                                bs => bs.Term(p => p.año, System.Convert.ToInt16(busqueda.campaña)),
                                                                bs => bs.Term(p => p.tipoPrecio, "med"),
                                                                bs => bs.Term(p => p.nomMes, busqueda.mes)
                                                                ))));

                    foreach (var hit in blando.Hits)
                    {
                        numerosD.Add(hit.Source.precio);

                    }
                    busqueda.numerosD = numerosD;
                }
                else
                {
                    var duro = elasticClient.Search<Precio>(s => s
                                                         .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(26)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, "td"),
                                                                bs => bs.Term(p => p.año, System.Convert.ToInt16(busqueda.campaña)),
                                                                bs => bs.Term(p => p.tipoPrecio, "med"),
                                                                bs => bs.Term(p => p.nomMes, busqueda.mes)
                                                                ))));
                    
                    foreach (var hit in duro.Hits)
                    {
                        numerosD.Add(hit.Source.precio);

                    }
                    busqueda.numerosD = numerosD;

                }


                var ultimo_precio = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.fecha))
                                                        .Query(q => q.Term(p => p.codigo, "tb")));


                foreach (var hit in filtro.Hits)
                {
                    Precio p = new Precio();
                    p.producto = busqueda.nombre;
                    p.variedad = busqueda.variedad;
                    p.precio = Convert.ToDouble(hit.Source.precio);
                    p.medida = hit.Source.medida;
                    p.fecha = hit.Source.fecha;
                    p.tipoPrecio = hit.Source.tipoPrecio;
                    precios.Add(p);
                    numeros.Add(p.precio);
                    fechas.Add(p.fecha);

                }
                foreach (var hit in estats.Hits)
                {
                    Estadistica e = new Estadistica();
                    e.campaña = hit.Source.campaña;
                    e.superficie = hit.Source.superficie;
                    e.produccion = hit.Source.produccion;
                    e.importacines = hit.Source.importacines;
                    e.exportaciones = hit.Source.exportaciones;
                    estadisticas.Add(e);
                }

                busqueda.precios = numeros;
                busqueda.resultados = precios;
                busqueda.maxHistorico = maxymin.Hits.First().Source;
                busqueda.minHistorico = maxymin.Hits.Last().Source;
                busqueda.actual = ultimo_precio.Hits.First().Source;
                busqueda.numCampañas = 6;
                busqueda.fechas = fechas;
                busqueda.estats = estadisticas;
                busqueda.numerosD = numerosD;

            }
            else
            {
                var maxymin = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.precio))
                                                        .Query(q => q
                                                        .Bool(qb => qb
                                                        .Must(
                                                                bs => bs.Term(p => p.codigo, "tb"),
                                                                bs => bs.Range(c => c.Field(p => p.precio).GreaterThan(0).Relation(RangeRelation.Within))
                                                         ))));

                var ultimo_precio = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.fecha))
                                                        .Query(q => q.Term(p => p.codigo, "tb")));

                var cargaIncial = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(26)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, "tb"),
                                                                bs => bs.Term(p => p.año, 2011),
                                                                bs => bs.Term(p => p.tipoPrecio, "med")
                                                                ))));

                var duro = elasticClient.Search<Precio>(s => s
                                                         .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(26)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, "td"),
                                                                bs => bs.Term(p => p.año, 2011),
                                                                bs => bs.Term(p => p.tipoPrecio, "med")
                                                                ))));

                var estats = elasticClient.Search<Estadistica>(s => s
                                                        .Index("cereales")
                                                        .Type("estadistica")
                                                        .Query(q => q.Term(p => p.codigo, "tb")));
                foreach (var hit in duro.Hits)
                {
                    numerosD.Add(hit.Source.precio);

                }
                foreach (var hit in cargaIncial.Hits)
                {
                    Precio p = new Precio();
                    p.precio = Convert.ToDouble(hit.Source.precio);
                    p.medida = hit.Source.medida;
                    p.fecha = hit.Source.fecha;
                    p.tipoPrecio = hit.Source.tipoPrecio;
                    precios.Add(p);
                    numeros.Add(p.precio);
                    fechas.Add(p.fecha);

                }
                foreach (var hit in estats.Hits)
                {
                    Estadistica e = new Estadistica();
                    e.campaña = hit.Source.campaña;
                    e.superficie = hit.Source.superficie;
                    e.produccion = hit.Source.produccion;
                    e.importacines = hit.Source.importacines;
                    e.exportaciones = hit.Source.exportaciones;
                    estadisticas.Add(e);
                }
                busqueda.maxHistorico = maxymin.Hits.First().Source;
                busqueda.minHistorico = maxymin.Hits.Last().Source;
                busqueda.actual = ultimo_precio.Hits.First().Source;
                busqueda.numCampañas = 6;
                busqueda.precios = numeros;
                busqueda.resultados = precios;
                busqueda.fechas = fechas;
                busqueda.estats = estadisticas;
                busqueda.numerosD = numerosD;

            }


            return View(busqueda);
        }
        public ActionResult NoResult()
        {
            return View();
        }
    }
}