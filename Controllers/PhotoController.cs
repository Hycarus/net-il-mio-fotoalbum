using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {

        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            return View(PhotoManager.GetAllPhotos());
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Details(int id)
        {
            var photo = PhotoManager.GetPhoto(id, true);
            if (photo != null)
                return View(photo);
            else
                return View("errore");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            PhotoFormModel model = new PhotoFormModel();
            model.Photo = new Photo();
            model.CreateCategories();
            return View(model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhotoFormModel data)
        {
            if (!ModelState.IsValid)
            {
                data.CreateCategories();
                return View("Create", data);
            }
            data.SetImageFileFromFormFile();
            PhotoManager.InsertPhoto(data.Photo, data.SelectedCategories);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Edit(int id)
        {

            var photoToEdit = PhotoManager.GetPhoto(id);

            if (photoToEdit == null)
            {
                return NotFound();
            }
            else
            {
                PhotoFormModel model = new PhotoFormModel(photoToEdit);
                model.CreateCategories();
                return View(model);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PhotoFormModel data)
        {

            if (!ModelState.IsValid)
            {
                data.CreateCategories();
                return View("Edit", data);
            }
            data.SetImageFileFromFormFile();
            var photoToEdit = PhotoManager.UpdatePhoto(id, data.Photo, data.SelectedCategories);

            if (photoToEdit)
                return RedirectToAction("Index");
            else
                return NotFound();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (PhotoManager.DeletePhoto(id))
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