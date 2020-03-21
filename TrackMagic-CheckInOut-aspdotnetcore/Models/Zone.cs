using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using System.Text.Json;

namespace WebTrackMagic.Models
{
    public class Zone
    {
        public string Id { get; set; }

        [Required]
        public eZoneType Type { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "PhotoUrl")]
        public string PhotoUrl { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize<Zone>(this);
        }

        public Zone() { }
        public Zone(string id, eZoneType type, string name, string description, string photoUrl)
        {
            this.Id = id;
            this.Type = type;
            this.Name = name;
            this.Description = description;
            this.PhotoUrl = photoUrl;
        }

    }
}
