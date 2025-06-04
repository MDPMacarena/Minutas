namespace MinutasManage.Models.ViewModels
{
    public class SignUpViewModel
    {

        public int IdDepartamento { get; set; }

        public string NumEmpleado { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string ContraseñaHash { get; set; } = null!;

        public DateOnly FechaNacimiento { get; set; }

    }
}
