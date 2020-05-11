using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TinyLang.Compiler;
using TinyLang.Models;

namespace TinyLang.Tests.LexerTests
{
    [TestFixture]
    public class LexerTests
    {
        private ILexer _lexer;

        [SetUp]
        public void SetUp()
        {
            _lexer = new Lexer();
        }

        [TestCase("if else elseif func return true false;")]
        public void KeywordsMatchTest(string input)
        {
            var expectedTokens = new List<Token>()
            {
                new Token(TokenType.Keyword, "if", typeof(string)),
                new Token(TokenType.Keyword, "else", typeof(string)),
                new Token(TokenType.Keyword, "elseif", typeof(string)),
                new Token(TokenType.Keyword, "func", typeof(string)),
                new Token(TokenType.Keyword, "return", typeof(string)),
                new Token(TokenType.Keyword, "true", typeof(string)),
                new Token(TokenType.Keyword, "false", typeof(string)),
            };

            var actualTokens = _lexer.Tokenize(input);

            AssertTokensMatch(expectedTokens, actualTokens);
        }

        [TestCase("( ) { } ; ,")]
        public void SeparatorsMatchTest(string input)
        {
            var expectedTokens = new List<Token>()
            {
                new Token(TokenType.Separator, "(", typeof(string)),
                new Token(TokenType.Separator, ")", typeof(string)),
                new Token(TokenType.Separator, "{", typeof(string)),
                new Token(TokenType.Separator, "}", typeof(string)),
                new Token(TokenType.Separator, ";", typeof(string)),
                new Token(TokenType.Separator, ",", typeof(string)),
            };

            var actualTokens = _lexer.Tokenize(input);

            AssertTokensMatch(expectedTokens, actualTokens);
        }

        [TestCase("+ - * / = == ! != || && > >= < <=;")]
        public void OperatorssMatchTest(string input)
        {
            var expectedTokens = new List<Token>()
            {
                new Token(TokenType.Operator, "+", typeof(string)),
                new Token(TokenType.Operator, "-", typeof(string)),
                new Token(TokenType.Operator, "*", typeof(string)),
                new Token(TokenType.Operator, "/", typeof(string)),
                new Token(TokenType.Operator, "=", typeof(string)),
                new Token(TokenType.Operator, "==", typeof(string)),
                new Token(TokenType.Operator, "!", typeof(string)),
                new Token(TokenType.Operator, "!=", typeof(string)),
                new Token(TokenType.Operator, "||", typeof(string)),
                new Token(TokenType.Operator, "&&", typeof(string)),
                new Token(TokenType.Operator, ">", typeof(string)),
                new Token(TokenType.Operator, ">=", typeof(string)),
                new Token(TokenType.Operator, "<", typeof(string)),
                new Token(TokenType.Operator, "<=", typeof(string)),
            };

            var actualTokens = _lexer.Tokenize(input);

            AssertTokensMatch(expectedTokens, actualTokens);
        }



        [TestCase("SimpleAssignment_AndReturn_Function")]
        public void SimpleFunctionDeclaration_AndLiteralReturn_Test(string testCaseName)
        {
            var codeUnderTest = LoadTestCaseFromFile(testCaseName);
            var expectedTokens = new List<Token>()
            {
                new Token(TokenType.Keyword, "func", typeof(string)),
                new Token(TokenType.Identifier, "main", typeof(string)),
                new Token(TokenType.Separator, "(", typeof(string)),
                new Token(TokenType.Separator, ")", typeof(string)),
                new Token(TokenType.Separator, "{", typeof(string)),
                new Token(TokenType.Keyword, "return", typeof(string)),
                new Token(TokenType.Literal, 2, typeof(double)),
                new Token(TokenType.Separator, ";", typeof(string)),
                new Token(TokenType.Separator, "}", typeof(string))
            };

            var actualTokens = _lexer.Tokenize(codeUnderTest);

            AssertTokensMatch(expectedTokens, actualTokens);
        }

        [TestCase("SimpleIfCondition_InFunction")]
        public void SimpleIfCondition_AndReturn_InFunction(string testCaseName)
        {
            var codeUnderTest = LoadTestCaseFromFile(testCaseName);
            var expectedTokens = new List<Token>()
            {
                new Token(TokenType.Keyword, "func", typeof(string)),
                new Token(TokenType.Identifier, "main", typeof(string)),
                new Token(TokenType.Separator, "(", typeof(string)),
                new Token(TokenType.Separator, ")", typeof(string)),
                new Token(TokenType.Separator, "{", typeof(string)),
                new Token(TokenType.Keyword, "if", typeof(string)),
                new Token(TokenType.Separator, "(", typeof(string)),
                new Token(TokenType.Literal, 2, typeof(double)),
                new Token(TokenType.Operator, ">", typeof(string)),
                new Token(TokenType.Literal, 1, typeof(double)),
                new Token(TokenType.Separator, ")", typeof(string)),
                new Token(TokenType.Separator, "{", typeof(string)),
                new Token(TokenType.Keyword, "return", typeof(string)),
                new Token(TokenType.Literal, 2, typeof(double)),
                new Token(TokenType.Separator, ";", typeof(string)),
                new Token(TokenType.Separator, "}", typeof(string)),
                new Token(TokenType.Keyword, "return", typeof(string)),
                new Token(TokenType.Literal, 1, typeof(double)),
                new Token(TokenType.Separator, ";", typeof(string)),
                new Token(TokenType.Separator, "}", typeof(string))
            };

            var actualTokens = _lexer.Tokenize(codeUnderTest);

            AssertTokensMatch(expectedTokens, actualTokens);
        }

        [TestCase("ConditionalFunctionWithParameters")]
        public void SimpleIfCondition_WithParameters_AndReturn_InFunction(string testCaseName)
        {
            var codeUnderTest = LoadTestCaseFromFile(testCaseName);
            var expectedTokens = new List<Token>()
            {
                new Token(TokenType.Keyword, "func", typeof(string)),
                new Token(TokenType.Identifier, "main", typeof(string)),
                new Token(TokenType.Separator, "(", typeof(string)),
                new Token(TokenType.Identifier, "a", typeof(string)),
                new Token(TokenType.Separator, ",", typeof(string)),
                new Token(TokenType.Identifier, "b", typeof(string)),
                new Token(TokenType.Separator, ")", typeof(string)),
                new Token(TokenType.Separator, "{", typeof(string)),
                new Token(TokenType.Keyword, "if", typeof(string)),
                new Token(TokenType.Separator, "(", typeof(string)),
                new Token(TokenType.Identifier, "a", typeof(string)),
                new Token(TokenType.Operator, ">", typeof(string)),
                new Token(TokenType.Identifier, "b", typeof(string)),
                new Token(TokenType.Separator, ")", typeof(string)),
                new Token(TokenType.Separator, "{", typeof(string)),
                new Token(TokenType.Keyword, "return", typeof(string)),
                new Token(TokenType.Identifier, "b", typeof(string)),
                new Token(TokenType.Separator, ";", typeof(string)),
                new Token(TokenType.Separator, "}", typeof(string)),
                new Token(TokenType.Keyword, "return", typeof(string)),
                new Token(TokenType.Identifier, "a", typeof(string)),
                new Token(TokenType.Separator, ";", typeof(string)),
                new Token(TokenType.Separator, "}", typeof(string))
            };

            var actualTokens = _lexer.Tokenize(codeUnderTest);

            AssertTokensMatch(expectedTokens, actualTokens);
        }

        private void AssertTokensMatch(List<Token> expectedTokens, List<Token> actualTokens)
        {
            for (var i = 0; i < expectedTokens.Count; i++)
            {
                var expectedTokenType = expectedTokens[i].TokenType;
                var actualTokenType = actualTokens[i].TokenType;
                Assert.AreEqual(expectedTokenType, actualTokenType,
                    $"Expected token type {expectedTokenType} but was {actualTokenType} for token at index {i}");

                var expectedTokenValue = expectedTokens[i].TokenValue;
                var actualTokenValue = actualTokens[i].TokenValue;
                Assert.AreEqual(expectedTokenValue, actualTokenValue,
                        $"Expected token value {expectedTokenValue} but was {actualTokenValue} for token at index {i}");
            }
        }

        private string LoadTestCaseFromFile(string testCaseName)
        {
            var testCaseLocation =
                Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "TinyLangSourceCodeCases", $"{testCaseName}.ty");

            using (var reader = new StreamReader(testCaseLocation))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
