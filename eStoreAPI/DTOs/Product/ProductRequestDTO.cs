using System.ComponentModel.DataAnnotations;

namespace eStoreAPI.DTOs.Product
{
    public class ProductCreateRequestDTO
    {
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        [MaxLength(40, ErrorMessage = "Product Name length is 40")]
        public string ProductName { get; set; } = null!;
        [MaxLength(20, ErrorMessage = "Weight max length is 20")]
        public string Weight { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
    }
    public class ProductUpdateRequestDTO
    {
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        [MaxLength(40, ErrorMessage = "Product Name length is 40")]
        public string ProductName { get; set; } = null!;
        [Required(ErrorMessage = "Weight is required")]
        [MaxLength(20, ErrorMessage = "Weight max length is 20")]
        public string Weight { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
    }
}
