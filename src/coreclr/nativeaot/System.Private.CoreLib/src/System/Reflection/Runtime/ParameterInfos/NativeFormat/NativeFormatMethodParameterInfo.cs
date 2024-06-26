// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Runtime.CustomAttributes;
using System.Reflection.Runtime.General;
using System.Reflection.Runtime.General.NativeFormat;

using Internal.Metadata.NativeFormat;
using Internal.Reflection.Core;
using Internal.Reflection.Core.Execution;

namespace System.Reflection.Runtime.ParameterInfos.NativeFormat
{
    //
    // This implements ParameterInfo objects owned by MethodBase objects that have an associated Parameter metadata entity.
    //
    internal sealed partial class NativeFormatMethodParameterInfo : RuntimeFatMethodParameterInfo
    {
        private NativeFormatMethodParameterInfo(MethodBase member, int position, ParameterHandle parameterHandle, QSignatureTypeHandle qualifiedParameterTypeHandle, TypeContext typeContext)
            : base(member, position, qualifiedParameterTypeHandle, typeContext)
        {
            _parameter = parameterHandle.GetParameter(Reader);
        }

        private MetadataReader Reader
        {
            get
            {
                Debug.Assert(QualifiedParameterTypeHandle.Reader is MetadataReader);
                return (MetadataReader)QualifiedParameterTypeHandle.Reader;
            }
        }

        public sealed override ParameterAttributes Attributes
        {
            get
            {
                return _parameter.Flags;
            }
        }

        public sealed override string Name
        {
            get
            {
                return _parameter.Name.GetStringOrNull(this.Reader);
            }
        }

        public sealed override int MetadataToken
        {
            get
            {
                throw new InvalidOperationException(SR.NoMetadataTokenAvailable);
            }
        }

        protected sealed override IEnumerable<CustomAttributeData> TrueCustomAttributes => RuntimeCustomAttributeData.GetCustomAttributes(this.Reader, _parameter.CustomAttributes);

        protected sealed override bool GetDefaultValueIfAvailable(bool raw, out object? defaultValue)
        {
            return DefaultValueParser.GetDefaultValueFromConstantIfAny(Reader, _parameter.DefaultValue, ParameterType, raw, out defaultValue)
                || DefaultValueParser.GetDefaultValueFromAttributeIfAny(CustomAttributes, raw, out defaultValue);
        }

        private readonly Parameter _parameter;
    }
}
