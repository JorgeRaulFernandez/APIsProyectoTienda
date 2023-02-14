using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTienda.ADO.NET
{
    internal static class VentaHandler
    {
        //Planteo la cadena de conexion y la declaro como una variable de clase para no redundar codigo
        public static string cadenaConexion = "Data Source=DESKTOP-37MP24G;Initial Catalog=SistemaGestion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static List<Venta> TraerVentas(long id)
        {
            List<Venta> ventasUsuario = new List<Venta>();
            //Realizo la conexion a la BBDD. Utilizo el using para que la conexion se autogestione
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                //Planteo el comando, escribiendo la sentencia SQL y le paso la conexion
                SqlCommand comando = new SqlCommand("SELECT * FROM Venta WHERE IdUsuario = @id", conn);
                //Creo el SQL parameter
                SqlParameter idParametro = new SqlParameter();
                idParametro.Value = id;
                idParametro.SqlDbType = SqlDbType.BigInt;
                idParametro.ParameterName = "id";
                //Añado el parametro a mi comando
                comando.Parameters.Add(idParametro);
                //Abro la conexion
                conn.Open();
                //Uso el objeto SqlDataReader para tomar lo que viene de la consulta. No uso el data adapter dado que serviría para traer todas las filas y resulta innecesario para el caso
                SqlDataReader reader = comando.ExecuteReader();
                //Valido si el objeto reader tiene filas
                if (reader.HasRows)
                {
                    //Uso un bucle while para ver leer las filas
                    while (reader.Read())
                    {
                        reader.Read();
                        Venta ventaUsuarioTemp = new Venta();
                        ventaUsuarioTemp.Id = reader.GetInt64(0);
                        ventaUsuarioTemp.Comentarios = reader.GetString(1);
                        ventaUsuarioTemp.IdUsuario = reader.GetInt64(2);
                        //Agrego el producto a mi lista de productos
                        ventasUsuario.Add(ventaUsuarioTemp);
                    }
                }
                //Retorno mi lista de productos
                return ventasUsuario;
            }
        }
        public static long InsertarVenta(long idUserVenta)
        {
            //Realizo la conexion a la BBDD. Utilizo el using para que la conexion se autogestione
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                //Planteo el comando, escribiendo la sentencia SQL y le paso la conexion
                SqlCommand comando = new SqlCommand("INSERT INTO Venta (IdUsuario) VALUES (@idUsuario); SELECT @@IDENTITY", conn);
                //Creo el SQL parametro
                SqlParameter idUsuarioParam = new SqlParameter();
                idUsuarioParam.Value = idUserVenta;
                idUsuarioParam.SqlDbType = SqlDbType.BigInt;
                idUsuarioParam.ParameterName = "idUsuario";

                //Añado el parametro a mi comando
                comando.Parameters.Add(idUsuarioParam);

                //Abro la conexion
                conn.Open();
                //El objeto SqlCommand tiene un metodo que nos devuelve la cantidad de filas afectadas
                return Convert.ToInt64(comando.ExecuteScalar());
            }
        }
        public static void CargarVenta(long idUserVenta,List<Producto> nuevosProductosVendidos) {
            long idVentaAux=InsertarVenta(idUserVenta);
            foreach (Producto nuevoProductoVendido in nuevosProductosVendidos)
            {
                ProductoVendido productoVendido = new ProductoVendido(nuevoProductoVendido.Id,nuevoProductoVendido.Stock, nuevoProductoVendido.Id, idVentaAux);
                ProductoVendidoHandler.InsertarProductoVendido(productoVendido);
                ProductoHandler.updateStockProducto(productoVendido.Id, nuevoProductoVendido.Stock);
            }
        }
    }
}
