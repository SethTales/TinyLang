using System;

namespace TinyLang.Models
{
    public class Token
    {
        public Token(TokenType tokenType, object tokenValue, Type tokenValueType)
        {
            TokenType = tokenType;
            TokenValue = tokenValue;
            TokenValueType = tokenValueType;
        }
        public TokenType TokenType { get; }
        public object TokenValue { get; }
        public Type TokenValueType { get; }

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
}
