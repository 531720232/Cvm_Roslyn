﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Roslyn.Utilities
{
    internal static class ReflectionUtilities
    {
        private static readonly Type Missing = typeof(void);

        public static Type TryGetType(string assemblyQualifiedName)
        {
            try
            {
                // Note that throwOnError=false only suppresses some exceptions, not all.
                return Type.GetType(assemblyQualifiedName, throwOnError: false);
            }
            catch
            {
                return null;
            }
        }

        public static Type TryGetType(ref Type lazyType, string assemblyQualifiedName)
        {
            if (lazyType == null)
            {
                lazyType = TryGetType(assemblyQualifiedName) ?? Missing;
            }

            return (lazyType == Missing) ? null : lazyType;
        }

        /// <summary>
        /// Find a <see cref="Type"/> instance by first probing the contract name and then the name as it
        /// would exist in mscorlib.  This helps satisfy both the CoreCLR and Desktop scenarios. 
        /// </summary>
        public static Type GetTypeFromEither(string contractName, string desktopName)
        {
            var type = TryGetType(contractName);

            if (type == null)
            {
                type = TryGetType(desktopName);
            }

            return type;
        }

        public static Type GetTypeFromEither(ref Type lazyType, string contractName, string desktopName)
        {
            if (lazyType == null)
            {
                lazyType = GetTypeFromEither(contractName, desktopName) ?? Missing;
            }

            return (lazyType == Missing) ? null : lazyType;
        }

        public static T FindItem<T>(IEnumerable<T> collection, params Type[] paramTypes)
            where T : MethodBase
        {
            foreach (var current in collection)
            {
                var p = current.GetParameters();
                if (p.Length != paramTypes.Length)
                {
                    continue;
                }

                bool allMatch = true;
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    if (p[i].ParameterType != paramTypes[i])
                    {
                        allMatch = false;
                        break;
                    }
                }

                if (allMatch)
                {
                    return current;
                }
            }

            return null;
        }

        internal static MethodInfo GetDeclaredMethod(this Type typeInfo, string name, params Type[] paramTypes)
        {
            var arr =new List<MethodInfo>();

          
            
                var m = typeInfo.GetMethod(name, BindingFlags.DeclaredOnly);
            
            if(m!=null)
            {
                arr.Add(m);
            }
            return FindItem(arr, paramTypes);
        }

        internal static ConstructorInfo GetDeclaredConstructor(this Type typeInfo, params Type[] paramTypes)
        {
            return FindItem(typeInfo.GetConstructors(BindingFlags.DeclaredOnly), paramTypes);
        }

        public static T CreateDelegate<T>(this MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                return default;
            }

          
            return (T)(object)Delegate.CreateDelegate(typeof(T), methodInfo);
        }

        public static T InvokeConstructor<T>(this ConstructorInfo constructorInfo, params object[] args)
        {
            if (constructorInfo == null)
            {
                return default;
            }

            try
            {
                return (T)constructorInfo.Invoke(args);
            }
            catch (TargetInvocationException e)
            {
              
                Debug.Assert(false, "Unreachable");
                return default;
            }
        }

        public static object InvokeConstructor(this ConstructorInfo constructorInfo, params object[] args)
        {
            return constructorInfo.InvokeConstructor<object>(args);
        }

        public static T Invoke<T>(this MethodInfo methodInfo, object obj, params object[] args)
        {
            return (T)methodInfo.Invoke(obj, args);
        }
    }
}
