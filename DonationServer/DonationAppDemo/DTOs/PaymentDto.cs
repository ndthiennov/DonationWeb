namespace DonationAppDemo.DTOs
{
    public class PaymentRequestDto
    {
        public int CampaignId { get; set; }
        public int UserId { get; set; }
        public string UserRole { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
    public class PaymentResponseDto
    {
        public bool? PaymentResponse { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? PaymentDescription { get; set; }
        public string? PaymentOrderId { get; set; } //order_token of zalopay
        public string? PaymentTransactionId { get; set; } //zp_trans_token of zalopay
        public string? PaymentToken { get; set; }
        public DateTime PaymentDate { get; set; } //yyyymmdd
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public int CampaignId { get; set; }

    }
}
