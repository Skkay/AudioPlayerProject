using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayerProject.Controllers
{
    public class CssModeController : Controller
    {
        public IActionResult ToggleCSS()
        {
            if (HttpContext.Request.Cookies["darkMode"] == "no")
            {
                CookieOptions options = new CookieOptions();
                options.Expires = new DateTimeOffset(DateTime.Now.AddYears(1));
                HttpContext.Response.Cookies.Append("darkMode", "yes", options);
            }
            else
            {
                HttpContext.Response.Cookies.Append("darkMode", "no");
            }

            return Redirect(Request.Headers["Referer"]);
        }
    }
}
