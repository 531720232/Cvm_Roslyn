// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Roslyn.Utilities;
using System.IO;

namespace Microsoft.CodeAnalysis
{
    internal static class AssemblyIdentityUtils
    {
        public static AssemblyIdentity TryGetAssemblyIdentity(string filePath)
        {
         

            return new AssemblyIdentity("mscoree.dll",new Version("2.0.0"),"zh-CN",default,false);
        }
    }
}
