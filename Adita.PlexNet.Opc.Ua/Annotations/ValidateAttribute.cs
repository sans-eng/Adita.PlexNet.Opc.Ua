// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;

namespace Adita.PlexNet.Opc.Ua.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class ValidateAttribute : ValidationAttribute
    {
        #region Protected methods
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is SubscriptionBase instance)
            {
                return instance.ValidateProperty(validationContext.MemberName, validationContext);
            }

            return ValidationResult.Success;
        }
        #endregion Protected methods
    }
}
