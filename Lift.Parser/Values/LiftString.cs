using Lift.ErrorHandling;
using Lift.Lexing;

namespace Lift.Parser.Values
{
    public sealed class LiftString(Token blame, string value) : LiftValue(blame, "string")
    {
        public readonly string value = value;

        public override string ToString() => value;

        public override LiftValue Add(LiftValue other, ErrorCoil coil) => new LiftString(blame, value + other);
        public override LiftValue Subtract(LiftValue other, ErrorCoil coil)
        {
            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't subtract from string at {blame.Lexeme} on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
        public override LiftValue Multiply(LiftValue other, ErrorCoil coil)
        {
            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't multiply with string at {blame.Lexeme} on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
        public override LiftValue Divide(LiftValue other, ErrorCoil coil)
        {
            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't divide with string at {blame.Lexeme} on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
    }
}
