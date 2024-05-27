using System;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
	public class Photo
	{
		[Key] public int Id { get; set; }
		[Required(ErrorMessage = "Il titolo per la foto è richiesto")]
		public string Title { get; set; }
		[Required(ErrorMessage = "La descrizione per la foto è richiesta")]
		public string Description { get; set; }
		//[Required(ErrorMessage = "E' obbligatorio inserire una foto")]
		public byte[]? Image { get; set; }
        public string ImgSrc => Image != null ? $"data:image/png;base64,{Convert.ToBase64String(Image)}" : "";
        public bool IsVisible { get; set; }

		public List<Category>? Categories { get; set; }
		public Photo()
		{
		}
	}
}

