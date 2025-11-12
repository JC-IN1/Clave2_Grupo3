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
    public partial class PagosForm : Form
    {
        private List<Pago> listaPagos = new List<Pago>();
        private List<Reserva> listaReservas = new List<Reserva>();
        private int siguienteId = 1;
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
                //  Datos simulados de reservas
                cmbReserva.DataSource = null;
                cmbMetodo.Items.Clear();
                cmbEstado.Items.Clear();

                listaReservas = new List<Reserva>
        {
            new Reserva { Id = 1, PrecioTotal = 300, Estado = "Activa" },
            new Reserva { Id = 2, PrecioTotal = 450, Estado = "Activa" }
        };

                // Asignar la lista al ComboBox
                cmbReserva.DataSource = null;
                cmbReserva.DataSource = listaReservas;
                cmbReserva.DisplayMember = "Id";
                cmbReserva.ValueMember = "Id";

                // Métodos y estados de pago
                cmbMetodo.Items.Clear();
                cmbMetodo.Items.AddRange(new string[] { "Tarjeta", "Efectivo", "Transferencia" });

                cmbEstado.Items.Clear();
                cmbEstado.Items.AddRange(new string[] { "Pagado", "Pendiente", "Cancelado" });

                // Solo establecer índices si hay elementos
                if (cmbEstado.Items.Count > 0)
                    cmbEstado.SelectedIndex = 1;
                if (cmbMetodo.Items.Count > 0)
                    cmbMetodo.SelectedIndex = 0;

                // Cargar datos iniciales al DataGridView
                ActualizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el formulario de pagos: " + ex.Message);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbReserva.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar una reserva.");
                return;
            }

            Pago nuevo = new Pago
            {
                Id = siguienteId++,
                ReservaId = ((Reserva)cmbReserva.SelectedItem).Id,
                FechaPago = dtpFecha.Value,
                Monto = decimal.Parse(txtMonto.Text),
                Metodo = cmbMetodo.Text,
                EstadoPago = cmbEstado.Text
            };

            listaPagos.Add(nuevo);
            ActualizarGrid();
            LimpiarCampos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Debe seleccionar un pago para modificar.");
                return;
            }

            int id = int.Parse(txtId.Text);
            var pago = listaPagos.FirstOrDefault(p => p.Id == id);

            if (pago == null)
            {
                MessageBox.Show("No se encontró el pago seleccionado.");
                return;
            }

            // 🔹 Validar que haya una reserva seleccionada
            if (cmbReserva.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar una reserva antes de modificar.");
                return;
            }

            try
            {
                var reservaSeleccionada = (Reserva)cmbReserva.SelectedItem;

                pago.ReservaId = reservaSeleccionada.Id;
                pago.FechaPago = dtpFecha.Value;

                if (decimal.TryParse(txtMonto.Text, out decimal monto))
                {
                    pago.Monto = monto;
                }
                else
                {
                    MessageBox.Show("Monto no válido.");
                    return;
                }

                pago.Metodo = cmbMetodo.Text;
                pago.EstadoPago = cmbEstado.Text;

                ActualizarGrid();
                LimpiarCampos();
                MessageBox.Show("Pago modificado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el pago: " + ex.Message);
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text)) return;

            int id = int.Parse(txtId.Text);
            listaPagos.RemoveAll(p => p.Id == id);
            ActualizarGrid();
            LimpiarCampos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtMonto.Text.Trim();

            var filtrados = listaPagos
                .Where(p => p.Monto.ToString().Contains(filtro))
                .ToList();

            dgvPagos.DataSource = null;
            dgvPagos.DataSource = filtrados;
        }

        private void dgvPagos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  // Evita error si clicas encabezado o zona vacía

            var fila = dgvPagos.Rows[e.RowIndex];
            txtId.Text = fila.Cells["Id"].Value.ToString();
            txtMonto.Text = fila.Cells["Monto"].Value.ToString();
            cmbEstado.SelectedItem = fila.Cells["EstadoPago"].Value.ToString();
            cmbMetodo.SelectedItem = fila.Cells["Metodo"].Value.ToString();
        }


        private void ActualizarGrid()
        {
            try
            {
                dgvPagos.DataSource = null;

                if (listaPagos != null && listaPagos.Count > 0)
                {
                    dgvPagos.DataSource = listaPagos.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el grid: " + ex.Message);
            }
        }



        private void LimpiarCampos()
        {
            txtId.Clear();
            txtMonto.Clear();

            if (cmbMetodo.Items.Count > 0)
                cmbMetodo.SelectedIndex = -1;

            if (cmbEstado.Items.Count > 1)
                cmbEstado.SelectedIndex = 1;
            else if (cmbEstado.Items.Count > 0)
                cmbEstado.SelectedIndex = 0;

            if (cmbReserva.Items.Count > 0)
                cmbReserva.SelectedIndex = -1;
        }

    }
}

