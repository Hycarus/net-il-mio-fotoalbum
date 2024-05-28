using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Data
{
	public class PhotoManager
	{
		public static List<Photo> GetAllPhotographerPhotos(string id)
		{
			using PhotoContext db = new PhotoContext();
			return db.Photos.Where(x => x.OwnerId == id).Include(p => p.Categories).ToList();
		}

        public static List<Photo> GetAllPhotos()
        {
            using PhotoContext db = new PhotoContext();
            return db.Photos.Include(p => p.Categories).ToList();
        }

        public static List<Photo> GetAllVisiblePhotos()
        {
            using PhotoContext db = new PhotoContext();
            return db.Photos.Where(p => p.IsVisible).Include(p => p.Categories).ToList();
        }

        public static Photo GetPhoto(int id, bool includeReferences = true)
		{
			using PhotoContext db = new PhotoContext();
			if (includeReferences)
				return db.Photos.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();
			return db.Photos.FirstOrDefault(p => p.Id == id);
		}

		public static List<Category> GetAllCategories()
		{
            using PhotoContext db = new PhotoContext();
			return db.Categories.ToList();
        }

		public static void InsertPhoto(Photo photo, List<string> selectedCategories)
		{
            using PhotoContext db = new PhotoContext();
			photo.Categories = new List<Category>();
			if(selectedCategories != null)
			{
				foreach(var categoryId in selectedCategories)
				{
					int id = int.Parse(categoryId);
					var category = db.Categories.FirstOrDefault(c => c.Id == id);
					if (category != null)
						photo.Categories.Add(category);
				}
			}
			db.Photos.Add(photo);
			db.SaveChanges();
        }

        public static bool UpdateVisibility(int id, bool visible)
        {
            try
            {
                using PhotoContext db = new PhotoContext();
                var photoToEdit = db.Photos.Where(x => x.Id == id).FirstOrDefault();
                if(photoToEdit == null)
                {
                    return false;
                }
                photoToEdit.IsVisible = visible;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool UpdatePhoto(int id, Photo photo, List<string> selectedCategories)
        {
            try
            {
                using PhotoContext db = new PhotoContext();
                var photoToEdit = db.Photos.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();
                if (photoToEdit == null)
                    return false;
                photoToEdit.Title = photo.Title;
                photoToEdit.Description = photo.Description;
                photoToEdit.IsVisible = photo.IsVisible;
                if(photo.Image != null)
                    photoToEdit.Image = photo.Image;
                photoToEdit.Categories.Clear();
                if (selectedCategories != null)
                {
                    foreach (var category in selectedCategories)
                    {
                        int categoryId = int.Parse(category);
                        var categoryFromDb = db.Categories.FirstOrDefault(x => x.Id == categoryId);
                        if (categoryFromDb != null)
                            photoToEdit.Categories.Add(categoryFromDb);
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

        public static bool DeletePhoto(int id)
        {
            using PhotoContext db = new PhotoContext();
            var photo = db.Photos.FirstOrDefault(p => p.Id == id);
            if (photo == null)
                return false;
            db.Photos.Remove(photo);
            db.SaveChanges();
            return true;
        }

        public static List<Photo> GetPhotosByTitle(string name)
        {
            using PhotoContext db = new PhotoContext();
            return db.Photos.Where(p => p.Title.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}

