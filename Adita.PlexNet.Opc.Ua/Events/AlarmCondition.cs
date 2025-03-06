// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Annotations;

namespace Adita.PlexNet.Opc.Ua.Events
{
    /// <summary>
    /// Represents an alarm condition.
    /// </summary>
    public class AlarmCondition : AcknowledgeableCondition
    {
        [EventField(typeDefinitionId: ObjectTypeIds.AlarmConditionType, browsePath: "ActiveState/Id")]
        public bool? ActiveState { get; set; }
    }
}
