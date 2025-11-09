using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave2_Grupo3.Models
{
    public class Aerolinea
    {
        // Atributos
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }

        // Relación con vuelos
        private List<Vuelo> vuelos = new List<Vuelo>();

        // Métodos
        public void AgregarVuelo(Vuelo vuelo)
        {
            vuelos.Add(vuelo);
        }

        public List<Vuelo> ObtenerVuelos()
        {
            return vuelos;
        }
    }
}
