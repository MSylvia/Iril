﻿using System;
using System.Numerics;
using System.Collections.Generic;
using Iril.Types;
using System.Linq;
using System.Globalization;

namespace Iril.IR
{
    public abstract class Constant : Value
    {
        public abstract int Int32Value { get; }

        public override int GetInt32Value (Module module) => Int32Value;
    }

    public abstract class SimpleConstant : Constant
    {
        public override bool IsIdempotent (FunctionDefinition function) => true;
    }

    public abstract class ComplexConstant : Constant
    {
    }

    public class TypedConstant
    {
        public readonly LType Type;
        public readonly Constant Constant;

        public TypedConstant (LType type, Constant constant)
        {
            Type = type;
            Constant = constant ?? throw new ArgumentNullException (nameof (constant));
        }

        public override string ToString () => $"{Type} {Constant}";
    }

    public class BooleanConstant : SimpleConstant
    {
        public static readonly BooleanConstant True = new BooleanConstant (true);
        public static readonly BooleanConstant False = new BooleanConstant (false);

        public readonly bool IsTrue;

        BooleanConstant (bool isTrue)
        {
            IsTrue = isTrue;
        }

        public override int Int32Value => IsTrue ? 1 : 0;

        public override string ToString () => IsTrue ? "true" : "false";
    }

    public class BytesConstant : Constant
    {
        public readonly Symbol Bytes;

        public BytesConstant (Symbol bytes)
        {
            Bytes = bytes ?? throw new ArgumentNullException (nameof (bytes));
        }

        public override int Int32Value => 0;

        public override string ToString () => $"{Bytes}";
    }

    public class IntegerConstant : SimpleConstant
    {
        public static readonly IntegerConstant Zero = new IntegerConstant (BigInteger.Zero);

        public readonly BigInteger Value;

        public IntegerConstant (BigInteger value)
        {
            Value = value;
        }

        public override int Int32Value => (int)Value;

        public override string ToString () => Value.ToString ();
    }

    public class HexIntegerConstant : SimpleConstant
    {
        public readonly BigInteger Value;

        public HexIntegerConstant (BigInteger value)
        {
            Value = value;
        }

        public override int Int32Value => (int)Value;

        public override string ToString () => $"0x{Value:X}";
    }

    public class FloatConstant : SimpleConstant
    {
        public readonly double Value;

        public FloatConstant (double value)
        {
            Value = value;
        }

        public override int Int32Value => (int)Math.Round (Value);

        public override string ToString () => Value.ToString (System.Globalization.CultureInfo.InvariantCulture);
    }

    public class NullConstant : SimpleConstant
    {
        public static readonly NullConstant Null = new NullConstant ();

        NullConstant ()
        {
        }

        public override int Int32Value => 0;

        public override string ToString () => "null";
    }

    public class StructureConstant : ComplexConstant
    {
        public readonly TypedValue[] Elements;

        public StructureConstant (IEnumerable<TypedValue> elements)
        {
            if (elements == null) {
                throw new ArgumentNullException (nameof (elements));
            }

            Elements = elements.ToArray ();
        }

        public override int Int32Value => 0;
    }



    public class UndefinedConstant : SimpleConstant
    {
        public static UndefinedConstant Undefined = new UndefinedConstant ();

        public override string ToString () => "undef";

        public override int Int32Value => 0;
    }

    public class ZeroConstant : Constant
    {
        public static ZeroConstant Zero = new ZeroConstant ();

        public override string ToString () => "zeroinitializer";

        public override int Int32Value => 0;
    }
}