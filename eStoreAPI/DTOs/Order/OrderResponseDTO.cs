namespace eStoreAPI.DTOs.Order
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public int? MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }
        public String MemberName { get; set; } = "";
        public List<OrderDetailDTO> orderDetailDTOs { get; set; } = new List<OrderDetailDTO>();
    }

    public class OrderReportDTO : OrderResponseDTO
    {
        public decimal TotalAmount { get;set; }
    }

    public class OrderDetailDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
    }
}
