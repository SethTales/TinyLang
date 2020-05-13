using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TinyLang.Adapters.Factories;
using TinyLang.Constants;
using TinyLang.Models;

namespace TinyLang.Compiler
{
    public interface ILexer
    {
        List<Token> Tokenize(string input);
    }

    public class Lexer : ILexer
    {
        private const int EOF = -1;

        private readonly IStreamReaderAdapterFactory _streamReaderAdapterFactory;
        private readonly Regex _identifierRegex = new Regex("([_a-zA-Z]+)");
        private readonly Regex _numericRegex = new Regex("^[1-9]\\d*(\\.\\d+)?$|^[0]\\d*(\\.\\d+)?$");

        private List<Token> _tokens;
        private Token _previousToken = null;

        public Lexer(IStreamReaderAdapterFactory streamReaderAdapterFactory)
        {
            _streamReaderAdapterFactory = streamReaderAdapterFactory;
        }

        public List<Token> Tokenize(string input)
        {
            var tokenBuilder = new StringBuilder();
            _tokens = new List<Token>();

            using (var streamReaderAdapter = _streamReaderAdapterFactory.BuildStreamReaderAdapater(input))
            {
                while (streamReaderAdapter.Peek() != EOF)
                {
                    var currentChar = (char) streamReaderAdapter.Read();
                    //eat whitespace newline or carriage return characters
                    while (char.IsWhiteSpace(currentChar) || currentChar == '\n' || currentChar == '\r')
                    {
                        currentChar = (char)streamReaderAdapter.Read();
                    }

                    tokenBuilder.Append(currentChar);
                    var nextChar = (char)streamReaderAdapter.Peek();

                    //if current char is a separator add it as a token
                    if (LanguageConstants.Separators.Any(s => s.Equals(currentChar.ToString())))
                    {
                        var token = new Token(TokenType.Separator, currentChar.ToString(), typeof(string));
                        AddTokenAndClearTokenBuilder(tokenBuilder, token);
                        continue;
                    }

                    //iterate over characters in stream, reading them into the tokenBuilder buffer until we hit a separator or whitepsace
                    while (!LanguageConstants.Separators.Any(s => s.Equals(nextChar.ToString())) && !char.IsWhiteSpace(nextChar))
                    {
                        currentChar = (char)streamReaderAdapter.Read();
                        nextChar = (char)streamReaderAdapter.Peek();
                        tokenBuilder.Append(currentChar);
                    }

                    //at this point we've reached the end of a sequence
                    //it's likely we have a token in the tokenBuilder buffer
                    //so we check the token buffer against token patterns
                    var matchedToken = CheckForMatchedToken(tokenBuilder);

                    if (matchedToken != null)
                    {
                        AddTokenAndClearTokenBuilder(tokenBuilder, matchedToken);
                    }
                }
            }

            return _tokens;
        }

        private Token CheckForMatchedToken(StringBuilder tokenBuilder)
        {
            return MatchIdentiferOrKeyword(tokenBuilder) ?? MatchNumericLiteral(tokenBuilder) ?? MatchOperator(tokenBuilder) ?? MatchStringLiteral(tokenBuilder);
        }

        private Token MatchIdentiferOrKeyword(StringBuilder tokenBuilder)
        {
            var tokenValue = tokenBuilder.ToString();
            if (tokenValue.StartsWith('\"') && tokenValue.EndsWith('\"'))
            {
                return null;
            }

            var match = _identifierRegex.Match(tokenValue);
            if (!match.Success)
            {
                return null;
            }

            var matchValue = match.Value;

            var matchedKeyword = LanguageConstants.Keywords.FirstOrDefault(k => k.Equals(matchValue));
            return matchedKeyword != null ? new Token(TokenType.Keyword, matchValue, typeof(string)) : new Token(TokenType.Identifier, matchValue, typeof(string));
        }

        private Token MatchNumericLiteral(StringBuilder tokenBuilder)
        {
            var tokenValue = tokenBuilder.ToString();
            var match = _numericRegex.Match(tokenValue);
            return match.Success ? new Token(TokenType.Literal, double.Parse(tokenValue), typeof(double)) : null;
        }

        private Token MatchStringLiteral(StringBuilder tokenBuilder)
        {
            var tokenValue = tokenBuilder.ToString();
            return tokenValue.StartsWith('\"') && tokenValue.EndsWith('\"')
                ? new Token(TokenType.Literal, tokenValue, typeof(string))
                : null;
        }

        private Token MatchOperator(StringBuilder tokenBuilder)
        {
            var tokenValue = tokenBuilder.ToString();
            var matchedOperator = LanguageConstants.Operators.FirstOrDefault(o => o.Equals(tokenValue));
            return matchedOperator != null ? new Token(TokenType.Operator, tokenValue, typeof(string)) : null;
        }

        private void AddTokenAndClearTokenBuilder(StringBuilder tokenBuilder, Token matchedToken)
        {
            _tokens.Add(matchedToken);
            _previousToken = matchedToken;
            tokenBuilder.Clear();
        }
    }
}
