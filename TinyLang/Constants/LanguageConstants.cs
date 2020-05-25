using System;
using System.Collections.Generic;
using System.Text;

namespace TinyLang.Constants
{
    public static class LanguageConstants
    {
        public static readonly List<string> Keywords = new List<string>
        {
            "if",
            "else",
            "elseif",
            "func",
            "return",
            "true",
            "false"
        };

        public static readonly List<string> Operators = new List<string>
        {
            "+",
            "-",
            "*",
            "/",
            "=",
            "==",
            "!",
            "!=",
            "||",
            "&&",
            ">",
            ">=",
            "<",
            "<="
        };

        public static readonly List<string> Separators = new List<string>
        {
            "(",
            ")",
            "{",
            "}",
            ";",
            ","
        };

        public const char CommentIdentifier = '#';
    }
}
