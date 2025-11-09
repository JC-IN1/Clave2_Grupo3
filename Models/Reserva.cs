using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave2_Grupo3.Models
{
    public class Reserva
    {
        // Atributos
        public int Id { get; set; }
        public int VueloId { get; set; }
        public int PasajeroId { get; set; }
        public DateTime FechaReserva { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Estado { get; set; }
        public bool EquipajeBodega { get; set; }
        public bool EquipajeMano { get; set; }
        public string PreferenciaAsiento { get; set; }

        // Métodos
        public decimal CalcularPrecioFinal()
        {
            decimal precioBase = 100; // valor simulado
            if (EquipajeMano)
                precioBase += precioBase * 0.10m;
            if (EquipajeBodega)
                precioBase += precioBase * 0.20m;

            PrecioTotal = precioBase;
            return PrecioTotal;
        }

        public void ConfirmarReserva()
        {
            Estado = "Confirmada";
        }

        public void CancelarReserva()
        {
            Estado = "Cancelada";
        }
    }
}
