using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class FrmFactura : Form
    {
        DataTable dtDetalle = null;
        private static FrmFactura _instancia;
        public FrmFactura()
        {
            InitializeComponent();
        }
        public static FrmFactura GetInstance()
        {
            if (_instancia == null)
            {
                _instancia = new FrmFactura();
            }
            return _instancia;
        }
        public void SetProducto(string idproducto, string nombre, string valor)
        {
            this.txtIdProducto.Text = idproducto;
            this.txtProducto.Text = nombre;
            this.txtPrecio.Text = valor;
        }
        private void CrearTablaDetalle() { 
            dtDetalle= new DataTable("Detalle");
            dtDetalle.Columns.Add("idproducto", typeof(Int16));
            dtDetalle.Columns.Add("producto", typeof(string));
            dtDetalle.Columns.Add("cantidad", typeof(Int16));
            dtDetalle.Columns.Add("precio", typeof(decimal));
            dtDetalle.Columns.Add("iva", typeof(decimal));
            DataColumn dcsub =new DataColumn("subtotal", typeof(decimal));
            dcsub.Expression = "cantidad * precio";
            dtDetalle.Columns.Add(dcsub);
            DataColumn dciva = new DataColumn("subtotaliva", typeof(decimal));
            dciva.Expression = "subtotal * iva";
            dtDetalle.Columns.Add(dciva);
            dgvFacturas.DataSource= dtDetalle;

        }
        private void FrmFactura_Load(object sender, EventArgs e)
        {
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            CrearTablaDetalle();
            dgvFacturas.Columns["iva"].Visible = false;
            Mostrar();
            txtIdFactura.Text = NFactura.ConsultaSecuencial();
        }

        private void LimpiarDetalle() { 
            txtIdProducto.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtCantidad.Text = string.Empty;
            txtProducto.Text = string.Empty;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtIdProducto.Text ==String.Empty || txtPrecio.Text==String.Empty
                || txtCantidad.Text ==String.Empty)
            {
                MessageBox.Show("Debe ingresar todos los datos");
            }
            else
            {
                DataRow dr = dtDetalle.NewRow();
                dr["idproducto"] = txtIdProducto.Text;
                dr["producto"] = txtProducto.Text;
                dr["cantidad"] = txtCantidad.Text;
                dr["precio"] = txtPrecio.Text;
                decimal iva = 0;

                if (chkIva.Checked)
                {
                    iva = 0.12m;
                }
                dr["iva"] = iva;
                dtDetalle.Rows.Add(dr);
                LimpiarDetalle();
                CalcularTotales();
            }
        }

        private void CalcularTotales() {
            decimal subtotal = 0;
            decimal subtotaliva = 0;
            if (dtDetalle.Rows.Count > 0)
            {
                subtotal = (decimal)dtDetalle.Compute("sum(subtotal)", "");                
                subtotaliva = (decimal)dtDetalle.Compute("sum(subtotaliva)", "");                                
            }
            txtSubtotal.Text = subtotal.ToString("#0.00#");
            txtIva.Text = subtotaliva.ToString("#0.00#");
            txtTotal.Text = (subtotal + subtotaliva).ToString();

        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            decimal subtotal = 0;
            if (dtDetalle.Rows.Count>0)
            {
                int i = dgvFacturas.CurrentRow.Index;
                DataRow row= dtDetalle.Rows[i];
                dtDetalle.Rows.Remove(row);
                
                if (dtDetalle.Rows.Count > 0)
                {
                    subtotal = (decimal)dtDetalle.Compute("sum(subtotal)", "");
                }
            }
            CalcularTotales();


        }

        private void Mostrar()
        {
            dgvListado.DataSource = NFactura.Mostrar();
            lblTotalListado.Text = "Total registros: " + dgvListado.Rows.Count.ToString();
        }
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            string rpta = "";
            rpta = NFactura.Insertar(DateTime.Parse(txtFecha.Text), Convert.ToDecimal(txtTotal.Text), dtDetalle);
            if (rpta.Equals("OK"))
            {
                MessageBox.Show("Venta realizada correctamente");
                              
                dtDetalle.Rows.Clear();
                dgvFacturas.DataSource = dtDetalle;
                txtTotal.Text = string.Empty;
                txtIva.Text = string.Empty;
                txtSubtotal.Text = string.Empty;
                Mostrar();
                txtIdFactura.Text = NFactura.ConsultaSecuencial();
                FrmReporte frm = new FrmReporte();
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show(rpta);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            FrmReporte frm = new FrmReporte();
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmProductos frm = new FrmProductos();
            frm.ShowDialog();
        }

        private void FrmFactura_FormClosing(object sender, FormClosingEventArgs e)
        {
            _instancia = null;
        }
    }
}
