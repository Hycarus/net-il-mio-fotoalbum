using System;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Data
{
    public class CategoryManager
    {
        public static List<Photo> GetAllPhotos()
        {
            using PhotoContext db = new PhotoContext();
            return db.Photos.ToList();
        }

        public static Category GetCategory(int id, bool includeReferences = true)
        {
            using PhotoContext db = new PhotoContext();
            if (includeReferences)
                return db.Categories.Where(p => p.Id == id).Include(p => p.Photos).FirstOrDefault();
            return db.Categories.FirstOrDefault(p => p.Id == id);
        }

        public static void InsertCategory(Category category, List<string> selectedPhotos)
        {
            using PhotoContext db = new PhotoContext();
            category.Photos = new List<Photo>();
            if (selectedPhotos != null)
            {
                foreach (var photoId in selectedPhotos)
                {
                    int id = int.Parse(photoId);
                    var photo = db.Photos.FirstOrDefault(c => c.Id == id);
                    if (photo != null)
                        category.Photos.Add(photo);
                }
            }
            db.Categories.Add(category);
            db.SaveChanges();
        }

        public static bool UpdateCategory(int id, Category category, List<string> selectedPhotos)
        {
            try
            {
                using PhotoContext db = new PhotoContext();
                var categoryToEdit = db.Categories.Where(p => p.Id == id).Include(p => p.Photos).FirstOrDefault();
                if (categoryToEdit == null)
                    return false;
                categoryToEdit.Name = category.Name;
                categoryToEdit.Photos.Clear();
                if (selectedPhotos != null)
                {
                    foreach (var photo in selectedPhotos)
                    {
                        int photoId = int.Parse(photo);
                        var photoFromDb = db.Photos.FirstOrDefault(x => x.Id == photoId);
                        if (photoFromDb != null)
                            categoryToEdit.Photos.Add(photoFromDb);
                    }
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeleteCategory(int id)
        {
            using PhotoContext db = new PhotoContext();
            var category = db.Categories.FirstOrDefault(p => p.Id == id);
            if (category == null)
                return false;
            db.Categories.Remove(category);
            db.SaveChanges();
            return true;
        }
    }
}

