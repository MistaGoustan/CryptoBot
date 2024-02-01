using System.Runtime.Serialization;

namespace TCK.Common.WebJobs
{
    public sealed class BadRequestException : Exception
    {
        public BadRequestException(String reason)
            : base(reason)
        {
        }

        public BadRequestException(String reason, Exception innerException)
            : base(reason, innerException)
        {
        }

        public BadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
