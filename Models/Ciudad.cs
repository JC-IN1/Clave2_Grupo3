using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave2_Grupo3.Models
{
    public class Ciudad
    {
        // Atributos
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }

        // Constructor vacío
        public Ciudad() { }

        // Constructor con parámetros
        public Ciudad(int id, string nombre, string pais)
        {
            Id = id;
            Nombre = nombre;
            Pais = pais;
        }

        // Método
        public string MostrarDatos()
        {
            return $"{Nombre}, {Pais}";
        }
    }
}
