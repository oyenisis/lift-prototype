using Lift.ErrorHandling;

namespace Lift.Lexing
{
    public class Lexer(string text)
    {
        private static readonly Dictionary<string, TokenType> KEYWORDS = new()
        {
            {"import", TokenType.Import},
            {"from", TokenType.From}
        };

        public ErrorCoil Coil { get; } = new("Lexer");

        private readonly string _text = text;

        private int _start = 0;
        private int _current = 0;
        private int _line = 1;

        private readonly List<Token> _tokens = [];

        private bool AtEnd => _current >= _text.Length;

        private char Previous => _text[_current - 1];
        private char Peek => _text[_current];

        public List<Token> Lex()
        {
            while (!AtEnd)
            {
                _start = _current;

                switch (Advance())
                {
                    case ' ':
                    case '\r':
                    case '\t':
                        break;
                    case '\n':
                        _line++;
                        break;
                    case '(':
                        Token(TokenType.LParen);
                        break;
                    case ')':
                        Token(TokenType.RParen);
                        break;
                    case '[':
                        Token(TokenType.LBracket);
                        break;
                    case ']':
                        Token(TokenType.RBracket);
                        break;
                    case '{':
                        Token(TokenType.LBrace);
                        break;
                    case '}':
                        Token(TokenType.RBrace);
                        break;
                    case ':':
                        Token(TokenType.Colon);
                        break;
                    case ';':
                        Token(TokenType.Semicolon);
                        break;
                    case '.':
                        Token(TokenType.Dot);
                        break;
                    case ',':
                        Token(TokenType.Comma);
                        break;
                    case '"':
                        String();
                        break;
                    default:
                        {
                            if (char.IsAsciiDigit(Previous))
                            {
                                Number();
                                break;
                            }

                            if (IsIdentifierStart(Previous))
                            {
                                Identifier();
                                break;
                            }

                            Coil.AddError(new LiftMessage((ushort)LexerErrorCodes.UnrecognisedCharacter, $"Unrecognised character on line {_line}."));
                            break;
                        }
                }
            }

            return _tokens;
        }

        private void String()
        {
            while (Advance() != '"')
            {
                if (AtEnd)
                {
                    Coil.AddError(new LiftMessage((ushort)LexerErrorCodes.UnterminatedString, $"Unterminated string on line {_line}."));
                    break;
                }

                if (Previous == '\n')
                {
                    Coil.AddError(new LiftMessage((ushort)LexerErrorCodes.NewlineInString, $"Multiline string on line {_line}."));
                }

                if (Previous == '\\')
                {
                    switch (Advance())
                    {
                        case '\n':
                        case '"':
                        case '\\':
                            break;
                        default:
                            Coil.AddError(new LiftMessage((ushort)LexerErrorCodes.InvalidEscapeCharacter, $"Invalid escape character on line {_line}."));
                            break;
                    }
                }
            }

            _tokens.Add(new Token(TokenType.String, _line, _text[(_start + 1)..(_current - 1)]));
        }

        private void Number()
        {
            bool isReal = false;

            while (!AtEnd && char.IsAsciiDigit(Peek)) Advance();

            if (!AtEnd && Peek == '.')
            {
                isReal = true;
                Advance();
                while (!AtEnd && char.IsAsciiDigit(Peek)) Advance();
            }

            Token(isReal ? TokenType.Real : TokenType.Integer);
        }

        private void Identifier()
        {
            while (!AtEnd && IsIdentifier(Peek)) Advance();

            string identifier = _text[_start.._current];

            if (KEYWORDS.TryGetValue(identifier, out TokenType value)) Token(value);
            else Token(TokenType.Identifier);
        }

        private char Advance() => _text[_current++];

        private static bool IsIdentifierStart(char c) => char.IsAsciiLetter(c) || c == '_';
        private static bool IsIdentifier(char c) => IsIdentifierStart(c) || char.IsAsciiDigit(c);

        private void Token(TokenType type) => _tokens.Add(new Token(type, _line, _text[_start.._current]));
    }
}
