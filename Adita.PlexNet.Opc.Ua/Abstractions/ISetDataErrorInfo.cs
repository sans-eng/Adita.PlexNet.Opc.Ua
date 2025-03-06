﻿// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Adita.PlexNet.Opc.Ua.Abstractions
{
    /// <summary>
    /// Sets the result of create, read, write, or publish service calls.
    /// </summary>
    public interface ISetDataErrorInfo
    {
        /// <summary>
        /// Sets the result of a create, read, write, or publish service call.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="errors">The error messages.</param>
        void SetErrors(string propertyName, IEnumerable<string>? errors);
    }
}
