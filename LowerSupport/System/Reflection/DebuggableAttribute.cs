//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MIT license.
//// See the LICENSE file in the project root for more information.

//namespace System.Diagnostics
//{
//    // Attribute class used by the compiler to mark modules.  
//    // If present, then debugging information for everything in the
//    // assembly was generated by the compiler, and will be preserved
//    // by the Runtime so that the debugger can provide full functionality
//    // in the case of JIT attach. If not present, then the compiler may
//    // or may not have included debugging information, and the Runtime
//    // won't preserve the debugging info, which will make debugging after
//    // a JIT attach difficult.
//    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = false)]
//    public sealed class DebuggableAttribute : Attribute
//    {
//        [Flags]
//        public enum DebuggingModes
//        {
//            None = 0x0,
//            Default = 0x1,
//            DisableOptimizations = 0x100,
//            IgnoreSymbolStoreSequencePoints = 0x2,
//            EnableEditAndContinue = 0x4
//        }

//        public DebuggableAttribute(bool isJITTrackingEnabled, bool isJITOptimizerDisabled)
//        {
//            DebuggingFlags = 0;

//            if (isJITTrackingEnabled)
//            {
//                DebuggingFlags |= DebuggingModes.Default;
//            }

//            if (isJITOptimizerDisabled)
//            {
//                DebuggingFlags |= DebuggingModes.DisableOptimizations;
//            }
//        }

//        public DebuggableAttribute(DebuggingModes modes)
//        {
//            DebuggingFlags = modes;
//        }

//        public bool IsJITTrackingEnabled => (DebuggingFlags & DebuggingModes.Default) != 0;

//        public bool IsJITOptimizerDisabled => (DebuggingFlags & DebuggingModes.DisableOptimizations) != 0;

//        public DebuggingModes DebuggingFlags { get; }
//    }
//}
