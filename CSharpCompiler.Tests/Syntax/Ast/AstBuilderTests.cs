﻿using CSharpCompiler.Lexica;
using CSharpCompiler.Lexica.Tokens;
using CSharpCompiler.Syntax;
using CSharpCompiler.Syntax.Ast;
using CSharpCompiler.Syntax.Ast.Expressions;
using CSharpCompiler.Syntax.Ast.Statements;
using CSharpCompiler.Syntax.Ast.Types;
using CSharpCompiler.Tests.Assertions;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace CSharpCompiler.Tests.Syntax.Ast
{
    public class AstBuilderTests : TestCase
    {
        public AstBuilderTests(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public void BuildTest()
        {
            string content =
                @"int a = 1;
                  int b = 1;
                  writeLine(a + b);";

            SyntaxTree expected = new SyntaxTree(
                new DeclarationStmt(
                    new VarDeclaration(
                        PrimitiveType.INT, 
                        new VarDeclarator(new VarLocation("a"), new Literal(Tokens.INT_CONST("1")))
                    )
                ),
                new DeclarationStmt(
                    new VarDeclaration(
                        PrimitiveType.INT,
                        new VarDeclarator(new VarLocation("b"), new Literal(Tokens.INT_CONST("1")))
                    )
                ),
                new ExpressionStmt(
                    new InvokeExpression(
                        "writeLine",
                        new Argument(
                            new BinaryOperation(Tokens.PLUS, new VarLocation("a"), new VarLocation("b"))
                        )
                    )
                )
            );

            List<Token> tokens = Scanner.Scan(content);
            ParseTree parseTree = Parser.Parse(tokens);
            SyntaxTree syntaxTree = AstBuilder.Build(parseTree);

            syntaxTree.Should().Be(expected, Output);
        }
    }
}