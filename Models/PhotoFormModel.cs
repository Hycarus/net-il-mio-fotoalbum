using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using net_il_mio_fotoalbum.Data;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoFormModel
    {
        public Photo Photo { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<string>? SelectedCategories { get; set; }

        public IFormFile? ImageFormFile { get; set; }

        public PhotoFormModel()
        {
        }
        public PhotoFormModel(Photo p)
        {
            this.Photo = p;
        }

        public void CreateCategories()
        {
            this.Categories = new List<SelectListItem>();
            this.SelectedCategories = new List<string>();
            var categoriesFromDB = PhotoManager.GetAllCategories();
            foreach (var category in categoriesFromDB) 
            {
                bool isSelected = this.Photo.Categories?.Any(t => t.Id == category.Id) == true;
                this.Categories.Add(new SelectListItem() 
                {
                    Text = category.Name, 
                    Value = category.Id.ToString(), 
                    Selected = isSelected 
                });
                if (isSelected)
                    this.SelectedCategories.Add(category.Id.ToString()); 
            }
        }

        public byte[] SetImageFileFromFormFile()
        {
            if (ImageFormFile == null)
                return null;

            using var stream = new MemoryStream();
            this.ImageFormFile?.CopyTo(stream);
            Photo.Image = stream.ToArray();

            return Photo.Image;
        }
    }
}

