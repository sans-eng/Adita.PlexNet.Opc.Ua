// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions.Requests;
using Adita.PlexNet.Opc.Ua.Abstractions.Responses;

namespace Adita.PlexNet.Opc.Ua.Channels
{
    public class ServiceOperation : TaskCompletionSource<IServiceResponse>
    {
        public ServiceOperation(IServiceRequest request)
#if NET45
            : base(request)
#else
            : base(request, TaskCreationOptions.RunContinuationsAsynchronously)
#endif
        {
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        public IServiceRequest Request => (IServiceRequest)Task.AsyncState!;
    }
}