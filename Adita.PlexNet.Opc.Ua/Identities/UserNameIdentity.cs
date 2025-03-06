// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions.Identities;

namespace Adita.PlexNet.Opc.Ua.Identities
{
    public class UserNameIdentity : IUserIdentity
    {
        public UserNameIdentity(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
