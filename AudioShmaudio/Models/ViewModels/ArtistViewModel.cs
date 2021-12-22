using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace AudioShmaudio.Models
{
    public class ArtistViewModel
    {

        public ArtistViewModel()
        {
            Songs = new List<Song>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Prompt = "Название")]
        public string Name { get; set; }

        [Required]
        [Display(Prompt = "Описание")]
        public string Description { get; set; }

        public IFormFile Photo { get; set; }

        public List<Song> Songs { get; set; }

    }
}
