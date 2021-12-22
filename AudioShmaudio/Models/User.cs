using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AudioShmaudio.Models
{
    public class User: IdentityUser
    {
        public User()
        {
            Songs = new List<Song>();
            UserListenedSongs = new List<UserListenedSong>();
        }

        public string Login { get; set; }
        public List<Song> Songs { get; set; }
        public List<UserListenedSong> UserListenedSongs { get; set; }
    }
}
