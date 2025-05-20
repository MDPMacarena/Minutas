using Microsoft.EntityFrameworkCore;
using MinutasManage.Models;

namespace MinutasManage.Repositories
{
    public class MinutasRepository
    {
        private readonly DbminutasContext Context;
        public MinutasRepository(DbminutasContext context)
        {
            Context = context;
        }

        public void Insert(Minutas entity)
        {
            Context.Minutas.Add(entity);
            Context.SaveChanges();
        }

        public IEnumerable<Minutas> GetAll()
        {
            return Context.Minutas
                .Include(e => e.IdDepartamentoNavigation)
                .Include(e => e.IdCreadorNavigation)
                .ToList();
        }
        public Minutas? Get(object id)
        {
            return Context.Minutas
                .Include(e => e.IdDepartamentoNavigation)
                .Include(e => e.IdCreadorNavigation)
                .FirstOrDefault(e => e.Id == (int)id);
        }


        public void Update(Minutas entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        public void Delete(Minutas entity)
        {
            Context.Remove(entity);
            Context.SaveChanges();
        }



    }
}
