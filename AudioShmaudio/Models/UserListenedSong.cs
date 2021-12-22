namespace AudioShmaudio.Models
{
    public class UserListenedSong
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Song Song { get; set; }
        public int Count { get; set; }
    }
}
