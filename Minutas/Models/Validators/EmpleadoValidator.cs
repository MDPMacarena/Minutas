using MinutasManage.Repositories;
using System.Text;

namespace MinutasManage.Models.Validators
{
    public class EmpleadoValidator
    {
        private readonly Repository<Usuarios> repository;

        public EmpleadoValidator(Repository<Usuarios> repository)
        {
            this.repository = repository;
        }

        public bool ValidarEmpleado(Usuarios empleado, out string errores, out string avisos)
        {
            var sbErrores = new StringBuilder();
            var sbAvisos = new StringBuilder();

            if (empleado.IdRol == 0)
                sbAvisos.AppendLine("El rol del empleado será asignado posteriormente.");

            if (string.IsNullOrWhiteSpace(empleado.NumEmpleado))
            {
                sbErrores.AppendLine("El campo 'Número de empleado' no debe estar vacío,escribe un numero de empleado.");
            }
            else
            {
                if (empleado.NumEmpleado.Length > 20)
                {
                    sbErrores.AppendLine("El número de empleado excede la longitud máxima permitida.Por favor, verifique e intente de nuevo.");
                }

                if (empleado.NumEmpleado.Length < 4)
                {
                    sbErrores.AppendLine("El número de empleado debe contener al menos 4 caracteres.Por favor, verifique e intente de nuevo.");
                }

                var existeNumEmpleado = repository.GetAll()
                    .Any(u => u.NumEmpleado == empleado.NumEmpleado && u.Id != empleado.Id);
                if (existeNumEmpleado)
                    sbErrores.AppendLine("El número de empleado ingresado ya se encuentra registrado en el sistema.");
            }


            if (string.IsNullOrWhiteSpace(empleado.Nombre))
                sbErrores.AppendLine("El campo 'Nombre' no debe estar vacío, escribe un nombre.");
            else
            {
                // Validación nueva: longitud del nombre
                if (empleado.Nombre.Length > 100)
                    sbErrores.AppendLine("El nombre del empleado excede la longitud máxima permitida.Por favor, verifique e intente de nuevo.");
            }

            if (string.IsNullOrWhiteSpace(empleado.Correo))
            {
                sbErrores.AppendLine("El campo 'Correo electrónico' no debe estar vacío, escribe un correo electronico.");
            }
            else
            {
                if (empleado.Correo.Length > 100)
                {
                    sbErrores.AppendLine("El correo electrónico excede la longitud máxima permitida.Por favor, verifique e intente de nuevo.");
                }

                // Validar formato del correo
                var emailValidator = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
                if (!emailValidator.IsValid(empleado.Correo))
                    sbErrores.AppendLine("El formato del correo electrónico no es válido, escribe un correo valido.");

                var existeCorreo = repository.GetAll()
                    .Any(u => u.Correo == empleado.Correo && u.Id != empleado.Id);
                if (existeCorreo)
                    sbErrores.AppendLine("El correo electrónico ingresado ya está registrado en el sistema.Por favor, verifique e intente de nuevo.");
            }

            if (empleado.IdDepartamento <= 0)
                sbErrores.AppendLine("Debe seleccionar un departamento válido.");

            if (empleado.FechaNacimiento == DateOnly.MinValue)
            {
                sbErrores.AppendLine("La fecha de nacimiento no puede estar vacía,escriba una fecha.");
            }
            else if (empleado.FechaNacimiento > DateOnly.FromDateTime(DateTime.Today))
            {
                sbErrores.AppendLine("La fecha de nacimiento no puede ser posterior a la fecha actual.Por favor, verifique e intente de nuevo.");
            }




            errores = sbErrores.ToString();
            avisos = sbAvisos.ToString();

            return errores.Length == 0;
        }
    }
}
