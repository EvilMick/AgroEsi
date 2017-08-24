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
    public class ClimaController : Controller
    {
        ElasticClient elasticClient = Broker.EsClient();
        [HttpGet]
        public ActionResult Index(Busqueda busqueda)
        {
            List<Clima> datosCli = new List<Clima>();
            List<string> meses = new List<string>();
            List<double> temperaturas = new List<double>();
            List<double> lluvias = new List<double>();
            if (!string.IsNullOrEmpty(busqueda.botonbuscar))
            {
                var datosClima = elasticClient.Search<Clima>(s => s
                                                         .Index("meteo")
                                                         .Type("clima")
                                                         .Size(26)
                                                         .Sort(ss => ss.Ascending(c => c.numMes))
                                                         .Query(q => q
                                                         .Bool(qb => qb
                                                         .Must(
                                                                bs => bs.Term(c => c.año, busqueda.campaña)
                                                                ))));
                foreach (var dato in datosClima.Hits)
                {
                    Clima c = new usuarios.Models.Clima();
                    c.año = dato.Source.año;
                    c.nomMes = dato.Source.nomMes;
                    c.numMes = dato.Source.numMes;
                    c.Tmedia = dato.Source.Tmedia;
                    c.Tmax = dato.Source.Tmax;
                    c.Tmin = dato.Source.Tmin;
                    c.PresionATM = dato.Source.PresionATM;
                    c.Humedad = dato.Source.Humedad;
                    c.Lluvia = dato.Source.Lluvia;
                    c.Visibilidad = dato.Source.Visibilidad;
                    c.VelocidadMed = dato.Source.VelocidadMed;
                    c.VelocidadMax = dato.Source.VelocidadMax;
                    datosCli.Add(c);
                    meses.Add(c.nomMes);
                    lluvias.Add(c.Lluvia);
                    temperaturas.Add(c.Tmedia);
                }
                busqueda.datosClimaticos = datosCli;
                busqueda.lluvias = lluvias;
                busqueda.temp = temperaturas;
                busqueda.fechas = meses;
            }else
            {
                var datosClima = elasticClient.Search<Clima>(s => s
                                         .Index("meteo")
                                         .Type("clima")
                                         .Size(26)
                                         .Sort(ss => ss.Ascending(c => c.numMes))
                                         .Query(q => q
                                         .Bool(qb => qb
                                         .Must(
                                                bs => bs.Term(c => c.año, "2017")
                                                ))));
                foreach (var dato in datosClima.Hits)
                {
                    Clima c = new usuarios.Models.Clima();
                    c.nomMes = dato.Source.nomMes;
                    c.Tmedia = dato.Source.Tmedia;
                    c.Tmax = dato.Source.Tmax;
                    c.Tmin = dato.Source.Tmin;
                    c.PresionATM = dato.Source.PresionATM;
                    c.Humedad = dato.Source.Humedad;
                    c.Lluvia = dato.Source.Lluvia;
                    c.Visibilidad = dato.Source.Visibilidad;
                    c.VelocidadMed = dato.Source.VelocidadMed;
                    c.VelocidadMax = dato.Source.VelocidadMax;
                    datosCli.Add(c);
                    meses.Add(c.nomMes);
                    lluvias.Add(c.Lluvia);
                    temperaturas.Add(c.Tmedia);
                }
                busqueda.datosClimaticos = datosCli;
                busqueda.lluvias = lluvias;
                busqueda.temp = temperaturas;
                busqueda.fechas = meses;

            }
            return View(busqueda);
        }
    }
}