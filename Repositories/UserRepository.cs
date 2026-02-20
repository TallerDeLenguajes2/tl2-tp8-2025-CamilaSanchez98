using DistribuidoraInsumosMVC.Models;
using Microsoft.Data.Sqlite;
using DistribuidoraInsumosMVC.Interfaces;

namespace DistribuidoraInsumosMVC.Repositories
{
    public class UsuarioRepository : IUserRepository
    {
        private string CadenaConexion = "Data Source = Data/Tienda_final.db";
        public Usuario GetUser(string usuario, string contrasena)
        {
            Usuario user = null;
            //Consulta SQL que busca por Usuario Y Contrasena
            const string sql = @"SELECT Id, Nombre, User, Pass, Rol FROM Usuarios WHERE User = @Usuario AND Pass = @Contrasena";
            using var conexion = new SqliteConnection(CadenaConexion);
            conexion.Open();
            using var comando = new SqliteCommand(sql, conexion);

            comando.Parameters.AddWithValue("@Usuario", usuario);
            comando.Parameters.AddWithValue("@Contrasena",contrasena);
            using var reader = comando.ExecuteReader();
            if (reader.Read())
            {
                user = new Usuario
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Nombre = reader["Nombre"].ToString(),
                                        Username = reader["User"].ToString(),
                                        Password = reader["Pass"].ToString(),
                                        Rol = reader["Rol"].ToString()
                                        // Rol = (RolUsuario)Convert.ToInt32(reader["rolusuario"])
                                    };
            }
            return user;
        }
    }
}
/*

public Usuarios getUsuario(string nombreUsuario, string contrasenia)
    {
        SqliteConnection connection = new SqliteConnection(connectionString);
        Usuarios? usuario = null;
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM usuario WHERE nombre_usuario = @nombre AND password = @pass;";
        command.Parameters.Add(new SqliteParameter("@nombre", nombreUsuario));
        command.Parameters.Add(new SqliteParameter("@pass", contrasenia));
        connection.Open();
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                usuario = new Usuarios();
                usuario.Id = Convert.ToInt32(reader["id"]);
                usuario.NombreUsuario = reader["nombre_usuario"].ToString() ?? "";
                usuario.Password = reader["password"].ToString() ?? "";
                usuario.Rolusuario = (RolUsuario)Convert.ToInt32(reader["rolusuario"]);

            }
        }
        connection.Close();
        return usuario;
    }
*/