using System;
using Clave2_Grupo3.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clave2_Grupo3.Forms
{
    public partial class LoginForm : Form
    {
        public List<Usuario> usuarios = new List<Usuario>();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Carga de usuarios simulada (más adelante vendrán de MySQL)
            usuarios.Add(new Usuario { Id = 1, NombreUsuario = "admin", Contrasena = "1234", Rol = "Administrador" });
            usuarios.Add(new Usuario { Id = 2, NombreUsuario = "operador", Contrasena = "abcd", Rol = "Operador" });
            usuarios.Add(new Usuario { Id = 3, NombreUsuario = "Juan", Contrasena = "2345", Rol = "Cliente" });
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string user = txtUsuario.Text.Trim();
            string pass = txtContrasena.Text.Trim();

            bool accesoValido = false;

            foreach (var u in usuarios)
            {
                if (u.VerificarAcceso(user, pass))
                {
                    accesoValido = true;
                    MessageBox.Show($"Bienvenido {u.NombreUsuario}", "Acceso correcto");

                    // Abrir el formulario principal
                    this.Hide();
                    MainForm main = new MainForm(u);
                    main.Show();
                    break;
                }
            }

            if (!accesoValido)
            {
                lblMensaje.Text = "Usuario o contraseña incorrectos.";
            }
        }

    }
}
