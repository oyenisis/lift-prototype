namespace Lift.Lexing
{
    public enum LexerErrorCodes : ushort
    {
        UnrecognisedCharacter = 1,
        UnterminatedString = 2,
        InvalidEscapeCharacter = 3,
        NewlineInString = 4
    }
}
