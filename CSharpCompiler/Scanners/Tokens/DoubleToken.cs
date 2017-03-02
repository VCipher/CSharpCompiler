﻿using System;

namespace CSharpCompiler.Scanners.Tokens
{
    public sealed class DoubleToken : Token, IEquatable<DoubleToken>
    {
        public double Value { get; private set; }

        public DoubleToken(double value) : base(value.ToString(), TokenTag.DOUBLE_CONST)
        {
            Value = value;
        }

        public DoubleToken(string value) : base(value, TokenTag.FLOAT_CONST)
        {
            Value = double.Parse(value);
        }

        public bool Equals(DoubleToken other)
        {
            return base.Equals(other) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DoubleToken))
                return false;

            return Equals((DoubleToken)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int res = base.GetHashCode();
                res ^= Value.GetHashCode();

                return res;
            }
        }
    }
}