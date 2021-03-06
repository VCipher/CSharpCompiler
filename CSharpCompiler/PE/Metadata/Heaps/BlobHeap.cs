﻿using CSharpCompiler.Utility;

namespace CSharpCompiler.PE.Metadata.Heaps
{
    public sealed class BlobHeap : HeapBuffer
    {
        public BlobHeap() : base(new byte[] { 0x00 }) { }
        public BlobHeap(byte[] buffer) : base(buffer) { MoveTo(START_POSITION); }

        public uint WriteBlob(ByteBuffer blob)
        {
            var index = (uint)Position;
            WriteCompressedUInt32((uint)blob.Length);
            WriteBuffer(blob);

            return index;
        }

        public ByteBuffer ReadBlob(uint index)
        {
            MoveTo((int)index);
            var size = ReadCompressedUInt32();
            var bytes = ReadBytes((int)size);

            return new ByteBuffer(bytes);
        }
    }
}
