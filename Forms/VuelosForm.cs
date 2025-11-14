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
using MySql.Data.MySqlClient;
using Clave2_Grupo3.Conexion;

namespace Clave2_Grupo3.Forms
{
    public partial class VuelosForm : Form
    {
        public VuelosForm()
        {
            InitializeComponent();
        }
        private Usuario usuarioActual;
        public VuelosForm(Usuario usuario)
        {
            InitializeComponent();
            usuarioActual = usuario;
        }
        private void VuelosForm_Load(object sender, EventArgs e)
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    // Cargar Aerolíneas
                    MySqlDataAdapter daAerolineas = new MySqlDataAdapter("SELECT id, nombre FROM aerolineas", conn);
                    DataTable dtAerolineas = new DataTable();
                    daAerolineas.Fill(dtAerolineas);
                    cmbAerolinea.DataSource = dtAerolineas;
                    cmbAerolinea.DisplayMember = "nombre";
                    cmbAerolinea.ValueMember = "id";

                    // Cargar Aviones
                    MySqlDataAdapter daAviones = new MySqlDataAdapter("SELECT id, modelo FROM aviones", conn);
                    DataTable dtAviones = new DataTable();
                    daAviones.Fill(dtAviones);
                    cmbAvion.DataSource = dtAviones;
                    cmbAvion.DisplayMember = "modelo";
                    cmbAvion.ValueMember = "id";

                    // Cargar Rutas
                    MySqlDataAdapter daRutas = new MySqlDataAdapter("SELECT id, CONCAT(origen, ' → ', destino) AS descripcion FROM rutas", conn);
                    DataTable dtRutas = new DataTable();
                    daRutas.Fill(dtRutas);
                    cmbRuta.DataSource = dtRutas;
                    cmbRuta.DisplayMember = "descripcion";
                    cmbRuta.ValueMember = "id";
                }

                // Finalmente cargar los vuelos
                CargarCombosDesdeBD();
                CargarVuelosDesdeBD();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos del formulario de vuelos: " + ex.Message);
            }
        }

        //Metodo para el boton Agregar
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = @"INSERT INTO vuelos (aerolinea_id, avion_id, ruta_id, fecha_salida, fecha_llegada, tarifa_base, asientos_disponibles)
                             VALUES (@aerolinea, @avion, @ruta, @salida, @llegada, @tarifa, @asientos)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@aerolinea", cmbAerolinea.SelectedValue);
                    cmd.Parameters.AddWithValue("@avion", cmbAvion.SelectedValue);
                    cmd.Parameters.AddWithValue("@ruta", cmbRuta.SelectedValue);
                    cmd.Parameters.AddWithValue("@salida", dtpSalida.Value);
                    cmd.Parameters.AddWithValue("@llegada", dtpLlegada.Value);
                    cmd.Parameters.AddWithValue("@tarifa", decimal.Parse(txtTarifa.Text));
                    cmd.Parameters.AddWithValue("@asientos", int.Parse(txtAsientos.Text));

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Vuelo agregado correctamente.");
                }

                CargarVuelosDesdeBD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar vuelo: " + ex.Message);
            }
            LimpiarCampos();
        }

        //Metodo para el boton Moificar
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione un vuelo para modificar.");
                return;
            }

            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = @"UPDATE vuelos 
                             SET aerolinea_id=@aerolinea, avion_id=@avion, ruta_id=@ruta, 
                                 fecha_salida=@salida, fecha_llegada=@llegada, 
                                 tarifa_base=@tarifa, asientos_disponibles=@asientos
                             WHERE id=@id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@aerolinea", cmbAerolinea.SelectedValue);
                    cmd.Parameters.AddWithValue("@avion", cmbAvion.SelectedValue);
                    cmd.Parameters.AddWithValue("@ruta", cmbRuta.SelectedValue);
                    cmd.Parameters.AddWithValue("@salida", dtpSalida.Value);
                    cmd.Parameters.AddWithValue("@llegada", dtpLlegada.Value);
                    cmd.Parameters.AddWithValue("@tarifa", decimal.Parse(txtTarifa.Text));
                    cmd.Parameters.AddWithValue("@asientos", int.Parse(txtAsientos.Text));
                    cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Vuelo modificado correctamente.");
                }

                CargarVuelosDesdeBD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar vuelo: " + ex.Message);
            }
            LimpiarCampos();
        }

        //Metodo para el boton eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione un vuelo para eliminar.");
                return;
            }

            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = "DELETE FROM vuelos WHERE id=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Vuelo eliminado correctamente.");
                }

                CargarVuelosDesdeBD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar vuelo: " + ex.Message);
            }
            LimpiarCampos();
        }

        //Metodo para el boton buscar
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = cmbAerolinea.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                CargarVuelosDesdeBD();
                return;
            }

            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    string query = @"SELECT v.id, 
                                    a.nombre AS Aerolinea, 
                                    av.modelo AS Avion, 
                                    CONCAT(r.origen, ' → ', r.destino) AS Ruta,
                                    v.fecha_salida, v.fecha_llegada,
                                    v.tarifa_base, v.asientos_disponibles
                             FROM vuelos v
                             INNER JOIN aerolineas a ON v.aerolinea_id = a.id
                             INNER JOIN aviones av ON v.avion_id = av.id
                             INNER JOIN rutas r ON v.ruta_id = r.id
                             WHERE a.nombre LIKE @filtro 
                                OR av.modelo LIKE @filtro
                                OR r.origen LIKE @filtro 
                                OR r.destino LIKE @filtro";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvVuelos.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar vuelos: " + ex.Message);
            }
            LimpiarCampos();
        }


        private void dgvVuelos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Evita errores al hacer clic en encabezados

            try
            {
                DataGridViewRow fila = dgvVuelos.Rows[e.RowIndex];

                txtId.Text = fila.Cells["id"].Value?.ToString() ?? "";

                // Verifica si la columna existe antes de acceder
                if (fila.DataGridView.Columns.Contains("aerolinea_id"))
                    cmbAerolinea.SelectedValue = Convert.ToInt32(fila.Cells["aerolinea_id"].Value);

                if (fila.DataGridView.Columns.Contains("avion_id"))
                    cmbAvion.SelectedValue = Convert.ToInt32(fila.Cells["avion_id"].Value);

                if (fila.DataGridView.Columns.Contains("ruta_id"))
                    cmbRuta.SelectedValue = Convert.ToInt32(fila.Cells["ruta_id"].Value);

                dtpSalida.Value = Convert.ToDateTime(fila.Cells["fecha_salida"].Value);
                dtpLlegada.Value = Convert.ToDateTime(fila.Cells["fecha_llegada"].Value);
                txtTarifa.Text = fila.Cells["tarifa_base"].Value?.ToString() ?? "";
                txtAsientos.Text = fila.Cells["asientos_disponibles"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos del vuelo seleccionado: " + ex.Message);
            }
        }

        //Cargar combos desde la base de datos
        private void CargarCombosDesdeBD()
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    //  Aerolíneas
                    MySqlDataAdapter daAero = new MySqlDataAdapter("SELECT id, nombre FROM aerolineas", conn);
                    DataTable dtAero = new DataTable();
                    daAero.Fill(dtAero);
                    cmbAerolinea.DataSource = dtAero;
                    cmbAerolinea.DisplayMember = "nombre";
                    cmbAerolinea.ValueMember = "id";

                    // Aviones
                    MySqlDataAdapter daAvion = new MySqlDataAdapter("SELECT id, modelo FROM aviones", conn);
                    DataTable dtAvion = new DataTable();
                    daAvion.Fill(dtAvion);
                    cmbAvion.DataSource = dtAvion;
                    cmbAvion.DisplayMember = "modelo";
                    cmbAvion.ValueMember = "id";

                    //  Rutas
                    MySqlDataAdapter daRuta = new MySqlDataAdapter("SELECT id, CONCAT(origen, ' → ', destino) AS descripcion FROM rutas", conn);
                    DataTable dtRuta = new DataTable();
                    daRuta.Fill(dtRuta);
                    cmbRuta.DataSource = dtRuta;
                    cmbRuta.DisplayMember = "descripcion";
                    cmbRuta.ValueMember = "id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en los combos: " + ex.Message);
            }
        }

        //Cargar Vuelos desde la base da datos
        private void CargarVuelosDesdeBD()
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = @"SELECT v.id, 
                                    v.aerolinea_id, 
                                    v.avion_id, 
                                    v.ruta_id,
                                    v.fecha_salida,
                                    v.fecha_llegada,
                                    v.tarifa_base,
                                    v.asientos_disponibles
                             FROM vuelos v";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvVuelos.AutoGenerateColumns = true;
                    dgvVuelos.DataSource = dt;

                    // Encabezados 
                    dgvVuelos.Columns["id"].HeaderText = "ID";
                    dgvVuelos.Columns["aerolinea_id"].HeaderText = "Aerolínea ID";
                    dgvVuelos.Columns["avion_id"].HeaderText = "Avión ID";
                    dgvVuelos.Columns["ruta_id"].HeaderText = "Ruta ID";
                    dgvVuelos.Columns["fecha_salida"].HeaderText = "Fecha Salida";
                    dgvVuelos.Columns["fecha_llegada"].HeaderText = "Fecha Llegada";
                    dgvVuelos.Columns["tarifa_base"].HeaderText = "Tarifa Base ($)";
                    dgvVuelos.Columns["asientos_disponibles"].HeaderText = "Asientos";

                    foreach (DataGridViewColumn col in dgvVuelos.Columns)
                    {
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar vuelos: " + ex.Message);
            }
        }

        //Limpiar Campos
        private void LimpiarCampos()
        {
            txtId.Clear();
            txtTarifa.Clear();
            txtAsientos.Clear();
            cmbAerolinea.SelectedIndex = -1;
            cmbAvion.SelectedIndex = -1;
            cmbRuta.SelectedIndex = -1;
            dtpSalida.Value = DateTime.Now;
            dtpLlegada.Value = DateTime.Now;
        }

    }
}
