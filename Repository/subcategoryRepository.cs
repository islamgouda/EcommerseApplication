using EcommerseApplication.Models;

namespace EcommerseApplication.Repository
{
    public class subcategoryRepository : IsubCategory
    {
        Context context;
        public subcategoryRepository(Context _context)
        {
            this.context = _context;
        }
        public void delete(int id)
        {
           subCategory subCat= context.subCategories.FirstOrDefault(sub => sub.Id == id);
            context.subCategories.Remove(subCat);
            context.SaveChanges();
        }

        public List<subCategory> getAll()
        {
            return context.subCategories.ToList();
        }

        public List<subCategory> getAllByCategoryID(int categoryID)
        {
            return context.subCategories.Where(sub=>sub.CategoryId == categoryID).ToList();
        }

        public subCategory getByID(int id)
        {
            return context.subCategories.FirstOrDefault(sub => sub.Id == id);
        }

        public subCategory getByName(string name)
        {

            return context.subCategories.FirstOrDefault(sub => sub.Name == name);
        }

        public void insert(subCategory subCategory)
        {
            context.subCategories.Add(subCategory);
            context.SaveChanges();
        }

        public void update(int id, subCategory subCategory)
        {
            subCategory old= context.subCategories.FirstOrDefault(sub => sub.Id == id);
            old.Name=subCategory.Name;
            old.image = subCategory.image;
            old.arabicName=subCategory.arabicName;
            old.arabicDescription=subCategory.arabicDescription;
            old.CategoryId=subCategory.CategoryId;
           old.Description=subCategory.Description;
            context.SaveChanges();
        }
    }
}
