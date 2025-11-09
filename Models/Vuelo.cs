using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave2_Grupo3.Models
{
    public class Vuelo
    {
        // Atributos
        public int Id { get; set; }
        public int AerolineaId { get; set; }
        public int AvionId { get; set; }
        public int RutaId { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaLlegada { get; set; }
        public decimal TarifaBase { get; set; }
        public int AsientosDisponibles { get; set; }

        // Métodos
        public double CalcularDuracion()
        {
            TimeSpan duracion = FechaLlegada - FechaSalida;
            return duracion.TotalHours;
        }

        public void ActualizarAsientos(int cantidad)
        {
            AsientosDisponibles -= cantidad;
            if (AsientosDisponibles < 0) AsientosDisponibles = 0;
        }

        public decimal ObtenerTarifa()
        {
            return TarifaBase;
        }
    }
}
