using Lift.ErrorHandling;
using Lift.Lexing;

namespace Lift.Parser.Values
{
    public sealed class LiftBoolean(Token blame, bool value) : LiftValue(blame, "bool")
    {
        public readonly bool value = value;

        public override string ToString() => value.ToString();

        public override LiftValue Add(LiftValue other, ErrorCoil coil)
        {
            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't add bool and {other.typeName} on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
        public override LiftValue Subtract(LiftValue other, ErrorCoil coil)
        {
            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't subtract bool and {other.typeName} on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
        public override LiftValue Multiply(LiftValue other, ErrorCoil coil)
        {
            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't multiply bool and {other.typeName} on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
        public override LiftValue Divide(LiftValue other, ErrorCoil coil)
        {
            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't divide bool and {other.typeName} on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
    }
}
