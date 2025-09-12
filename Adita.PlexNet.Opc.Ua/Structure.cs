// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using Adita.PlexNet.Opc.Ua.Abstractions.Decoders;
using Adita.PlexNet.Opc.Ua.Abstractions.Encodables;
using Adita.PlexNet.Opc.Ua.Abstractions.Encoders;
using Adita.PlexNet.Opc.Ua.Annotations;
using Adita.PlexNet.Opc.Ua.Extensions;
using Adita.PlexNet.Opc.Ua.Internal.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Adita.PlexNet.Opc.Ua
{
    /// <summary>
    /// A base implementation of a Structure.
    /// </summary>
    [DataTypeId(DataTypeIds.Structure)]
    public abstract class Structure : ObservableValidator, IEncodable
    {
        #region Private static fields
        private static readonly ConcurrentDictionary<Type, IReadOnlyList<StructMemberInfo>> _cachedStructMemberInfos = [];
        #endregion Private static fields

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
        public void Encode(IEncoder encoder)
        {
            var namespaceUri = GetNamespaceUri();
            if (namespaceUri != null)
            {
                encoder.PushNamespace(namespaceUri);
                OnEncoding(encoder);
                encoder.PopNamespace();
            }
        }
        /// <summary>
        /// Decode current <see cref="Structure"/> using specified <paramref name="decoder"/>.
        /// </summary>
        /// <param name="decoder">The <see cref="IDecoder"/> to decode <see cref="Structure"/>.</param>
        public void Decode(IDecoder decoder)
        {

            var namespaceUri = GetNamespaceUri();
            if (namespaceUri != null)
            {
                decoder.PushNamespace(namespaceUri);
                OnDecoding(decoder);
                decoder.PopNamespace();
            }
        }
        /// <summary>
        /// When overriden, override the struct members auto encoding.
        /// </summary>
        /// <param name="encoder">The <see cref="IEncoder"/> to encode <see cref="Structure"/>.</param>
        public virtual void OnEncoding(IEncoder encoder)
        {
            foreach (var structMemberInfo in GetStrucMemberInfos(GetType()))
            {
                if (structMemberInfo.TargetPropertyInfo.CanRead)
                {
                    var value = structMemberInfo.TargetPropertyInfo.GetValue(this);
                    encoder.Write(value, structMemberInfo.MemberName);
                }
            }
        }
        /// <summary>
        /// When overriden, override the struct members auto decoding.
        /// </summary>
        /// <param name="decoder">The <see cref="IDecoder"/> to decode <see cref="Structure"/>.</param>
        public virtual void OnDecoding(IDecoder decoder)
        {
            foreach (var structMemberInfo in GetStrucMemberInfos(GetType()))
            {
                if (structMemberInfo.TargetPropertyInfo.CanWrite)
                {
                    var value = decoder.Read(structMemberInfo.TargetPropertyInfo.PropertyType, structMemberInfo.MemberName);
                    structMemberInfo.TargetPropertyInfo.SetValue(this, value);
                }
            }
        }
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

        #region Private methods
        private string? GetNamespaceUri()
        {
            return GetType().GetCustomAttribute<NamespaceAttribute>()?.Uri;
        }
        private IEnumerable<StructMemberInfo> GetStrucMemberInfos(Type type)
        {
            return _cachedStructMemberInfos.GetOrAdd(type, t =>
            {
                var StructMemberInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                 .Where(p => p.GetCustomAttribute<StructMemberAttribute>() != null)
                 .Select(pi => new StructMemberInfo(pi, pi.GetCustomAttribute<StructMemberAttribute>()!.MemberName)).ToList();

                var StructMemberFieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(p => p.GetCustomAttribute<StructMemberAttribute>() != null)
                    .Select(fi => new { FieldInfo = fi, Attribute = fi.GetCustomAttribute<StructMemberAttribute>()!.MemberName });

                if (StructMemberFieldInfos.Any())
                {
                    var foundPropertyInfos = StructMemberFieldInfos
                        .Where(fi => type.GetProperty(fi.FieldInfo.Name.ToPascalCase()) != null)
                        .Select(fi => new StructMemberInfo(type.GetProperty(fi.FieldInfo.Name.ToPascalCase())!, fi.Attribute));

                    StructMemberInfos.AddRange(foundPropertyInfos);
                }

                return StructMemberInfos;
            });
        }
        #endregion Private methods

        #region Event handlers
        private void OnChildStructureValueChanged(object? sender, EventArgs e)
        {
            OnValueChanged();
        }
        #endregion Event handlers
    }
}
