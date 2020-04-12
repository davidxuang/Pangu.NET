using System.Text.RegularExpressions;

namespace Pangu
{
    public static class Pangu
    {
        private const string _CJK = "\u2e80-\u2eff\u2f00-\u2fdf\u3040-\u309f\u30a0-\u30fa\u30fc-\u30ff\u3100-\u312f\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff";

        private static readonly Regex _CJK_ToFullwidth_CJK = new Regex(
            $"([{_CJK}])[ ]*([\\:]+|\\.)[ ]*([{_CJK}])"
        );
        private static readonly MatchEvaluator _CJK_ToFullwidth_CJK_Evaluator = new MatchEvaluator(
            match => match.Captures[0].Value
                   + match.Captures[1].Value.Replace(':', '：').Replace('.', '。')
                   + match.Captures[2].Value
            );
        private static readonly Regex _CJK_ToFullwidth = new Regex(
            $"([{_CJK}])[ ]*([~\\!;,\\?]+)[ ]*"
        );
        private static readonly MatchEvaluator _CJK_ToFullwidth_Evaluator = new MatchEvaluator(
            match => match.Captures[0].Value
                   + match.Captures[1].Value.Replace('~', '～').Replace('!', '！').Replace(';', '；').Replace(',', '，').Replace('?', '？')
            );

        private static readonly Regex _Dots_CJK = new Regex(
            $"([{_CJK}])(#([^ ]))"
        );
        private static readonly Regex _CJK_Colon_ANs = new Regex(
            $"([{_CJK}])\\:([A-Z0-9\\(\\)])"
        );

        private static readonly Regex _AnyCJK = new Regex(
            $"[{_CJK}]"
        );

        private static readonly Regex _CJK_Quote = new Regex(
            $"([{_CJK}])([`\"\u05f4])"
        );
        private static readonly Regex _Quote_CJK = new Regex(
            "([`\"\u05f4])([{CJK}])"
        );
        private static readonly Regex _Quote_Any_Quote = new Regex(
            "([`\"\u05f4]+)(\\s*)(.+?)(\\s*)([`\"\u05f4]+)"
        );

        private static readonly Regex _CJK_PossessiveSingleQuote = new Regex(
            $"([{_CJK}])('[^s])"
        );
        private static readonly Regex _SingleQuote_CJK = new Regex(
            $"(')([{_CJK}])"
        );
        private static readonly Regex _PossessiveSingleQuote = new Regex(
            $"([{_CJK}A-Za-z0-9])( )('s)"
        );

        private static readonly Regex _Hash_ANs_CJK_HASH = new Regex(
            $"([{_CJK}])(#)([{_CJK}]+)(#)([{_CJK}])"
        );
        private static readonly Regex _CJK_Hash = new Regex(
            $"([{_CJK}])(#([^ ]))"
        );
        private static readonly Regex _Hash_CJK = new Regex(
            $"(#([^ ]))([{_CJK}])"
        );

        private static readonly Regex _CJK_Operator_ANs = new Regex(
            $"([{_CJK}])([\\+\\-\\*\\/=&\\|<>])([A-Za-z0-9])"
        );
        private static readonly Regex _ANs_Operator_CJK = new Regex(
            $"([A-Za-z0-9])([\\+\\-\\*\\/=&\\|<>])([{_CJK}])"
        );

        private static readonly Regex _Slash_As = new Regex(
            "([/]) ([a-z\\-_\\./]+)"
        );
        private static readonly Regex _Slash_As_Slash = new Regex(
            "([/\\.])([A-Za-z\\-_\\./]+) ([/])"
        );

        private static readonly Regex _CJK_LeftBracket = new Regex(
            $"([{_CJK}])([\\(\\[\\{{<>\u201c])"
        );
        private static readonly Regex _RightBracket_CJK = new Regex(
            $"([\\)\\]\\}}<>\u201d])([{_CJK}])"
        );
        private static readonly Regex _LeftBracket_Any_RightBracket = new Regex(
            $"([\\(\\[\\{{<\u201c]+)(\\s*)(.+?)(\\s*)([\\)\\]\\}}>\u201d]+)"
        );
        private static readonly Regex _ANsCJK_LeftBracket_Any_RightBracket = new Regex(
            $"([A-Za-z0-9{_CJK}])[ ]*([\u201c])([A-Za-z0-9{_CJK}\\-_ ]+)([\u201d])"
        );
        private static readonly Regex _LeftBracket_Any_RightBracket_ANsCJK = new Regex(
            $"([\u201c])([A-Za-z0-9{_CJK}\\-_ ]+)([\u201d])[ ]*([A-Za-z0-9{_CJK}])"
        );

        private static readonly Regex _AN_LeftBracket = new Regex(
            "([A-Za-z0-9])([\\(\\[\\{])"
        );
        private static readonly Regex _RightBracket_AN = new Regex(
            "([\\)\\]\\}])([A-Za-z0-9])"
        );

        private static readonly Regex _CJK_ANs = new Regex(
            $"([{_CJK}])([A-Za-z\u0370-\u03ff0-9@\\$%\\^&\\*\\-\\+\\\\=\\|/\u00a1-\u00ff\u2150-\u218f\u2700—\u27bf])"
        );
        private static readonly Regex _ANs_CJK = new Regex(
            $"([A-Za-z\u0370-\u03ff0-9~\\!\\$%\\^&\\*\\-\\+\\\\=\\|;:,\\./\\?\u00a1-\u00ff\u2150-\u218f\u2700—\u27bf])([{_CJK}])"
        );

        private static readonly Regex _Persent_A = new Regex(
            "(%)([A-Za-z])"
        );

        private static readonly Regex _MiddleDot = new Regex(
            "([ ]*)([\u00b7\u2022\u2027])([ ]*)"
        );

        public static void SpacingText(ref string text)
        {
            if (text.Length <= 1 || _AnyCJK.IsMatch(text)) return;

            text = _CJK_ToFullwidth_CJK.Replace(text, _CJK_ToFullwidth_CJK_Evaluator);
            text = _CJK_ToFullwidth.Replace(text, _CJK_ToFullwidth_Evaluator);

            text = _Dots_CJK.Replace(text, "$1 $2");
            text = _CJK_Colon_ANs.Replace(text, "$1：$2");

            text = _CJK_Quote.Replace(text, "$1 $2");
            text = _Quote_CJK.Replace(text, "$1 $2");
            text = _Quote_Any_Quote.Replace(text, "$1$3$5");

            text = _CJK_PossessiveSingleQuote.Replace(text, "$1 $2");
            text = _SingleQuote_CJK.Replace(text, "$1 $2");
            text = _PossessiveSingleQuote.Replace(text, "$1's");

            text = _Hash_ANs_CJK_HASH.Replace(text, "$1 $2$3$4 $5");
            text = _CJK_Hash.Replace(text, "$1 $2");
            text = _Hash_CJK.Replace(text, "$1 $3");

            text = _CJK_Operator_ANs.Replace(text, "$1 $2 $3");
            text = _ANs_Operator_CJK.Replace(text, "$1 $2 $3");

            text = _Slash_As.Replace(text, "$1$2");
            text = _Slash_As_Slash.Replace(text, "$1$2$3");

            text = _CJK_LeftBracket.Replace(text, "$1 $2");
            text = _RightBracket_CJK.Replace(text, "$1 $2");
            text = _LeftBracket_Any_RightBracket.Replace(text, "$1$3$5");
            text = _ANsCJK_LeftBracket_Any_RightBracket.Replace(text, "$1 $2$3$4");
            text = _LeftBracket_Any_RightBracket_ANsCJK.Replace(text, "$1$2$3 $4");

            text = _AN_LeftBracket.Replace(text, "$1 $2");
            text = _RightBracket_AN.Replace(text, "$1 $2");

            text = _CJK_ANs.Replace(text, "$1 $2");
            text = _ANs_CJK.Replace(text, "$1 $2");

            text = _Persent_A.Replace(text, "$1 $2");
            text = _MiddleDot.Replace(text, "・");
        }

        public static string SpacingText(string text)
        {
            string output = text;
            SpacingText(output);
            return output;
        }
    }
}

namespace Pangu.Extensions
{
    public static class Extensions
    {
        public static string SpacingText(this string text)
        {
            string output = text;
            Pangu.SpacingText(output);
            return output;
        }
    }
}
