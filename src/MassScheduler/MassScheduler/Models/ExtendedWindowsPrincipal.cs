using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace MassScheduler.Models
{
    public class ExtendedWindowsPrincipal : WindowsPrincipal
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }

        public ExtendedWindowsPrincipal(WindowsIdentity identity)
            : base(identity)
        {
            if(identity == null)
                throw new Exception("WindowsIdentity could not be found.");

            var loginDetails = identity.Name.Split('\\');
            var username = loginDetails[1].ToUpper();
            var domain = loginDetails[0];

            using (var principalContext = new PrincipalContext(ContextType.Domain, domain))
            {
                var userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);
                if (userPrincipal != null)
                {
                    FirstName = userPrincipal.GivenName;
                    LastName = userPrincipal.Surname;
                    EmailAddress = userPrincipal.EmailAddress;
                }
            }
        }
    }
}