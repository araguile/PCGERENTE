using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmProductos : Form
    {
        public FrmProductos()
        {
            InitializeComponent();
        }
        private void CrearData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("nombre", typeof(string));
            dt.Columns.Add("precio", typeof(string));

            DataRow dr = dt.NewRow();
            dr["id"] = 2;
            dr["nombre"] = "Mouse";
            dr["precio"] = "3,5";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 4;
            dr["nombre"] = "Impresora";
            dr["precio"] = "49";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 6;
            dr["nombre"] = "Monitor";
            dr["precio"] = "100";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 7;
            dr["nombre"] = "Microfono";
            dr["precio"] = "5";
            dt.Rows.Add(dr);

            dataGridView1.DataSource = dt;
        }

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            CrearData();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FrmFactura frm = FrmFactura.GetInstance();
            frm.SetProducto(dataGridView1.CurrentRow.Cells["id"].Value.ToString(),
                            dataGridView1.CurrentRow.Cells["nombre"].Value.ToString(),
                            dataGridView1.CurrentRow.Cells["precio"].Value.ToString());
            this.Hide();
            frm.Show();
        }
    }
}
