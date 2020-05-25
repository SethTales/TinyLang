using System;

namespace TinyLang.Models
{
    public class Token
    {
        public Token(TokenType tokenType, string tokenValue)
        {
            TokenType = tokenType;
            TokenValue = tokenValue;
        }
        public TokenType TokenType { get; }
        public string TokenValue { get; }
        public TokenLocation TokenLocation { get; set; }

    }

    public enum TokenType
    {
        Identifier,
        Operator,
        Keyword,
        Separator,
        Literal,
        Comment
    }
    
    public class TokenLocation
    {
        public TokenLocation()
        {

        }
        public TokenLocation(TokenLocation tokenLocation)
        {
            LineNumber = tokenLocation.LineNumber;
            TokenBeginIndex = tokenLocation.TokenBeginIndex;
            TokenEndIndex = tokenLocation.TokenEndIndex;
        }
        public int LineNumber { get; set; }
        public int TokenBeginIndex { get; set; }
        public int TokenEndIndex { get; set; }
    }
}
