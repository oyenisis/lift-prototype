using Lift.Lexing;

namespace Lift.Parser
{
    public abstract record class Statement(Token Blame)
    {
        public sealed record class ExpressionStmt(Expression Expression) : Statement(Expression.Blame);
        public sealed record class Import(Token Blame, List<Token> Identifiers, string Source) : Statement(Blame);
    }
}
