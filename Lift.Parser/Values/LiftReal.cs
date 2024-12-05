using Lift.ErrorHandling;
using Lift.Lexing;

namespace Lift.Parser.Values
{
    public sealed class LiftReal(Token blame, double value) : LiftValue(blame, "real")
    {
        public readonly double value = value;

        public override string ToString() => value.ToString();

        public override LiftValue Add(LiftValue other, ErrorCoil coil)
        {
            if (other is LiftInteger i)
            {
                return new LiftReal(blame, value + i.value);
            }
            if (other is LiftReal r)
            {
                return new LiftReal(blame, value + r.value);
            }
            if (other is LiftString s)
            {
                return new LiftString(blame, value + s.value);
            }

            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't add real and {other.typeName} on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
        public override LiftValue Subtract(LiftValue other, ErrorCoil coil)
        {
            if (other is LiftInteger i)
            {
                return new LiftReal(blame, value - i.value);
            }
            if (other is LiftReal r)
            {
                return new LiftReal(blame, value - r.value);
            }

            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't subtract real and {other.typeName} on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
        public override LiftValue Multiply(LiftValue other, ErrorCoil coil)
        {
            if (other is LiftInteger i)
            {
                return new LiftReal(blame, value * i.value);
            }
            if (other is LiftReal r)
            {
                return new LiftReal(blame, value * r.value);
            }

            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't multiply real and {other.typeName} on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
        public override LiftValue Divide(LiftValue other, ErrorCoil coil)
        {
            if (other is LiftInteger i)
            {
                return new LiftReal(blame, (double)value / i.value);
            }
            if (other is LiftReal r)
            {
                return new LiftReal(blame, (double)value / r.value);
            }

            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't divide real and {other.typeName} on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
    }
}
