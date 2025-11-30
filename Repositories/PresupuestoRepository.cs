using DistribuidoraInsumosMVC.Models;
using Microsoft.Data.Sqlite;
using System.Data;

namespace DistribuidoraInsumosMVC.Repositories
{
    public class PresupuestoRepository
    {
        private string cadenaConexion = "Data Source = Data/Tienda_final.db";

        public bool CrearPresupuesto(Presupuesto presupuesto)
        {
            foreach (var detalleActual in presupuesto.detalle)
            {
                var encontrado = new ProductoRepository().GetProductoById(detalleActual.producto.id);
                if (encontrado == null) return false;
            }
            using var connection = new SqliteConnection(cadenaConexion);
            connection.Open();

            string query = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@nombre, @fecha)";
            using var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@nombre", presupuesto.nombreDestinatario));
            command.Parameters.Add(new SqliteParameter("@fecha", presupuesto.fechaCreacion));

            int filas = command.ExecuteNonQuery();
            if (filas == 0) return false;

            //obtengo el ultimo ID generado de presupuesto (xq se cargó..)
            // long idPresupuestoUltimo = command.LastInsertRowId; NO COMPATIBLE CON MI VERSION
            
            int idPresupuestoLast = (int)(long)new SqliteCommand("SELECT last_insert_rowid()", connection).ExecuteScalar();

            // Insertar los detalles
            foreach (var detalle in presupuesto.detalle)
            {
                string queryDetalle = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPres, @idProd, @cantidad)";
                using var commandDetalle = new SqliteCommand(queryDetalle, connection);
                commandDetalle.Parameters.Add(new SqliteParameter("@idPres", idPresupuestoLast));
                commandDetalle.Parameters.Add(new SqliteParameter("@idProd", detalle.producto.id));
                commandDetalle.Parameters.Add(new SqliteParameter("@cantidad", detalle.cantidad));

                int filasDetalle = commandDetalle.ExecuteNonQuery();
                if (filasDetalle == 0) return false;
                filasDetalle = 0;
            }
            return true;
        }

        public List<Presupuesto> ListarPresupuestos()
        {
            using var connection = new SqliteConnection(cadenaConexion);
            connection.Open();

            string query = "SELECT * FROM Presupuestos";
            using var command = new SqliteCommand(query, connection);
            using SqliteDataReader reader = command.ExecuteReader();
            var presupuestos = MapearPresupuestos(reader);

            return presupuestos;
        }

        private List<Presupuesto> MapearPresupuestos(SqliteDataReader reader)
        {
            var presupuestos = new List<Presupuesto>();
            while (reader.Read())
            {
                var presupuesto = new Presupuesto
                {
                    idPresupuesto = (reader["idPresupuesto"]!= DBNull.Value)? Convert.ToInt32(reader["idPresupuesto"]):0,
                    nombreDestinatario = (reader["NombreDestinatario"] != DBNull.Value) ? reader["NombreDestinatario"].ToString(): string.Empty,
                    fechaCreacion = (reader["FechaCreacion"] != DBNull.Value)? Convert.ToDateTime(reader["FechaCreacion"]): DateTime.MinValue
                };
                presupuestos.Add(presupuesto);
            }
            return presupuestos;
        }
        public Presupuesto GetPresupuestoById(int idPresupuesto)
        {
            using var connection = new SqliteConnection(cadenaConexion);
            connection.Open();

            string query = "SELECT * FROM Presupuestos WHERE idPresupuesto = @id";
            using var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", idPresupuesto));

            using var reader = command.ExecuteReader(CommandBehavior.SingleRow);

            if (reader.Read())
            {
                return new Presupuesto
                {
                    idPresupuesto = (reader["idPresupuesto"] != DBNull.Value) ? Convert.ToInt32(reader["idPresupuesto"]) : 0,
                    nombreDestinatario = (reader["NombreDestinatario"] != DBNull.Value) ? reader["NombreDestinatario"].ToString() : string.Empty,
                    fechaCreacion = (reader["FechaCreacion"] != DBNull.Value) ? Convert.ToDateTime(reader["FechaCreacion"]) : DateTime.MinValue
                };
            }
            return null;
        }
        public bool AgregarProducto(int idPresupuesto, PresupuestoDetalle detalle)
        {
            var encontrado = new ProductoRepository().GetProductoById(detalle.producto.id);
            if (encontrado == null) return false;

            using var connection = new SqliteConnection(cadenaConexion);
            connection.Open();

            var presupuesto = new PresupuestoRepository().GetPresupuestoById(idPresupuesto);
            if (presupuesto == null) return false;

            string query = "INSERT INTO PresupuestosDetalle (idProducto,idPresupuesto,Cantidad) VALUES (@idProd,@idPres,@cantidad)"; 
            
            using var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@idProd",detalle.producto.id));
            command.Parameters.Add(new SqliteParameter("@idPres",idPresupuesto));
            command.Parameters.Add(new SqliteParameter("@cantidad",detalle.cantidad));
            return command.ExecuteNonQuery() > 0;
        }
        //agregar ActualizarPresupuesto()
        public bool ActualizarPresupuesto(int idPresupuesto,Presupuesto presupuestoNuevo)
        {
            return true;
        }

        public bool EliminarPresupuesto(int idPresupuesto)
        {
            using var connection = new SqliteConnection(cadenaConexion);
            connection.Open();

            var presupuesto = new PresupuestoRepository().GetPresupuestoById(idPresupuesto);
            if (presupuesto == null) return false;

            string query0 = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id";
            using var command0 = new SqliteCommand(query0, connection);
            command0.Parameters.Add(new SqliteParameter("@id", idPresupuesto));

            if(command0.ExecuteNonQuery() > 0){
                string query = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.Add(new SqliteParameter("@id", idPresupuesto));
                return command.ExecuteNonQuery() > 0;
            }
            return command0.ExecuteNonQuery()>0;
        }
    }
}