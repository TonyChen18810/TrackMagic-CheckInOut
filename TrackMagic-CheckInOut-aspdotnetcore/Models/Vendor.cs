using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using System.Text.Json;

namespace WebTrackMagic.Models
{
    public class Vendor
    {
        public string Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Contact { get; set; }

        public string CompanyName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public eVendorType Type { get; set; }

        [Required]
        //[RegularExpression(@"^[0 - 9]{10}$", ErrorMessage = "Invalid phone format")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "PhotoUrl")]
        public string PhotoUrl { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize<Vendor>(this);
        }

        public Vendor() { }
        public Vendor(string id, string contact, string companyName, string email, string phoneNumber, eVendorType type, string photoUrl)
        {
            this.Id = id;
            this.Contact = contact;
            this.CompanyName = companyName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Type = type;
            this.PhotoUrl = photoUrl;
        }

    }
}
