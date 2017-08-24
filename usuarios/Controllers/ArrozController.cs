using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using usuarios.Models;

namespace usuarios.Controllers
{
    public class ArrozController : Controller
    {
        ElasticClient elasticClient = Broker.EsClient();
        [HttpGet]
        public ActionResult Index(Busqueda busqueda)
        {
            List<Precio> precios = new List<Precio>();
            List<double> numerosI = new List<double>();
            List<double> numerosJ = new List<double>();
            List<double> preciosFiltro = new List<double>();
            List<string> fechas = new List<string>();
            List<Estadistica> estadisticas = new List<Estadistica>();

            if (!string.IsNullOrEmpty(busqueda.botonbuscar))
            {
                string code = "azi";

                if (busqueda.variedad == "Japonica")
                {
                    code = "azj";

                    var indica = elasticClient.Search<Precio>(s => s
                                                         .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(26)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, "azi"),
                                                                bs => bs.Term(p => p.año, System.Convert.ToInt16(busqueda.campaña)),
                                                                bs => bs.Term(p => p.nomMes, busqueda.mes),
                                                                bs => bs.Term(p => p.tipoPrecio, "max")
                                                                ))));
                    foreach (var hit in indica.Hits)
                    {
                        numerosI.Add(hit.Source.precio);

                    }

                }else
                {
                    var japonica = elasticClient.Search<Precio>(s => s
                                                         .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(26)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, "azj"),
                                                                bs => bs.Term(p => p.año, System.Convert.ToInt16(busqueda.campaña)),
                                                                bs => bs.Term(p => p.nomMes, busqueda.mes),
                                                                bs => bs.Term(p => p.tipoPrecio, "max")
                                                                ))));
                    foreach (var hit in japonica.Hits)
                    {
                        numerosJ.Add(hit.Source.precio);

                    }

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
                                                                bs => bs.Term(p => p.nomMes,busqueda.mes),
                                                                bs => bs.Term(p => p.tipoPrecio, "max")
                                                                ))));
                if (filtro.Hits.Count == 0)
                {
                    return RedirectToAction("NoResult");
                }

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
                    preciosFiltro.Add(p.precio);
                    fechas.Add(p.fecha);

                }
                if (code=="azi")
                {
                    busqueda.preciosI = preciosFiltro;
                    busqueda.preciosJ = numerosJ;
                }else
                {
                    busqueda.preciosJ = preciosFiltro;
                    busqueda.preciosI = numerosI;
                }

                var estats = elasticClient.Search<Estadistica>(s => s
                                                        .Index("cereales")
                                                        .Type("estadistica")
                                                        .Query(q => q.Term(p => p.codigo, "az")));

                var maxymin = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.precio))
                                                        .Query(q => q
                                                        .Bool(qb => qb
                                                        .Must(
                                                                bs => bs.Term(p => p.codigo, "azi"),
                                                                bs => bs.Range(c => c.Field(p => p.precio).GreaterThan(0).Relation(RangeRelation.Within))
                                                         ))));


                var ultimo_precio = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.fecha))
                                                        .Query(q => q.Term(p => p.codigo, "azi")));


                
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
                busqueda.precios=preciosFiltro;
                busqueda.resultados = precios;
                busqueda.maxHistorico = maxymin.Hits.First().Source;
                busqueda.minHistorico = maxymin.Hits.Last().Source;
                busqueda.actual = ultimo_precio.Hits.First().Source;
                busqueda.numCampañas = 7;
                busqueda.fechas = fechas;
                busqueda.estats = estadisticas;

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
                                                                bs => bs.Term(p => p.codigo, "azi"),
                                                                bs => bs.Range(c => c.Field(p => p.precio).GreaterThan(0).Relation(RangeRelation.Within))
                                                         ))));

                var ultimo_precio = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                        .Type("precio")
                                                        .Size(10000)
                                                        .Sort(ss => ss.Descending(p => p.fecha))
                                                        .Query(q => q.Term(p => p.codigo, "azi")));

                var cargaIncial = elasticClient.Search<Precio>(s => s
                                                        .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(26)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, "azi"),
                                                                bs => bs.Term(p => p.año, 2016),
                                                                bs => bs.Term(p => p.tipoPrecio, "max")
                                                                ))));

                var japonica = elasticClient.Search<Precio>(s => s
                                                         .Index("agroesi")
                                                         .Type("precio")
                                                         .Size(26)
                                                         .Sort(ss => ss.Ascending(p => p.fecha))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(p => p.codigo, "azj"),
                                                                bs => bs.Term(p => p.año, 2016),
                                                                bs => bs.Term(p => p.tipoPrecio, "max")
                                                                ))));

                var estats = elasticClient.Search<Estadistica>(s => s
                                                        .Index("cereales")
                                                        .Type("estadistica")
                                                        .Query(q => q.Term(p => p.codigo, "az")));
                foreach (var hit in japonica.Hits)
                {
                    numerosJ.Add(hit.Source.precio);

                }
                foreach (var hit in cargaIncial.Hits)
                {
                    Precio p = new Precio();
                    p.precio = Convert.ToDouble(hit.Source.precio);
                    p.medida = hit.Source.medida;
                    p.fecha = hit.Source.fecha;
                    p.tipoPrecio = hit.Source.tipoPrecio;
                    precios.Add(p);
                    preciosFiltro.Add(p.precio);
                    numerosI.Add(p.precio);
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
                busqueda.numCampañas = 7;
                busqueda.preciosI = numerosI;
                busqueda.resultados = precios;
                busqueda.fechas = fechas;
                busqueda.estats = estadisticas;
                busqueda.preciosJ = numerosJ;
                busqueda.precios = preciosFiltro;
            }


            return View(busqueda);
        }
        public ActionResult NoResult()
        {
            return View();
        }
    }
}