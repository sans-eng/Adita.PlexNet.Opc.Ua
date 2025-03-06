// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions.Identities;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.X509;

namespace Adita.PlexNet.Opc.Ua.Identities
{
    public class X509Identity : IUserIdentity
    {
        public X509Identity(X509Certificate certificate, RsaKeyParameters privateKey)
        {
            Certificate = certificate;
            PrivateKey = privateKey;
        }

        public X509Certificate Certificate { get; }

        public RsaKeyParameters PrivateKey { get; }
    }
}
