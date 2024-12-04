namespace Lift.Lexing.Tests
{
    public class ValueTests
    {
        [Fact]
        public void Integer1()
        {
            Lexer lexer = new("5 69\n1663");
            List<Token> tokens = lexer.Lex();

            if (lexer.Coil.HasErrors() || lexer.Coil.HasWarnings()) Assert.Fail();

            Assert.Equal(TokenType.Integer, tokens[0].Type);
            Assert.Equal(TokenType.Integer, tokens[1].Type);
            Assert.Equal(TokenType.Integer, tokens[2].Type);
            Assert.Equal("5", tokens[0].Lexeme);
            Assert.Equal("69", tokens[1].Lexeme);
            Assert.Equal("1663", tokens[2].Lexeme);
            Assert.Equal(1, tokens[0].Line);
            Assert.Equal(1, tokens[1].Line);
            Assert.Equal(2, tokens[2].Line);
        }

        [Fact]
        public void Real1()
        {
            Lexer lexer = new("5.7 69.42\n\n1663.8");
            List<Token> tokens = lexer.Lex();

            if (lexer.Coil.HasErrors() || lexer.Coil.HasWarnings()) Assert.Fail();

            Assert.Equal(TokenType.Real, tokens[0].Type);
            Assert.Equal(TokenType.Real, tokens[1].Type);
            Assert.Equal(TokenType.Real, tokens[2].Type);
            Assert.Equal("5.7", tokens[0].Lexeme);
            Assert.Equal("69.42", tokens[1].Lexeme);
            Assert.Equal("1663.8", tokens[2].Lexeme);
            Assert.Equal(1, tokens[0].Line);
            Assert.Equal(1, tokens[1].Line);
            Assert.Equal(3, tokens[2].Line);
        }
    }
}