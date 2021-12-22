using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AudioShmaudio.Models
{
    public class ApplicationContext: IdentityDbContext<User>
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options):
            base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<UserListenedSong> UserListenedSongs { get; set; }

    }
}
