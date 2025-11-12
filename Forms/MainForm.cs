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

                // Ocultar todo por defecto
                btnUsuarios.Visible = false;
                btnVuelos.Visible = false;
                btnReservas.Visible = false;
                btnPagos.Visible = false;

                // Mostrar botones según rol
                if (usuarioActual.Rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                {
                    btnUsuarios.Visible = true;
                    btnVuelos.Visible = true;
                    btnReservas.Visible = true;
                    btnPagos.Visible = true;
                }
                else if (usuarioActual.Rol.Equals("Operador", StringComparison.OrdinalIgnoreCase))
                {
                    btnVuelos.Visible = true; // el operador también puede ver vuelos
                    btnReservas.Visible = true;
                    btnPagos.Visible = true;
                }
                else // Cliente
                {
                    btnReservas.Visible = true;
                    btnPagos.Visible = true;
                }
            }
            else
            {
                lblBienvenida.Text = "Bienvenido al sistema";
                btnUsuarios.Visible = false;
                btnVuelos.Visible = false;
                btnReservas.Visible = false;
                btnPagos.Visible = false;
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

        private void btnVuelos_Click(object sender, EventArgs e)
        {
            VuelosForm formVuelos = new VuelosForm();
            formVuelos.ShowDialog();
        }

        private void btnReservas_Click(object sender, EventArgs e)
        {
            ReservasForm formReservas = new ReservasForm(usuarioActual);
            formReservas.ShowDialog();
        }

        private void btnPagos_Click(object sender, EventArgs e)
        {
            PagosForm formPagos = new PagosForm(usuarioActual);
            formPagos.ShowDialog();
        }
    }
}
