// System.Runtime.CompilerServices.TupleElementNamesAttribute
using System;
using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue | AttributeTargets.Struct)]
    [CLSCompliant(false)]
    public sealed class TupleElementNamesAttribute : Attribute
    {
        public IList<string> TransformNames
        {
            get
            {
                throw null;
            }
        }

        public TupleElementNamesAttribute(string[] transformNames)
        {
        }
    }
}