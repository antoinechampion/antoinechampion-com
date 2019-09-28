using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace antoinechampion_com.Pages
{
    [Route("api")]
    public class Api : Controller
    {
        [HttpGet("mirror/set/{*tags}")]
        public async Task SetMirror(string tags)
        {
            using (var fileStream = new StreamWriter("mirror.txt", false))
            {
                await fileStream.WriteAsync(tags);
            }

        }

        [HttpGet("mirror/get")]
        public string GetMirror()
        {
            var str = "";
            using (var sr = new StreamReader("mirror.txt"))
            {
                str = sr.ReadToEnd();
            }

            return str;
        }
    }
}
