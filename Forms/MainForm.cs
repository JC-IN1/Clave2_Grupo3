using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Clave2_Grupo3.Models;

namespace Clave2_Grupo3.Forms
{
    public partial class MainForm : Form
    {
        private Usuario usuarioActual;
        public MainForm(Usuario usuario)
        {
            InitializeComponent();
            usuarioActual = usuario;
        }
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (usuarioActual != null)
            {
                lblBienvenida.Text = $"Bienvenido {usuarioActual.NombreUsuario}";

                // 👇 Se muestra solo si el rol es exactamente "Administrador" (sin importar mayúsculas)
                if (usuarioActual.Rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                {
                    btnUsuarios.Visible = true;
                }
                else
                {
                    btnUsuarios.Visible = false;
                }
            }
            else
            {
                lblBienvenida.Text = "Bienvenido al sistema";
                btnUsuarios.Visible = false;
            }
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            UsuariosForm formUsuarios = new UsuariosForm();
            formUsuarios.ShowDialog();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }
    }
}
