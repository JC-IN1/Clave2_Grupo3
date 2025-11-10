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
            //  Simular vuelos y pasajeros existentes
            listaVuelos.Add(new Vuelo { Id = 1, TarifaBase = 250, AsientosDisponibles = 30 });
            listaVuelos.Add(new Vuelo { Id = 2, TarifaBase = 400, AsientosDisponibles = 20 });

            listaPasajeros.Add(new Pasajero { Id = 1, Nombre = "Juan Pérez" });
            listaPasajeros.Add(new Pasajero { Id = 2, Nombre = "María López" });

            if (usuarioActual.Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
            {
                // El cliente solo puede verse a sí mismo
                cmbPasajero.DataSource = listaPasajeros
                    .Where(p => p.Nombre.IndexOf(usuarioActual.NombreUsuario, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                // Si no coincide, lo agregamos con su nombre
                if (cmbPasajero.Items.Count == 0)
                {
                    var clientePasajero = new Pasajero
                    {
                        Id = listaPasajeros.Count + 1,
                        Nombre = usuarioActual.NombreUsuario
                    };
                    listaPasajeros.Add(clientePasajero);
                    cmbPasajero.DataSource = new List<Pasajero> { clientePasajero };
                }
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


            cmbEstado.Items.AddRange(new string[] { "Activa", "Cancelada" });
            cmbEstado.SelectedIndex = 0;

            ActualizarGrid();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if (cmbVuelo.SelectedItem == null) return;

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
            if (cmbVuelo.SelectedItem == null || cmbPasajero.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un vuelo y un pasajero.");
                return;
            }

            Reserva nueva = new Reserva
            {
                Id = siguienteId++,
                VueloId = ((Vuelo)cmbVuelo.SelectedItem).Id,
                PasajeroId = ((Pasajero)cmbPasajero.SelectedItem).Id,
                FechaReserva = DateTime.Now,
                PrecioTotal = decimal.Parse(txtPrecio.Text),
                Estado = cmbEstado.SelectedItem.ToString(),
                EquipajeMano = chkEquipajeMano.Checked,
                EquipajeBodega = chkEquipajeBodega.Checked,
                PreferenciaAsiento = txtAsiento.Text
            };

            listaReservas.Add(nueva);
            ActualizarGrid();
            LimpiarCampos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text)) return;

            int id = int.Parse(txtId.Text);
            var reserva = listaReservas.FirstOrDefault(r => r.Id == id);

            if (reserva != null)
            {
                reserva.VueloId = ((Vuelo)cmbVuelo.SelectedItem).Id;
                reserva.PasajeroId = ((Pasajero)cmbPasajero.SelectedItem).Id;
                reserva.EquipajeMano = chkEquipajeMano.Checked;
                reserva.EquipajeBodega = chkEquipajeBodega.Checked;
                reserva.PreferenciaAsiento = txtAsiento.Text;
                reserva.Estado = cmbEstado.SelectedItem.ToString();
                reserva.PrecioTotal = decimal.Parse(txtPrecio.Text);

                ActualizarGrid();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text)) return;

            int id = int.Parse(txtId.Text);
            listaReservas.RemoveAll(r => r.Id == id);
            ActualizarGrid();
            LimpiarCampos();
        }

        private void dgvReservas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvReservas.Rows[e.RowIndex];
                txtId.Text = fila.Cells["Id"].Value.ToString();
                txtPrecio.Text = fila.Cells["PrecioTotal"].Value.ToString();
                cmbEstado.SelectedItem = fila.Cells["Estado"].Value.ToString();
            }
        }

        private void ActualizarGrid()
        {
            dgvReservas.DataSource = null;
            dgvReservas.DataSource = listaReservas;
        }

        private void LimpiarCampos()
        {
            txtId.Clear();
            txtAsiento.Clear();
            txtPrecio.Clear();
            chkEquipajeMano.Checked = false;
            chkEquipajeBodega.Checked = false;
            cmbEstado.SelectedIndex = 0;
        }
    }
}
