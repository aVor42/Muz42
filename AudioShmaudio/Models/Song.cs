using System.Collections.Generic;

namespace AudioShmaudio.Models
{
    public class Song
    {

        public Song()
        {
            Users = new List<User>();
            UserListenedSongs = new List<UserListenedSong>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Src { get; set; }
        public Artist Artist { get; set; }
        public List<User> Users { get; set; }
        public List<UserListenedSong> UserListenedSongs { get; set; }
    }
}
