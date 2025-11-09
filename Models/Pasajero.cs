using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave2_Grupo3.Models
{
    public class Pasajero : Persona
    {
        // Atributos
        public string PreferenciaAsiento { get; set; }
        public bool EquipajeMano { get; set; }
        public bool EquipajeBodega { get; set; }

        // Métodos
        public decimal CalcularPrecioVuelo(decimal tarifaBase)
        {
            decimal total = tarifaBase;
            if (EquipajeMano)
                total += tarifaBase * 0.10m; // +10%
            if (EquipajeBodega)
                total += tarifaBase * 0.20m; // +20%
            return total;
        }

        public override string MostrarDatos()
        {
            return base.MostrarDatos() +
                   $", Asiento: {PreferenciaAsiento}, Mano: {EquipajeMano}, Bodega: {EquipajeBodega}";
        }
    }
}
