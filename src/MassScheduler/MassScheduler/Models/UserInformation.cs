using System;

namespace MassScheduler.Models
{
    public class UserInformation
    {
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }

        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        public UserInformation(ExtendedWindowsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException("Windows princpal cannot be null.", "princpal");
            }

            Username = principal.Identity.Name;
            FirstName = principal.FirstName;
            LastName = principal.LastName;
            EmailAddress = principal.EmailAddress;
        }
    }
}