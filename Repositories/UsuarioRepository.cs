using DistribuidoraInsumosMVC.Models;
using Microsoft.Data.Sqlite;
using System.Data;
using MVC.Interfaces;

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
                                    Id = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Username = reader.GetString(2),
                                    Password = reader.GetString(3),
                                    Rol = reader.GetString(4)
                                    };
            }
            return user;
        }
    }
}