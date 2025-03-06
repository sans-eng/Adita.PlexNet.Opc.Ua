﻿// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Adita.PlexNet.Opc.Ua
{
    [Flags]
    public enum AccessLevelFlags : byte
    {
        None = 0,
        CurrentRead = 1,
        CurrentWrite = 2,
        HistoryRead = 4,
        HistoryWrite = 8,
        SemanticChange = 16,
        StatusWrite = 32,
        TimestampWrite = 64,
    }
}