﻿using CSharpCompiler.Lexica.Tokens;

namespace CSharpCompiler.Syntax.Ast.Expressions.Relations
{
    public sealed class GreaterOrEqualOperation : BinaryOperation
    {
        public GreaterOrEqualOperation(Token @operator, Expression leftOperand, Expression rightOperand) 
            : base(@operator, leftOperand, rightOperand)
        { }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.VisitGreaterOrEqualOperation(this);
        }
    }
}
