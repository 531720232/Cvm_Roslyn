// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis.CodeGen;
using Microsoft.CodeAnalysis.PooledObjects;

namespace Microsoft.CodeAnalysis.Emit
{
    /// <summary>
    /// Debugging information associated with the specified method that is emitted by the compiler to support Edit and Continue.
    /// </summary>
    public struct EditAndContinueMethodDebugInformation
    {
        internal readonly int MethodOrdinal;
        internal readonly ImmutableArray<LocalSlotDebugInfo> LocalSlots;
        internal readonly ImmutableArray<LambdaDebugInfo> Lambdas;
        internal readonly ImmutableArray<ClosureDebugInfo> Closures;

        internal EditAndContinueMethodDebugInformation(int methodOrdinal, ImmutableArray<LocalSlotDebugInfo> localSlots, ImmutableArray<ClosureDebugInfo> closures, ImmutableArray<LambdaDebugInfo> lambdas)
        {
            Debug.Assert(methodOrdinal >= -1);

            this.MethodOrdinal = methodOrdinal;
            this.LocalSlots = localSlots;
            this.Lambdas = lambdas;
            this.Closures = closures;
        }

        /// <summary>
        /// Deserializes Edit and Continue method debug information from specified blobs.
        /// </summary>
        /// <param name="compressedSlotMap">Local variable slot map.</param>
        /// <param name="compressedLambdaMap">Lambda and closure map.</param>
        /// <exception cref="InvalidDataException">Invalid data.</exception>

        private static InvalidDataException CreateInvalidDataException(ImmutableArray<byte> data, int offset)
        {
            const int maxReportedLength = 1024;

            int start = Math.Max(0, offset - maxReportedLength / 2);
            int end = Math.Min(data.Length, offset + maxReportedLength / 2);

            byte[] left = new byte[offset - start];
            data.CopyTo(start, left, 0, left.Length);

            byte[] right = new byte[end - offset];
            data.CopyTo(offset, right, 0, right.Length);

            throw new IOException();

        }

        #region Local Slots

        private const byte SyntaxOffsetBaseline = 0xff;

        /// <exception cref="InvalidDataException">Invalid data.</exception>

        internal void SerializeLocalSlots(BlobBuilder writer)
        {
            int syntaxOffsetBaseline = -1;
            foreach (LocalSlotDebugInfo localSlot in this.LocalSlots)
            {
                if (localSlot.Id.SyntaxOffset < syntaxOffsetBaseline)
                {
                    syntaxOffsetBaseline = localSlot.Id.SyntaxOffset;
                }
            }

            if (syntaxOffsetBaseline != -1)
            {
                writer.WriteByte(SyntaxOffsetBaseline);
                writer.WriteCompressedInteger(-syntaxOffsetBaseline);
            }

            foreach (LocalSlotDebugInfo localSlot in this.LocalSlots)
            {
                SynthesizedLocalKind kind = localSlot.SynthesizedKind;
                Debug.Assert(kind <= SynthesizedLocalKind.MaxValidValueForLocalVariableSerializedToDebugInformation);

                if (!kind.IsLongLived())
                {
                    writer.WriteByte(0);
                    continue;
                }

                byte b = (byte)(kind + 1);
                Debug.Assert((b & (1 << 7)) == 0);

                bool hasOrdinal = localSlot.Id.Ordinal > 0;

                if (hasOrdinal)
                {
                    b |= 1 << 7;
                }

                writer.WriteByte(b);
                writer.WriteCompressedInteger(localSlot.Id.SyntaxOffset - syntaxOffsetBaseline);

                if (hasOrdinal)
                {
                    writer.WriteCompressedInteger(localSlot.Id.Ordinal);
                }
            }
        }

        #endregion

    }
}
