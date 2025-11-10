
namespace Clave2_Grupo3.Forms
{
    partial class ReservasForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblId = new System.Windows.Forms.Label();
            this.lblVuelo = new System.Windows.Forms.Label();
            this.cmbVuelo = new System.Windows.Forms.ComboBox();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lblPasajero = new System.Windows.Forms.Label();
            this.cmbPasajero = new System.Windows.Forms.ComboBox();
            this.chkEquipajeMano = new System.Windows.Forms.CheckBox();
            this.chkEquipajeBodega = new System.Windows.Forms.CheckBox();
            this.lblAsiento = new System.Windows.Forms.Label();
            this.txtAsiento = new System.Windows.Forms.TextBox();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.btnCalcular = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dgvReservas = new System.Windows.Forms.DataGridView();
            this.lblReserva = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas)).BeginInit();
            this.SuspendLayout();
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(106, 170);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(19, 17);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "Id";
            // 
            // lblVuelo
            // 
            this.lblVuelo.AutoSize = true;
            this.lblVuelo.Location = new System.Drawing.Point(100, 230);
            this.lblVuelo.Name = "lblVuelo";
            this.lblVuelo.Size = new System.Drawing.Size(44, 17);
            this.lblVuelo.TabIndex = 1;
            this.lblVuelo.Text = "Vuelo";
            // 
            // cmbVuelo
            // 
            this.cmbVuelo.FormattingEnabled = true;
            this.cmbVuelo.Location = new System.Drawing.Point(253, 223);
            this.cmbVuelo.Name = "cmbVuelo";
            this.cmbVuelo.Size = new System.Drawing.Size(219, 24);
            this.cmbVuelo.TabIndex = 2;
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(253, 170);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(100, 22);
            this.txtId.TabIndex = 3;
            // 
            // lblPasajero
            // 
            this.lblPasajero.AutoSize = true;
            this.lblPasajero.Location = new System.Drawing.Point(91, 281);
            this.lblPasajero.Name = "lblPasajero";
            this.lblPasajero.Size = new System.Drawing.Size(64, 17);
            this.lblPasajero.TabIndex = 4;
            this.lblPasajero.Text = "Pasajero";
            // 
            // cmbPasajero
            // 
            this.cmbPasajero.FormattingEnabled = true;
            this.cmbPasajero.Location = new System.Drawing.Point(253, 274);
            this.cmbPasajero.Name = "cmbPasajero";
            this.cmbPasajero.Size = new System.Drawing.Size(219, 24);
            this.cmbPasajero.TabIndex = 5;
            // 
            // chkEquipajeMano
            // 
            this.chkEquipajeMano.AutoSize = true;
            this.chkEquipajeMano.Location = new System.Drawing.Point(509, 280);
            this.chkEquipajeMano.Name = "chkEquipajeMano";
            this.chkEquipajeMano.Size = new System.Drawing.Size(144, 21);
            this.chkEquipajeMano.TabIndex = 6;
            this.chkEquipajeMano.Text = "Equipaje en Mano";
            this.chkEquipajeMano.UseVisualStyleBackColor = true;
            // 
            // chkEquipajeBodega
            // 
            this.chkEquipajeBodega.AutoSize = true;
            this.chkEquipajeBodega.Location = new System.Drawing.Point(509, 318);
            this.chkEquipajeBodega.Name = "chkEquipajeBodega";
            this.chkEquipajeBodega.Size = new System.Drawing.Size(158, 21);
            this.chkEquipajeBodega.TabIndex = 7;
            this.chkEquipajeBodega.Text = "Equipaje en Bodega";
            this.chkEquipajeBodega.UseVisualStyleBackColor = true;
            // 
            // lblAsiento
            // 
            this.lblAsiento.AutoSize = true;
            this.lblAsiento.Location = new System.Drawing.Point(56, 357);
            this.lblAsiento.Name = "lblAsiento";
            this.lblAsiento.Size = new System.Drawing.Size(152, 17);
            this.lblAsiento.TabIndex = 8;
            this.lblAsiento.Text = "Preferencia de Asiento";
            // 
            // txtAsiento
            // 
            this.txtAsiento.Location = new System.Drawing.Point(253, 357);
            this.txtAsiento.Name = "txtAsiento";
            this.txtAsiento.Size = new System.Drawing.Size(100, 22);
            this.txtAsiento.TabIndex = 9;
            // 
            // lblPrecio
            // 
            this.lblPrecio.AutoSize = true;
            this.lblPrecio.Location = new System.Drawing.Point(91, 426);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(79, 17);
            this.lblPrecio.TabIndex = 10;
            this.lblPrecio.Text = "Precio total";
            // 
            // txtPrecio
            // 
            this.txtPrecio.Location = new System.Drawing.Point(253, 426);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.ReadOnly = true;
            this.txtPrecio.Size = new System.Drawing.Size(100, 22);
            this.txtPrecio.TabIndex = 11;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(103, 487);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(52, 17);
            this.lblEstado.TabIndex = 12;
            this.lblEstado.Text = "Estado";
            // 
            // cmbEstado
            // 
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Location = new System.Drawing.Point(253, 487);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(121, 24);
            this.cmbEstado.TabIndex = 13;
            // 
            // btnCalcular
            // 
            this.btnCalcular.Location = new System.Drawing.Point(31, 601);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(124, 37);
            this.btnCalcular.TabIndex = 14;
            this.btnCalcular.Text = "Calcular Precio";
            this.btnCalcular.UseVisualStyleBackColor = true;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(179, 600);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(80, 38);
            this.btnGuardar.TabIndex = 15;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnModificar
            // 
            this.btnModificar.Location = new System.Drawing.Point(291, 598);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(83, 40);
            this.btnModificar.TabIndex = 16;
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = true;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(416, 600);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(93, 38);
            this.btnEliminar.TabIndex = 17;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(536, 601);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(84, 37);
            this.btnBuscar.TabIndex = 18;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            // 
            // dgvReservas
            // 
            this.dgvReservas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReservas.Location = new System.Drawing.Point(702, 120);
            this.dgvReservas.Name = "dgvReservas";
            this.dgvReservas.RowHeadersWidth = 51;
            this.dgvReservas.RowTemplate.Height = 24;
            this.dgvReservas.Size = new System.Drawing.Size(551, 518);
            this.dgvReservas.TabIndex = 19;
            this.dgvReservas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReservas_CellClick);
            // 
            // lblReserva
            // 
            this.lblReserva.AutoSize = true;
            this.lblReserva.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReserva.Location = new System.Drawing.Point(306, 66);
            this.lblReserva.Name = "lblReserva";
            this.lblReserva.Size = new System.Drawing.Size(142, 32);
            this.lblReserva.TabIndex = 20;
            this.lblReserva.Text = "Reservas";
            // 
            // ReservasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 746);
            this.Controls.Add(this.lblReserva);
            this.Controls.Add(this.dgvReservas);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCalcular);
            this.Controls.Add(this.cmbEstado);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.txtPrecio);
            this.Controls.Add(this.lblPrecio);
            this.Controls.Add(this.txtAsiento);
            this.Controls.Add(this.lblAsiento);
            this.Controls.Add(this.chkEquipajeBodega);
            this.Controls.Add(this.chkEquipajeMano);
            this.Controls.Add(this.cmbPasajero);
            this.Controls.Add(this.lblPasajero);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.cmbVuelo);
            this.Controls.Add(this.lblVuelo);
            this.Controls.Add(this.lblId);
            this.Name = "ReservasForm";
            this.Text = "ReservasForm";
            this.Load += new System.EventHandler(this.ReservasForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblVuelo;
        private System.Windows.Forms.ComboBox cmbVuelo;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblPasajero;
        private System.Windows.Forms.ComboBox cmbPasajero;
        private System.Windows.Forms.CheckBox chkEquipajeMano;
        private System.Windows.Forms.CheckBox chkEquipajeBodega;
        private System.Windows.Forms.Label lblAsiento;
        private System.Windows.Forms.TextBox txtAsiento;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Button btnCalcular;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DataGridView dgvReservas;
        private System.Windows.Forms.Label lblReserva;
    }
}