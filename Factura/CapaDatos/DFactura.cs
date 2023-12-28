using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class DFactura
    {
        #region atributos
        private int idfactura;
        private DateTime fecha;
        private decimal total;
        #endregion
        #region propiedades
        public int Idfactura { get => idfactura; set => idfactura = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public decimal Total { get => total; set => total = value; }
        #endregion
        #region metodos
        public DataTable Mostrar() {
            DataTable dtResultado= new DataTable("Factura");
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = DConexion.CnStr;
                cn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "spmostrar_factura";

                SqlDataAdapter da=new SqlDataAdapter();
                da.SelectCommand=cmd;
                da.Fill(dtResultado);

            }
            catch (Exception)
            {

                throw;
            }
            finally {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            return dtResultado;
        }
        public string ConsultaSecuencial()
        {
            SqlConnection cn = new SqlConnection();
            DFactura factura= new DFactura();
            string rpta = "";
            try
            {
                cn.ConnectionString = DConexion.CnStr;
                cn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spconsultasecuencial";

                SqlParameter ParIdFactura = new SqlParameter();
                ParIdFactura.ParameterName = "@idfactura";
                ParIdFactura.SqlDbType = SqlDbType.Int;
                ParIdFactura.Direction = ParameterDirection.Output;
                ParIdFactura.Value = factura.idfactura;
                cmd.Parameters.Add(ParIdFactura);

                rpta = cmd.ExecuteNonQuery() == 1 ? "OK" : "OK";
                if (rpta.Equals("OK"))
                {
                    factura.idfactura = Convert.ToInt16(cmd.Parameters["@idfactura"].Value);
                }

                }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
            return factura.idfactura.ToString();
        }
        public string Inserta(DFactura factura, List<DDetalleFactura> lstDetalle)
        {
            string rpta = "";
            SqlConnection cn = new SqlConnection();

            try
            {
                cn.ConnectionString = DConexion.CnStr;
                cn.Open();

                SqlTransaction trans = cn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.Transaction = trans;
                cmd.CommandText = "spinsertar_factura";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter ParIdVenta = new SqlParameter();
                ParIdVenta.ParameterName = "@idfactura";
                ParIdVenta.SqlDbType = SqlDbType.Int;
                ParIdVenta.Direction = ParameterDirection.Output;
                ParIdVenta.Value = factura.idfactura;
                cmd.Parameters.Add(ParIdVenta);

                SqlParameter ParFecha = new SqlParameter();
                ParFecha.ParameterName = "@fecha";
                ParFecha.SqlDbType = SqlDbType.DateTime;
                ParFecha.Value = factura.fecha;
                cmd.Parameters.Add(ParFecha);

                SqlParameter ParTotal = new SqlParameter();
                ParTotal.ParameterName = "@total";
                ParTotal.SqlDbType = SqlDbType.Decimal;
                ParTotal.Value = factura.total;
                cmd.Parameters.Add(ParTotal);

                rpta = cmd.ExecuteNonQuery() == 1 ? "OK" : "No se pudo insertar";
                if (rpta.Equals("OK"))
                {
                    foreach (DDetalleFactura det in lstDetalle)
                    {
                        det.Factura.idfactura = Convert.ToInt16(cmd.Parameters["@idfactura"].Value);
                        rpta = det.Inserta(det, ref cn, ref trans);
                        if (!rpta.Equals("OK"))
                        {
                            break;
                        }
                    }
                }
                if (rpta.Equals("OK"))
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }

            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
            return rpta;
        }
        #endregion
    }
}
