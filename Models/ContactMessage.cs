using System;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
	public class ContactMessage
	{
        [Key] public int Id { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

        public ContactMessage()
		{
		}
	}
}

