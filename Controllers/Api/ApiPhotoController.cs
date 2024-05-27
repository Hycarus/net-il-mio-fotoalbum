
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace net_il_mio_fotoalbum.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiPhotoController : ControllerBase
    {
        // GET: api/values
        [HttpGet]
        public IActionResult GetAllPhotos(string? name)
        {
            if (name == null)
                return Ok(PhotoManager.GetAllVisiblePhotos());
            return Ok(PhotoManager.GetPhotosByTitle(name));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetPhotoById(int id)
        {
            var photo = PhotoManager.GetPhoto(id);
            if (photo == null)
                return NotFound();
            return Ok(photo);
        }

        [HttpGet("{name}")]
        public IActionResult GetPhotoByTitle(string name)
        {
            var photo = PhotoManager.GetPhotosByTitle(name);
            if (photo == null)
                return NotFound("ERRORE");
            return Ok(photo);
        }


        // POST api/values
        [HttpPost]
        public IActionResult CreatePhoto([FromBody] Photo photo)
        {
            PhotoManager.InsertPhoto(photo, null);
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult UpdatePhoto(int id, [FromBody] PhotoFormModel data)
        {
            var photoToEdit = PhotoManager.UpdatePhoto(id, data.Photo, data.SelectedCategories);
            if (photoToEdit != null)
            {
                return Ok(photoToEdit);
            }
            else
                return NotFound();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult DeletePhoto(int id)
        {
            if (PhotoManager.DeletePhoto(id))
                return Ok();
            else
                return NotFound();
        }

        [HttpPost]
        public IActionResult CreateContactMessage([FromBody] ContactMessage message)
        {
            ContactMessageManager.InsertMessage(message);
            return Ok();
        }
    }
}

