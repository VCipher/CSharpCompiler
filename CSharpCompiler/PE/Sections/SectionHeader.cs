﻿using System.Runtime.InteropServices;

namespace CSharpCompiler.PE.Sections
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    public struct SectionHeader
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Name;

        [FieldOffset(8)]
        public uint PhysicalAddress;
        [FieldOffset(8)]
        public uint VirtualSize;

        [FieldOffset(12)]
        public uint VirtualAddress;
        [FieldOffset(16)]
        public uint SizeOfRawData;
        [FieldOffset(20)]
        public uint PointerToRawData;
        [FieldOffset(24)]
        public uint PointerToRelocations;
        [FieldOffset(28)]
        public uint PointerToLinenumbers;
        [FieldOffset(32)]
        public ushort NumberOfRelocations;
        [FieldOffset(34)]
        public ushort NumberOfLinenumbers;
        [FieldOffset(36)]
        public SectionCharacteristic Characteristics;
    }
}
