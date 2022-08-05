using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public class PartenerRepository : Ipartener
    {
        Context context;
        public PartenerRepository(Context _context)
        {
            this.context = _context;
        }
        public void delete(int id)
        {
            Partener part = context.Parteners.FirstOrDefault(p => p.Id == id);
            
            context.Parteners.Remove(part);
            context.SaveChanges();
        }

        public List<Partener> getAll()
        {
         return  context.Parteners.ToList();
        }

        public Partener getByID(int id)
        {
            return context.Parteners.FirstOrDefault(e=>e.Id==id);
        }

        public Partener getByName(string name)
        {
            return context.Parteners.FirstOrDefault(e => e.Name==name);
        }

        public void insert(Partener partener)
        {
            context.Parteners.Add(partener);
            context.SaveChanges();
        }
        public Partener getByUserID(int id)
        {
            return context.Parteners.FirstOrDefault(p => p.userID==id);
        }
        public void update(int id, Partener partener)
        {
            Partener old = context.Parteners.FirstOrDefault(p => p.Id == id);
            old.Name= partener.Name;
            old.addressID= partener.addressID;
            old.userID= partener.userID;
            old.numberOfBranches=partener.numberOfBranches;
            old.Type= partener.Type;
            context.SaveChanges();
        }
    }
}
