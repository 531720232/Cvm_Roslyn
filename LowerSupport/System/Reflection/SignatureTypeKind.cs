#region 程序集 System.Reflection.Metadata, Version=1.4.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// C:\Users\Administrator\.nuget\packages\system.reflection.metadata\1.6.0\lib\netstandard2.0\System.Reflection.Metadata.dll
#endregion

namespace System.Reflection.Metadata
{
    //
    // 摘要:
    //     Indicates the type definition of the signature.
    public enum SignatureTypeKind : byte
    {
        //
        // 摘要:
        //     It isn&#39;t known in the current context if the type reference or definition
        //     is a class or value type.
        Unknown = 0,
        //
        // 摘要:
        //     The type definition or reference refers to a value type.
        ValueType = 17,
        //
        // 摘要:
        //     The type definition or reference refers to a class.
        Class = 18
    }
}