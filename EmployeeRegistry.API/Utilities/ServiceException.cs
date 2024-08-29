using System;

namespace EmployeeRegistry.API.Utilities
{
    public class ServiceException : Exception
    {
        public ServiceException() : base() { }

        public ServiceException(string message) : base(message) { }

        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
