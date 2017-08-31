using PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace usuarios.Models
{
    public class Busqueda
    {
        [Display(Name = "Producto")]
        public string producto { get; set; }
        public string nombre { get; set; }
        public string variedad { get; set; }
        public int? pagina { get; set; }
        public string campaña { get; set; }
        public string mes { get; set; }
        public string botonbuscar { get; set; }
        public List<Precio> resultados { get; set; }
        public List<double> precios { get; set; }
        public List<double> numerosD { get; set; }
        public List<double> preciosI { get; set; }
        public List<double> preciosJ { get; set; }
        public List<Noticia> noticias { get; set; }
        public List<Noticia> noticiasGN { get; set; }
        public List<Noticia> noticiasAG { get; set; }
        public Precio maxHistorico { get; set; }
        public Precio minHistorico { get; set; }
        public Precio actual { get; set; }
        public int numCampañas { get; set; }
        public List<string> fechas { get; set; }
        public List<Estadistica> estats { get; set; }
        public List<Clima> datosClimaticos { get; set; }
        public List<double> lluvias { get; set;}
        public List<double> temp { get; set; }
        public IPagedList<Noticia> listn { get; set; }
        public string lonja { get; set; }

        public List<SelectListItem> campañasMaiz = new List<SelectListItem>();
        public List<SelectListItem> campañasM
        {
            get
            {
                for(int i = 2017; i >2010; i--)
                {
                    campañasMaiz.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                return campañasMaiz;
            }
        }
        public List<SelectListItem> campañasTrigo = new List<SelectListItem>();
        public List<SelectListItem> campañasT
        {
            get
            {
                for (int i = 2017; i > 2010; i--)
                {
                    campañasTrigo.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                return campañasTrigo;
            }
        }
        public List<SelectListItem> campañasCebada = new List<SelectListItem>();
        public List<SelectListItem> campañasC
        {
            get
            {
                for (int i = 2016; i > 2010; i--)
                {
                    campañasCebada.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                return campañasCebada;
            }
        }
        public List<SelectListItem> campañasArroz = new List<SelectListItem>();
        public List<SelectListItem> campañasA
        {
            get
            {
                for(int i = 2016; i>2009; i--)
                {
                    campañasArroz.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                return campañasArroz;
            }
        }
        public List<SelectListItem> campañasSoja = new List<SelectListItem>();
        public List<SelectListItem> campañasS
        {
            get
            {
                for(int i = 2017; i >2001; i--)
                {
                    campañasSoja.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                return campañasSoja;
            }
        }
        public List<SelectListItem> prodGN = new List<SelectListItem>();
        public List<SelectListItem> productosGN
        {
            get
            {
                prodGN.Add(new SelectListItem() { Text = "Leche de cabra", Value = "LC" });
                prodGN.Add(new SelectListItem() { Text = "Leche de oveja con D.O.", Value = "LOSDO" });
                prodGN.Add(new SelectListItem() { Text = "Leche de oveja sin D.O.", Value = "LOCDO" });
                prodGN.Add(new SelectListItem() { Text = "Cabrito basto de 7 a 10kg", Value = "C7B10" });
                prodGN.Add(new SelectListItem() { Text = "Cabrito fino de 7 a 9kg", Value = "C7F9" });
                prodGN.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 10 a 15kg", Value = "CM10CI15" });
                prodGN.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 15 a 19kg", Value = "CM15CI19" });
                prodGN.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 19 a 23kg", Value = "CM19CI23" });
                prodGN.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 23 a 25kg", Value = "CM23CI25" });
                prodGN.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 25 a 28kg", Value = "CM25CI28" });
                prodGN.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 28 a 34kg", Value = "CM28CI34" });
                prodGN.Add(new SelectListItem() { Text = "Cordero manchego con IGP media 10 kg", Value = "CMCIM10" });
                prodGN.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 10 a 15kg", Value = "CSI10YO15" });
                prodGN.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 15 a 19kg", Value = "CSI15YO19" });
                prodGN.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 19 a 23kg", Value = "CSI19YO23" });
                prodGN.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 23 a 25kg", Value = "CSI23YO25" });
                prodGN.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 25 a 28kg", Value = "CSI25YO28" });
                prodGN.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 28 a 34kg", Value = "CSI28YO34" });
                prodGN.Add(new SelectListItem() { Text = "Cordero sin IGP y otros media 10 kg", Value = "CSIYOM10" });
                prodGN.Add(new SelectListItem() { Text = "Ternero cruzado 1ª Calidad (Base 200kg)", Value = "TO200C1" });
                prodGN.Add(new SelectListItem() { Text = "Ternera cruzada 1ª Calidad (Base 200kg)", Value = "TA200C1" });
                prodGN.Add(new SelectListItem() { Text = "Ternero cruzado 2ª Calidad (Base 200kg)", Value = "TO200C2" });
                prodGN.Add(new SelectListItem() { Text = "Ternera cruzada 2ª Calidad (Base 200kg)", Value = "TA200C2" });
                prodGN.Add(new SelectListItem() { Text = "Ternero del país (Base 200kg)", Value = "TO200PA" });
                prodGN.Add(new SelectListItem() { Text = "Ternera del país (Base 200kg)", Value = "TA200PA" });
                return prodGN;
            }
        }
        public List<SelectListItem> añosCli = new List<SelectListItem>();
        public List<SelectListItem> añosClima
        {
            get
            {
                for (int i=2017; i>1972 ; i--)
                {
                    añosCli.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                return añosCli;
            }
        }
        public List<SelectListItem> meses = new List<SelectListItem>();
        public List<SelectListItem> mesesAño
        {
            get
            {
                meses.Add(new SelectListItem() { Text = "Enero", Value = "enero" });
                meses.Add(new SelectListItem() { Text = "Febrero", Value = "febrero" });
                meses.Add(new SelectListItem() { Text = "Marzo", Value = "marzo" });
                meses.Add(new SelectListItem() { Text = "Abril", Value = "abril" });
                meses.Add(new SelectListItem() { Text = "Mayo", Value = "mayo" });
                meses.Add(new SelectListItem() { Text = "Junio", Value = "junio" });
                meses.Add(new SelectListItem() { Text = "Julio", Value = "julio" });
                meses.Add(new SelectListItem() { Text = "Agosto", Value = "agosto" });
                meses.Add(new SelectListItem() { Text = "Septiembre", Value = "septiembre" });
                meses.Add(new SelectListItem() { Text = "Octubre", Value = "octubre" });
                meses.Add(new SelectListItem() { Text = "Noviembre", Value = "noviembre" });
                meses.Add(new SelectListItem() { Text = "Diciembre", Value = "diciembre" });
                return meses;
            }
        }
        public List<string> prodyvar(string code)
        {
            List<string> datos = new List<string>();

            switch (code)
            {
                case "LC":
                    datos.Add("Leche");
                    datos.Add("de cabra");
                    break;
                case "LOSDO":
                    datos.Add("Leche de Oveja");
                    datos.Add("sin D.O.");
                    break;
                case "LOCDO":
                    datos.Add("Leche de Oveja");
                    datos.Add("con D.O.");
                    break;
                case "C7B10":
                    datos.Add("Cabrito Basto");
                    datos.Add("de 7 a 10kg");
                    break;
                case "C7F9":
                    datos.Add("Cabrito Fino");
                    datos.Add("de 7 a 9kg");
                    break;
                case "CM10CI15":
                    datos.Add("Cordero Manchego con IGP");
                    datos.Add("de 10 a 15kg");
                    break;
                case "CM15CI19":
                    datos.Add("Cordero Manchego con IGP");
                    datos.Add("de 15 a 19kg");
                    break;
                case "CM19CI23":
                    datos.Add("Cordero Manchego con IGP");
                    datos.Add("de 19 a 23kg");
                    break;
                case "CM23CI25":
                    datos.Add("Cordero Manchego con IGP");
                    datos.Add("de 23 a 25kg");
                    break;
                case "CM25CI28":
                    datos.Add("Cordero Manchego con IGP");
                    datos.Add("de 25 a 28kg");
                    break;
                case "CM28CI34":
                    datos.Add("Cordero Manchego con IGP");
                    datos.Add("de 28 a 34kg");
                    break;
                case "CMCIM10":
                    datos.Add("Cordero Manchego con IGP");
                    datos.Add("media 10kg");
                    break;
                case "CSI10YO15":
                    datos.Add("Cordero sin IGP y otros");
                    datos.Add("de 10 a 15kg");
                    break;
                case "CSI15YO19":
                    datos.Add("Cordero sin IGP y otros");
                    datos.Add("de 15 a 19kg");
                    break;
                case "CSI19YO23":
                    datos.Add("Cordero sin IGP y otros");
                    datos.Add("de 19 a 23kg");
                    break;
                case "CSI23YO25":
                    datos.Add("Cordero sin IGP y otros");
                    datos.Add("de 23 a 25kg");
                    break;
                case "CSI25YO28":
                    datos.Add("Cordero sin IGP y otros");
                    datos.Add("de 25 a 28kg");
                    break;
                case "CSI28YO34":
                    datos.Add("Cordero sin IGP y otros");
                    datos.Add("de 28 a 34kg");
                    break;
                case "CSIYOM10":
                    datos.Add("Cordero sin IGP y otros");
                    datos.Add("media 10kg");
                    break;
                case "TA200C1":
                    datos.Add("Ternera cruzada");
                    datos.Add("1ª Calidad (Base 200kg)");
                    break;
                case "TO200C1":
                    datos.Add("Ternero cruzado");
                    datos.Add("1ª Calidad (Base 200kg)");
                    break;
                case "TA200C2":
                    datos.Add("Ternera cruzada");
                    datos.Add("2ª Calidad (Base 200kg)");
                    break;
                case "TO200C2":
                    datos.Add("Ternero cruzado");
                    datos.Add("2ª Calidad (Base 200kg)");
                    break;
                case "TA200PA":
                    datos.Add("Ternera");
                    datos.Add("del país (Base 200kg)");
                    break;
                case "TO200PA":
                    datos.Add("Ternero");
                    datos.Add("del país (Base 200kg)");
                    break;
            }

            return datos;
        }
        public List<SelectListItem> prodOviCap = new List<SelectListItem>();
        public List<SelectListItem> productosOVCA
        {
            get
            {
                prodOviCap.Add(new SelectListItem() { Text = "Leche de cabra", Value = "LC" });
                prodOviCap.Add(new SelectListItem() { Text = "Leche de oveja con D.O.", Value = "LOSDO" });
                prodOviCap.Add(new SelectListItem() { Text = "Leche de oveja sin D.O.", Value = "LOCDO" });
                prodOviCap.Add(new SelectListItem() { Text = "Cabrito basto de 7 a 10kg", Value = "C7B10" });
                prodOviCap.Add(new SelectListItem() { Text = "Cabrito fino de 7 a 9kg", Value = "C7F9" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 10 a 15kg", Value = "CM10CI15" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 15 a 19kg", Value = "CM15CI19" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 19 a 23kg", Value = "CM19CI23" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 23 a 25kg", Value = "CM23CI25" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 25 a 28kg", Value = "CM25CI28" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero manchego con IGP de 28 a 34kg", Value = "CM28CI34" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero manchego con IGP media 10 kg", Value = "CMCIM10" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 10 a 15kg", Value = "CSI10YO15" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 15 a 19kg", Value = "CSI15YO19" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 19 a 23kg", Value = "CSI19YO23" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 23 a 25kg", Value = "CSI23YO25" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 25 a 28kg", Value = "CSI25YO28" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero sin IGP y otros de 28 a 34kg", Value = "CSI28YO34" });
                prodOviCap.Add(new SelectListItem() { Text = "Cordero sin IGP y otros media 10 kg", Value = "CSIYOM10" });
                return prodOviCap;
            }
        }
        public List<SelectListItem> prodVac = new List<SelectListItem>();
        public List<SelectListItem> productosVC
        {
            get
            {
                prodVac.Add(new SelectListItem() { Text = "Ternero cruzado 1ª Calidad (Base 200kg)", Value = "TO200C1" });
                prodVac.Add(new SelectListItem() { Text = "Ternera cruzada 1ª Calidad (Base 200kg)", Value = "TA200C1" });
                prodVac.Add(new SelectListItem() { Text = "Ternero cruzado 2ª Calidad (Base 200kg)", Value = "TO200C2" });
                prodVac.Add(new SelectListItem() { Text = "Ternera cruzada 2ª Calidad (Base 200kg)", Value = "TA200C2" });
                prodVac.Add(new SelectListItem() { Text = "Ternero del país (Base 200kg)", Value = "TO200PA" });
                prodVac.Add(new SelectListItem() { Text = "Ternera del país (Base 200kg)", Value = "TA200PA" });
                return prodVac;
            }
        }
        public List<SelectListItem> campañasab = new List<SelectListItem>();
        public List<SelectListItem> campañasAlbacete
        {
            get
            {
                for (int i = 2017; i > 1998; i--)
                {
                    campañasab.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                return campañasab;
            }
        }
        public List<SelectListItem> campañastv = new List<SelectListItem>();
        public List<SelectListItem> campañasTalavera
        {
            get
            {
                for (int i = 2017; i > 2001; i--)
                {
                    campañastv.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                return campañastv;
            }
        }
        public List<SelectListItem> campañasgn = new List<SelectListItem>();
        public List<SelectListItem> campañasGN
        {
            get
            {
                for (int i = 2017; i > 2008; i--)
                {
                    campañasgn.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                return campañasgn;
            }
        }
        public List<SelectListItem> campañasag = new List<SelectListItem>();
        public List<SelectListItem> campañasAG
        {
            get
            {
                for (int i = 2017; i > 2012; i--)
                {
                    campañasag.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                return campañasag;
            }
        }

    }
    }