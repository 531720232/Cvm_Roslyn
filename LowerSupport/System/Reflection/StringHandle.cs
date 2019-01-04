using System.Reflection.Metadata.Ecma335;

namespace System.Reflection.Metadata
{
	public readonly struct StringHandle : IEquatable<StringHandle>
	{
		internal enum VirtualIndex
		{
			System_Runtime_WindowsRuntime,
			System_Runtime,
			System_ObjectModel,
			System_Runtime_WindowsRuntime_UI_Xaml,
			System_Runtime_InteropServices_WindowsRuntime,
			System_Numerics_Vectors,
			Dispose,
			AttributeTargets,
			AttributeUsageAttribute,
			Color,
			CornerRadius,
			DateTimeOffset,
			Duration,
			DurationType,
			EventHandler1,
			EventRegistrationToken,
			Exception,
			GeneratorPosition,
			GridLength,
			GridUnitType,
			ICommand,
			IDictionary2,
			IDisposable,
			IEnumerable,
			IEnumerable1,
			IList,
			IList1,
			INotifyCollectionChanged,
			INotifyPropertyChanged,
			IReadOnlyDictionary2,
			IReadOnlyList1,
			KeyTime,
			KeyValuePair2,
			Matrix,
			Matrix3D,
			Matrix3x2,
			Matrix4x4,
			NotifyCollectionChangedAction,
			NotifyCollectionChangedEventArgs,
			NotifyCollectionChangedEventHandler,
			Nullable1,
			Plane,
			Point,
			PropertyChangedEventArgs,
			PropertyChangedEventHandler,
			Quaternion,
			Rect,
			RepeatBehavior,
			RepeatBehaviorType,
			Size,
			System,
			System_Collections,
			System_Collections_Generic,
			System_Collections_Specialized,
			System_ComponentModel,
			System_Numerics,
			System_Windows_Input,
			Thickness,
			TimeSpan,
			Type,
			Uri,
			Vector2,
			Vector3,
			Vector4,
			Windows_Foundation,
			Windows_UI,
			Windows_UI_Xaml,
			Windows_UI_Xaml_Controls_Primitives,
			Windows_UI_Xaml_Media,
			Windows_UI_Xaml_Media_Animation,
			Windows_UI_Xaml_Media_Media3D,
			Count
		}

		private readonly uint _value;

		internal uint RawValue => _value;

		internal bool IsVirtual => ((int)_value & -2147483648) != 0;

		/// <returns></returns>
		public bool IsNil => ((int)_value & -1610612737) == 0;

		internal StringKind StringKind => (StringKind)(_value >> 29);

		private StringHandle(uint value)
		{
			_value = value;
		}

		internal static StringHandle FromOffset(int heapOffset)
		{
			return new StringHandle((uint)(0 | heapOffset));
		}

		internal static StringHandle FromVirtualIndex(VirtualIndex virtualIndex)
		{
			return new StringHandle((uint)((VirtualIndex)(-2147483648) | virtualIndex));
		}

		internal static StringHandle FromWriterVirtualIndex(int virtualIndex)
		{
			return new StringHandle((uint)(-2147483648 | virtualIndex));
		}

		internal StringHandle WithWinRTPrefix()
		{
			return new StringHandle((uint)(-1610612736 | (int)_value));
		}

		internal StringHandle WithDotTermination()
		{
			return new StringHandle(0x20000000 | _value);
		}

		internal StringHandle SuffixRaw(int prefixByteLength)
		{
			return new StringHandle((uint)(0 | ((int)_value + prefixByteLength)));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static implicit operator Handle(StringHandle handle)
		{
			return new Handle((byte)(((uint)((int)handle._value & -2147483648) >> 24) | 0x78 | ((handle._value & 0x60000000) >> 29)), (int)(handle._value & 0x1FFFFFFF));
		}

		/// <param name="handle"></param>
		/// <returns></returns>
		public static explicit operator StringHandle(Handle handle)
		{
			if ((handle.VType & -132) != 120)
			{
				Throw.InvalidCast();
			}
			return new StringHandle((uint)(((handle.VType & 0x80) << 24) | ((handle.VType & 3) << 29) | handle.Offset));
		}

		internal int GetHeapOffset()
		{
			return (int)(_value & 0x1FFFFFFF);
		}

		internal VirtualIndex GetVirtualIndex()
		{
			return (VirtualIndex)(_value & 0x1FFFFFFF);
		}

		internal int GetWriterVirtualIndex()
		{
			return (int)(_value & 0x1FFFFFFF);
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is StringHandle)
			{
				return Equals((StringHandle)obj);
			}
			return false;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(StringHandle other)
		{
			return _value == other._value;
		}

		/// <returns></returns>
		public override int GetHashCode()
		{
			return (int)_value;
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(StringHandle left, StringHandle right)
		{
			return left.Equals(right);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(StringHandle left, StringHandle right)
		{
			return !left.Equals(right);
		}
	}
}
