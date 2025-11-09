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
        public string Origen { get; set; }     
        public string Destino { get; set; }    
        public double DistanciaKm { get; set; }

        // Métodos
        public string Descripcion
        {
            get { return $"{Origen} - {Destino}"; }
        }

        public double CalcularDuracionPromedio()
        {
            return DistanciaKm / 800.0; // ejemplo
        }

        public string MostrarDatos()
        {
            return $"Ruta: {Origen} - {Destino} ({DistanciaKm} km)";
        }
    }
}
