﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BrockAllen.OAuth2
{
    public class OAuth2CallbackController : Controller
    {
        public ActionResult Callback()
        {
            var result = OAuth2Client.ProcessCallback(HttpContext);
            if (result.Error != null)
            {
                return Content(result.Error + "<br>" + result.ErrorDetails);
            }

            var sam = FederatedAuthentication.SessionAuthenticationModule;
            if (sam != null)
            {
                var cp = new ClaimsPrincipal(new ClaimsIdentity(result.Claims, "OAuth"));
                var token = new SessionSecurityToken(cp);
                sam.WriteSessionTokenToCookie(token);
            }

            return Redirect(result.ReturnUrl);
        }
    }
}