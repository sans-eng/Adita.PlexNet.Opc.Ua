﻿// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Annotations;

namespace Adita.PlexNet.Opc.Ua
{
    [DataTypeId(DataTypeIds.DiagnosticInfo)]
    public sealed class DiagnosticInfo
    {
        public DiagnosticInfo(int namespaceUri = -1, int symbolicId = -1, int locale = -1, int localizedText = -1, string? additionalInfo = null, StatusCode innerStatusCode = default(StatusCode), DiagnosticInfo? innerDiagnosticInfo = null)
        {
            NamespaceUri = namespaceUri;
            SymbolicId = symbolicId;
            Locale = locale;
            LocalizedText = localizedText;
            AdditionalInfo = additionalInfo;
            InnerStatusCode = innerStatusCode;
            InnerDiagnosticInfo = innerDiagnosticInfo;
        }

        public int NamespaceUri { get; }

        public int SymbolicId { get; }

        public int Locale { get; }

        public int LocalizedText { get; }

        public string? AdditionalInfo { get; }

        public StatusCode InnerStatusCode { get; }

        public DiagnosticInfo? InnerDiagnosticInfo { get; }
    }
}