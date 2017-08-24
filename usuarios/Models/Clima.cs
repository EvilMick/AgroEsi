using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace usuarios.Models
{
    public class Clima
    {
        public int año { get; set; }
        public int numMes { get; set; }
        public string nomMes { get; set; }
        public double Tmedia { get; set; }
        public double Tmax { get; set; }
        public double Tmin { get; set; }
        public double PresionATM { get; set; }
        public double Humedad { get; set; }
        public double Lluvia { get; set; }
        public double Visibilidad { get; set; }
        public double VelocidadMed { get; set; }
        public double VelocidadMax { get; set; }
        public Clima()
        {

        }

    }
}