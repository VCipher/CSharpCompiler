﻿using CSharpCompiler.PE.Metadata.Heaps;
using CSharpCompiler.Semantics.Metadata;

namespace CSharpCompiler.PE.Metadata.Tables
{
    public struct FileRow
    {
        public readonly FileAttributes Attributes;
        public readonly uint Name;
        public readonly uint HashValue;

        public FileRow(FileAttributes attributes, uint name, uint hashValue) : this()
        {
            Attributes = attributes;
            Name = name;
            HashValue = hashValue;
        }
    }

    public sealed class FileTable : MetadataTable<FileRow>
    {
        public FileTable() : base() { }
        public FileTable(int count) : base(count) { }

        protected override FileRow ReadRow(TableHeap heap)
        {
            return new FileRow(
                (FileAttributes)heap.ReadUInt32(),
                heap.ReadString(),
                heap.ReadBlob()
            );
        }

        protected override void WriteRow(FileRow row, TableHeap heap)
        {
            heap.WriteUInt32((uint)row.Attributes);
            heap.WriteString(row.Name);
            heap.WriteBlob(row.HashValue);
        }
    }
}