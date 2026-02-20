using DistribuidoraInsumosMVC.Interfaces;
using DistribuidoraInsumosMVC.Models;
using Microsoft.Data.Sqlite;
using System.Data;

namespace DistribuidoraInsumosMVC.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        // private string cadenaConexion = "Data Source = Data/Tienda_final.db";
        private readonly string _ConnectionString;

        public ProductoRepository(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }
        public ProductoRepository(){}

        public bool CrearProducto(Producto producto) //recibe un objeto Producto
        {

            using var connection = new SqliteConnection(_ConnectionString);
            connection.Open();

            string query = "INSERT INTO Productos (descripcion,precio) VALUES (@descripcion,@precio)"; //id AI
            using var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@descripcion", producto.descripcion));
            command.Parameters.Add(new SqliteParameter("@precio", producto.precio));
            return command.ExecuteNonQuery() >0 ;

            // connection.Close(); NO HACE FALTA PORQUE USO "USING", se llama automaticamente el Dispose() al salir del bloque 
        }

        public bool ActualziarProducto(int idProducto, Producto productoModificado) //recibe id y objeto producto
        {
            using var connection = new SqliteConnection(_ConnectionString);
            connection.Open();

            string query = "UPDATE Productos SET descripcion = @descripcion, precio = @precio WHERE idProducto = @id";
            using var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", idProducto));
            command.Parameters.Add(new SqliteParameter("@descripcion", productoModificado.descripcion));
            command.Parameters.Add(new SqliteParameter("@precio", productoModificado.precio));
            return command.ExecuteNonQuery() >0;
        }

        public List<Producto> ListarProductos() //devuelve un lsit de productos REGISTRADOS
        {
            using var connection = new SqliteConnection(_ConnectionString);
            connection.Open();

            string query = "SELECT * FROM Productos";
            using var command = new SqliteCommand(query, connection);
            using SqliteDataReader reader = command.ExecuteReader(); //si no uso el using se mantiene un PUNTERO ABIERTO sobre la cconexion
            var productos = MapearProductos(reader);

            return productos; // tendra prods o lista vacia
        }

        private List<Producto> MapearProductos(SqliteDataReader reader)
        {
            var productos = new List<Producto>();
            while (reader.Read())
            {
                var producto = new Producto
                {
                    id = Convert.ToInt32(reader["idProducto"]),
                    descripcion = reader["descripcion"].ToString(),
                    precio = Convert.ToInt32(reader["precio"])

                    /*ENUM: EstadoPedido = (Estado)Enum.Parse(typeof(Estado), reader["estado"].ToString()),
                        Id_usuario_asignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : (int?)null
                    */
                };
                productos.Add(producto);
            }
            return productos;
        }

        public Producto GetProductoById(int idProducto) // recibe ID devuelve producto
        {
            using var connection = new SqliteConnection(_ConnectionString);
            connection.Open();

            string query = "SELECT * FROM Productos WHERE idProducto = @id";
            using var command = new SqliteCommand(query, connection); //using para eivtar que queden referencias activas
            command.Parameters.Add(new SqliteParameter("@id", idProducto));
            using SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                var producto = new Producto
                {
                    id = Convert.ToInt32(reader["idProducto"]),
                    descripcion = reader["descripcion"].ToString(),
                    precio = Convert.ToInt32(reader["precio"])
                };
                return producto;
            }
            return null;
        }
        public Producto GetProductoById2(int idProducto)
        {
            using var connection = new SqliteConnection(_ConnectionString);
            connection.Open();

            string query = "SELECT * FROM Productos WHERE idProducto = @id";
            using var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", idProducto));

            using var lector = command.ExecuteReader(CommandBehavior.SingleRow); //trae solo 1 fila

            if (lector.Read())
            {
                return new Producto
                {
                    id = (lector["idProducto"] != DBNull.Value) ? Convert.ToInt32(lector["idProducto"]) : 0,
                    descripcion = (lector["descripcion"] != DBNull.Value) ? lector["descripcion"].ToString() : string.Empty,
                    precio = (lector["precio"] != DBNull.Value) ? Convert.ToInt32(lector["precio"]) : 0
                };
            }
            return null;
        }
        //el gestor se encarga por medio del CASCADE de eliminar los que usan idProd como FK
        public bool EliminarProducto(int idProducto)
        {
            using var connection = new SqliteConnection(_ConnectionString);
            connection.Open();

            string query = "DELETE FROM Productos WHERE idProducto = @id";
            using var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", idProducto));
            return command.ExecuteNonQuery() > 0;
        }
    }
}