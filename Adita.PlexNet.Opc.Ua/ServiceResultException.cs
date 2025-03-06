// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Adita.PlexNet.Opc.Ua
{
#if NETSTANDARD
    [Serializable]
#endif
    public sealed class ServiceResultException : Exception
    {
        public ServiceResultException(ServiceResult result)
            : base(result.ToString())
        {
            HResult = unchecked((int)(uint)result.StatusCode);
        }

        public ServiceResultException(StatusCode statusCode)
            : base(StatusCodes.GetDefaultMessage(statusCode))
        {
            HResult = unchecked((int)(uint)statusCode);
        }

        public ServiceResultException(StatusCode statusCode, string message)
            : base(message)
        {
            HResult = unchecked((int)(uint)statusCode);
        }

        public ServiceResultException(StatusCode statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = unchecked((int)(uint)statusCode);
        }

        public ServiceResultException() : base()
        {
        }

        public ServiceResultException(string? message) : base(message)
        {
        }

        public ServiceResultException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets the StatusCode of the ServiceResult.
        /// </summary>
        public StatusCode StatusCode => (uint)HResult;
    }
}