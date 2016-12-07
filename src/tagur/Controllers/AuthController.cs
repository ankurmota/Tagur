
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
 
using System.IO;
using Microsoft.Extensions.Options;
using tagur.Data;
using Microsoft.EntityFrameworkCore;

namespace tagur.Controllers
{
    public class AuthController : Controller
    {
        [Route("api/auth/home")]
        [HttpGet]
        public IActionResult Home(string UserId)
        {   
            if (string.IsNullOrEmpty(Common.ViewModel.User.accessToken))
            {
                return Redirect(Helpers.GraphHelper.CreateOAuthCodeRequestUri(UserId).OriginalString);
            }
            else
            {
                return Redirect("/");
            }
        }

        [Route("api/auth/receivetoken")]
        [HttpGet()]
        public async Task<IActionResult> ReceiveToken(string code = null, string state = null)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var accessToken = await Helpers.GraphHelper.GetAccessTokenAsync(code);

                var profile = await Helpers.GraphHelper.GetUserProfileAsync(accessToken);

                Helpers.TelemetryHelper.AddEvent("UserAuthenticated");

                Common.ViewModel.User = profile;                
            }

            string url = string.Format("{0}#portfolio", Url.Action("Index", "Home"));
            return Redirect(url);

        }
    }
}
