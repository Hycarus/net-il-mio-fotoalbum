using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {

        [Authorize(Roles = "PHOTOGRAPHER, ADMIN")]
        public IActionResult Index()
        {
            if (User.IsInRole("ADMIN"))
            {
                return View(PhotoManager.GetAllPhotos());
            }
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(PhotoManager.GetAllPhotographerPhotos(id));
        }

        public IActionResult UserIndex(string id)
        {
            return View(PhotoManager.GetAllPhotographerPhotos(id));
        }

        [Authorize(Roles = "PHOTOGRAPHER, ADMIN")]
        public IActionResult Details(int id)
        {
            var photo = PhotoManager.GetPhoto(id, true);
            if (photo != null)
                return View(photo);
            else
                return View("errore");
        }

        [Authorize(Roles = "PHOTOGRAPHER, ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            PhotoFormModel model = new PhotoFormModel();
            model.Photo = new Photo();
            model.CreateCategories();
            return View(model);
        }

        [Authorize(Roles = "PHOTOGRAPHER, ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhotoFormModel data)
        {
            data.Photo.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                data.CreateCategories();
                return View("Create", data);
            }
            data.SetImageFileFromFormFile();
            PhotoManager.InsertPhoto(data.Photo, data.SelectedCategories);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "PHOTOGRAPHER, ADMIN")]
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

        [Authorize(Roles = "PHOTOGRAPHER, ADMIN")]
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

        [Authorize(Roles = "PHOTOGRAPHER, ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Visibility(int id, PhotoFormModel data)
        {
            
            if (data.Photo.IsVisible)
                data.Photo.IsVisible = true;
            else
                data.Photo.IsVisible = false;
            var photoToEdit = PhotoManager.UpdateVisibility(id, data.Photo.IsVisible);
            if (photoToEdit)
                return RedirectToAction("Index");
            else
                return NotFound();
        }

        [Authorize(Roles = "PHOTOGRAPHER, ADMIN")]
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