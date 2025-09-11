// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Adita.PlexNet.Opc.Ua.Abstractions.Decoders;
using Adita.PlexNet.Opc.Ua.Abstractions.Encodables;
using Adita.PlexNet.Opc.Ua.Abstractions.Encoders;
using Adita.PlexNet.Opc.Ua.Annotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Adita.PlexNet.Opc.Ua
{
    /// <summary>
    /// A base implementation of a Structure.
    /// </summary>
    [DataTypeId(DataTypeIds.Structure)]
    public abstract class Structure : ObservableValidator, IEncodable
    {
        #region Public properties
        /// <summary>
        /// When overriden, Gets whether current <see cref="Structure"/> has default value.
        /// </summary>
        public virtual bool IsDefault
        {
            get;
        }
        #endregion Public properties

        #region Event
        /// <summary>
        /// Occurs when the value of current <see cref="Structure"/> has changed.
        /// </summary>
        public event EventHandler? ValueChanged;
        #endregion Event

        #region Public methods
        /// <summary>
        /// Encode current <see cref="Structure"/> using specified <paramref name="encoder"/>.
        /// </summary>
        /// <param name="encoder">The <see cref="IEncoder"/> to encode <see cref="Structure"/>.</param>
        public abstract void Encode(IEncoder encoder);
        /// <summary>
        /// Decode current <see cref="Structure"/> using specified <paramref name="decoder"/>.
        /// </summary>
        /// <param name="decoder">The <see cref="IDecoder"/> to decode <see cref="Structure"/>.</param>
        public abstract void Decode(IDecoder decoder);
        #endregion Public methods

        #region Protected methods
        /// <summary>
        /// Raises <see cref="ValueChanged"/>.
        /// </summary>
        protected void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
        /// <inheritdoc/>
        protected override void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName) && GetType().GetProperty(e.PropertyName)?.GetValue(this) is Structure propertyValue)
            {
                propertyValue.ValueChanged -= OnChildStructureValueChanged;
            }
            base.OnPropertyChanging(e);
        }
        /// <inheritdoc/>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            OnValueChanged();
            if (!string.IsNullOrEmpty(e.PropertyName) && GetType().GetProperty(e.PropertyName)?.GetValue(this) is Structure propertyValue)
            {
                propertyValue.ValueChanged += OnChildStructureValueChanged;
            }
            base.OnPropertyChanged(e);
        }
        #endregion Protected methods

        #region Event handlers
        private void OnChildStructureValueChanged(object? sender, EventArgs e)
        {
            OnValueChanged();
        }
        #endregion Event handlers
    }
}
