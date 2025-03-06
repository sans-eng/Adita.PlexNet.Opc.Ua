﻿// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Adita.PlexNet.Opc.Ua.Constants
{
    public static class SecurityPolicyUris
    {
        public const string None = "http://opcfoundation.org/UA/SecurityPolicy#None";
        public const string Basic128Rsa15 = "http://opcfoundation.org/UA/SecurityPolicy#Basic128Rsa15";
        public const string Basic256 = "http://opcfoundation.org/UA/SecurityPolicy#Basic256";
        public const string Https = "http://opcfoundation.org/UA/SecurityPolicy#Https";
        public const string Basic256Sha256 = "http://opcfoundation.org/UA/SecurityPolicy#Basic256Sha256";
        public const string Aes128_Sha256_RsaOaep = "http://opcfoundation.org/UA/SecurityPolicy#Aes128_Sha256_RsaOaep";
        public const string Aes256_Sha256_RsaPss = "http://opcfoundation.org/UA/SecurityPolicy#Aes256_Sha256_RsaPss";
    }
}