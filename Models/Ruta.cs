using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave2_Grupo3.Models
{
    public class Ruta
    {
        // Atributos
        public int Id { get; set; }
        public Ciudad Origen { get; set; }
        public Ciudad Destino { get; set; }
        public double DistanciaKm { get; set; }

        // Constructor vacío
        public Ruta() { }

        // Constructor con parámetros
        public Ruta(int id, Ciudad origen, Ciudad destino, double distanciaKm)
        {
            Id = id;
            Origen = origen;
            Destino = destino;
            DistanciaKm = distanciaKm;
        }

        // Métodos
        public double CalcularDuracionPromedio()
        {
            // velocidad promedio de 800 km/h
            double duracionHoras = DistanciaKm / 800.0;
            return Math.Round(duracionHoras, 2);
        }

        public string MostrarDatos()
        {
            return $"{Origen?.MostrarDatos()} → {Destino?.MostrarDatos()} | Distancia: {DistanciaKm} km";
        }
    }
}
