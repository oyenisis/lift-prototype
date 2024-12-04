namespace Lift.Lexing
{
    public record class Token(TokenType Type, int Line, string Lexeme);
}
