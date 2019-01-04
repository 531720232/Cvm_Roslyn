using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace System.Reflection
{
	internal static class Throw
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidCast()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidArgument(string message, string parameterName)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidArgument_OffsetForVirtualHeapHandle()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static Exception InvalidArgument_UnexpectedHandleKind(HandleKind kind)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static Exception InvalidArgument_Handle(string parameterName)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void SignatureNotVarArg()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ControlFlowBuilderNotAvailable()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidOperationBuilderAlreadyLinked()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidOperation(string message)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidOperation_LabelNotMarked(int id)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void LabelDoesntBelongToBuilder(string parameterName)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void HeapHandleRequired()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void EntityOrUserStringHandleRequired()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidToken()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ArgumentNull(string parameterName)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ArgumentEmptyString(string parameterName)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ArgumentEmptyArray(string parameterName)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ValueArgumentNull()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void BuilderArgumentNull()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ArgumentOutOfRange(string parameterName)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ArgumentOutOfRange(string parameterName, string message)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void BlobTooLarge(string parameterName)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void IndexOutOfRange()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void TableIndexOutOfRange()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ValueArgumentOutOfRange()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void OutOfBounds()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void WriteOutOfBounds()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidCodedIndex()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidHandle()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidCompressedInteger()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidSerializedString()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ImageTooSmall()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ImageTooSmallOrContainsInvalidOffsetOrCount()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ReferenceOverflow()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void TableNotSorted(TableIndex tableIndex)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidOperation_TableNotSorted(TableIndex tableIndex)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void InvalidOperation_PEImageNotAvailable()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void TooManySubnamespaces()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ValueOverflow()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void SequencePointValueOutOfRange()
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void HeapSizeLimitExceeded(HeapIndex heap)
		{
			throw new Exception();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void PEReaderDisposed()
		{
			throw new Exception();
		}
	}
}
