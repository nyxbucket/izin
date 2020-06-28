using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace izin.Helper
{
    public static class UserExtended
    {
        static IEnumerable<Claim> GetClaims(IIdentity identity)
        {
            return (identity as ClaimsIdentity).Claims;
        }

        static Claim GetClaimByTypeName(IPrincipal user, string type)
        {
            return GetClaims(user.Identity).Where(m => m.Type == type).FirstOrDefault();
        }

        public static string GetUserPropertyValue(this IPrincipal user, string name)
        {
            var claim = GetClaimByTypeName(user, name);

            return claim != null ? claim.Value : null;
        }
    }
}