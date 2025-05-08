using Microsoft.EntityFrameworkCore;
using MinutasManage.Models;
using MinutasManage.Repositories;
using System.Text;

public class DepartamentoRepository
{
    private readonly DbminutasContext Context;

    public DepartamentoRepository(DbminutasContext context)
    {
        Context = context;
    }

    public void Insert(Departamento dep)
    {
        Context.Departamento.Add(dep);
        Context.SaveChanges();
    }

    public IEnumerable<Departamento> GetAll()
    {
        return Context.Departamento
                      .Include(d => d.IdJefeNavigation)
                      .Include(d => d.IdDeptSuperiorNavigation)
                      .ToList();
    }

    public Departamento? Get(object id)
    {
        return Context.Departamento
                      .Include(d => d.IdJefeNavigation)
                      .Include(d => d.IdDeptSuperiorNavigation)
                      .FirstOrDefault(d => d.Id == (int)id);
    }

    public void Update(Departamento dep)
    {
        Context.Departamento.Update(dep);
        Context.SaveChanges();
    }

    public void Delete(Departamento dep)
    {
        Context.Departamento.Remove(dep);
        Context.SaveChanges();
    }

    public void Eliminar(Departamento dep)
    {
        if (dep.Minutas.Any() || dep.Usuarios.Any() || dep.InverseIdDeptSuperiorNavigation.Any())
        {
            dep.Activo = false;
            Update(dep); // borrado lógico
        }
        else
        {
            Delete(dep); // borrado físico
        }
    }

    public void EditarDepartamento(Departamento dep)
    {
        var departamento = Get(dep.Id);
        if (departamento != null)
        {
            departamento.Nombre = dep.Nombre;
            departamento.IdJefe = dep.IdJefe;
            departamento.IdDeptSuperior = dep.IdDeptSuperior;
            Update(departamento);
        }
    }

    public IEnumerable<Departamento> GetDepartamentosActivos()
    {
        return Context.Departamento
                      .Include(d => d.IdJefeNavigation)
                      .Include(d => d.IdDeptSuperiorNavigation)
                      .Where(d => d.Activo.HasValue && d.Activo.Value)
                      .OrderBy(d => d.Nombre)
                      .ToList();
    }

    public bool ValidarDepartamento(Departamento dep, out string errores, out string avisos)
    {
        var sbErrores = new StringBuilder();
        var sbAvisos = new StringBuilder();

        if (string.IsNullOrWhiteSpace(dep.Nombre))
            sbErrores.AppendLine("El nombre del departamento está vacío.");

        if (dep.IdJefe <= 0)
            sbErrores.AppendLine("El Id del jefe no es válido.");

        if (dep.IdDeptSuperior.HasValue && dep.IdDeptSuperior <= 0)
            sbErrores.AppendLine("El Id del departamento superior no es válido.");

        // Opcional: validar que no se autorefiera
        if (dep.IdDeptSuperior == dep.Id)
            sbErrores.AppendLine("El departamento no puede ser su propio superior.");

        errores = sbErrores.ToString();
        avisos = sbAvisos.ToString();

        return errores.Length == 0;
    }
}

