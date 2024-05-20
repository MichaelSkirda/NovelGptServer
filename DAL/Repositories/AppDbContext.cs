using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<MessageDto> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
            if(Database.EnsureCreated())
            {
				User admin = new User()
				{
					Email = "adminmail@rensharp.com",
					Password = "r3NSh@r33r#"
				};
				Users.Add(admin);
				SaveChanges();
			}
            
		}


    }
}
