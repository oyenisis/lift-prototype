using Lift.ErrorHandling;
using Lift.Lexing;

namespace Lift.Parser.Values
{
    public abstract class LiftValue(Token blame, string typeName)
    {
        public readonly string typeName = typeName;
        public readonly Token blame = blame;

        public abstract new string ToString();

        public abstract LiftValue Add(LiftValue other, ErrorCoil coil);
        public abstract LiftValue Subtract(LiftValue other, ErrorCoil coil);
        public abstract LiftValue Multiply(LiftValue other, ErrorCoil coil);
        public abstract LiftValue Divide(LiftValue other, ErrorCoil coil);
    }
}
