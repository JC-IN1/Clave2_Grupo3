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
    public partial class ReservasForm : Form
    {
        private List<Reserva> listaReservas = new List<Reserva>();
        private List<Vuelo> listaVuelos = new List<Vuelo>();
        private List<Pasajero> listaPasajeros = new List<Pasajero>();
        private int siguienteId = 1;
        private Usuario usuarioActual;

        public ReservasForm()
        {
            InitializeComponent();
        }

        public ReservasForm(Usuario usuario)
        {
            InitializeComponent();
            usuarioActual = usuario;
        }

        private void ReservasForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 🔹 Simular vuelos y pasajeros existentes
                listaVuelos.Add(new Vuelo { Id = 1, TarifaBase = 250, AsientosDisponibles = 30 });
                listaVuelos.Add(new Vuelo { Id = 2, TarifaBase = 400, AsientosDisponibles = 20 });

                listaPasajeros.Add(new Pasajero { Id = 1, Nombre = "Juan Pérez" });
                listaPasajeros.Add(new Pasajero { Id = 2, Nombre = "María López" });

                if (usuarioActual != null && usuarioActual.Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
                {
                    // El cliente solo puede verse a sí mismo
                    var filtrados = listaPasajeros
                        .Where(p => p.Nombre.IndexOf(usuarioActual.NombreUsuario, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                    if (filtrados.Count == 0)
                    {
                        var clientePasajero = new Pasajero
                        {
                            Id = listaPasajeros.Count + 1,
                            Nombre = usuarioActual.NombreUsuario
                        };
                        listaPasajeros.Add(clientePasajero);
                        filtrados.Add(clientePasajero);
                    }

                    cmbPasajero.DataSource = filtrados;
                }
                else
                {
                    // Admin y operador pueden ver todos
                    cmbPasajero.DataSource = listaPasajeros;
                }

                cmbPasajero.DisplayMember = "Nombre";
                cmbPasajero.ValueMember = "Id";

                cmbVuelo.DataSource = listaVuelos;
                cmbVuelo.DisplayMember = "Id";
                cmbVuelo.ValueMember = "Id";

                cmbEstado.Items.Clear();
                cmbEstado.Items.AddRange(new string[] { "Activa", "Cancelada" });
                cmbEstado.SelectedIndex = 0;

                ActualizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar formulario: " + ex.Message);
            }
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if (cmbVuelo.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un vuelo antes de calcular el precio.");
                return;
            }

            var vuelo = (Vuelo)cmbVuelo.SelectedItem;
            decimal total = vuelo.TarifaBase;

            if (chkEquipajeBodega.Checked)
                total += 40;
            if (chkEquipajeMano.Checked)
                total += 15;

            txtPrecio.Text = total.ToString("F2");
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbVuelo.SelectedItem == null || cmbPasajero.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar un vuelo y un pasajero.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrecio.Text))
                {
                    MessageBox.Show("Debe calcular el precio antes de guardar.");
                    return;
                }

                var nueva = new Reserva
                {
                    Id = siguienteId++,
                    VueloId = ((Vuelo)cmbVuelo.SelectedItem).Id,
                    PasajeroId = ((Pasajero)cmbPasajero.SelectedItem).Id,
                    FechaReserva = DateTime.Now,
                    PrecioTotal = decimal.Parse(txtPrecio.Text),
                    Estado = cmbEstado.SelectedItem?.ToString() ?? "Activa",
                    EquipajeMano = chkEquipajeMano.Checked,
                    EquipajeBodega = chkEquipajeBodega.Checked,
                    PreferenciaAsiento = txtAsiento.Text
                };

                listaReservas.Add(nueva);
                ActualizarGrid();
                LimpiarCampos();
                MessageBox.Show("Reserva registrada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la reserva: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Debe seleccionar una reserva para modificar.");
                return;
            }

            int id = int.Parse(txtId.Text);
            var reserva = listaReservas.FirstOrDefault(r => r.Id == id);

            if (reserva == null)
            {
                MessageBox.Show("No se encontró la reserva seleccionada.");
                return;
            }

            if (cmbVuelo.SelectedItem == null || cmbPasajero.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un vuelo y un pasajero válidos.");
                return;
            }

            try
            {
                reserva.VueloId = ((Vuelo)cmbVuelo.SelectedItem).Id;
                reserva.PasajeroId = ((Pasajero)cmbPasajero.SelectedItem).Id;
                reserva.EquipajeMano = chkEquipajeMano.Checked;
                reserva.EquipajeBodega = chkEquipajeBodega.Checked;
                reserva.PreferenciaAsiento = txtAsiento.Text;
                reserva.Estado = cmbEstado.SelectedItem?.ToString() ?? "Activa";
                reserva.PrecioTotal = decimal.TryParse(txtPrecio.Text, out decimal total) ? total : 0;

                ActualizarGrid();
                LimpiarCampos();
                MessageBox.Show("Reserva modificada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar reserva: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Debe seleccionar una reserva para eliminar.");
                return;
            }

            int id = int.Parse(txtId.Text);
            listaReservas.RemoveAll(r => r.Id == id);
            ActualizarGrid();
            LimpiarCampos();
            MessageBox.Show("Reserva eliminada correctamente.");
        }

        private void dgvReservas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // evita el error
            if (e.RowIndex < 0) return;

            try
            {
                var fila = dgvReservas.Rows[e.RowIndex];

                txtId.Text = fila.Cells["Id"].Value.ToString();
                txtPrecio.Text = fila.Cells["PrecioTotal"].Value.ToString();
                cmbEstado.SelectedItem = fila.Cells["Estado"].Value.ToString();

                // Validaciones seguras
                chkEquipajeMano.Checked = fila.Cells["EquipajeMano"].Value is bool mano && mano;
                chkEquipajeBodega.Checked = fila.Cells["EquipajeBodega"].Value is bool bodega && bodega;
                txtAsiento.Text = fila.Cells["PreferenciaAsiento"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar la fila: " + ex.Message);
            }
        }


        private void ActualizarGrid()
        {
            try
            {
                // Desactivar temporalmente los eventos del DataGridView
                dgvReservas.CellClick -= dgvReservas_CellClick;

                dgvReservas.DataSource = null;

                if (listaReservas != null && listaReservas.Count > 0)
                {
                    dgvReservas.DataSource = listaReservas.ToList();
                }

                dgvReservas.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el grid: " + ex.Message);
            }
            finally
            {
                // Volver a activar el evento después de actualizar
                dgvReservas.CellClick += dgvReservas_CellClick;
            }
        }


        private void LimpiarCampos()
        {
            txtId.Clear();
            txtAsiento.Clear();
            txtPrecio.Clear();
            chkEquipajeMano.Checked = false;
            chkEquipajeBodega.Checked = false;

            if (cmbEstado.Items.Count > 0)
                cmbEstado.SelectedIndex = 0;
        }

        private void lblReserva_Click(object sender, EventArgs e)
        {

        }
    }
}
