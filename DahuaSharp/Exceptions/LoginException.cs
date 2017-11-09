using System;

namespace DahuaSharp
{
    public class LoginException : Exception
    {
        public LoginException(int returnCode) : base(FormatMessage(returnCode))
        { 
        }

        private static String FormatMessage(int returnCode)
        {
            return String.Format("Failed to login, the return code is {0}", returnCode);
        }
    }
}
