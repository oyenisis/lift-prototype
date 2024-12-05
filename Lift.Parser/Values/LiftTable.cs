using Lift.ErrorHandling;
using Lift.Lexing;

namespace Lift.Parser.Values
{
    public sealed class LiftTable(Token blame, List<LiftValue> values) : LiftValue(blame, "table")
    {
        public readonly List<LiftValue> values = values;

        public override string ToString()
        {
            string s = "[";

            foreach (LiftValue v in values)
            {
                s += v.ToString();
                s += ',';
            }

            s = s[..-1];
            s += ']';
            return s;
        }

        public override LiftValue Add(LiftValue other, ErrorCoil coil)
        {
            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't add with table on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
        public override LiftValue Subtract(LiftValue other, ErrorCoil coil)
        {
            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't subtract with table on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
        public override LiftValue Multiply(LiftValue other, ErrorCoil coil)
        {
            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't multiply with table on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
        public override LiftValue Divide(LiftValue other, ErrorCoil coil)
        {
            coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidBinaryOperation, $"Can't divide with table on line {blame.Line}"));
            return new LiftTable(Parser.ZERO_BLAME, []);
        }
    }
}
