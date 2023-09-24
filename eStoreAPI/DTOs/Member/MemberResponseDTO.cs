using System.ComponentModel.DataAnnotations;

namespace eStoreAPI.DTOs.Member
{
    public class MemberResponseDTO
    {
        public int MemberId { get; set; }
        public string Email { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class MemberCreateRequestDTO
    {
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }

        [MaxLength(40, ErrorMessage ="Companyname Length error")]
        [Required(ErrorMessage = "CompanyName is required")]
        public string CompanyName { get; set; }
        
        [MaxLength(15,ErrorMessage = "City Length error")]
        [Required(ErrorMessage = "City is required")]
       
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(15, ErrorMessage = "Country Length error")]
        public string Country { get; set; } 

        [Required(ErrorMessage = "Password length error")]
        public string Password { get; set; } = "Default password"; 
    }
    public class MemberUpdateRequestDTO
    {
        public int id { get; set; }
        [MaxLength(40, ErrorMessage = "Companyname Length error")]
        [Required(ErrorMessage = "CompanyName is required")]
        public string CompanyName { get; set; }

        [MaxLength(15, ErrorMessage = "City Length error")]
        [Required(ErrorMessage = "City is required")]

        public string City { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(15, ErrorMessage = "Country Length error")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Password length error")]
        public string Password { get; set; } = "Default password";
    }
}
