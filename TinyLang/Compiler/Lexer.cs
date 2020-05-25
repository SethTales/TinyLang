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
        private const int EOF_MARKER = -1;

        private readonly IStreamReaderAdapterFactory _streamReaderAdapterFactory;
        private readonly TokenLocation _currentTokenLocation;

        private List<Token> _tokens;
        private int _columnIndex;

        public Lexer(IStreamReaderAdapterFactory streamReaderAdapterFactory)
        {
            _streamReaderAdapterFactory = streamReaderAdapterFactory;
            _currentTokenLocation = new TokenLocation
            {
                LineNumber = 1,
                TokenBeginIndex = 0,
                TokenEndIndex = 0
            };
        }

        public List<Token> Tokenize(string input)
        {
            var tokenBuilder = new StringBuilder();
            _tokens = new List<Token>();
            _columnIndex = 0;

            using (var streamReaderAdapter = _streamReaderAdapterFactory.BuildStreamReaderAdapater(input))
            {
                while (streamReaderAdapter.Peek() != EOF_MARKER)
                {
                    var currentChar = (char) streamReaderAdapter.Read();
                    if (IsCommentLine(currentChar))
                    {
                        streamReaderAdapter.DiscardCurrentLine();
                        ResetTokenLocationForNewLine(_currentTokenLocation);
                        tokenBuilder.Clear();
                        continue;
                    }
                    _columnIndex++;

                    //eat whitespace, newline or carriage return characters
                    while (char.IsWhiteSpace(currentChar) || currentChar == '\n' || currentChar == '\r')
                    {
                        if (currentChar == '\n')
                        {
                            ResetTokenLocationForNewLine(_currentTokenLocation);
                        }
                        currentChar = (char)streamReaderAdapter.Read();
                        if (IsCommentLine(currentChar))
                        {
                            streamReaderAdapter.DiscardCurrentLine();
                            ResetTokenLocationForNewLine(_currentTokenLocation);
                            tokenBuilder.Clear();
                            break;
                        }
                        _columnIndex++;
                    }
                    if (IsCommentLine(currentChar))
                    {
                        continue;
                    }

                    tokenBuilder.Append(currentChar);
                    _currentTokenLocation.TokenBeginIndex = _columnIndex;
                    var nextChar = (char)streamReaderAdapter.Peek();

                    //if current char is a separator add it as a token
                    if (LanguageConstants.Separators.Any(s => s.Equals(currentChar.ToString())))
                    {
                        _currentTokenLocation.TokenEndIndex = _columnIndex;
                        var token = new Token(TokenType.Separator, currentChar.ToString())
                        {
                            TokenLocation = new TokenLocation(_currentTokenLocation)
                        };
                        AddTokenAndClearTokenBuilder(tokenBuilder, token);
                        continue;
                    }

                    //iterate over characters in stream, reading them into the tokenBuilder buffer until we hit a separator or whitepsace
                    while (!LanguageConstants.Separators.Any(s => s.Equals(nextChar.ToString())) && !char.IsWhiteSpace(nextChar))
                    {
                        currentChar = (char)streamReaderAdapter.Read();
                        if (IsCommentLine(currentChar))
                        {
                            streamReaderAdapter.DiscardCurrentLine();
                            ResetTokenLocationForNewLine(_currentTokenLocation);
                            tokenBuilder.Clear();
                            break;
                        }
                        _columnIndex++;
                        nextChar = (char)streamReaderAdapter.Peek();
                        tokenBuilder.Append(currentChar);
                    }
                    if (IsCommentLine(currentChar))
                    {
                        continue;
                    }

                    //at this point we've reached the end of a sequence
                    //it's likely we have a token in the tokenBuilder buffer
                    //so we check the token buffer against token patterns
                    var matchedToken = CheckForMatchedToken(tokenBuilder);

                    if (matchedToken != null)
                    {
                        _currentTokenLocation.TokenEndIndex = _columnIndex;
                        matchedToken.TokenLocation = new TokenLocation(_currentTokenLocation);
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

        private  Token MatchIdentiferOrKeyword(StringBuilder tokenBuilder)
        {

            var tokenValue = tokenBuilder.ToString();
            if (tokenValue.StartsWith('\"') && tokenValue.EndsWith('\"'))
            {
                return null;
            }

            var identifierRegex = new Regex("([_a-zA-Z]+)");
            var match = identifierRegex.Match(tokenValue);
            if (!match.Success)
            {
                return null;
            }

            var matchedKeyword = LanguageConstants.Keywords.FirstOrDefault(k => k.Equals(tokenValue));
            return matchedKeyword != null
                ? new Token(TokenType.Keyword, tokenValue)
                : new Token(TokenType.Identifier, tokenValue);
        }

        private Token MatchNumericLiteral(StringBuilder tokenBuilder)
        {
            var numericRegex = new Regex("^[1-9]\\d*(\\.\\d+)?$|^[0]\\d*(\\.\\d+)?$");
            var tokenValue = tokenBuilder.ToString();
            var match = numericRegex.Match(tokenValue);
            return match.Success
                ? new Token(TokenType.Literal, tokenValue)
                : null;
        }

        private Token MatchStringLiteral(StringBuilder tokenBuilder)
        {
            var tokenValue = tokenBuilder.ToString();
            return tokenValue.StartsWith('\"') && tokenValue.EndsWith('\"')
                ? new Token(TokenType.Literal, tokenValue)
                : null;
        }

        private Token MatchOperator(StringBuilder tokenBuilder)
        {
            var tokenValue = tokenBuilder.ToString();
            var matchedOperator = LanguageConstants.Operators.FirstOrDefault(o => o.Equals(tokenValue));
            return matchedOperator != null ? new Token(TokenType.Operator, tokenValue) : null;
        }

        private void AddTokenAndClearTokenBuilder(StringBuilder tokenBuilder, Token matchedToken)
        {
            _tokens.Add(matchedToken);
            tokenBuilder.Clear();
        }

        private void ResetTokenLocationForNewLine(TokenLocation currentTokenLocation)
        {
            _currentTokenLocation.LineNumber++;
            _columnIndex = 0;
            _currentTokenLocation.TokenBeginIndex = 0;
            _currentTokenLocation.TokenEndIndex = 0;
        }

        private bool IsCommentLine(char currentChar)
        {
            return currentChar.Equals(LanguageConstants.CommentIdentifier);
        }
    }
}
