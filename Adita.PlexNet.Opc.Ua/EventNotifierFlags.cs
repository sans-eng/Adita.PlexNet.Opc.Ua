// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Adita.PlexNet.Opc.Ua
{
    [Flags]
    public enum EventNotifierFlags : byte
    {
        None = 0,
        SubscribeToEvents = 1,
        HistoryRead = 4,
        HistoryWrite = 8,
    }
}