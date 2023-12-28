using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;

namespace CapaNegocio
{
    public class NFactura
    {
        public static DataTable Mostrar()
        {
            return new DFactura().Mostrar();
        }
        public static string ConsultaSecuencial()
        {
            return new DFactura().ConsultaSecuencial();
        }
        public static string Insertar(DateTime fecha, decimal total, DataTable detalle)
        {
            DFactura obj = new DFactura();
            obj.Fecha = fecha;
            obj.Total = total;
            List<DDetalleFactura> lista = new List<DDetalleFactura>();
            foreach (DataRow item in detalle.Rows)
            {
                DDetalleFactura det = new DDetalleFactura();
                det.Idproducto = Convert.ToInt16(item["idproducto"]);
                det.Cantidad = Convert.ToInt16(item["cantidad"]);
                det.Subtotal = Convert.ToDecimal(item["subtotal"]);
                det.Iva = Convert.ToDecimal(item["subtotaliva"]);
                lista.Add(det);
            }
            return obj.Inserta(obj, lista);

        }
    }
}
