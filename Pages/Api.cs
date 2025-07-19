using Microsoft.AspNetCore.Mvc;

namespace antoinechampion_com.Pages
{
    [Route("api")]
    public class Api : Controller
    {
        [HttpGet("redirect/{permalink}")]
        public IActionResult RedirectionEndpoint(string permalink)
        {
            switch (permalink)
            {
                case "mariage":
                    return Redirect("https://www.example.com/");
                default:
                    return NotFound();
            }
        }
    }
}
