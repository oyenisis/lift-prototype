namespace Lift.Lexing
{
    public enum TokenType
    {
        // Single Character Tokens
        LParen, RParen, LBrace, RBrace, LBracket, RBracket, Semicolon, Colon, Comma, Dot, Eq, Bang, Plus, Minus, Star, Slash, Greater, Less,

        // Dual Character Tokens
        EqEq, BangEq, GreaterEq, LessEq,

        // Values
        String, Integer, Real,

        // Keywords
        True, False, Import, From,

        // Other
        Identifier, Error,
    }
}
