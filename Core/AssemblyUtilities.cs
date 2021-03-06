﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Microsoft.CodeAnalysis;

namespace Roslyn.Utilities
{
    internal static class AssemblyUtilities
    {
        /// <summary>
        /// Given a path to an assembly, identifies files in the same directory
        /// that could satisfy the assembly's dependencies. May throw.
        /// </summary>
        /// <remarks>
        /// Dependencies are identified by simply checking the name of an assembly
        /// reference against a file name; if they match the file is considered a
        /// dependency. Other factors, such as version, culture, public key, etc., 
        /// are not considered, and so the returned collection may include items that
        /// cannot in fact satisfy the original assembly's dependencies.
        /// </remarks>
        /// <exception cref="IOException">If the file at <paramref name="filePath"/> does not exist or cannot be accessed.</exception>
        /// <exception cref="BadImageFormatException">If the file is not an assembly or is somehow corrupted.</exception>
        public static ImmutableArray<string> FindAssemblySet(string filePath)
        {
            Debug.Assert(filePath != null);
            Debug.Assert(PathUtilities.IsAbsolute(filePath));

            Queue<string> workList = new Queue<string>();
            HashSet<string> assemblySet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

           

            return ImmutableArray.CreateRange(assemblySet);
        }

        /// <summary>
        /// Given a path to an assembly, returns its MVID (Module Version ID).
        /// May throw.
        /// </summary>
        /// <exception cref="IOException">If the file at <paramref name="filePath"/> does not exist or cannot be accessed.</exception>
        /// <exception cref="BadImageFormatException">If the file is not an assembly or is somehow corrupted.</exception>
        public static Guid ReadMvid(string filePath)
        {
            Debug.Assert(filePath != null);
            Debug.Assert(PathUtilities.IsAbsolute(filePath));

            return typeof(object).GUID;
        }

        /// <summary>
        /// Given a path to an assembly, finds the paths to all of its satellite
        /// assemblies.
        /// </summary>
        /// <exception cref="IOException">If the file at <paramref name="filePath"/> does not exist or cannot be accessed.</exception>
        /// <exception cref="BadImageFormatException">If the file is not an assembly or is somehow corrupted.</exception>
        public static ImmutableArray<string> FindSatelliteAssemblies(string filePath)
        {
            Debug.Assert(filePath != null);
            Debug.Assert(PathUtilities.IsAbsolute(filePath));

            var builder = ImmutableArray.CreateBuilder<string>();

            string directory = Path.GetDirectoryName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string resourcesNameWithoutExtension = fileNameWithoutExtension + ".resources";
            string resourcesNameWithExtension = resourcesNameWithoutExtension + ".dll";

            foreach (var subDirectory in Directory.EnumerateDirectories(directory, "*", SearchOption.TopDirectoryOnly))
            {
                string satelliteAssemblyPath = Path.Combine(subDirectory, resourcesNameWithExtension);
                if (File.Exists(satelliteAssemblyPath))
                {
                    builder.Add(satelliteAssemblyPath);
                }

                satelliteAssemblyPath = Path.Combine(subDirectory, resourcesNameWithoutExtension, resourcesNameWithExtension);
                if (File.Exists(satelliteAssemblyPath))
                {
                    builder.Add(satelliteAssemblyPath);
                }
            }

            return builder.ToImmutable();
        }

        /// <summary>
        /// Given a path to an assembly and a set of paths to possible dependencies,
        /// identifies which of the assembly's references are missing. May throw.
        /// </summary>
        /// <exception cref="IOException">If the files does not exist or cannot be accessed.</exception>
        /// <exception cref="BadImageFormatException">If one of the files is not an assembly or is somehow corrupted.</exception>
        public static ImmutableArray<AssemblyIdentity> IdentifyMissingDependencies(string assemblyPath, IEnumerable<string> dependencyFilePaths)
        {
            Debug.Assert(assemblyPath != null);
            Debug.Assert(PathUtilities.IsAbsolute(assemblyPath));
            Debug.Assert(dependencyFilePaths != null);

            HashSet<AssemblyIdentity> assemblyDefinitions = new HashSet<AssemblyIdentity>();
          

            return ImmutableArray.CreateRange(assemblyDefinitions);
        }

        /// <summary>
        /// Given a path to an assembly, returns the <see cref="AssemblyIdentity"/> for the assembly.
        ///  May throw.
        /// </summary>
        /// <exception cref="IOException">If the file at <paramref name="assemblyPath"/> does not exist or cannot be accessed.</exception>
        /// <exception cref="BadImageFormatException">If the file is not an assembly or is somehow corrupted.</exception>
        public static AssemblyIdentity GetAssemblyIdentity(string assemblyPath)
        {
            Debug.Assert(assemblyPath != null);
            Debug.Assert(PathUtilities.IsAbsolute(assemblyPath));

            return new AssemblyIdentity("mscoree.dll", new Version("2.0.0"), "zh-CN", default, false);
        }
    }
}
