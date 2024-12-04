namespace Lift.Lexing.Tests
{
    public class StringTests
    {
        [Fact]
        public void String1()
        {
            Lexer lexer = new("\"Hello, World!\"");
            List<Token> tokens = lexer.Lex();

            if (lexer.Coil.HasErrors() || lexer.Coil.HasWarnings()) Assert.Fail();

            Assert.Equal(TokenType.String, tokens[0].Type);
            Assert.Equal("Hello, World!", tokens[0].Lexeme);
            Assert.Equal(1, tokens[0].Line);
        }

        [Fact]
        public void String2()
        {
            Lexer lexer = new("\"Hello, \nWorld!");
            lexer.Lex();

            if (!lexer.Coil.HasErrors()) Assert.Fail();

            Assert.Equal((ushort)LexerErrorCodes.NewlineInString, lexer.Coil.Errors[0].Code);
            Assert.Equal((ushort)LexerErrorCodes.UnterminatedString, lexer.Coil.Errors[1].Code);
        }
    }
}
