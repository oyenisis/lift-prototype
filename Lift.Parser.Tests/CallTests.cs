using Lift.Lexing;
using Lift.Parser.Values;

namespace Lift.Parser.Tests
{
    public class CallTests
    {
        [Fact]
        public void ZeroArg()
        {
            List<Token> tokens = [
                new Token(TokenType.Identifier, 1, "method"),
                new Token(TokenType.LParen, 1, "("),
                new Token(TokenType.RParen, 1, ")")
            ];

            Parser parser = new(tokens);
            List<Statement> program = parser.Parse();

            if (parser.Coil.HasErrors() || parser.Coil.HasWarnings()) Assert.Fail();

            Assert.IsType<Statement.ExpressionStmt>(program[0]);

            Expression e = (program[0] as Statement.ExpressionStmt)!.Expression;

            Assert.Equal(tokens[0], e.Blame);
            Assert.IsType<Expression.Call>(e);
        }

        [Fact]
        public void SingleArg()
        {
            List<Token> tokens = [
                new Token(TokenType.Identifier, 1, "method"),
                new Token(TokenType.LParen, 1, "("),
                new Token(TokenType.Integer, 1, "69"),
                new Token(TokenType.RParen, 1, ")")
            ];

            Parser parser = new(tokens);
            List<Statement> program = parser.Parse();

            if (parser.Coil.HasErrors() || parser.Coil.HasWarnings()) Assert.Fail();

            Assert.IsType<Statement.ExpressionStmt>(program[0]);

            Expression e = (program[0] as Statement.ExpressionStmt)!.Expression;

            Assert.Equal(tokens[0], e.Blame);
            Assert.IsType<Expression.Call>(e);

            Expression.Call call = (Expression.Call)e;

            Assert.Equal(tokens[0], call.Identifier);
            Assert.IsType<Expression.Literal>(call.Arguments[0]);

            Expression.Literal arg1 = (Expression.Literal)call.Arguments[0];

            Assert.Equal(tokens[2], arg1.Value.blame);
            Assert.IsType<LiftInteger>(arg1.Value);
            Assert.Equal(69, ((LiftInteger)arg1.Value).value);
        }

        [Fact]
        public void MultiArg()
        {
            List<Token> tokens = [
                new Token(TokenType.Identifier, 1, "method"),
                new Token(TokenType.LParen, 1, "("),
                new Token(TokenType.Integer, 1, "69"),
                new Token(TokenType.Comma, 1, ","),
                new Token(TokenType.True, 1, "true"),
                new Token(TokenType.RParen, 1, ")")
            ];

            Parser parser = new(tokens);
            List<Statement> program = parser.Parse();

            if (parser.Coil.HasErrors() || parser.Coil.HasWarnings()) Assert.Fail();

            Assert.IsType<Statement.ExpressionStmt>(program[0]);

            Expression e = (program[0] as Statement.ExpressionStmt)!.Expression;

            Assert.Equal(tokens[0], e.Blame);
            Assert.IsType<Expression.Call>(e);

            Expression.Call call = (Expression.Call)e;

            Assert.Equal(tokens[0], call.Identifier);
            Assert.IsType<Expression.Literal>(call.Arguments[0]);
            Assert.IsType<Expression.Literal>(call.Arguments[1]);

            Expression.Literal arg1 = (Expression.Literal)call.Arguments[0];
            Expression.Literal arg2 = (Expression.Literal)call.Arguments[1];

            Assert.Equal(tokens[2], arg1.Value.blame);
            Assert.IsType<LiftInteger>(arg1.Value);
            Assert.Equal(69, ((LiftInteger)arg1.Value).value);

            Assert.Equal(tokens[4], arg2.Value.blame);
            Assert.IsType<LiftBoolean>(arg2.Value);
            Assert.True(((LiftBoolean)arg2.Value).value);
        }

        [Fact]
        public void ExpressionArg()
        {
            List<Token> tokens = [
                new Token(TokenType.Identifier, 1, "method"),
                new Token(TokenType.LParen, 1, "("),
                new Token(TokenType.Integer, 1, "69"),
                new Token(TokenType.Plus, 1, "+"),
                new Token(TokenType.LParen, 1, "("),
                new Token(TokenType.Real, 1, "7.9"),
                new Token(TokenType.Plus, 1, "+"),
                new Token(TokenType.Integer, 1, "2"),
                new Token(TokenType.RParen, 1, ")"),
                new Token(TokenType.RParen, 1, ")")
            ];

            Parser parser = new(tokens);
            List<Statement> program = parser.Parse();

            if (parser.Coil.HasErrors() || parser.Coil.HasWarnings()) Assert.Fail();

            Assert.IsType<Statement.ExpressionStmt>(program[0]);

            Expression e = (program[0] as Statement.ExpressionStmt)!.Expression;

            Assert.Equal(tokens[0], e.Blame);
            Assert.IsType<Expression.Call>(e);

            Expression.Call call = (Expression.Call)e;

            Assert.Equal(tokens[0], call.Identifier);
            Assert.IsType<Expression.Binary>(call.Arguments[0]);

            Expression.Binary arg1 = (Expression.Binary)call.Arguments[0];

            Assert.Equal(tokens[3], arg1.Blame);
            Assert.IsType<Expression.Literal>(arg1.Left);
            Assert.IsType<Expression.Group>(arg1.Right);

            Expression.Literal left = (Expression.Literal)arg1.Left;

            Assert.IsType<LiftInteger>(left.Value);
            Assert.Equal(69, ((LiftInteger)left.Value).value);

            Expression.Group right = (Expression.Group)arg1.Right;

            Assert.IsType<Expression.Binary>(right.Expression);

            Expression.Binary groupExpr = (Expression.Binary)right.Expression;

            Assert.Equal(tokens[6], groupExpr.Blame);
            Assert.IsType<Expression.Literal>(groupExpr.Left);
            Assert.IsType<Expression.Literal>(groupExpr.Right);

            Expression.Literal groupLeft = (Expression.Literal)groupExpr.Left;
            Expression.Literal groupRight = (Expression.Literal)groupExpr.Right;

            Assert.IsType<LiftReal>(groupLeft.Value);
            Assert.Equal(7.9d, ((LiftReal)groupLeft.Value).value);
            Assert.IsType<LiftInteger>(groupRight.Value);
            Assert.Equal(2, ((LiftInteger)groupRight.Value).value);
        }
    }
}