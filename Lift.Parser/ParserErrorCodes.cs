namespace Lift.Parser
{
    public enum ParserErrorCodes : ushort
    {
        InvalidBinaryOperation = 1,
        UnterminatedGrouping = 2,
        InvalidToken = 3,
        InvalidImportContent = 4,
        ExpectedIdentifierInImport = 5,
        MissingClosingBraceOnImport = 6,
        MissingFromInImport = 7,
        MissingSourceInImport = 8,
        OpenCall = 9
    }
}
