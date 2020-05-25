using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TinyLang.Adapters.Factories;
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
            _lexer = new Lexer(new StreamReaderAdapterFactory());
        }

        [TestCase("if else elseif func return true false;")]
        public void KeywordsMatchTest(string input)
        {
            var expectedTokens = new List<Token>
            {
                new Token(TokenType.Keyword, "if")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 1, TokenEndIndex = 2}
                },
                new Token(TokenType.Keyword, "else")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 4, TokenEndIndex = 7}
                },
                new Token(TokenType.Keyword, "elseif")
                {
                    TokenLocation =  new TokenLocation{LineNumber = 1, TokenBeginIndex = 9, TokenEndIndex = 14}
                },
                new Token(TokenType.Keyword, "func")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 16, TokenEndIndex = 19}
                },
                new Token(TokenType.Keyword, "return")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 21, TokenEndIndex = 26}
                },
                new Token(TokenType.Keyword, "true")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 28, TokenEndIndex = 31}
                },
                new Token(TokenType.Keyword, "false")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 33, TokenEndIndex = 37}
                },
                new Token(TokenType.Separator, ";")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 38, TokenEndIndex = 38}
                }
            };

            var actualTokens = _lexer.Tokenize(input);

            AssertTokensMatch(expectedTokens, actualTokens);
        }

        [TestCase("( ) { } ; ,")]
        public void SeparatorsMatchTest(string input)
        {
            var expectedTokens = new List<Token>
            {
                new Token(TokenType.Separator, "(")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 1, TokenEndIndex = 1}
                },
                new Token(TokenType.Separator, ")")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 3, TokenEndIndex = 3}
                },
                new Token(TokenType.Separator, "{")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 5, TokenEndIndex = 5}
                },
                new Token(TokenType.Separator, "}")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 7, TokenEndIndex = 7}
                },
                new Token(TokenType.Separator, ";")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 9, TokenEndIndex = 9}
                },
                new Token(TokenType.Separator, ",")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 11, TokenEndIndex = 11}
                }
            };

            var actualTokens = _lexer.Tokenize(input);

            AssertTokensMatch(expectedTokens, actualTokens);
        }

        [TestCase("+ - * / = == ! != || && > >= < <=;")]
        public void OperatorssMatchTest(string input)
        {
            var expectedTokens = new List<Token>
            {
                new Token(TokenType.Operator, "+")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 1, TokenEndIndex = 1}
                },
                new Token(TokenType.Operator, "-")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 3, TokenEndIndex = 3}
                },
                new Token(TokenType.Operator, "*")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 5, TokenEndIndex = 5}
                },
                new Token(TokenType.Operator, "/")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 7, TokenEndIndex = 7}
                },
                new Token(TokenType.Operator, "=")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 9, TokenEndIndex = 9}
                },
                new Token(TokenType.Operator, "==")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 11, TokenEndIndex = 12}
                },
                new Token(TokenType.Operator, "!")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 14, TokenEndIndex = 14}
                },
                new Token(TokenType.Operator, "!=")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 16, TokenEndIndex = 17}
                },
                new Token(TokenType.Operator, "||")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 19, TokenEndIndex = 20}
                },
                new Token(TokenType.Operator, "&&")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 22, TokenEndIndex = 23}
                },
                new Token(TokenType.Operator, ">")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 25, TokenEndIndex = 25}
                },
                new Token(TokenType.Operator, ">=")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 27, TokenEndIndex = 28}
                },
                new Token(TokenType.Operator, "<")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 30, TokenEndIndex = 30}
                },
                new Token(TokenType.Operator, "<=")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 32, TokenEndIndex = 33}
                }
            };

            var actualTokens = _lexer.Tokenize(input);

            AssertTokensMatch(expectedTokens, actualTokens);
        }



        [TestCase("SimpleAssignment_AndReturn_Function")]
        public void SimpleFunctionDeclaration_AndLiteralReturn_Test(string testCaseName)
        {
            var codeUnderTest = LoadTestCaseFromFile(testCaseName);
            var expectedTokens = new List<Token>
            {
                new Token(TokenType.Keyword, "func")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 1, TokenEndIndex = 4}
                },
                new Token(TokenType.Identifier, "main")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 6, TokenEndIndex = 9}
                },
                new Token(TokenType.Separator, "(")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, ")")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, "{")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 13, TokenEndIndex = 13}
                },
                new Token(TokenType.Keyword, "return")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 2, TokenEndIndex = 7}
                },
                new Token(TokenType.Literal, "2")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 9, TokenEndIndex = 9}
                },
                new Token(TokenType.Separator, ";")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, "}")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 1, TokenEndIndex = 1}
                }
            };

            var actualTokens = _lexer.Tokenize(codeUnderTest);

            AssertTokensMatch(expectedTokens, actualTokens);
        }

        [TestCase("SimpleIfCondition_InFunction")]
        public void SimpleIfCondition_AndReturn_InFunction(string testCaseName)
        {
            var codeUnderTest = LoadTestCaseFromFile(testCaseName);
            var expectedTokens = new List<Token>
            {
                new Token(TokenType.Keyword, "func")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 1, TokenEndIndex = 4}
                },
                new Token(TokenType.Identifier, "main")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 6, TokenEndIndex = 9}
                },
                new Token(TokenType.Separator, "(")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, ")")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, "{")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 13, TokenEndIndex = 13}
                },
                new Token(TokenType.Keyword, "if")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 2, TokenEndIndex = 3}
                },
                new Token(TokenType.Separator, "(")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 5, TokenEndIndex = 5}
                },
                new Token(TokenType.Literal, "2")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 6, TokenEndIndex = 6}
                },
                new Token(TokenType.Operator, ">")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 8, TokenEndIndex = 8}
                },
                new Token(TokenType.Literal, "1")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, ")")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, "{")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 13, TokenEndIndex = 13}
                },
                new Token(TokenType.Keyword, "return")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 3, TokenEndIndex = 8}
                },
                new Token(TokenType.Literal, "2")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, ";")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, "}")
                {
                    TokenLocation = new TokenLocation{LineNumber = 4, TokenBeginIndex = 2, TokenEndIndex = 2}
                },
                new Token(TokenType.Keyword, "return")
                {
                    TokenLocation = new TokenLocation{LineNumber = 5, TokenBeginIndex = 2, TokenEndIndex = 7}
                },
                new Token(TokenType.Literal, "1")
                {
                    TokenLocation = new TokenLocation{LineNumber = 5, TokenBeginIndex = 9, TokenEndIndex = 9}
                },
                new Token(TokenType.Separator, ";")
                {
                    TokenLocation = new TokenLocation{LineNumber = 5, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, "}")
                {
                    TokenLocation = new TokenLocation{LineNumber = 6, TokenBeginIndex = 1, TokenEndIndex = 1}
                }
            };

            var actualTokens = _lexer.Tokenize(codeUnderTest);

            AssertTokensMatch(expectedTokens, actualTokens);
        }

        [TestCase("ConditionalFunctionWithParameters")]
        public void SimpleIfCondition_WithParameters_AndReturn_InFunction(string testCaseName)
        {
            var codeUnderTest = LoadTestCaseFromFile(testCaseName);
            var expectedTokens = new List<Token>
            {
                new Token(TokenType.Keyword, "func")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 1, TokenEndIndex = 4}
                },
                new Token(TokenType.Identifier, "main")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 6, TokenEndIndex = 9}
                },
                new Token(TokenType.Separator, "(")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Identifier, "a")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, ",")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 12, TokenEndIndex = 12}
                },
                new Token(TokenType.Identifier, "b")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 14, TokenEndIndex = 14}
                },
                new Token(TokenType.Separator, ")")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 15, TokenEndIndex = 15}
                },
                new Token(TokenType.Separator, "{")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 17, TokenEndIndex = 17}
                },
                new Token(TokenType.Keyword, "if")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 2, TokenEndIndex = 3}
                },
                new Token(TokenType.Separator, "(")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 5, TokenEndIndex = 5}
                },
                new Token(TokenType.Identifier, "a")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 6, TokenEndIndex = 6}
                },
                new Token(TokenType.Operator, ">")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 8, TokenEndIndex = 8}
                },
                new Token(TokenType.Identifier, "b")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, ")")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, "{")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 13, TokenEndIndex = 13}
                },
                new Token(TokenType.Keyword, "return")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 3, TokenEndIndex = 8}
                },
                new Token(TokenType.Identifier, "b")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, ";")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, "}")
                {
                    TokenLocation = new TokenLocation{LineNumber = 4, TokenBeginIndex = 2, TokenEndIndex = 2}
                },
                new Token(TokenType.Keyword, "return")
                {
                    TokenLocation = new TokenLocation{LineNumber = 5, TokenBeginIndex = 2, TokenEndIndex = 7}
                },
                new Token(TokenType.Identifier, "a")
                {
                    TokenLocation = new TokenLocation{LineNumber = 5, TokenBeginIndex = 9, TokenEndIndex = 9}
                },
                new Token(TokenType.Separator, ";")
                {
                    TokenLocation = new TokenLocation{LineNumber = 5, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, "}")
                {
                    TokenLocation = new TokenLocation{LineNumber = 6, TokenBeginIndex = 1, TokenEndIndex = 1}
                }
            };

            var actualTokens = _lexer.Tokenize(codeUnderTest);

            AssertTokensMatch(expectedTokens, actualTokens);
        }

        [TestCase("ConditionalFunctionWithParametersAndStringLiteral")]
        public void SimpleIfCondition_WithParameters_AndReturn_AndStringLiteral_InFunction(string testCaseName)
        {
            var codeUnderTest = LoadTestCaseFromFile(testCaseName);
            var expectedTokens = new List<Token>()
            {
                new Token(TokenType.Keyword, "func")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 1, TokenEndIndex = 4}
                },
                new Token(TokenType.Identifier, "main")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 6, TokenEndIndex = 9}
                },
                new Token(TokenType.Separator, "(")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Identifier, "a")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, ",")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 12, TokenEndIndex = 12}
                },
                new Token(TokenType.Identifier, "b")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 14, TokenEndIndex = 14}
                },
                new Token(TokenType.Separator, ")")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 15, TokenEndIndex = 15}
                },
                new Token(TokenType.Separator, "{")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 17, TokenEndIndex = 17}
                },
                new Token(TokenType.Keyword, "if")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 2, TokenEndIndex = 3}
                },
                new Token(TokenType.Separator, "(")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 5, TokenEndIndex = 5}
                },
                new Token(TokenType.Identifier, "a")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 6, TokenEndIndex = 6}
                },
                new Token(TokenType.Operator, ">")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 8, TokenEndIndex = 8}
                },
                new Token(TokenType.Identifier, "b")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, ")")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, "{")
                {
                    TokenLocation = new TokenLocation{LineNumber = 2, TokenBeginIndex = 13, TokenEndIndex = 13}
                },
                new Token(TokenType.Keyword, "return")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 3, TokenEndIndex = 8}
                },
                new Token(TokenType.Identifier, "b")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, ";")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, "}")
                {
                    TokenLocation = new TokenLocation{LineNumber = 4, TokenBeginIndex = 2, TokenEndIndex = 2}
                },
                new Token(TokenType.Keyword, "return")
                {
                    TokenLocation = new TokenLocation{LineNumber = 5, TokenBeginIndex = 2, TokenEndIndex = 7}
                },
                new Token(TokenType.Literal, "\"hello\"")
                {
                    TokenLocation = new TokenLocation{LineNumber = 5, TokenBeginIndex = 9, TokenEndIndex = 15}
                },
                new Token(TokenType.Separator, ";")
                {
                    TokenLocation = new TokenLocation{LineNumber = 5, TokenBeginIndex = 16, TokenEndIndex = 16}
                },
                new Token(TokenType.Separator, "}")
                {
                    TokenLocation = new TokenLocation{LineNumber = 6, TokenBeginIndex = 1, TokenEndIndex = 1}
                }
            };

            var actualTokens = _lexer.Tokenize(codeUnderTest);

            AssertTokensMatch(expectedTokens, actualTokens);
        }

        [TestCase("ConditionalFunctionWithParametersAndStringLiteralAndComment")]
        public void SimpleIfCondition_WithParameters_AndReturn_AndStringLiteral_AndComment_InFunction(string testCaseName)
        {
            var codeUnderTest = LoadTestCaseFromFile(testCaseName);
            var expectedTokens = new List<Token>()
            {
                new Token(TokenType.Keyword, "func")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 1, TokenEndIndex = 4}
                },
                new Token(TokenType.Identifier, "main")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 6, TokenEndIndex = 9}
                },
                new Token(TokenType.Separator, "(")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Identifier, "a")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, ",")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 12, TokenEndIndex = 12}
                },
                new Token(TokenType.Identifier, "b")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 14, TokenEndIndex = 14}
                },
                new Token(TokenType.Separator, ")")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 15, TokenEndIndex = 15}
                },
                new Token(TokenType.Separator, "{")
                {
                    TokenLocation = new TokenLocation{LineNumber = 1, TokenBeginIndex = 17, TokenEndIndex = 17}
                },
                new Token(TokenType.Keyword, "if")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 2, TokenEndIndex = 3}
                },
                new Token(TokenType.Separator, "(")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 5, TokenEndIndex = 5}
                },
                new Token(TokenType.Identifier, "a")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 6, TokenEndIndex = 6}
                },
                new Token(TokenType.Operator, ">")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 8, TokenEndIndex = 8}
                },
                new Token(TokenType.Identifier, "b")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, ")")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, "{")
                {
                    TokenLocation = new TokenLocation{LineNumber = 3, TokenBeginIndex = 13, TokenEndIndex = 13}
                },
                new Token(TokenType.Keyword, "return")
                {
                    TokenLocation = new TokenLocation{LineNumber = 4, TokenBeginIndex = 3, TokenEndIndex = 8}
                },
                new Token(TokenType.Identifier, "b")
                {
                    TokenLocation = new TokenLocation{LineNumber = 4, TokenBeginIndex = 10, TokenEndIndex = 10}
                },
                new Token(TokenType.Separator, ";")
                {
                    TokenLocation = new TokenLocation{LineNumber = 4, TokenBeginIndex = 11, TokenEndIndex = 11}
                },
                new Token(TokenType.Separator, "}")
                {
                    TokenLocation = new TokenLocation{LineNumber = 5, TokenBeginIndex = 2, TokenEndIndex = 2}
                },
                new Token(TokenType.Keyword, "return")
                {
                    TokenLocation = new TokenLocation{LineNumber = 6, TokenBeginIndex = 2, TokenEndIndex = 7}
                },
                new Token(TokenType.Literal, "\"hello\"")
                {
                    TokenLocation = new TokenLocation{LineNumber = 6, TokenBeginIndex = 9, TokenEndIndex = 15}
                },
                new Token(TokenType.Separator, ";")
                {
                    TokenLocation = new TokenLocation{LineNumber = 6, TokenBeginIndex = 16, TokenEndIndex = 16}
                },
                new Token(TokenType.Separator, "}")
                {
                    TokenLocation = new TokenLocation{LineNumber = 7, TokenBeginIndex = 1, TokenEndIndex = 1}
                }
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
                    $"Expected token type {expectedTokenType} but was {actualTokenType} for token \"{actualTokens[i].TokenValue}\" at index {i}");

                var expectedTokenValue = expectedTokens[i].TokenValue;
                var actualTokenValue = actualTokens[i].TokenValue;
                Assert.AreEqual(expectedTokenValue, actualTokenValue,
                        $"Expected token value {expectedTokenValue} but was {actualTokenValue} for token \"{actualTokens[i].TokenValue}\" at index {i}");

                var expectedTokenLocation = expectedTokens[i].TokenLocation;
                var actualTokenLocation = actualTokens[i].TokenLocation;
                Assert.AreEqual(expectedTokenLocation.LineNumber, actualTokenLocation.LineNumber,
                    $"Expected line number {expectedTokenLocation.LineNumber} but was {actualTokenLocation.LineNumber} for token \"{actualTokens[i].TokenValue}\" at index {i}");
                Assert.AreEqual(expectedTokenLocation.TokenBeginIndex, actualTokenLocation.TokenBeginIndex,
                    $"Expected begin index of {expectedTokenLocation.TokenBeginIndex} but was {actualTokenLocation.TokenBeginIndex} for token \"{actualTokens[i].TokenValue}\" at index {i}");
                Assert.AreEqual(expectedTokenLocation.TokenEndIndex, actualTokenLocation.TokenEndIndex,
                    $"Expected end index of {expectedTokenLocation.TokenEndIndex} but was {actualTokenLocation.TokenEndIndex} for token \"{actualTokens[i].TokenValue}\" at index {i}");
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
