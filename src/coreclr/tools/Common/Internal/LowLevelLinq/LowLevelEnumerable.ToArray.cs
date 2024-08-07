// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Internal.LowLevelLinq
{
    internal static partial class LowLevelEnumerable
    {
        public static T[] ToArray<T>(this IEnumerable<T> values)
        {
            Debug.Assert(values != null);

            ArrayBuilder<T> arrayBuilder = default;
            foreach (T value in values)
            {
                arrayBuilder.Add(value);
            }
            return arrayBuilder.ToArray();
        }
    }
}
