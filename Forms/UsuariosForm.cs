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
using MySql.Data.MySqlClient;
using Clave2_Grupo3.Conexion;

namespace Clave2_Grupo3.Forms
{
    public partial class UsuariosForm : Form
    {
        public UsuariosForm()
        {
            InitializeComponent();
        }
        private Usuario usuarioActual;
        public UsuariosForm(Usuario usuario)
        {
            InitializeComponent();
            usuarioActual = usuario;
        }
        private void UsuariosForm_Load(object sender, EventArgs e)
        {
            cmbRol.Items.AddRange(new string[] { "Administrador", "Operador", "Cliente" });

            // Asegurar que el DataGridView permita seleccionar fila completa
            dgvUsuarios.AutoGenerateColumns = true;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.RowHeadersVisible = false;

            CargarUsuariosDesdeBD();
        }

        //Metodo para boton Agregar
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Debe ingresar usuario y contraseña.");
                return;
            }

            try
            {
                    ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = "INSERT INTO usuarios (nombre_usuario, contrasena, rol) VALUES (@nombre, @contrasena, @rol)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@contrasena", txtContrasena.Text);
                    cmd.Parameters.AddWithValue("@rol", cmbRol.Text);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Usuario agregado correctamente");
                CargarUsuariosDesdeBD();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar usuario: " + ex.Message);
            }
        }

        //Metodo para boton Modificar
        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Si no hay texto en txtId, intentar obtener el seleccionado del grid
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                if (dgvUsuarios.CurrentRow != null)
                {
                    txtId.Text = dgvUsuarios.CurrentRow.Cells["id"].Value?.ToString() ?? "";
                    txtUsuario.Text = dgvUsuarios.CurrentRow.Cells["nombre_usuario"].Value?.ToString() ?? "";
                    txtContrasena.Text = dgvUsuarios.CurrentRow.Cells["contrasena"].Value?.ToString() ?? "";
                    cmbRol.Text = dgvUsuarios.CurrentRow.Cells["rol"].Value?.ToString() ?? "";
                }
                else
                {
                    MessageBox.Show("Seleccione un usuario antes de modificar.");
                    return;
                }
            }

            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = "UPDATE usuarios SET nombre_usuario=@nombre, contrasena=@contrasena, rol=@rol WHERE id=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@contrasena", txtContrasena.Text);
                    cmd.Parameters.AddWithValue("@rol", cmbRol.Text);
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Usuario modificado correctamente");
                CargarUsuariosDesdeBD();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar usuario: " + ex.Message);
            }
        }

        //Metodo para botno ELiminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione un usuario para eliminar.");
                return;
            }

            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = "DELETE FROM usuarios WHERE id=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Usuario eliminado correctamente");
                CargarUsuariosDesdeBD();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar usuario: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtUsuario.Text.Trim();

            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    string query = "SELECT * FROM usuarios WHERE nombre_usuario LIKE @filtro";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvUsuarios.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar usuario: " + ex.Message);
            }
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evitar errores si se hace clic en encabezado
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow fila = dgvUsuarios.Rows[e.RowIndex];

                txtId.Text = fila.Cells["id"].Value?.ToString() ?? "";
                txtUsuario.Text = fila.Cells["nombre_usuario"].Value?.ToString() ?? "";
                txtContrasena.Text = fila.Cells["contrasena"].Value?.ToString() ?? "";
                cmbRol.Text = fila.Cells["rol"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar fila seleccionada: " + ex.Message);
            }
        }


        //Cargar usuarios desde la base de datos
        private void CargarUsuariosDesdeBD()
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT * FROM usuarios";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvUsuarios.AutoGenerateColumns = true;
                    dgvUsuarios.DataSource = dt;
                }

                // Cambiar encabezados 
                if (dgvUsuarios.Columns.Contains("id"))
                    dgvUsuarios.Columns["id"].HeaderText = "ID";

                if (dgvUsuarios.Columns.Contains("nombre_usuario"))
                    dgvUsuarios.Columns["nombre_usuario"].HeaderText = "Nombre del Usuario";

                if (dgvUsuarios.Columns.Contains("contrasena"))
                    dgvUsuarios.Columns["contrasena"].HeaderText = "Contraseña";

                if (dgvUsuarios.Columns.Contains("rol"))
                    dgvUsuarios.Columns["rol"].HeaderText = "Rol";

                // Centrar los encabezados
                foreach (DataGridViewColumn col in dgvUsuarios.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
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
