using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy
{
    public class UserMapper : IUserMapper
    {
        public static List<AuthenticatedUser> loggedInUsers = new List<AuthenticatedUser>();

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var userRecord = loggedInUsers.Where(u => u.UserGuid == identifier).FirstOrDefault();

            if (userRecord == null)
                return null;
            else
                return userRecord;
        }

        public static Guid? ValidateUser(string username, string password)
        {
            // Query Active Directory
            // bool valid = PrincipalContext.ValidateUser(username, password);
            // if (valid) Find UserPrincipalExt object
            // return UserObject class filled with GUID and other data such as membership, displayname, etc

            // Return the user's GUID from Active Directory

            Guid newGuid = Guid.NewGuid();

            var userRecord = loggedInUsers.Where(u => u.UserGuid == newGuid).FirstOrDefault();
            if (userRecord == null)
            {
                AuthenticatedUser newUser = new AuthenticatedUser();
                newUser.UserGuid = Guid.NewGuid();
                newUser.UserName = username;
                newUser.Claims = new[] { "Super" };
                loggedInUsers.Add(newUser);

                userRecord = newUser;
            }

            return userRecord.UserGuid;
        }
    }
}