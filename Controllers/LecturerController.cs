using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PROG6212_Part1.Models;

namespace PROG6212_Part1.Controllers
{
    public class LecturerController : Controller
    {
        // Shared list of claims
        public static List<Claim> claims = new List<Claim>();

        // Define allowed file types and file size limit (5MB)
        private static readonly string[] AllowedFileExtensions = { ".pdf", ".docx", ".xlsx" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        // Action to load the Submit and Track Claim page
        public IActionResult SubmitAndTrackClaim()
        {
            return View(claims); // Pass the claims data to the view
        }

        // POST: Action to handle claim submission with file upload
        [HttpPost]
        public IActionResult SubmitClaim(string lecturer, int hoursWorked, decimal hourlyRate, string notes, IFormFile document)
        {
            // Validate input fields
            if (hoursWorked <= 0 || hourlyRate <= 0 || document == null)
            {
                ViewBag.ErrorMessage = "Please provide valid data and upload a document.";
                return View("SubmitAndTrackClaim", claims);
            }

            // Validate file type and size
            var fileExtension = Path.GetExtension(document.FileName).ToLower();
            if (!AllowedFileExtensions.Contains(fileExtension))
            {
                ViewBag.ErrorMessage = "Invalid file type. Only .pdf, .docx, and .xlsx files are allowed.";
                return View("SubmitAndTrackClaim", claims);
            }
            if (document.Length > MaxFileSize)
            {
                ViewBag.ErrorMessage = "File size exceeds 5MB.";
                return View("SubmitAndTrackClaim", claims);
            }

            // Generate a new claim ID
            int newClaimId = claims.Count + 1;

            // Define the storage path 
            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            // Define the file path
            string uniqueFileName = $"{newClaimId}_{Path.GetFileName(document.FileName)}";
            string filePath = Path.Combine(uploadFolder, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                document.CopyTo(fileStream);
            }

            // Add the claim to the list
            claims.Add(new Claim
            {
                ClaimId = newClaimId,
                Lecturer = lecturer,
                HoursWorked = hoursWorked,
                HourlyRate = hourlyRate,
                Notes = notes,
                DocumentPath = $"/uploads/{uniqueFileName}", // Store relative path
                Status = "Pending"
            });

            // Redirect to the Submit and Track Claim page
            return RedirectToAction("SubmitAndTrackClaim");
        }
    }
}
