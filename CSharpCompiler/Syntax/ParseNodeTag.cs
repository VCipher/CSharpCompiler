﻿namespace CSharpCompiler.Syntax
{
    public enum ParseNodeTag
    {
        Program,
        StmtSeq,
        Stmt,
        Terminal,
        DeclarationStmt,
        Block,
        VarDeclaration,
        Type,
        PrimitiveType,
        ExpressionStmt,
        Expression,
        ArithmeticExpression,
        FactorExpression,
        UnaryExpression,
        ConditionExpression,
        RelationExpression,
        CastExpression,
        Literal,
        PrimaryExpression,
        ParenthesisExpression,
        InvokeExpression,
        ArgumentList,
        Argument,
        VarAccess,
        ElementAccess,
        ExpressionList,
        PostfixDecrement,
        PostfixIncrement,
        ObjectCreation,
        VarDeclarator
    }
}
