using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave2_Grupo3.Models
{
    public class Tripulante : Persona
    {
        // Atributos
        public string Cargo { get; set; }
        public string NumEmpleado { get; set; }

        // Métodos
        public void AsignarVuelo(Vuelo vuelo)
        {
            Console.WriteLine($"El tripulante {Nombre} ha sido asignado al vuelo con ID {vuelo.Id}.");
        }

        public override string MostrarDatos()
        {
            return base.MostrarDatos() + $", Cargo: {Cargo}, N° Empleado: {NumEmpleado}";
        }
    }
}
