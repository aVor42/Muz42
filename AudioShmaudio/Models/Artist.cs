using System.Collections.Generic;

namespace AudioShmaudio.Models
{
    public class Artist
    {

        public Artist()
        {
            Songs = new List<Song>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public List<Song> Songs { get; set; }
    }
}
