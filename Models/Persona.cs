using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave2_Grupo3.Models
{
    public class Persona
    {
        // Atributos
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Documento { get; set; }

        // Métodos
        public virtual string MostrarDatos()
        {
            return $"ID: {Id}, Nombre: {Nombre}, Fecha Nacimiento: {FechaNacimiento:dd/MM/yyyy}, Documento: {Documento}";
        }
    }
}
