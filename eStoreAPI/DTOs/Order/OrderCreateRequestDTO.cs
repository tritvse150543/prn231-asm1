using System.ComponentModel.DataAnnotations;

namespace eStoreAPI.DTOs.Order
{
    public class OrderCreateRequestDTO
    {
        public int? MemberId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }
        public List<BuyProductRequest> ProductIds { get; set; } = new List<BuyProductRequest>();
    }
    public class OrderUpdateRequestDTO
    {
        public int? MemberId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }
    }
}
