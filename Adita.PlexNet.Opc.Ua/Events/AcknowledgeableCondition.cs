// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Annotations;

namespace Adita.PlexNet.Opc.Ua.Events
{
    /// <summary>
    /// Represents an acknowledgeable condition.
    /// </summary>
    public class AcknowledgeableCondition : Condition
    {
        [EventField(typeDefinitionId: ObjectTypeIds.AcknowledgeableConditionType, browsePath: "AckedState/Id")]
        public bool? AckedState { get; set; }

        [EventField(typeDefinitionId: ObjectTypeIds.AcknowledgeableConditionType, browsePath: "ConfirmedState/Id")]
        public bool? ConfirmedState { get; set; }
    }
}
