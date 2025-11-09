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
    public partial class VuelosForm : Form
    {
        private List<Vuelo> listaVuelos = new List<Vuelo>();
        private List<Aerolinea> listaAerolineas = new List<Aerolinea>();
        private List<Avion> listaAviones = new List<Avion>();
        private List<Ruta> listaRutas = new List<Ruta>();
        private int siguienteId = 1;
        public VuelosForm()
        {
            InitializeComponent();
        }

        private void VuelosForm_Load(object sender, EventArgs e)
        {
            // 🔹 Datos simulados para los combos
            listaAerolineas.Add(new Aerolinea { Id = 1, Nombre = "Avianca", Codigo = "AV" });
            listaAerolineas.Add(new Aerolinea { Id = 2, Nombre = "Copa Airlines", Codigo = "CM" });

            listaAviones.Add(new Avion { Id = 1, Modelo = "Airbus A320", Capacidad = 180, Matricula = "N123AV" });
            listaAviones.Add(new Avion { Id = 2, Modelo = "Boeing 737", Capacidad = 160, Matricula = "HP567CM" });

            listaRutas.Add(new Ruta { Id = 1, Origen = "San Salvador", Destino = "Panama", DistanciaKm = 1000 });
            listaRutas.Add(new Ruta { Id = 2, Origen = "San Salvador", Destino = "Miami", DistanciaKm = 1800 });

            cmbAerolinea.DataSource = listaAerolineas;
            cmbAerolinea.DisplayMember = "Nombre";
            cmbAerolinea.ValueMember = "Id";

            cmbAvion.DataSource = listaAviones;
            cmbAvion.DisplayMember = "Modelo";
            cmbAvion.ValueMember = "Id";

            cmbRuta.DataSource = listaRutas;
            cmbRuta.DisplayMember = "Descripcion";
            cmbRuta.ValueMember = "Id";

            ActualizarGrid();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
            {
                if (cmbAerolinea.SelectedItem == null || cmbAvion.SelectedItem == null || cmbRuta.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los datos del vuelo.");
                    return;
                }

                Vuelo nuevo = new Vuelo
                {
                    Id = siguienteId++,
                    AerolineaId = ((Aerolinea)cmbAerolinea.SelectedItem).Id,
                    AvionId = ((Avion)cmbAvion.SelectedItem).Id,
                    RutaId = ((Ruta)cmbRuta.SelectedItem).Id,
                    FechaSalida = dtpSalida.Value,
                    FechaLlegada = dtpLlegada.Value,
                    TarifaBase = decimal.Parse(txtTarifa.Text),
                    AsientosDisponibles = int.Parse(txtAsientos.Text)
                };

                listaVuelos.Add(nuevo);
                ActualizarGrid();
                LimpiarCampos();
            }

            private void btnModificar_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtId.Text)) return;

                int id = int.Parse(txtId.Text);
                var vuelo = listaVuelos.FirstOrDefault(v => v.Id == id);

                if (vuelo != null)
                {
                    vuelo.AerolineaId = ((Aerolinea)cmbAerolinea.SelectedItem).Id;
                    vuelo.AvionId = ((Avion)cmbAvion.SelectedItem).Id;
                    vuelo.RutaId = ((Ruta)cmbRuta.SelectedItem).Id;
                    vuelo.FechaSalida = dtpSalida.Value;
                    vuelo.FechaLlegada = dtpLlegada.Value;
                    vuelo.TarifaBase = decimal.Parse(txtTarifa.Text);
                    vuelo.AsientosDisponibles = int.Parse(txtAsientos.Text);

                    ActualizarGrid();
                    LimpiarCampos();
                }
            }

            private void btnEliminar_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtId.Text)) return;

                int id = int.Parse(txtId.Text);
                listaVuelos.RemoveAll(v => v.Id == id);
                ActualizarGrid();
                LimpiarCampos();
            }

            private void btnBuscar_Click(object sender, EventArgs e)
            {
                string buscar = txtTarifa.Text.Trim();
                var filtrados = listaVuelos
                    .Where(v => v.TarifaBase.ToString().Contains(buscar))
                    .ToList();

                dgvVuelos.DataSource = null;
                dgvVuelos.DataSource = filtrados;
            }

            private void dgvVuelos_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex >= 0)
                {
                    var fila = dgvVuelos.Rows[e.RowIndex];
                    txtId.Text = fila.Cells["Id"].Value.ToString();
                    txtTarifa.Text = fila.Cells["TarifaBase"].Value.ToString();
                    txtAsientos.Text = fila.Cells["AsientosDisponibles"].Value.ToString();
                }
            }

            private void ActualizarGrid()
            {
                dgvVuelos.DataSource = null;
                dgvVuelos.DataSource = listaVuelos;
            }

            private void LimpiarCampos()
            {
                txtId.Clear();
                txtTarifa.Clear();
                txtAsientos.Clear();
                cmbAerolinea.SelectedIndex = -1;
                cmbAvion.SelectedIndex = -1;
                cmbRuta.SelectedIndex = -1;
            }

    }
}
