﻿using System;

namespace CSharpCompiler.Semantics.Metadata
{
    public sealed class ArrayType : TypeSpecification
    {
        public int Rank { get; private set; }

        public ArrayType(ITypeInfo containedType, int rank = 1) : base(containedType)
        {
            Rank = rank;
            Name = GetArrayName(containedType);
            ElementType = GetElementType(rank);
        }

        private static ElementType GetElementType(int rank)
        {
            if (rank <= 0) throw new ArgumentOutOfRangeException("rank");
            return (rank == 1) ? ElementType.SizeArray : ElementType.Array;
        }

        private static string GetArrayName(ITypeInfo containedType)
        {
            if (containedType == null) throw new ArgumentNullException("containedType");
            return string.Format("{0}[]", containedType.Name);
        }
    }
}
