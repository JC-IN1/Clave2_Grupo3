using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave2_Grupo3.Models
{
    public class Avion
    {
        // Atributos
        public int Id { get; set; }
        public string Modelo { get; set; }
        public int Capacidad { get; set; }
        public string Matricula { get; set; }

        // Métodos
        public bool VerificarDisponibilidad()
        {
            // Simulación de disponibilidad
            return Capacidad > 0;
        }

        public string MostrarDatos()
        {
            return $"Avión: {Modelo}, Matrícula: {Matricula}, Capacidad: {Capacidad}";
        }
    }
}
