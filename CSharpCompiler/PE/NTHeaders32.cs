﻿using System.Runtime.InteropServices;

namespace CSharpCompiler.PE
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NTHeaders32
    {
        public uint Signature;
        public FileHeader FileHeader;
        public OptionHeader32 OptionalHeader;
    }
}
