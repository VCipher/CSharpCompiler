﻿using System.Collections.ObjectModel;

namespace CSharpCompiler.Semantics.Metadata
{
    public interface IMethodInfo : IMetadataEntity
    {
        string Name { get; }
        CallingConventions CallingConventions { get; }
        ITypeInfo DeclaringType { get; }
        ITypeInfo ReturnType { get; }
        Collection<ParameterDefinition> Parameters { get; }
    }
}