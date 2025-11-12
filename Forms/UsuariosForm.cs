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
            cmbRol.Items.AddRange(new string[] { "Administrador", "Operador", "Cliente" });

            // Asegurar que el DataGridView permita seleccionar fila completa
            dgvUsuarios.AutoGenerateColumns = true;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.RowHeadersVisible = false;

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
            MessageBox.Show($"txtId='{txtId.Text}' selectedRows={dgvUsuarios.SelectedRows.Count} rows={dgvUsuarios.Rows.Count}");

            try
            {
                // Si no hay Id en txtId, intentar tomarlo de la selección actual
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    if (dgvUsuarios.SelectedRows.Count > 0)
                    {
                        var filaSel = dgvUsuarios.SelectedRows[0];
                        txtId.Text = filaSel.Cells["Id"].Value?.ToString() ?? "";
                        txtUsuario.Text = filaSel.Cells["NombreUsuario"].Value?.ToString() ?? "";
                        txtContrasena.Text = filaSel.Cells["Contrasena"].Value?.ToString() ?? "";
                        cmbRol.Text = filaSel.Cells["Rol"].Value?.ToString() ?? "";
                    }
                    else
                    {
                        MessageBox.Show("Seleccione un usuario de la tabla para modificar.", "Atención");
                        return;
                    }
                }

                if (!int.TryParse(txtId.Text, out int id))
                {
                    MessageBox.Show("Id inválido.", "Error");
                    return;
                }

                var usuario = listaUsuarios.FirstOrDefault(u => u.Id == id);
                if (usuario == null)
                {
                    MessageBox.Show("Usuario no encontrado.", "Error");
                    return;
                }

                // Validaciones de campos
                if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text))
                {
                    MessageBox.Show("Complete usuario y contraseña.", "Atención");
                    return;
                }

                // Actualizar objeto
                usuario.NombreUsuario = txtUsuario.Text.Trim();
                usuario.Contrasena = txtContrasena.Text.Trim();
                usuario.Rol = cmbRol.Text.Trim();

                // Refrescar grid y limpiar
                ActualizarGrid();
                LimpiarCampos();

                MessageBox.Show("Usuario modificado correctamente.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar usuario: " + ex.Message);
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
            // Evitar encabezado o índices inválidos
            if (dgvUsuarios.Rows.Count == 0 || e.RowIndex < 0) return;

            try
            {
                var fila = dgvUsuarios.Rows[e.RowIndex];

                // Usar safe access y ToString() condicional para evitar NullReference
                txtId.Text = fila.Cells["Id"].Value?.ToString() ?? "";
                txtUsuario.Text = fila.Cells["Nombre del Usuario"].Value?.ToString() ?? "";
                txtContrasena.Text = fila.Cells["Contraseña"].Value?.ToString() ?? "";
                cmbRol.Text = fila.Cells["Rol"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar fila seleccionada: " + ex.Message);
            }
        }
        private void ActualizarGrid()
        {
            try
            {
                dgvUsuarios.CellClick -= dgvUsuarios_CellClick;

                dgvUsuarios.AutoGenerateColumns = false; 
                dgvUsuarios.DataSource = null;

                if (listaUsuarios == null)
                    listaUsuarios = new List<Usuario>();

                dgvUsuarios.DataSource = listaUsuarios;
                dgvUsuarios.AutoGenerateColumns = true;

                dgvUsuarios.ClearSelection();
                txtId.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la lista de usuarios: " + ex.Message);
            }
            finally
            {
                dgvUsuarios.CellClick += dgvUsuarios_CellClick;
            }
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
