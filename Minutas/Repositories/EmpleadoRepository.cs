﻿using Microsoft.EntityFrameworkCore;
using MinutasManage.Models;
using System.Text;

namespace MinutasManage.Repositories
{
    public class EmpleadoRepository
    {
        private readonly DbminutasContext Context;

        public EmpleadoRepository(DbminutasContext context)
        {
            Context = context;
        }

        public void Insert(Usuarios entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public IEnumerable<Usuarios> GetAll()
        {
            return Context.Usuarios.Include(e => e.IdDepartamentoNavigation);
        }

        public Usuarios? Get(object id)
        {
            return Context.Usuarios
                .Include(e => e.IdDepartamentoNavigation)
                .FirstOrDefault(e => e.Id == (int)id);
        }

        public void Update(Usuarios entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        public void Delete(Usuarios entity)
        {
            Context.Remove(entity);
            Context.SaveChanges();
        }

        public bool ValidarEmpleado(Usuarios empleado, out string errores, out string avisos)
        {
            var sbErrores = new StringBuilder();
            var sbAvisos = new StringBuilder();

            if (empleado.IdRol == 0)
                sbAvisos.AppendLine("El rol será asignado más tarde.");

            if (string.IsNullOrWhiteSpace(empleado.NumEmpleado))
            {
                sbErrores.AppendLine("El número de empleado está vacío.");
            }
            else
            {
                var existeNumEmpleado = Context.Usuarios
                    .Any(u => u.NumEmpleado == empleado.NumEmpleado && u.Id != empleado.Id);
                if (existeNumEmpleado)
                    sbErrores.AppendLine("El número de empleado ya está registrado.");
            }

            if (string.IsNullOrWhiteSpace(empleado.Nombre))
                sbErrores.AppendLine("El nombre está vacío.");

            if (string.IsNullOrWhiteSpace(empleado.Correo))
            {
                sbErrores.AppendLine("El correo está vacío.");
            }
            else
            {
                // Validar formato del correo
                var emailValidator = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
                if (!emailValidator.IsValid(empleado.Correo))
                    sbErrores.AppendLine("El formato del correo no es válido.");

                var existeCorreo = Context.Usuarios
                    .Any(u => u.Correo == empleado.Correo && u.Id != empleado.Id);
                if (existeCorreo)
                    sbErrores.AppendLine("El correo ya está registrado.");
            }

            if (empleado.IdDepartamento <= 0)
                sbErrores.AppendLine("El ID del departamento no es válido.");

            errores = sbErrores.ToString();
            avisos = sbAvisos.ToString();

            return errores.Length == 0;
        }

        public void Eliminar(Usuarios empleado)
        {
            if (empleado.Minutas.Any() || empleado.MinutaUsuario.Any())
            {
                empleado.Activo = false;
                Update(empleado);
            }
            else
            {
                Delete(empleado);
            }
        }

        public void EditarEmpleado(Usuarios empleado)
        {
            var emp = Get(empleado.Id);
            if (emp != null)
            {
                emp.NumEmpleado = empleado.NumEmpleado;
                emp.Nombre = empleado.Nombre;
                emp.Correo = empleado.Correo;
                emp.IdDepartamento = empleado.IdDepartamento;
                emp.IdRol = empleado.IdRol;
                emp.FechaNacimiento = empleado.FechaNacimiento;


                Update(emp);
            }
        }

        public IEnumerable<Usuarios> GetEmpleadosActivos()
        {
            return Context.Usuarios
                .Include(e => e.IdDepartamentoNavigation)
                .Where(e => e.Activo == true)
                .OrderBy(e => e.Nombre);
        }
    }

}
