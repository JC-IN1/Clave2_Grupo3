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
    public partial class UsuariosForm : Form
    {
        private List<Usuario> listaUsuarios = new List<Usuario>();
        private int siguienteId = 1;
        public UsuariosForm()
        {
            InitializeComponent();
        }

        private void UsuariosForm_Load(object sender, EventArgs e)
        {
            cmbRol.Items.AddRange(new string[] { "Administrador", "Operador" });
            ActualizarGrid();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Debe ingresar usuario y contraseña.");
                return;
            }

            Usuario nuevo = new Usuario
            {
                Id = siguienteId++,
                NombreUsuario = txtUsuario.Text,
                Contrasena = txtContrasena.Text,
                Rol = cmbRol.Text
            };

            listaUsuarios.Add(nuevo);
            ActualizarGrid();
            LimpiarCampos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione un usuario para modificar.");
                return;
            }

            int id = int.Parse(txtId.Text);
            var usuario = listaUsuarios.FirstOrDefault(u => u.Id == id);

            if (usuario != null)
            {
                usuario.NombreUsuario = txtUsuario.Text;
                usuario.Contrasena = txtContrasena.Text;
                usuario.Rol = cmbRol.Text;

                ActualizarGrid();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione un usuario para eliminar.");
                return;
            }

            int id = int.Parse(txtId.Text);
            listaUsuarios.RemoveAll(u => u.Id == id);
            ActualizarGrid();
            LimpiarCampos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string buscar = txtUsuario.Text.Trim();
            var filtrados = listaUsuarios.Where(u => u.NombreUsuario.ToLower().Contains(buscar.ToLower())).ToList();

            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = filtrados;
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtId.Text = dgvUsuarios.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                txtUsuario.Text = dgvUsuarios.Rows[e.RowIndex].Cells["NombreUsuario"].Value.ToString();
                txtContrasena.Text = dgvUsuarios.Rows[e.RowIndex].Cells["Contrasena"].Value.ToString();
                cmbRol.Text = dgvUsuarios.Rows[e.RowIndex].Cells["Rol"].Value.ToString();
            }
        }

        private void ActualizarGrid()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = listaUsuarios;
        }

        private void LimpiarCampos()
        {
            txtId.Clear();
            txtUsuario.Clear();
            txtContrasena.Clear();
            cmbRol.SelectedIndex = -1;
        }
    }
}
