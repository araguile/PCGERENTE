using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class DDetalleFactura
    {
        #region atributos
        private DFactura _factura;
        private int iddetalle;
        private int idproducto;
        private int cantidad;
        private decimal subtotal;
        private decimal iva;
        #endregion
        #region propiedades
        public DFactura Factura { get => _factura; set => _factura = value; }
        public int Iddetalle { get => iddetalle; set => iddetalle = value; }
        public int Idproducto { get => idproducto; set => idproducto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public decimal Subtotal { get => subtotal; set => subtotal = value; }
        public decimal Iva { get => iva; set => iva = value; }
        #endregion
        #region metodos
        public DDetalleFactura()
        {
            _factura = new DFactura();
        }
        public DDetalleFactura(int idproducto, int cantidad, decimal subtotal,decimal iva)
        {
            _factura = new DFactura();
            this.Idproducto = idproducto;
            this.Cantidad = cantidad;
            this.subtotal = subtotal;
            this.iva = iva;
        }
        public string Inserta(DDetalleFactura detalle, ref SqlConnection cn, ref SqlTransaction tran)
        {
            string rpta = "";
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.Transaction = tran;
                cmd.CommandText = "spinsertar_detalle_factura";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter ParIdVenta = new SqlParameter();
                ParIdVenta.ParameterName = "@idventa";
                ParIdVenta.SqlDbType = SqlDbType.Int;
                ParIdVenta.Value = detalle.Factura.Idfactura;
                cmd.Parameters.Add(ParIdVenta);

                SqlParameter ParIdProducto = new SqlParameter();
                ParIdProducto.ParameterName = "@idproducto";
                ParIdProducto.SqlDbType = SqlDbType.Int;
                ParIdProducto.Value = detalle.Idproducto;
                cmd.Parameters.Add(ParIdProducto);

                SqlParameter ParCantidad = new SqlParameter();
                ParCantidad.ParameterName = "@cantidad";
                ParCantidad.SqlDbType = SqlDbType.Int;
                ParCantidad.Value = detalle.Cantidad;
                cmd.Parameters.Add(ParCantidad);

                SqlParameter ParSubtotal = new SqlParameter();
                ParSubtotal.ParameterName = "@subtotal";
                ParSubtotal.SqlDbType = SqlDbType.Decimal;
                ParSubtotal.Value = detalle.Subtotal;
                cmd.Parameters.Add(ParSubtotal);

                SqlParameter ParIva = new SqlParameter();
                ParIva.ParameterName = "@iva";
                ParIva.SqlDbType = SqlDbType.Decimal;
                ParIva.Value = detalle.iva;
                cmd.Parameters.Add(ParIva);

                rpta = cmd.ExecuteNonQuery() == 1 ? "OK" : "no se pudo insertar";
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            return rpta;
        }
        #endregion
    }
}
