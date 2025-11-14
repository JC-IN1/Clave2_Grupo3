using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Clave2_Grupo3.Conexion
{
    public class ConexionBD
    {
        private string cadenaConexion ="Server=127.0.0.1;Port=3306;Database=sistema_vuelos;Uid=vuelos_app;Pwd=1234;";

        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }
    }
}
