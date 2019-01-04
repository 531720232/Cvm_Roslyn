// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Microsoft.CodeAnalysis.Diagnostics
{
    internal sealed class CompilationUnitCompletedEvent : CompilationEvent
    {
        public CompilationUnitCompletedEvent(Compilation compilation, SyntaxTree compilationUnit) : base(compilation)
        {
            this.CompilationUnit = compilationUnit;
        }
        public CompilationUnitCompletedEvent(CompilationUnitCompletedEvent original, SemanticModel newSemanticModel) : this(original.Compilation, original.CompilationUnit)
        {
            SemanticModel = newSemanticModel;
        }
        private WeakReference _weakModel;
        public SemanticModel SemanticModel
        {
            get
            {
               
                var weakModel = _weakModel;
                SemanticModel semanticModel=null;
                if (weakModel == null || weakModel.Target!=null)
                {
                    semanticModel =(SemanticModel) weakModel.Target;
                    semanticModel = Compilation.GetSemanticModel(CompilationUnit);
                    _weakModel = new WeakReference(semanticModel);
                }
                return semanticModel;
            }
            private set
            {
                _weakModel = new WeakReference(value);
            }
        }
        override public void FlushCache()
        {
        }

        public SyntaxTree CompilationUnit { get; }
        public CompilationUnitCompletedEvent WithSemanticModel(SemanticModel model)
        {
            return new CompilationUnitCompletedEvent(this, model);
        }
        public override string ToString()
        {
            return "CompilationUnitCompletedEvent(" + CompilationUnit.FilePath + ")";
        }
    }
}
