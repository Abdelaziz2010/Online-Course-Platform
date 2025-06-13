namespace EduPlatform.Domain.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int EnrollmentId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;
    
    // Stripe specific property
    public string? StripePaymentIntentId { get; set; }

    public virtual Enrollment Enrollment { get; set; } = null!;
}
