﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.CodeAnalysis
{
    internal static class TypeAttributesExtensions
    {
        public static bool IsInterface(this TypeAttributes flags)
        {
            return (flags & TypeAttributes.Interface) != 0;
        }

        public static bool IsWindowsRuntime(this TypeAttributes flags)
        {
            return false;//(flags & TypeAttributes.WindowsRuntime) != 0;
        }

        public static bool IsPublic(this TypeAttributes flags)
        {
            return (flags & TypeAttributes.Public) != 0;
        }

        public static bool IsSpecialName(this TypeAttributes flags)
        {
            return (flags & TypeAttributes.SpecialName) != 0;
        }

        /// <summary>
        /// Extracts <see cref="CharSet"/> information from TypeDef flags.
        /// Returns 0 if the value is invalid.
        /// </summary>
        internal static CharSet ToCharSet(this TypeAttributes flags)
        {
            switch (flags & TypeAttributes.StringFormatMask)
            {
                case TypeAttributes.AutoClass:
                    return Cci.Constants.CharSet_Auto;

                case TypeAttributes.AnsiClass:
                    return CharSet.Ansi;

                case TypeAttributes.UnicodeClass:
                    return CharSet.Unicode;

                default:
                    return 0;
            }
        }
    }
}
