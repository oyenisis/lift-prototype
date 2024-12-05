using Lift.ErrorHandling;
using Lift.Lexing;
using Lift.Parser.Values;
using System.Globalization;

namespace Lift.Parser
{
    public sealed class Parser(List<Token> tokens)
    {
        private sealed class ParseException() : Exception();

        public static readonly Token ZERO_BLAME = new(TokenType.Error, 0, "<ERR>");

        public ErrorCoil Coil { get; } = new("Parser");

        private readonly List<Token> _tokens = tokens;

        private int _current = 0;

        private bool AtEnd => _current >= _tokens.Count;

        private Token Previous => _tokens[_current - 1];
        private Token Peek => _tokens[_current];

        public List<Statement> Parse()
        {
            List<Statement> statements = [];

            while (!AtEnd)
            {
                Statement? s = Statement();
                if (s is not null) statements.Add(s);
            }

            return statements;
        }

        private Statement? Statement()
        {
            try
            {
                if (Match(TokenType.Import)) return Import();

                return new Statement.ExpressionStmt(Expression());
            }
            catch (ParseException)
            {
                Recover();
                return null;
            }
        }

        private Statement Import()
        {
            Token blame = Previous;

            List<Token> imports = [];

            if (Peek.Type == TokenType.Identifier)
            {
                imports.Add(Advance());
            }
            else
            {
                Consume(TokenType.LBrace, new LiftMessage((ushort)ParserErrorCodes.InvalidImportContent, $"Import should be either single identifier or comma-separated list surrounded by braces on line {Previous.Line}"));

                do
                {
                    imports.Add(Consume(TokenType.Identifier, new LiftMessage((ushort)ParserErrorCodes.ExpectedIdentifierInImport, $"Expected identifier in import on line {Previous.Line}. Got '{Previous.Lexeme}'")));
                } while (Match(TokenType.Comma));

                Consume(TokenType.LBrace, new LiftMessage((ushort)ParserErrorCodes.MissingClosingBraceOnImport, $"Missing '}}' after import on line {Previous.Line}"));
            }

            Consume(TokenType.From, new LiftMessage((ushort)ParserErrorCodes.MissingFromInImport, $"Expected 'from' after imports on line {Previous.Line}"));

            string source = Consume(TokenType.String, new LiftMessage((ushort)ParserErrorCodes.MissingSourceInImport, $"Expected import source as string after 'from' on line {Previous.Line}")).Lexeme;

            return new Statement.Import(blame, imports, source);
        }

        private Expression Expression() => Equality();

        private Expression Equality()
        {
            Expression e = Comparison();

            while (Match(TokenType.BangEq, TokenType.EqEq))
            {
                Token op = Previous;
                Expression right = Comparison();
                e = new Expression.Binary(op, e, right);
            }

            return e;
        }

        private Expression Comparison()
        {
            Expression e = Term();

            while (Match(TokenType.Greater, TokenType.GreaterEq, TokenType.Less, TokenType.LessEq))
            {
                Token op = Previous;
                Expression right = Term();
                return new Expression.Binary(op, e, right);
            }

            return e;
        }

        private Expression Term()
        {
            Expression e = Factor();

            while (Match(TokenType.Minus, TokenType.Plus))
            {
                Token op = Previous;
                Expression right = Factor();
                return new Expression.Binary(op, e, right);
            }

            return e;
        }

        private Expression Factor()
        {
            Expression e = Unary();

            while (Match(TokenType.Slash, TokenType.Star))
            {
                Token op = Previous;
                Expression right = Unary();
                return new Expression.Binary(op, e, right);
            }

            return e;
        }

        private Expression Unary()
        {
            if (Match(TokenType.Bang, TokenType.Minus))
            {
                Token op = Previous;
                Expression right = Unary();
                return new Expression.Unary(op, right);
            }

            return Primary();
        }

        private Expression Primary()
        {
            if (Match(TokenType.False)) return new Expression.Literal(new LiftBoolean(Previous, false));
            if (Match(TokenType.True)) return new Expression.Literal(new LiftBoolean(Previous, true));

            if (Match(TokenType.Integer)) return new Expression.Literal(new LiftInteger(Previous, int.Parse(Previous.Lexeme)));
            if (Match(TokenType.Real)) return new Expression.Literal(new LiftReal(Previous, double.Parse(Previous.Lexeme, CultureInfo.InvariantCulture)));
            if (Match(TokenType.String)) return new Expression.Literal(new LiftString(Previous, Previous.Lexeme));

            if (Match(TokenType.LParen))
            {
                Expression e = Expression()!;
                Consume(TokenType.RParen, new LiftMessage((ushort)ParserErrorCodes.UnterminatedGrouping, $"Expected ')' to close expression on line {Previous.Line}"));
                return new Expression.Group(e);
            }

            if (Match(TokenType.Identifier))
            {
                Token identifier = Previous;

                if (Match(TokenType.LParen))
                {
                    List<Expression> arguments = [];

                    if (Peek.Type != TokenType.RParen)
                    {
                        do
                        {
                            arguments.Add(Expression());
                        } while (Match(TokenType.Comma));
                    }

                    Consume(TokenType.RParen, new LiftMessage((ushort)ParserErrorCodes.OpenCall, $"Expected ')' to close call on line {Previous.Line}"));

                    return new Expression.Call(identifier, arguments);
                }
                return new Expression.Var(identifier);
            }

            Coil.AddError(new LiftMessage((ushort)ParserErrorCodes.InvalidToken, $"Invalid token '{Advance().Lexeme}' on line {Previous.Line}"));
            throw new ParseException();
        }

        private void Recover()
        {
            while (!AtEnd && !Match(TokenType.Import, TokenType.Identifier)) Advance();

            if (!AtEnd) _current--;
        }

        private Token Advance()
        {
            return _tokens[_current++];
        }

        private Token Consume(TokenType type, LiftMessage message)
        {
            if (AtEnd) throw new ParseException();

            Token t = Advance();
            if (t.Type != type)
            {
                Coil.AddError(message);
                throw new ParseException();
            }
            return t;
        }

        private bool Match(params TokenType[] types)
        {
            if (AtEnd) return false;
            if (types.Contains(Peek.Type))
            {
                Advance();
                return true;
            }
            return false;
        }
    }
}
