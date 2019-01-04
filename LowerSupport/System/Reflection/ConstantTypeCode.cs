#region 程序集 System.Reflection.Metadata, Version=1.4.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// C:\Users\Administrator\.nuget\packages\system.reflection.metadata\1.6.0\lib\netstandard2.0\System.Reflection.Metadata.dll
#endregion

namespace System.Reflection.Metadata
{
    //
    // 摘要:
    //     Specifies values that represent types of metadata constants.
    public enum ConstantTypeCode : byte
    {
        //
        // 摘要:
        //     An invalid type.
        Invalid = 0,
        //
        // 摘要:
        //     A Boolean type.
        Boolean = 2,
        //
        // 摘要:
        //     A character type.
        Char = 3,
        //
        // 摘要:
        //     A signed 1-byte integer type.
        SByte = 4,
        //
        // 摘要:
        //     An unsigned 1-byte integer.
        Byte = 5,
        //
        // 摘要:
        //     A signed 2-byte integer type.
        Int16 = 6,
        //
        // 摘要:
        //     An unsigned 2-byte integer type.
        UInt16 = 7,
        //
        // 摘要:
        //     A signed 4-byte integer type.
        Int32 = 8,
        //
        // 摘要:
        //     An unsigned 4-byte integer type.
        UInt32 = 9,
        //
        // 摘要:
        //     A signed 8-byte integer type.
        Int64 = 10,
        //
        // 摘要:
        //     An unsigned 8-byte integer type.
        UInt64 = 11,
        //
        // 摘要:
        //     A 4-byte floating point type.
        Single = 12,
        //
        // 摘要:
        //     An 8-byte floating point type.
        Double = 13,
        //
        // 摘要:
        //     A System.String type.
        String = 14,
        //
        // 摘要:
        //     A null reference.
        NullReference = 18
    }
}