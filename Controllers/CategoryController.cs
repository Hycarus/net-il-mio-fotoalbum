using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;

namespace net_il_mio_fotoalbum.Controllers
{
    public class CategoryController : Controller
    {

        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            return View(PhotoManager.GetAllCategories());
        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            CategoryFormModel model = new CategoryFormModel();
            model.Category = new Category();
            model.CreatePhotos();
            return View(model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryFormModel data)
        {
            if (!ModelState.IsValid)
            {
                data.CreatePhotos();
                return View("Create", data);
            }

            CategoryManager.InsertCategory(data.Category, data.SelectedPhotos);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Edit(int id)
        {

            var categoryToEdit = CategoryManager.GetCategory(id);

            if (categoryToEdit == null)
            {
                return NotFound();
            }
            else
            {
                CategoryFormModel model = new CategoryFormModel(categoryToEdit);
                model.CreatePhotos();
                return View(model);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CategoryFormModel data)
        {

            if (!ModelState.IsValid)
            {
                data.CreatePhotos();
                return View("Edit", data);
            }

            var categoryToEdit = CategoryManager.UpdateCategory(id, data.Category, data.SelectedPhotos);

            if (categoryToEdit)
                return RedirectToAction("Index");
            else
                return NotFound();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (CategoryManager.DeleteCategory(id))
                return RedirectToAction("Index");
            else
                return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}