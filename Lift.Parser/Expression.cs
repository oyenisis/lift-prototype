using Lift.Lexing;
using Lift.Parser.Values;

namespace Lift.Parser
{
    public abstract record class Expression(Token Blame)
    {
        public sealed record class Binary(Token Operator, Expression Left, Expression Right) : Expression(Operator);
        public sealed record class Call(Token Identifier, List<Expression> Arguments) : Expression(Identifier);
        public sealed record class Group(Expression Expression) : Expression(Expression.Blame);
        public sealed record class Literal(LiftValue Value) : Expression(Value.blame);
        public sealed record class Unary(Token Operator, Expression Right) : Expression(Operator);
        public sealed record class Var(Token Identifier) : Expression(Identifier);
    }
}
