using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave2_Grupo3.Models
{
    public class Usuario
    {
        // Atributos
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public string Rol { get; set; } // "Administrador", "Operador"

        // Métodos
        public bool VerificarAcceso(string usuario, string contrasena)
        {
            return NombreUsuario == usuario && Contrasena == contrasena;
        }

        public override string ToString()
        {
            return $"{NombreUsuario} ({Rol})";
        }
    }
}
