using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave2_Grupo3.Models
{
    public class Pago
    {
        // Atributos
        public int Id { get; set; }
        public int ReservaId { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string Metodo { get; set; }
        public string EstadoPago { get; set; }

        // Métodos
        public void RegistrarPago()
        {
            EstadoPago = "Procesado";
            FechaPago = DateTime.Now;
        }

        public string VerificarEstado()
        {
            return $"Estado del pago #{Id}: {EstadoPago}";
        }
    }
}
