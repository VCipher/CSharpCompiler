﻿using CSharpCompiler.PE.Metadata.Heaps;
using CSharpCompiler.Semantics.Metadata;

namespace CSharpCompiler.PE.Metadata.Tables
{
    public struct PropertyRow
    {
        public readonly PropertyAttributes Attributes;
        public readonly uint Name;
        public readonly uint Type;

        public PropertyRow(PropertyAttributes attributes, uint name, uint type) : this()
        {
            Attributes = attributes;
            Name = name;
            Type = type;
        }
    }

    public sealed class PropertyTable : MetadataTable<PropertyRow>
    {
        public PropertyTable() : base() { }
        public PropertyTable(int count) : base(count) { }

        protected override PropertyRow ReadRow(TableHeap heap)
        {
            return new PropertyRow(
                (PropertyAttributes)heap.ReadUInt16(),
                heap.ReadString(),
                heap.ReadBlob()
            );
        }

        protected override void WriteRow(PropertyRow row, TableHeap heap)
        {
            heap.WriteUInt16((ushort)row.Attributes);
            heap.WriteString(row.Name);
            heap.WriteBlob(row.Type);
        }
    }
}
