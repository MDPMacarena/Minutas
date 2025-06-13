using MinutasManage.Repositories;
using System.Text;

namespace MinutasManage.Models.Validators
{
    public class PerfilValidator
    {
        public PerfilValidator(Repository<Usuarios> repository)
        {
            Repository = repository;
        }

        public Repository<Usuarios> Repository { get; }

        public bool ValidarPerfil(Usuarios empleado, out string errores)
        {
            var sbErrores = new StringBuilder();

            if (string.IsNullOrWhiteSpace(empleado.Nombre))
                sbErrores.AppendLine("El campo 'Nombre' no debe estar vacío, escribe un nombre.");
            else if (empleado.Nombre.Length > 100)
                sbErrores.AppendLine("El nombre del empleado excede la longitud máxima permitida.");

            if (string.IsNullOrWhiteSpace(empleado.Correo))
                sbErrores.AppendLine("El campo 'Correo electrónico' no debe estar vacío.");
            else
            {
                if (empleado.Correo.Length > 100)
                    sbErrores.AppendLine("El correo electrónico excede la longitud máxima permitida.");

                var emailValidator = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
                if (!emailValidator.IsValid(empleado.Correo))
                    sbErrores.AppendLine("El formato del correo electrónico no es válido.");

                var existeCorreo = Repository.GetAll()
                    .Any(u => u.Correo == empleado.Correo && u.Id != empleado.Id);
                if (existeCorreo)
                    sbErrores.AppendLine("El correo electrónico ingresado ya está registrado.");
            }

            errores = sbErrores.ToString();
            return errores.Length == 0;
        }
    }
}
