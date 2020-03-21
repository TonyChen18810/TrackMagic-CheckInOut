using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Firebase.Database;

namespace WebTrackMagic.Models
{
    public enum eDepartment
    {
        IT, Kitchen, Wait, Bar, Temp, Vendor
    }

    public class Staff
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        public string Title { get; set; }

        [Required]
        //[RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required]
        //public Dept? Department { get; set; }
        [Required]
        public eDepartment Department { get; set; }

        [Required]
        //[RegularExpression(@"^[0 - 9]{10}$", ErrorMessage = "Invalid phone format")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "PhotoUrl")]
        public string PhotoUrl { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize<Staff>(this);
        }


        public Staff() { }
        public Staff(string id, string title, string name, string email, string phoneNumber, eDepartment department, string photoUrl)
        {
            this.Id = id;
            this.Title = title;
            this.Name = name;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Department = department;
            this.PhotoUrl = photoUrl;
        }
        //public static explicit operator Staff(FirebaseObject<Staff> v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

