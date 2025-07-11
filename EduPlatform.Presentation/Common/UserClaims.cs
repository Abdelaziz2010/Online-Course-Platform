﻿using System.Security.Claims;

namespace EduPlatform.Presentation.Common
{
    public class UserClaims : IUserClaims
    {
        public IHttpContextAccessor HttpContextAccessor { get; }

        public UserClaims(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentContextUserId()
        {
            return GetCurrentUserId();
        }

        private string GetClaimInfo(string property)
        {
            var propertyData = "";

            var identity = HttpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                propertyData = identity.Claims.FirstOrDefault(d => d.Type.Contains(property))?.Value;
            }

            return propertyData;
        }

        public string GetCurrentUserEmail()
        {
            return GetClaimInfo("emails");
        }

        public string GetCurrentUserId()
        {
            return GetClaimInfo("objectidentifier");
        }

        public List<string> GetUserRoles()
        {
            var roles = GetClaimInfo("extension_userRoles"); ;
            return string.IsNullOrEmpty(roles) ? new List<string>() : roles.Split(',').ToList();
        }

        public int GetUserId()
        {
            var userId = GetClaimInfo("extension_userId");
            return Convert.ToInt32(userId);
        }
    }
}
