using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


namespace AudioShmaudio.Models
{
    public class SongViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Prompt = "Название")]
        public string Name { get; set; }

        [Required]
        [Display(Prompt = "Артист")]
        public int ArtistId { get; set; }

        [Required]
        [Display(Name = "Длительность", Prompt = "0:00")]
        public string Duration { get; set; }

        public bool IsSongInPlaylist { get; set; }

        public IFormFile File { get; set; }

        public IEnumerable<Artist> Artists { get; set; }
    }
}