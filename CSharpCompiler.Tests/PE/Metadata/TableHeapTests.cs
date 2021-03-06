﻿using CSharpCompiler.Compilation;
using CSharpCompiler.Tests;
using CSharpCompiler.Tests.Assertions;
using CSharpCompiler.Tests.Data;
using CSharpCompiler.Utility;
using Xunit;
using Xunit.Abstractions;

namespace CSharpCompiler.PE.Metadata.Tests
{
    public class TableHeapTests : TestCase
    {
        public TableHeapTests(ITestOutputHelper output) : base(output)
        { }

        [Theory]
        [FileData("Content/Tests/TableHeapTest.txt")]
        public void TableHeapTest(string content, ByteBuffer expected)
        {
            var assemblyDef = Compiler.CompileAssembly(content);
            var metadata = MetadataBuilder.Build(assemblyDef);

            metadata.Heaps.Tables.Should().Be(expected, Output);
        }
    }
}
