using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Clave2_Grupo3.Models;
using Clave2_Grupo3.Conexion;

namespace Clave2_Grupo3.Forms
{
    public partial class ReservasForm : Form
    {
        public ReservasForm()
        {
            InitializeComponent();
        }
        private Usuario usuarioActual;

        public ReservasForm(Usuario usuario)
        {
            InitializeComponent();
            usuarioActual = usuario;
        }

        private void ReservasForm_Load(object sender, EventArgs e)
        {
            try
            {
                CargarCombosDesdeBD();
                CargarReservasDesdeBD();
                CargarPasajerosDesdeUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar formulario: " + ex.Message);
            }
        }

        // 🔹 Cargar combos desde la base
        private void CargarCombosDesdeBD()
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    // 🔹 Cargar vuelos
                    MySqlDataAdapter daVuelos = new MySqlDataAdapter(
                        "SELECT id, CONCAT('Vuelo ', id) AS descripcion FROM vuelos", conn);
                    DataTable dtVuelos = new DataTable();
                    daVuelos.Fill(dtVuelos);
                    cmbVuelo.DataSource = dtVuelos;
                    cmbVuelo.DisplayMember = "descripcion";
                    cmbVuelo.ValueMember = "id";

                    CargarPasajerosDesdeUsuarios();


                    cmbEstado.Items.Clear();
                    cmbEstado.Items.AddRange(new string[] { "Activa", "Pagada", "Cancelada" });
                    cmbEstado.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar combos: " + ex.Message);
            }
        }
        private void CargarPasajerosDesdeUsuarios()
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    string query = @"SELECT id, nombre_usuario 
                             FROM usuarios 
                             WHERE rol = 'Cliente'";

                    // Si es cliente, solo cargar SU propio registro
                    if (usuarioActual != null &&
                        usuarioActual.Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
                    {
                        query += " AND nombre_usuario = @nombre";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (usuarioActual != null &&
                        usuarioActual.Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
                    {
                        cmd.Parameters.AddWithValue("@nombre", usuarioActual.NombreUsuario);
                    }

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbPasajero.DataSource = dt;
                    cmbPasajero.DisplayMember = "nombre_usuario";  // lo que se muestra
                    cmbPasajero.ValueMember = "id";                // id del usuario
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pasajeros desde usuarios: " + ex.Message);
            }
        }

        private void btnCalcular_Click(object sender, EventArgs e) //Metodo para boton Calcular Precio 
        {
            if (cmbVuelo.SelectedValue == null) //Validacion de entrada de datos
            {
                MessageBox.Show("Seleccione un vuelo para calcular el precio.");
                return;
            }

            try
            {
                int vueloId = Convert.ToInt32(cmbVuelo.SelectedValue);
                decimal tarifaBase = 0m;

                // Obtener tarifa_base desde la BD
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = @"SELECT v.tarifa_base, 
                                    a.nombre AS Aerolinea,
                                    CONCAT(r.origen, ' → ', r.destino) AS Ruta,
                                    av.modelo AS Avion
                             FROM vuelos v
                             INNER JOIN aerolineas a ON v.aerolinea_id = a.id
                             INNER JOIN aviones av ON v.avion_id = av.id
                             INNER JOIN rutas r ON v.ruta_id = r.id
                             WHERE v.id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", vueloId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tarifaBase = Convert.ToDecimal(reader["tarifa_base"]);
                                lblDetalleVuelo.Text = $"{reader["Aerolinea"]} | {reader["Ruta"]} | {reader["Avion"]}";
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el vuelo seleccionado.");
                                return;
                            }
                        }
                    }
                }

                // 🔹 Calcular recargos según equipaje
                decimal total = tarifaBase;
                string detalleEquipaje = "";

                if (chkEquipajeMano.Checked)
                {
                    total += tarifaBase * 0.10m; // +10%
                    detalleEquipaje += "Con equipaje de mano (+10%)";
                }

                if (chkEquipajeBodega.Checked)
                {
                    total += tarifaBase * 0.20m; // +20%
                    detalleEquipaje += (detalleEquipaje != "" ? " y " : "") + "con equipaje en bodega (+20%)";
                }

                // 🔹 Mostrar resultado
                txtPrecio.Text = total.ToString("F2");

                MessageBox.Show(
                    $"Vuelo seleccionado: {lblDetalleVuelo.Text}\n" +
                    $"{detalleEquipaje}\n" +
                    $"Precio total: ${total:F2}",
                    "Resumen de Reserva",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al calcular precio: " + ex.Message);
            }
        }
        private void CargarReservasDesdeBD()
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    string query = @"
                SELECT r.id,
                       u.nombre_usuario AS Pasajero,
                       CONCAT('Vuelo ', v.id) AS Vuelo,
                       r.fecha_reserva,
                       r.precio_total,
                       r.estado,
                       r.equipaje_mano,
                       r.equipaje_bodega,
                       r.preferencia_asiento
                FROM reservas r
                INNER JOIN usuarios u ON r.pasajero_id = u.id
                INNER JOIN vuelos v ON r.vuelo_id = v.id
            ";

                    // Si es cliente, mostrar solo sus reservas
                    if (usuarioActual != null &&
                        usuarioActual.Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
                    {
                        query += " WHERE u.nombre_usuario = @cliente";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (usuarioActual != null &&
                        usuarioActual.Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
                    {
                        cmd.Parameters.AddWithValue("@cliente", usuarioActual.NombreUsuario);
                    }

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReservas.DataSource = dt;

                    // Alinear encabezados
                    foreach (DataGridViewColumn col in dgvReservas.Columns)
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar reservas: " + ex.Message);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();

                    int vueloId = Convert.ToInt32(cmbVuelo.SelectedValue);
                    int pasajeroId = 0;

                    // 🧩 Verificar asientos disponibles
                    int asientosDisponibles = 0;
                    string queryAsientos = "SELECT asientos_disponibles FROM vuelos WHERE id = @id";
                    using (MySqlCommand cmdAsientos = new MySqlCommand(queryAsientos, conn))
                    {
                        cmdAsientos.Parameters.AddWithValue("@id", vueloId);
                        object result = cmdAsientos.ExecuteScalar();
                        if (result != null)
                            asientosDisponibles = Convert.ToInt32(result);
                    }

                    if (asientosDisponibles <= 0)
                    {
                        MessageBox.Show("❌ No hay asientos disponibles para este vuelo.",
                            "Sin cupo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Si el usuario es CLIENTE, buscar o crear su pasajero
                    if (usuarioActual != null && usuarioActual.Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
                    {
                        string queryPasajero = "SELECT id FROM pasajeros WHERE nombre = @nombre LIMIT 1";
                        using (MySqlCommand cmdP = new MySqlCommand(queryPasajero, conn))
                        {
                            cmdP.Parameters.AddWithValue("@nombre", usuarioActual.NombreUsuario);
                            object idEncontrado = cmdP.ExecuteScalar();

                            if (idEncontrado != null)
                            {
                                pasajeroId = Convert.ToInt32(idEncontrado);
                            }
                            else
                            {
                                // Crear nuevo pasajero si no existe
                                string insertPasajero = "INSERT INTO pasajeros (nombre) VALUES (@nombre)";
                                using (MySqlCommand cmdInsert = new MySqlCommand(insertPasajero, conn))
                                {
                                    cmdInsert.Parameters.AddWithValue("@nombre", usuarioActual.NombreUsuario);
                                    cmdInsert.ExecuteNonQuery();
                                    pasajeroId = (int)cmdInsert.LastInsertedId;
                                }
                            }
                        }
                    }
                    else
                    {
                        pasajeroId = Convert.ToInt32(cmbPasajero.SelectedValue);
                    }

                    // 🧮 Validar precio antes de guardar
                    if (string.IsNullOrWhiteSpace(txtPrecio.Text))
                    {
                        MessageBox.Show("Debe calcular el precio antes de guardar la reserva.");
                        return;
                    }

                    decimal precio = decimal.Parse(txtPrecio.Text);

                    // 💾 Insertar reserva
                    string queryReserva = @"INSERT INTO reservas 
                (vuelo_id, pasajero_id, fecha_reserva, precio_total, estado, equipaje_mano, equipaje_bodega, preferencia_asiento)
                VALUES (@vuelo, @pasajero, NOW(), @precio, @estado, @mano, @bodega, @asiento)";

                    using (MySqlCommand cmdReserva = new MySqlCommand(queryReserva, conn))
                    {
                        cmdReserva.Parameters.AddWithValue("@vuelo", vueloId);
                        cmdReserva.Parameters.AddWithValue("@pasajero", pasajeroId);
                        cmdReserva.Parameters.AddWithValue("@precio", precio);
                        cmdReserva.Parameters.AddWithValue("@estado", cmbEstado.Text);
                        cmdReserva.Parameters.AddWithValue("@mano", chkEquipajeMano.Checked);
                        cmdReserva.Parameters.AddWithValue("@bodega", chkEquipajeBodega.Checked);
                        cmdReserva.Parameters.AddWithValue("@asiento", txtAsiento.Text);

                        cmdReserva.ExecuteNonQuery();
                    }

                    // 🔄 Restar un asiento disponible
                    string queryActualizar = "UPDATE vuelos SET asientos_disponibles = asientos_disponibles - 1 WHERE id = @id";
                    using (MySqlCommand cmdUpdate = new MySqlCommand(queryActualizar, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@id", vueloId);
                        cmdUpdate.ExecuteNonQuery();
                    }

                    MessageBox.Show("✅ Reserva agregada correctamente. Asiento confirmado.",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // 🔁 Refrescar datos
                CargarReservasDesdeBD();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la reserva: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LimpiarCampos();
        }


        // 🔹 Modificar
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione una reserva para modificar.");
                return;
            }

            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = @"UPDATE reservas SET 
                                        vuelo_id=@vuelo, pasajero_id=@pasajero, 
                                        precio_total=@precio, estado=@estado, 
                                        equipaje_mano=@mano, equipaje_bodega=@bodega, 
                                        preferencia_asiento=@asiento
                                     WHERE id=@id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@vuelo", cmbVuelo.SelectedValue);
                    cmd.Parameters.AddWithValue("@pasajero", cmbPasajero.SelectedValue);
                    cmd.Parameters.AddWithValue("@precio", decimal.Parse(txtPrecio.Text));
                    cmd.Parameters.AddWithValue("@estado", cmbEstado.Text);
                    cmd.Parameters.AddWithValue("@mano", chkEquipajeMano.Checked);
                    cmd.Parameters.AddWithValue("@bodega", chkEquipajeBodega.Checked);
                    cmd.Parameters.AddWithValue("@asiento", txtAsiento.Text);
                    cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Reserva modificada correctamente.");
                }

                CargarReservasDesdeBD();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar reserva: " + ex.Message);
            }
            LimpiarCampos();
        }

        // Eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione una reserva para eliminar.");
                return;
            }

            try
            {
                ConexionBD conexion = new ConexionBD();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    conn.Open();
                    string query = "DELETE FROM reservas WHERE id=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Reserva eliminada correctamente.");
                }

                CargarReservasDesdeBD();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar reserva: " + ex.Message);
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

                    string query = @"
                SELECT r.id,
                       u.nombre_usuario AS Pasajero,
                       CONCAT('Vuelo ', v.id) AS Vuelo,
                       r.fecha_reserva,
                       r.precio_total,
                       r.estado,
                       r.equipaje_mano,
                       r.equipaje_bodega,
                       r.preferencia_asiento
                FROM reservas r
                INNER JOIN usuarios u ON r.pasajero_id = u.id
                INNER JOIN vuelos v ON r.vuelo_id = v.id
                WHERE 1 = 1
            ";

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;

                        // --- filtro por pasajero (nombre parcial) ---
                        if (!string.IsNullOrWhiteSpace(cmbPasajero.Text))
                        {
                            query += " AND u.nombre_usuario LIKE @pasajero";
                            cmd.Parameters.AddWithValue("@pasajero", "%" + cmbPasajero.Text.Trim() + "%");
                        }

                        // --- filtro por estado (si hay una selección en el combo) ---
                        if (cmbEstado.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(cmbEstado.Text))
                        {
                            query += " AND r.estado = @estado";
                            cmd.Parameters.AddWithValue("@estado", cmbEstado.Text.Trim());
                        }

                        cmd.CommandText = query;

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgvReservas.DataSource = dt;

                        // centrar encabezados si quieres
                        foreach (DataGridViewColumn col in dgvReservas.Columns)
                            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar reservas: " + ex.Message);
            }
        }

        private void dgvReservas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                var fila = dgvReservas.Rows[e.RowIndex];

                // ID
                txtId.Text = fila.Cells["id"].Value?.ToString();

                // PASAJERO (por nombre)
                string pasajeroNombre = fila.Cells["Pasajero"].Value?.ToString();
                if (!string.IsNullOrEmpty(pasajeroNombre))
                {
                    cmbPasajero.Text = pasajeroNombre;
                }

                // VUELO ("Vuelo X" → obtener X)
                string vueloTexto = fila.Cells["Vuelo"].Value?.ToString();
                if (!string.IsNullOrEmpty(vueloTexto))
                {
                    // Extraer número del vuelo
                    int vueloId = int.Parse(vueloTexto.Replace("Vuelo ", "").Trim());
                    cmbVuelo.SelectedValue = vueloId;
                }

                // PRECIO
                txtPrecio.Text = fila.Cells["precio_total"].Value?.ToString();

                // ESTADO
                cmbEstado.Text = fila.Cells["estado"].Value?.ToString();

                // EQUIPAJES
                chkEquipajeMano.Checked =
                    fila.Cells["equipaje_mano"].Value != DBNull.Value &&
                    Convert.ToBoolean(fila.Cells["equipaje_mano"].Value);

                chkEquipajeBodega.Checked =
                    fila.Cells["equipaje_bodega"].Value != DBNull.Value &&
                    Convert.ToBoolean(fila.Cells["equipaje_bodega"].Value);

                // ASIENTO
                txtAsiento.Text = fila.Cells["preferencia_asiento"].Value?.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la reserva seleccionada: " + ex.Message);
            }
        }



        // 🔹 Limpiar campos
        private void LimpiarCampos()
        {
            txtId.Clear();
            txtAsiento.Clear();
            txtPrecio.Clear();
            cmbVuelo.SelectedIndex = -1;
            cmbPasajero.SelectedIndex = -1;
            cmbEstado.SelectedIndex = 0;
            chkEquipajeMano.Checked = false;
            chkEquipajeBodega.Checked = false;
        }
    }
}
