﻿using System;
using Iril.Types;
using System.Collections.Generic;
using System.Linq;

namespace Iril.IR
{
    public abstract class Instruction
    {
        public static readonly Instruction ZeroI32 = new TruncInstruction (new TypedValue (IntegerType.I32, ZeroConstant.Zero), IntegerType.I32);
        public static readonly Instruction OneI32 = new TruncInstruction (new TypedValue (IntegerType.I32, IntegerConstant.One), IntegerType.I32);

        public abstract IEnumerable<LocalSymbol> ReferencedLocals { get; }
        public virtual bool IsIdempotent (FunctionDefinition function) => false;
        public abstract LType ResultType (Module module);
    }

    public abstract class TerminatorInstruction : Instruction
    {
        public abstract IEnumerable<LocalSymbol> NextLabelSymbols { get; }

        public override LType ResultType (Module module) => VoidType.Void;
    }

    public class AshrInstruction : BinaryInstruction
    {
        public AshrInstruction (LType type, Value op1, Value op2, bool exact)
            : base (type, op1, op2)
        {
        }
    }

    public abstract class BinaryInstruction : Instruction
    {
        public readonly LType Type;
        public readonly Value Op1;
        public readonly Value Op2;

        protected BinaryInstruction (LType type, Value op1, Value op2)
        {
            Type = type;
            Op1 = op1;
            Op2 = op2;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Op1.ReferencedLocals.Concat (Op2.ReferencedLocals);
        public override LType ResultType (Module module) => Type;
        public override bool IsIdempotent (FunctionDefinition function) => Op1.IsIdempotent (function) && Op2.IsIdempotent (function);
    }

    public abstract class AtomicBinaryInstruction : BinaryInstruction
    {
        public readonly bool IsAtomic;
        public AtomicBinaryInstruction (LType type, Value op1, Value op2, bool isAtomic)
            : base (type, op1, op2)
        {
            IsAtomic = isAtomic;
        }
    }

    public class AddInstruction : AtomicBinaryInstruction
    {
        public AddInstruction (LType type, Value op1, Value op2, bool isAtomic)
            : base (type, op1, op2, isAtomic: isAtomic)
        {
        }
    }

    public class AllocaInstruction : Instruction
    {
        public readonly LType Type;
        public readonly int Align;
        public readonly TypedValue NumElements;

        public AllocaInstruction (LType type, int align, TypedValue numElements)
        {
            Type = type;
            Align = align;
            NumElements = numElements;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals =>
            NumElements != null ? NumElements.ReferencedLocals : Enumerable.Empty<LocalSymbol> ();
        public override LType ResultType (Module module) => Type is ArrayType ? Type : new PointerType (Type, 0);
    }

    public class AndInstruction : BinaryInstruction
    {
        public AndInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class BitcastInstruction : Instruction
    {
        public readonly TypedValue Input;
        public readonly LType OutputType;

        public BitcastInstruction (TypedValue input, LType outputType)
        {
            Input = input;
            OutputType = outputType;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Input.ReferencedLocals;
        public override LType ResultType (Module module) => OutputType;
        public override bool IsIdempotent (FunctionDefinition function) => Input.Value.IsIdempotent (function);
    }

    public abstract class BrInstruction : TerminatorInstruction
    {
    }

    public class ConditionalBrInstruction : BrInstruction
    {
        public readonly Value Condition;
        public readonly LabelValue IfTrue;
        public readonly LabelValue IfFalse;

        public ConditionalBrInstruction (Value condition, LabelValue ifTrue, LabelValue ifFalse)
        {
            Condition = condition;
            IfTrue = ifTrue;
            IfFalse = ifFalse;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Condition.ReferencedLocals;

        public override IEnumerable<LocalSymbol> NextLabelSymbols => new[] { IfTrue.Symbol, IfFalse.Symbol };
    }

    public class UnconditionalBrInstruction : BrInstruction
    {
        public readonly LabelValue Destination;

        public UnconditionalBrInstruction (LabelValue destination)
        {
            Destination = destination;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Enumerable.Empty<LocalSymbol> ();

        public override IEnumerable<LocalSymbol> NextLabelSymbols => new[] { Destination.Symbol };
    }

    public class CallInstruction : Instruction
    {
        public readonly LType ReturnType;
        public readonly Value Pointer;
        public readonly Argument[] Arguments;
        public readonly bool Tail;

        public CallInstruction (LType returnType, Value pointer, IEnumerable<Argument> arguments, bool tail)
        {
            ReturnType = returnType;
            Pointer = pointer;
            Arguments = arguments.ToArray ();
            Tail = tail;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals {
            get {
                if (Pointer is GlobalValue g) {
                    switch (g.Symbol.Text)
                    {
                        case "@llvm.dbg.value":
                            yield break;
                    }
                }
                foreach (var l in Pointer.ReferencedLocals) {
                    yield return l;
                }
                foreach (var a in Arguments) {
                    foreach (var l in a.Value.ReferencedLocals) {
                        yield return l;
                    }
                }
            }
        }

        public override LType ResultType (Module module) => ReturnType;

        public override string ToString ()
        {
            return $"{ReturnType} {Pointer}({String.Join (", ", (object[])Arguments)})";
        }

        public override bool IsIdempotent (FunctionDefinition function)
        {
            if (Pointer is GlobalValue g) {
                switch (g.Symbol.Text) {
                    case "@llvm.dbg.declare":
                    case "@llvm.dbg.value":
                        return true;
                    case "@llvm.fabs.f64":
                    case "@llvm.sqrt.f64":
                    case "@llvm.ceil.f64":
                    case "@llvm.pow.f64":
                        foreach (var a in Arguments) {
                            if (!a.Value.IsIdempotent (function))
                                return false;
                        }
                        return true;
                }
            }
            return false;
        }
    }

    public class Argument
    {
        public readonly LType Type;
        public readonly Value Value;
        public readonly ParameterAttributes Attributes;

        public Argument (LType type, Value value, ParameterAttributes attributes)
        {
            Type = type;
            Value = value;
            Attributes = attributes;
        }

        public override string ToString ()
        {
            return $"{Type} {Value}";
        }
    }

    public class DivInstruction : BinaryInstruction
    {
        public DivInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class ExtractElementInstruction : Instruction
    {
        public readonly TypedValue Value;
        public readonly TypedValue Index;

        public ExtractElementInstruction (TypedValue value, TypedValue index)
        {
            Value = value ?? throw new ArgumentNullException (nameof (value));
            Index = index ?? throw new ArgumentNullException (nameof (index));
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals =>
            Value.ReferencedLocals.Concat (Index.ReferencedLocals);

        public override LType ResultType (Module module) => ((VectorType)Value.Type).ElementType;

        public override bool IsIdempotent (FunctionDefinition function) =>
            Value.Value.IsIdempotent (function) && Index.Value.IsIdempotent (function);
    }

    public class ExtractValueInstruction : Instruction
    {
        public readonly TypedValue Value;
        public readonly Value[] Indices;

        public ExtractValueInstruction (TypedValue value, List<Value> indices)
        {
            Value = value ?? throw new ArgumentNullException (nameof (value));
            Indices = indices?.ToArray () ?? throw new ArgumentNullException (nameof (indices));
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals =>
            Value.ReferencedLocals.Concat (Indices.SelectMany (x => x.ReferencedLocals));

        public override LType ResultType (Module module)
        {
            var t = Value.Type;
            foreach (var i in Indices) {
                var rt = t.Resolve (module);
                if (rt is ArrayType art) {
                    t = art.ElementType;
                }
                else if (rt is LiteralStructureType s) {
                    if (i is Constant c) {
                        var e = s.Elements[c.Int32Value];
                        t = e;
                    }
                    else {
                        throw new Exception ($"Cannot get element {i} at compile time");
                    }
                }
                else {
                    throw new Exception ($"Cannot get element type of {t} for extractvalue");
                }
            }
            return new PointerType (t, 0);
        }

        public override bool IsIdempotent (FunctionDefinition function) =>
            Value.Value.IsIdempotent (function) && Indices.All (x => x.IsIdempotent (function));
    }

    public class FaddInstruction : BinaryInstruction
    {
        public FaddInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class FcmpInstruction : Instruction
    {
        public readonly FcmpCondition Condition;
        public readonly LType Type;
        public readonly Value Op1;
        public readonly Value Op2;

        public FcmpInstruction (FcmpCondition condition, LType type, Value op1, Value op2)
        {
            Condition = condition;
            Type = type;
            Op1 = op1;
            Op2 = op2;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Op1.ReferencedLocals.Concat (Op2.ReferencedLocals);
        public override LType ResultType (Module module)
        {
            if (Type is VectorType v) {
                return new VectorType (v.Length, IntegerType.I1);
            }
            return IntegerType.I1;
        }
        public override bool IsIdempotent (FunctionDefinition function) => Op1.IsIdempotent (function) && Op2.IsIdempotent (function);
    }

    public enum FcmpCondition
    {
        False,
        True,
        Ordered,
        OrderedEqual,
        OrderedNotEqual,
        OrderedGreaterThan,
        OrderedGreaterThanOrEqual,
        OrderedLessThan,
        OrderedLessThanOrEqual,
        Unordered,
        UnorderedEqual,
        UnorderedNotEqual,
        UnorderedGreaterThan,
        UnorderedGreaterThanOrEqual,
        UnorderedLessThan,
        UnorderedLessThanOrEqual,
    }

    public class FdivInstruction : BinaryInstruction
    {
        public FdivInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class FenceInstruction : Instruction
    {
        public readonly AtomicConstraint Constraint;

        public FenceInstruction (AtomicConstraint constraint)
        {
            Constraint = constraint;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Enumerable.Empty<LocalSymbol> ();

        public override LType ResultType (Module module) => VoidType.Void;
    }

    public enum AtomicConstraint
    {
        SequentiallyConsistent
    }

    public class FmulInstruction : BinaryInstruction
    {
        public FmulInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class FpextInstruction : ConversionInstruction
    {
        public FpextInstruction (TypedValue input, LType outputType)
            : base (input, outputType)
        {
        }
    }

    public class FptosiInstruction : ConversionInstruction
    {
        public FptosiInstruction (TypedValue input, LType outputType)
            : base (input, outputType)
        {
        }
    }

    public class FptouiInstruction : ConversionInstruction
    {
        public FptouiInstruction (TypedValue input, LType outputType)
            : base (input, outputType)
        {
        }
    }

    public class FptruncInstruction : ConversionInstruction
    {
        public FptruncInstruction (TypedValue input, LType outputType)
            : base (input, outputType)
        {
        }
    }

    public class FsubInstruction : BinaryInstruction
    {
        public FsubInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class GetElementPointerInstruction : Instruction
    {
        public readonly LType Type;
        public readonly TypedValue Pointer;
        public readonly TypedValue[] Indices;

        public GetElementPointerInstruction (LType type, TypedValue pointer, IEnumerable<TypedValue> indices)
        {
            Type = type;
            Pointer = pointer;
            Indices = indices.ToArray ();
        }

        public override string ToString () => $"getelementptr {Pointer} [{string.Join (", ", (object[])Indices)}]";
        public override IEnumerable<LocalSymbol> ReferencedLocals =>
            Pointer.ReferencedLocals.Concat (Indices.SelectMany (x => x.Value.ReferencedLocals));
        public override LType ResultType (Module module)
        {
            var t = Type;
            foreach (var i in Indices.Skip (1)) {
                var rt = t.Resolve (module);
                if (rt is ArrayType art) {
                    t = art.ElementType;
                }
                else if (rt is LiteralStructureType s) {
                    if (i.Value is Constant c) {
                        var e = s.Elements[c.Int32Value];
                        t = e;
                    }
                    else {
                        throw new Exception ($"Cannot get element {i.Value} at compile time");
                    }
                }
                else {
                    throw new Exception ("Cannot get element type of " + t);
                }
            }
            return new PointerType (t, 0);
        }
        public override bool IsIdempotent (FunctionDefinition function)
        {
            if (!Pointer.Value.IsIdempotent (function))
                return false;
            foreach (var i in Indices) {
                if (!i.Value.IsIdempotent (function))
                    return false;
            }
            return true;
        }
    }

    public class IcmpInstruction : Instruction
    {
        public readonly IcmpCondition Condition;
        public readonly LType Type;
        public readonly Value Op1;
        public readonly Value Op2;

        public IcmpInstruction (IcmpCondition condition, LType type, Value op1, Value op2)
        {
            Condition = condition;
            Type = type;
            Op1 = op1;
            Op2 = op2;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Op1.ReferencedLocals.Concat (Op2.ReferencedLocals);
        public override LType ResultType (Module module)
        {
            if (Type is VectorType v) {
                return new VectorType (v.Length, IntegerType.I1);
            }
            return IntegerType.I1;
        }
        public override bool IsIdempotent (FunctionDefinition function) => Op1.IsIdempotent (function) && Op2.IsIdempotent (function);
    }

    public enum IcmpCondition
    {
        Equal,
        NotEqual,
        UnsignedGreaterThan,
        UnsignedGreaterThanOrEqual,
        UnsignedLessThan,
        UnsignedLessThanOrEqual,
        SignedGreaterThan,
        SignedGreaterThanOrEqual,
        SignedLessThan,
        SignedLessThanOrEqual,
    }

    public class InsertElementInstruction : Instruction
    {
        public readonly TypedValue Value;
        public readonly TypedValue Element;
        public readonly TypedValue Index;

        public InsertElementInstruction (TypedValue value, TypedValue element, TypedValue index)
        {
            Value = value ?? throw new ArgumentNullException (nameof (value));
            Element = element ?? throw new ArgumentNullException (nameof (element));
            Index = index ?? throw new ArgumentNullException (nameof (index));
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals =>
            Value.ReferencedLocals.Concat (Element.ReferencedLocals).Concat (Index.ReferencedLocals);

        public override LType ResultType (Module module) => Value.Type;
    }

    public class InsertValueInstruction : Instruction
    {
        public readonly TypedValue Value;
        public readonly TypedValue Element;
        public readonly Value[] Indices;

        public InsertValueInstruction (TypedValue value, TypedValue element, List<Value> indices)
        {
            Value = value ?? throw new ArgumentNullException (nameof (value));
            Element = element ?? throw new ArgumentNullException (nameof (element));
            Indices = indices?.ToArray () ?? throw new ArgumentNullException (nameof (indices));
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals =>
            Value.ReferencedLocals.Concat (Element.ReferencedLocals).Concat (Indices.SelectMany (x => x.ReferencedLocals));

        public override LType ResultType (Module module) => Value.Type;
    }

    public class InttoptrInstruction : ConversionInstruction
    {
        public InttoptrInstruction (TypedValue input, LType outputType)
            : base (input, outputType)
        {
        }
    }

    public class InvokeInstruction : TerminatorInstruction
    {
        public readonly LType ReturnType;
        public readonly Value Pointer;
        public readonly Argument[] Arguments;
        public readonly LabelValue NormalLabel;
        public readonly LabelValue ExceptionLabel;

        public InvokeInstruction (LType returnType, Value pointer, IEnumerable<Argument> arguments, LabelValue normalLabel, LabelValue exceptionLabel)
        {
            ReturnType = returnType;
            Pointer = pointer;
            Arguments = arguments.ToArray ();
            NormalLabel = normalLabel;
            ExceptionLabel = exceptionLabel;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals {
            get {
                foreach (var l in Pointer.ReferencedLocals) {
                    yield return l;
                }
                foreach (var a in Arguments) {
                    foreach (var l in a.Value.ReferencedLocals) {
                        yield return l;
                    }
                }
            }
        }

        public override IEnumerable<LocalSymbol> NextLabelSymbols => new[] { NormalLabel.Symbol, ExceptionLabel.Symbol };

        public override LType ResultType (Module module) => ReturnType;

        public override string ToString ()
        {
            return $"invoke {ReturnType} {Pointer}({String.Join (", ", (object[])Arguments)})";
        }

        public override bool IsIdempotent (FunctionDefinition function)
        {
            return false;
        }
    }

    public class LandingPadInstruction : Instruction
    {
        public readonly LType Type;
        public readonly TypedValue CatchValue;

        public LandingPadInstruction (LType type, TypedValue catchValue = null)
        {
            Type = type ?? throw new ArgumentNullException (nameof (type));
            CatchValue = catchValue;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => CatchValue?.ReferencedLocals ?? Enumerable.Empty<LocalSymbol> ();
        public override LType ResultType (Module module) => Type;
    }

    public class LoadInstruction : Instruction
    {
        public readonly LType Type;
        public readonly TypedValue Pointer;
        public readonly bool IsVolatile;
        public readonly bool IsAtomic;

        public LoadInstruction (LType type, TypedValue pointer, bool isVolatile, bool isAtomic)
        {
            Type = type;
            Pointer = pointer;
            IsVolatile = isVolatile;
            IsAtomic = isAtomic;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Pointer.ReferencedLocals;
        public override LType ResultType (Module module) => Type;

        public override string ToString () => $"load type={Type}, pointer={Pointer}";
    }

    public class LshrInstruction : BinaryInstruction
    {
        public LshrInstruction (LType type, Value op1, Value op2, bool exact)
            : base (type, op1, op2)
        {
        }
    }

    public class MultiplyInstruction : BinaryInstruction
    {
        public MultiplyInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class OrInstruction : BinaryInstruction
    {
        public OrInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class PhiInstruction : Instruction
    {
        public readonly LType Type;
        public readonly PhiValue[] Values;

        public PhiInstruction (LType type, IEnumerable<PhiValue> values)
        {
            Type = type;
            Values = values.ToArray ();
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals {
            get {
                foreach (var v in Values) {
                    foreach (var l in v.Value.ReferencedLocals) {
                        yield return l;
                    }
                }
            }
        }

        public override LType ResultType (Module module) => Type;
    }

    public class PhiValue
    {
        public readonly Value Value;
        public readonly Value Label;

        public PhiValue (Value value, Value label)
        {
            Value = value;
            Label = label;
        }
    }

    public class PtrtointInstruction : ConversionInstruction
    {
        public PtrtointInstruction (TypedValue input, LType outputType)
            : base (input, outputType)
        {
        }
    }

    public class UdivInstruction : BinaryInstruction
    {
        public UdivInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class UnreachableInstruction : TerminatorInstruction
    {
        public static readonly UnreachableInstruction Unreachable = new UnreachableInstruction ();

        public override IEnumerable<LocalSymbol> ReferencedLocals => Enumerable.Empty<LocalSymbol> ();

        public override IEnumerable<LocalSymbol> NextLabelSymbols => Enumerable.Empty<LocalSymbol> ();

        public override LType ResultType (Module module) => VoidType.Void;
    }

    public class UremInstruction : BinaryInstruction
    {
        public UremInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class ResumeInstruction : TerminatorInstruction
    {
        public readonly TypedValue Value;

        public ResumeInstruction (TypedValue value)
        {
            Value = value;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Value.ReferencedLocals;

        public override IEnumerable<LocalSymbol> NextLabelSymbols => Enumerable.Empty<LocalSymbol> ();
    }

    public class RetInstruction : TerminatorInstruction
    {
        public readonly TypedValue Value;

        public RetInstruction (TypedValue value)
        {
            Value = value;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Value.ReferencedLocals;

        public override IEnumerable<LocalSymbol> NextLabelSymbols => Enumerable.Empty<LocalSymbol> ();
    }

    public class SdivInstruction : BinaryInstruction
    {
        public SdivInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class SelectInstruction : Instruction
    {
        public readonly LType Type;
        public readonly Value Condition;
        public readonly TypedValue Value1;
        public readonly TypedValue Value2;

        public SelectInstruction (LType type, Value condition, TypedValue value1, TypedValue value2)
        {
            Type = type;
            Condition = condition;
            Value1 = value1;
            Value2 = value2;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Condition.ReferencedLocals.Concat (Value1.ReferencedLocals).Concat (Value2.ReferencedLocals);
        public override LType ResultType (Module module) => Value1.Type;
    }

    public class SextInstruction : Instruction
    {
        public readonly TypedValue Value;
        public readonly LType Type;

        public SextInstruction (TypedValue value, LType type)
        {
            Value = value;
            Type = type;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Value.ReferencedLocals;
        public override LType ResultType (Module module) => Type;
    }

    public class ShlInstruction : BinaryInstruction
    {
        public ShlInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class ShuffleVectorInstruction : Instruction
    {
        public readonly TypedValue Value1;
        public readonly TypedValue Value2;
        public readonly TypedValue Mask;

        public readonly VectorType Type;

        public ShuffleVectorInstruction (TypedValue value1, TypedValue value2, TypedValue mask)
        {
            Value1 = value1 ?? throw new ArgumentNullException (nameof (value1));
            Value2 = value2 ?? throw new ArgumentNullException (nameof (value2));
            Mask = mask ?? throw new ArgumentNullException (nameof (mask));

            Type = new VectorType (((VectorType)Mask.Type).Length, ((VectorType)Value1.Type).ElementType);
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals =>
            Value1.ReferencedLocals.Concat (Value2.ReferencedLocals);

        public override LType ResultType (Module module) => Type;
    }

    public class SitofpInstruction : ConversionInstruction
    {
        public SitofpInstruction (TypedValue input, LType outputType)
            : base (input, outputType)
        {
        }
    }

    public class SremInstruction : BinaryInstruction
    {
        public SremInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class StoreInstruction : Instruction
    {
        public readonly TypedValue Value;
        public readonly TypedValue Pointer;
        public readonly bool IsVolatile;

        public StoreInstruction (TypedValue value, TypedValue pointer, bool isVolatile)
        {
            Value = value;
            Pointer = pointer;
            IsVolatile = isVolatile;
        }

        public override string ToString () => $"{Pointer} <- {Value}";
        public override IEnumerable<LocalSymbol> ReferencedLocals => Value.ReferencedLocals.Concat (Pointer.ReferencedLocals);
        public override LType ResultType (Module module) => VoidType.Void;
    }

    public class SubInstruction : AtomicBinaryInstruction
    {
        public SubInstruction (LType type, Value op1, Value op2, bool isAtomic)
            : base (type, op1, op2, isAtomic: isAtomic)
        {
        }
    }

    public class SwitchInstruction : TerminatorInstruction
    {
        public readonly TypedValue Value;
        public readonly LabelValue DefaultLabel;
        public readonly SwitchCase[] Cases;

        public SwitchInstruction (TypedValue value, LabelValue pointer, IEnumerable<SwitchCase> cases)
        {
            Value = value;
            DefaultLabel = pointer;
            Cases = cases.ToArray ();
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Value.ReferencedLocals.Concat (DefaultLabel.ReferencedLocals);
        public override LType ResultType (Module module) => VoidType.Void;
        public override IEnumerable<LocalSymbol> NextLabelSymbols =>
            Cases.Select (x => x.Label.Symbol).Concat (new[] { DefaultLabel.Symbol });
    }

    public class SwitchCase
    {
        public readonly TypedConstant Value;
        public readonly LabelValue Label;

        public SwitchCase (TypedConstant value, LabelValue label)
        {
            Value = value;
            Label = label;
        }
    }

    public class TruncInstruction : ConversionInstruction
    {
        public TruncInstruction (TypedValue value, LType type)
            : base (value, type)
        {
        }
    }

    public abstract class ConversionInstruction : Instruction
    {
        public readonly TypedValue Value;
        public readonly LType Type;

        protected ConversionInstruction (TypedValue value, LType type)
        {
            Value = value;
            Type = type;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Value.ReferencedLocals;
        public override LType ResultType (Module module) => Type;
        public override bool IsIdempotent (FunctionDefinition function) => Value.Value.IsIdempotent (function);
    }

    public class UitofpInstruction : ConversionInstruction
    {
        public UitofpInstruction (TypedValue input, LType outputType)
            : base (input, outputType)
        {
        }
    }

    public class XorInstruction : BinaryInstruction
    {
        public XorInstruction (LType type, Value op1, Value op2)
            : base (type, op1, op2)
        {
        }
    }

    public class ZextInstruction : Instruction
    {
        public readonly TypedValue Value;
        public readonly LType Type;

        public ZextInstruction (TypedValue value, LType type)
        {
            Value = value;
            Type = type;
        }

        public override IEnumerable<LocalSymbol> ReferencedLocals => Value.ReferencedLocals;
        public override LType ResultType (Module module) => Type;
        public override bool IsIdempotent (FunctionDefinition function) => Value.Value.IsIdempotent (function);
    }
}
