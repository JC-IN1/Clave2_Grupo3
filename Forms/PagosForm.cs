using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Clave2_Grupo3.Conexion;
using Clave2_Grupo3.Models;
using MySql.Data.MySqlClient;

namespace Clave2_Grupo3.Forms
{
    public partial class PagosForm : Form
    {
        private Usuario usuarioActual;
        public PagosForm()
        {
            InitializeComponent();
        }

        public PagosForm(Usuario usuario)
        {
            InitializeComponent();
            usuarioActual = usuario;
        }
        private void PagosForm_Load(object sender, EventArgs e)
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    CargarReservas(); 
                    cmbReserva.SelectedIndexChanged += cmbReserva_SelectedIndexChanged;
                }

                // Métodos y estados de pago
                cmbMetodo.Items.Clear();
                cmbMetodo.Items.AddRange(new string[] { "Tarjeta", "Efectivo", "Transferencia" });
                cmbEstado.Items.Clear();
                cmbEstado.Items.AddRange(new string[] { "Pendiente", "Pagado", "Cancelado" });
                cmbEstado.SelectedIndex = 0;

                CargarPagosDesdeBD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pagos: " + ex.Message);
            }
        }
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    string query = @"
                INSERT INTO pagos (reserva_id, fecha_pago, monto, metodo, estado_pago)
                VALUES (@reserva, @fecha, @monto, @metodo, @estado)
            ";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@reserva", cmbReserva.SelectedValue);
                    cmd.Parameters.AddWithValue("@fecha", dtpFecha.Value);
                    cmd.Parameters.AddWithValue("@monto", decimal.Parse(txtMonto.Text));
                    cmd.Parameters.AddWithValue("@metodo", cmbMetodo.Text);
                    cmd.Parameters.AddWithValue("@estado", cmbEstado.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Pago registrado correctamente.");
                }

                CargarPagosDesdeBD();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar pago: " + ex.Message);
            }
        }


        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione un pago para modificar.");
                return;
            }

            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    string query = @"UPDATE pagos 
                             SET reserva_id=@reserva, monto=@monto, metodo=@metodo, estado_pago=@estado
                             WHERE id=@id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));
                    cmd.Parameters.AddWithValue("@reserva", cmbReserva.SelectedValue);
                    cmd.Parameters.AddWithValue("@monto", decimal.Parse(txtMonto.Text));
                    cmd.Parameters.AddWithValue("@metodo", cmbMetodo.Text);
                    cmd.Parameters.AddWithValue("@estado", cmbEstado.Text);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Pago modificado correctamente.");

                CargarPagosDesdeBD();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar pago: " + ex.Message);
            }
            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione un pago para eliminar.");
                return;
            }

            if (MessageBox.Show("¿Está seguro de eliminar este pago?",
                "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = "DELETE FROM pagos WHERE id=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Pago eliminado correctamente.");

                CargarPagosDesdeBD();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar pago: " + ex.Message);
            }
            LimpiarCampos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    string query = @"SELECT p.id, 
                                    p.reserva_id AS Reserva,
                                    p.monto, 
                                    p.metodo, 
                                    p.estado_pago,
                                    p.fecha_pago
                             FROM pagos p
                             WHERE 1 = 1"; // base para concatenar filtros dinámicos

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;

                        // ------ Filtros dinámicos ------
                        if (!string.IsNullOrWhiteSpace(txtMonto.Text))
                        {
                            // permitimos buscar por parte del monto (ej: "12" encuentra 12.00, 120.00, etc.)
                            query += " AND p.monto LIKE @monto";
                            cmd.Parameters.AddWithValue("@monto", "%" + txtMonto.Text.Trim() + "%");
                        }

                        if (cmbMetodo.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(cmbMetodo.Text))
                        {
                            query += " AND p.metodo = @metodo";
                            cmd.Parameters.AddWithValue("@metodo", cmbMetodo.Text);
                        }

                        if (cmbEstado.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(cmbEstado.Text))
                        {
                            query += " AND p.estado_pago = @estado";
                            cmd.Parameters.AddWithValue("@estado", cmbEstado.Text);
                        }

                        if (cmbReserva.SelectedIndex != -1 && cmbReserva.SelectedValue != null)
                        {
                            // si el usuario seleccionó una reserva en el combo, filtramos por id exacto
                            query += " AND p.reserva_id = @reservaId";
                            cmd.Parameters.AddWithValue("@reservaId", Convert.ToInt32(cmbReserva.SelectedValue));
                        }

                        cmd.CommandText = query;

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgvPagos.DataSource = dt;

                        // centrar encabezados como hábito
                        foreach (DataGridViewColumn col in dgvPagos.Columns)
                            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar pagos: " + ex.Message);
            }
            LimpiarCampos();
        }

        private void dgvPagos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow fila = dgvPagos.Rows[e.RowIndex];

                // ID del pago (string en textbox)
                object idObj = fila.Cells["id"].Value;
                txtId.Text = idObj?.ToString() ?? "";

                // Monto (seguro)
                txtMonto.Clear();
                if (fila.DataGridView.Columns.Contains("monto"))
                {
                    var montoObj = fila.Cells["monto"].Value;
                    if (montoObj != null && decimal.TryParse(montoObj.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal montoVal))
                    {
                        txtMonto.Text = montoVal.ToString("F2");
                    }
                    else
                    {
                        // intentar con la cultura actual como fallback
                        if (montoObj != null && decimal.TryParse(montoObj.ToString(), out montoVal))
                            txtMonto.Text = montoVal.ToString("F2");
                    }
                }

                // Método y estado (texto directo, sin conversiones)
                if (fila.DataGridView.Columns.Contains("metodo"))
                    cmbMetodo.Text = fila.Cells["metodo"].Value?.ToString() ?? "";

                if (fila.DataGridView.Columns.Contains("estado_pago"))
                    cmbEstado.Text = fila.Cells["estado_pago"].Value?.ToString() ?? "";

                // Reserva: usamos la columna técnica 'reserva_id' para SelectedValue
                if (fila.DataGridView.Columns.Contains("reserva_id"))
                {
                    var resObj = fila.Cells["reserva_id"].Value;
                    if (resObj != null && int.TryParse(resObj.ToString(), out int reservaId))
                    {
                        cmbReserva.SelectedValue = reservaId;
                    }
                    else
                    {
                        // si no podemos parsear, deseleccionamos
                        cmbReserva.SelectedIndex = -1;
                    }
                }
                else
                {
                    // si por alguna razón no existe la columna reserva_id, intentamos extraer de la columna Reserva
                    if (fila.DataGridView.Columns.Contains("Reserva"))
                    {
                        cmbReserva.SelectedIndex = -1; // no podemos mapear
                    }
                }

                // Fecha pago (segura)
                if (fila.DataGridView.Columns.Contains("fecha_pago"))
                {
                    var fechaObj = fila.Cells["fecha_pago"].Value;
                    if (fechaObj != null && DateTime.TryParse(fechaObj.ToString(), out DateTime fechaVal))
                    {
                        dtpFecha.Value = fechaVal;
                    }
                    else
                    {
                        dtpFecha.Value = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el pago seleccionado: " + ex.Message);
            }
        }

        private void CargarPagosDesdeBD()
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    string query = @"
                SELECT p.id,
                       p.reserva_id AS reserva_id,
                       CONCAT(rt.origen, ' → ', rt.destino, ' | ', v.fecha_salida) AS Reserva,
                       p.fecha_pago,
                       p.monto,
                       p.metodo,
                       p.estado_pago,
                       r.pasajero_id
                FROM pagos p
                INNER JOIN reservas r ON p.reserva_id = r.id
                INNER JOIN vuelos v ON r.vuelo_id = v.id
                INNER JOIN rutas rt ON v.ruta_id = rt.id
            ";

                    if (usuarioActual != null && usuarioActual.Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
                    {
                        query += " WHERE r.pasajero_id = @pid";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (usuarioActual != null && usuarioActual.Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
                            cmd.Parameters.AddWithValue("@pid", usuarioActual.Id);

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgvPagos.AutoGenerateColumns = true;
                        dgvPagos.DataSource = dt;

                        // Encabezados (si existen)
                        if (dgvPagos.Columns.Contains("id")) dgvPagos.Columns["id"].HeaderText = "ID";
                        if (dgvPagos.Columns.Contains("Reserva")) dgvPagos.Columns["Reserva"].HeaderText = "Reserva";
                        if (dgvPagos.Columns.Contains("fecha_pago")) dgvPagos.Columns["fecha_pago"].HeaderText = "Fecha Pago";
                        if (dgvPagos.Columns.Contains("monto")) dgvPagos.Columns["monto"].HeaderText = "Monto ($)";
                        if (dgvPagos.Columns.Contains("metodo")) dgvPagos.Columns["metodo"].HeaderText = "Método";
                        if (dgvPagos.Columns.Contains("estado_pago")) dgvPagos.Columns["estado_pago"].HeaderText = "Estado";

                        // ocultar columnas técnicas que no queremos mostrar
                        if (dgvPagos.Columns.Contains("reserva_id")) dgvPagos.Columns["reserva_id"].Visible = false;
                        if (dgvPagos.Columns.Contains("pasajero_id")) dgvPagos.Columns["pasajero_id"].Visible = false;

                        // centrar encabezados
                        foreach (DataGridViewColumn col in dgvPagos.Columns)
                            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pagos: " + ex.Message);
            }
        }


        private void CargarReservas()
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    string query = @"
                SELECT r.id,
                       CONCAT(rt.origen, ' → ', rt.destino, ' | ', v.fecha_salida) AS descripcion,
                       r.precio_total
                FROM reservas r
                INNER JOIN vuelos v ON r.vuelo_id = v.id
                INNER JOIN rutas rt ON v.ruta_id = rt.id;
            ";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbReserva.DataSource = dt;
                    cmbReserva.DisplayMember = "descripcion";   // Lo que se muestra
                    cmbReserva.ValueMember = "id";             // ID real de la reserva
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar reservas: " + ex.Message);
            }
        }
        private void cmbReserva_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbReserva.SelectedValue == null) return;

            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    string query = "SELECT precio_total FROM reservas WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", cmbReserva.SelectedValue);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        txtMonto.Text = Convert.ToDecimal(result).ToString("F2");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el monto de la reserva: " + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            txtId.Clear();
            txtMonto.Clear();
            cmbMetodo.SelectedIndex = -1;
            cmbEstado.SelectedIndex = -1;
            cmbReserva.SelectedIndex = -1;
        }
    }
}

