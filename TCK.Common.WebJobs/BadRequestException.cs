using System.Runtime.Serialization;

namespace TCK.Common.WebJobs
{
    public sealed class BadRequestException : Exception
    {
        public BadRequestException(string reason)
            : base(reason)
        {
        }

        public BadRequestException(string reason, Exception innerException)
            : base(reason, innerException)
        {
        }

        public BadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
