namespace MVC.Interfaces{
    public interface IAuthenticationService
    {
        bool Login(string username, string Password);
        void Logout();
        bool IsAuthenticated();
        // Verifica si el usuario actual tiene el rol requerido (ej. "Administrador").
        bool HasAccessLevel(string requiredAccessLevel);
    }
}
/* para la tabla User...
CREATE TABLE Usuarios (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    User TEXT NOT NULL UNIQUE,
    Pass TEXT NOT NULL,
    Rol TEXT NOT NULL
);

-- Usuario Administrador
INSERT INTO Usuarios (Nombre, User, Pass, Rol)
VALUES ('Juan Pérez', 'admin', 'admin123', 'Administrador');

-- Usuario Cliente
INSERT INTO Usuarios (Nombre, User, Pass, Rol)
VALUES ('María Gómez', 'cliente', 'cliente123', 'Cliente');

-- Usuario adicional (opcional)
INSERT INTO Usuarios (Nombre, User, Pass, Rol)
VALUES ('Pedro López', 'pedro', 'pass123', 'Cliente');

*/