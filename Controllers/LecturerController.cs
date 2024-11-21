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
            if (string.IsNullOrWhiteSpace(lecturer))
            {
                ViewBag.ErrorMessage = "Lecturer name is required.";
                return View("SubmitAndTrackClaim", claims);
            }
            if (hoursWorked <= 0)
            {
                ViewBag.ErrorMessage = "Hours worked must be greater than 0.";
                return View("SubmitAndTrackClaim", claims);
            }
            if (hourlyRate <= 0)
            {
                ViewBag.ErrorMessage = "Hourly rate must be greater than 0.";
                return View("SubmitAndTrackClaim", claims);
            }
            if (document == null)
            {
                ViewBag.ErrorMessage = "Please upload a valid document.";
                return View("SubmitAndTrackClaim", claims);
            }

            

    }
}
