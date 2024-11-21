using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PROG6212_Part1.Models;

namespace PROG6212_Part1.Controllers
{
    public class CoordinatorsAndManagersController : Controller
    {
        // Static list of claims shared with LecturerController
        private static List<Claim> claims = LecturerController.claims;

        // Display the claims for review
        public IActionResult ReviewClaims()
        {
            return View(claims);
        }

        // Automated verification method
        [HttpPost]
        public IActionResult AutoVerifyClaims()
        {
            foreach (var claim in claims.Where(c => c.Status == "Pending"))
            {
                if (VerifyClaim(claim))
                {
                    claim.Status = "Approved";
                }
                else
                {
                    claim.Status = "Needs Review";
                }
            }
            return RedirectToAction("ReviewClaims");
        }

        // Verification logic based on predefined criteria
        private bool VerifyClaim(Claim claim)
        {
            const int maxHours = 40;          //  Maximum hours per week
            const decimal maxHourlyRate = 500; //  Maximum allowed hourly rate

            // Check if the claim meets criteria
            return claim.HoursWorked <= maxHours && claim.HourlyRate <= maxHourlyRate;
        }

       
    }
}
