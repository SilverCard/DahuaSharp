using System;
using System.Runtime.Serialization;

namespace DahuaSharp
{
    public class DahuaHttpException : Exception
    {
        public DahuaHttpException()
        {
        }

        public DahuaHttpException(string message) : base(message)
        {
        }

        public DahuaHttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DahuaHttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
