// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions.Identities;

namespace Adita.PlexNet.Opc.Ua.Identities
{
    public class IssuedIdentity : IUserIdentity
    {
        public IssuedIdentity(byte[] tokenData)
        {
            TokenData = tokenData;
        }

        public byte[] TokenData { get; }
    }
}
