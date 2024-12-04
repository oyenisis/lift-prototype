namespace Lift.Lexing
{
    public enum TokenType
    {
        // Single Character Tokens
        LParen, RParen, LBrace, RBrace, LBracket, RBracket, Semicolon, Colon, Comma, Dot,

        // Values
        String, Integer, Real,

        // Keywords
        True, False, Import, From,

        // Other
        Identifier,
    }
}
