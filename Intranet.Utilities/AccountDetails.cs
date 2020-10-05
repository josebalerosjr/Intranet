using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace Intranet.Uti
{
    /**
     *  This class handles the method in getting details from
     *  the Active Directory like First Name, Last Name, Department etc.
     */

    public static class AccountManagementExtensions
    {
        public static String GetProperty(this Principal principal, String property)
        {
            DirectoryEntry directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry;
            if (directoryEntry.Properties.Contains(property))
                return directoryEntry.Properties[property].Value.ToString();
            else
                return String.Empty;
        }

        public static String GetDepartment(this Principal principal)
        {
            return principal.GetProperty("department"); // this line returns the value of the department from local AD
        }

        public static String GetDisplayname(this Principal principal)
        {
            return principal.DisplayName; // this line returns the value of the user's display name from local AD
        }

        public static String GetUserPrincipalName(this Principal principal)
        {
            return principal.UserPrincipalName; // this line returns the value of the user's Email from local AD
        }
    }
}