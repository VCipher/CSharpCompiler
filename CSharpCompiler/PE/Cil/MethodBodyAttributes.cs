﻿using System;

namespace CSharpCompiler.PE.Cil
{
    [Flags]
    public enum MethodBodyAttributes : byte
    {
        InitLocals = 0x10,
        MoreSects = 0x08,
        TinyFormat = 0x02,
        FatFormat = 0x03
    }
}
