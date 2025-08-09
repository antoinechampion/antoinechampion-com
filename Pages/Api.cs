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
                    return Redirect("https://achampion-public.s3.fr-par.scw.cloud/Mariage%20Paola%20et%20Antoine.pdf");
                default:
                    return NotFound();
            }
        }
    }
}
