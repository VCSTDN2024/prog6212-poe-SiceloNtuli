using Microsoft.AspNetCore.Mvc;
using PROG6212_Part1.Models;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace PROG6212_Part1.Controllers
{
    public class HRController : Controller
    {
        // Shared list of claims from LecturerController
        private static List<Claim> claims = LecturerController.claims;

        // Generate PDF Report for Approved Claims
        public IActionResult GenerateReport()
        {
            var approvedClaims = claims.Where(c => c.Status == "Approved").ToList();
            if (!approvedClaims.Any())
            {
                ViewBag.Message = "No approved claims to generate a report.";
                return View("HRDashboard");
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document doc = new Document();
                PdfWriter.GetInstance(doc, memoryStream).CloseStream = false;
                doc.Open();

                doc.Add(new Paragraph("Approved Claims Report:"));
                doc.Add(new Paragraph(" "));

                // Updated table with 6 columns (added "Contact Details")
                PdfPTable table = new PdfPTable(6);
                table.AddCell("Claim ID:");
                table.AddCell("Lecturer");
                table.AddCell("Contact Details:");
                table.AddCell("Hours Worked:");
                table.AddCell("Hourly Rate:");
                table.AddCell("Final Payment:");

                foreach (var claim in approvedClaims)
                {
                    table.AddCell(claim.ClaimId.ToString());
                    table.AddCell(claim.Lecturer);
                    table.AddCell(claim.Notes ?? "N/A"); // Add contact details
                    table.AddCell(claim.HoursWorked.ToString());
                    table.AddCell("R " + claim.HourlyRate.ToString("N2")); // Format as Rands
                    table.AddCell("R " + claim.FinalPayment.ToString("N2"));
                }

                doc.Add(table);
                doc.Close();

                memoryStream.Position = 0;
                return File(memoryStream.ToArray(), "application/pdf", "ApprovedClaimsReport.pdf");
            }
        }


        public class LecturerUpdateModel
        {
            public string LecturerName { get; set; }
            public string NewLecturerName { get; set; } // For updating lecturer's name
            public decimal? NewHourlyRate { get; set; } // For updating hourly rate
            public string NewContactInfo { get; set; }  // For updating contact details
        }


        // Action to update lecturer information
        public IActionResult UpdateLecturerInfo()
        {
            var lecturers = claims.Select(c => c.Lecturer).Distinct();
            return View(lecturers);
        }

        [HttpPost]
        public IActionResult UpdateLecturerInfo(LecturerUpdateModel updateModel)
        {
            var lecturerClaims = claims.Where(c => c.Lecturer == updateModel.LecturerName).ToList();

            if (lecturerClaims.Any())
            {
                foreach (var claim in lecturerClaims)
                {
                    if (!string.IsNullOrEmpty(updateModel.NewLecturerName))
                        claim.Lecturer = updateModel.NewLecturerName;

                    if (updateModel.NewHourlyRate.HasValue)
                        claim.HourlyRate = updateModel.NewHourlyRate.Value;

                    if (!string.IsNullOrEmpty(updateModel.NewContactInfo))
                        claim.Notes = updateModel.NewContactInfo;  // Assuming 'Notes' stores contact info or similar data.
                }
            }

            // Return to dashboard or a confirmation view
            return RedirectToAction("HRDashboard");
        }


        public IActionResult HRDashboard()
        {
            return View(claims);
        }
    }
}
