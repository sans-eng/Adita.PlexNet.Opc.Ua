// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.X509;

namespace Adita.PlexNet.Opc.Ua.Abstractions
{
    /// <summary>
    /// The certificate store interface.
    /// </summary>
    public interface ICertificateStore
    {
        /// <summary>
        /// Gets the local certificate and private key.
        /// </summary>
        /// <param name="applicationDescription">The application description.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>The local certificate and private key.</returns>
        Task<(X509Certificate? Certificate, RsaKeyParameters? Key)> GetLocalCertificateAsync(ApplicationDescription applicationDescription, ILogger? logger, CancellationToken token);

        /// <summary>
        /// Validates the remote certificate.
        /// </summary>
        /// <param name="certificate">The remote certificate.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>The validator result.</returns>
        Task<bool> ValidateRemoteCertificateAsync(X509Certificate certificate, ILogger? logger, CancellationToken token);
    }
}
