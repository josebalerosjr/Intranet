﻿using Hangfire.Dashboard;
using Intranet.Utilities;

namespace Intranet.Classes
{
    internal interface IDashboasrdAuthorizationFilter
    {
    }

    //[Authorize(Roles = "Office of the Chief Information Officer")] //   This designates a authorization for Roles.
    public class AuthorizationFilter : IDashboasrdAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // allow all authenticated users to see the dashboard (potentially dangerous)
            return httpContext.User.IsInRole(SD.CIOAdmin);
            //return true;
        }
    }
}