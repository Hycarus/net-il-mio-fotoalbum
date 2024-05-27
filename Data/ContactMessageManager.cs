using System;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Data
{
	public class ContactMessageManager
	{
        public static void InsertMessage(ContactMessage message)
        {
            using PhotoContext db = new PhotoContext();
            db.Messages.Add(message);
            db.SaveChanges();
        }
    }
}

