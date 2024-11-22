
namespace PROG6212_Part1.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public string? Lecturer { get; set; }
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = "Pending";
        public string? DocumentPath { get; set; }

      

        // Automatically calculate the final payment
        public decimal FinalPayment => HoursWorked * HourlyRate;
    }

}
