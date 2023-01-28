//////////////////////////////////////////////////////////////////////////////
// This source code and all associated files and resources are copyrighted by
// the author(s). This source code and all associated files and resources may
// be used as long as they are used according to the terms and conditions set
// forth in The Code Project Open License (CPOL), which may be viewed at
// http://www.blackbeltcoder.com/Legal/Licenses/CPOL.
//
// This code was original published on Black Belt Coder
// (http://www.blackbeltcoder.com).
//
// Copyright (c) 2011 Jonathan Wood
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SwissEphNet
{
    /// <summary>
    /// Class that provides functionality of the standard C library sscanf()
    /// function.
    /// </summary>
    /// <remarks>
    /// Based on http://www.blackbeltcoder.com/Articles/strings/a-sscanf-replacement-for-net.
    /// </remarks>
    partial class C
    {
        /// <summary>
        /// Parser
        /// </summary>
        /// <remarks>
        /// All unused methods from the original code are commented for tests
        /// </remarks>
        class TextParser
        {
            private string _text;
            private int _pos;

            //public string Text { get { return _text; } }
            public int Position { get { return _pos; } }
            //public int Remaining { get { return _text.Length - _pos; } }
            public static char NullChar = (char)0;

            //public TextParser() {
            //    Reset(null);
            //}

            public TextParser(string text) {
                Reset(text);
            }

            ///// <summary>
            ///// Resets the current position to the start of the current document
            ///// </summary>
            //public void Reset() {
            //    _pos = 0;
            //}

            /// <summary>
            /// Sets the current document and resets the current position to the start of it
            /// </summary>
            /// <param name="html"></param>
            public void Reset(string text) {
                _text = (text != null) ? text : String.Empty;
                _pos = 0;
            }

            /// <summary>
            /// Indicates if the current position is at the end of the current document
            /// </summary>
            public bool EndOfText {
                get { return (_pos >= _text.Length); }
            }

            /// <summary>
            /// Returns the character at the current position, or a null character if we're
            /// at the end of the document
            /// </summary>
            /// <returns>The character at the current position</returns>
            public char Peek() {
                return Peek(0);
            }

            /// <summary>
            /// Returns the character at the specified number of characters beyond the current
            /// position, or a null character if the specified position is at the end of the
            /// document
            /// </summary>
            /// <param name="ahead">The number of characters beyond the current position</param>
            /// <returns>The character at the specified position</returns>
            public char Peek(int ahead) {
                int pos = (_pos + ahead);
                if (pos < _text.Length)
                    return _text[pos];
                return NullChar;
            }

            ///// <summary>
            ///// Extracts a substring from the specified position to the end of the text
            ///// </summary>
            ///// <param name="start"></param>
            ///// <returns></returns>
            //public string Extract(int start) {
            //    return Extract(start, _text.Length);
            //}

            /// <summary>
            /// Extracts a substring from the specified range of the current text
            /// </summary>
            /// <param name="start"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            public string Extract(int start, int end) {
                return _text.Substring(start, end - start);
            }

            /// <summary>
            /// Moves the current position ahead one character
            /// </summary>
            public void MoveAhead() {
                MoveAhead(1);
            }

            /// <summary>
            /// Moves the current position ahead the specified number of characters
            /// </summary>
            /// <param name="ahead">The number of characters to move ahead</param>
            public void MoveAhead(int ahead) {
                _pos = Math.Min(_pos + ahead, _text.Length);
            }

            ///// <summary>
            ///// Moves to the next occurrence of the specified string
            ///// </summary>
            ///// <param name="s">String to find</param>
            ///// <param name="ignoreCase">Indicates if case-insensitive comparisons are used</param>
            //public void MoveTo(string s, bool ignoreCase = false) {
            //    _pos = _text.IndexOf(s, _pos, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            //    if (_pos < 0)
            //        _pos = _text.Length;
            //}

            /// <summary>
            /// Moves to the next occurrence of the specified character
            /// </summary>
            /// <param name="c">Character to find</param>
            public void MoveTo(char c) {
                _pos = _text.IndexOf(c, _pos);
                if (_pos < 0)
                    _pos = _text.Length;
            }

            ///// <summary>
            ///// Moves to the next occurrence of any one of the specified
            ///// characters
            ///// </summary>
            ///// <param name="chars">Array of characters to find</param>
            //public void MoveTo(char[] chars) {
            //    _pos = _text.IndexOfAny(chars, _pos);
            //    if (_pos < 0)
            //        _pos = _text.Length;
            //}

            ///// <summary>
            ///// Moves to the next occurrence of any character that is not one
            ///// of the specified characters
            ///// </summary>
            ///// <param name="chars">Array of characters to move past</param>
            //public void MovePast(char[] chars) {
            //    while (IsInArray(Peek(), chars))
            //        MoveAhead();
            //}

            ///// <summary>
            ///// Determines if the specified character exists in the specified
            ///// character array.
            ///// </summary>
            ///// <param name="c">Character to find</param>
            ///// <param name="chars">Character array to search</param>
            ///// <returns></returns>
            //protected bool IsInArray(char c, char[] chars) {
            //    foreach (char ch in chars) {
            //        if (c == ch)
            //            return true;
            //    }
            //    return false;
            //}

            ///// <summary>
            ///// Moves the current position to the first character that is part of a newline
            ///// </summary>
            //public void MoveToEndOfLine() {
            //    char c = Peek();
            //    while (c != '\r' && c != '\n' && !EndOfText) {
            //        MoveAhead();
            //        c = Peek();
            //    }
            //}

            /// <summary>
            /// Moves the current position to the next character that is not whitespace
            /// </summary>
            public void MovePastWhitespace() {
                while (Char.IsWhiteSpace(Peek()))
                    MoveAhead();
            }
        }

        /// <summary>
        /// Class that provides functionality of the standard C library sscanf()
        /// function.
        /// </summary>
        class ScanFormatted
        {
            // Format type specifiers
            protected enum Types
            {
                Character,
                Decimal,
                Float,
                Hexadecimal,
                Octal,
                ScanSet,
                String,
                Unsigned
            }

            // Format modifiers
            protected enum Modifiers
            {
                None,
                ShortShort,
                Short,
                Long,
                LongLong
            }

            // Delegate to parse a type
            protected delegate bool ParseValue(TextParser input, FormatSpecifier spec);

            // Class to associate format type with type parser
            protected class TypeParser
            {
                public Types Type { get; set; }
                public ParseValue Parser { get; set; }
            }

            // Class to hold format specifier information
            protected class FormatSpecifier
            {
                public Types Type { get; set; }
                public Modifiers Modifier { get; set; }
                public int Width { get; set; }
                public bool NoResult { get; set; }
                public string ScanSet { get; set; }
                public bool ScanSetExclude { get; set; }
            }

            // Lookup table to find parser by parser type
            protected TypeParser[] _typeParsers;

            // Holds results after calling Parse()
            public List<object> Results;

            // Constructor
            public ScanFormatted() {
                // Populate parser type lookup table
                _typeParsers = new TypeParser[] {
				new TypeParser() { Type = Types.Character, Parser = ParseCharacter },
				new TypeParser() { Type = Types.Decimal, Parser = ParseDecimal },
				new TypeParser() { Type = Types.Float, Parser = ParseFloat },
				new TypeParser() { Type = Types.Hexadecimal, Parser = ParseHexadecimal },
				new TypeParser() { Type = Types.Octal, Parser = ParseOctal },
				new TypeParser() { Type = Types.ScanSet, Parser = ParseScanSet },
				new TypeParser() { Type = Types.String, Parser = ParseString },
				new TypeParser() { Type = Types.Unsigned, Parser = ParseDecimal }
			};
                // Allocate results collection
                Results = new List<object>();
            }

            /// <summary>
            /// Parses the input string according to the rules in the
            /// format string. Similar to the standard C library's
            /// sscanf() function. Parsed fields are placed in the
            /// class' Results member.
            /// </summary>
            /// <param name="input">String to parse</param>
            /// <param name="format">Specifies rules for parsing input</param>
            public int Parse(string input, string format) {
                TextParser inp = new TextParser(input);
                TextParser fmt = new TextParser(format);
                List<object> results = new List<object>();
                FormatSpecifier spec = new FormatSpecifier();
                int count = 0;

                // Clear any previous results
                Results.Clear();

                // Process input string as indicated in format string
                while (!fmt.EndOfText && !inp.EndOfText) {
                    if (ParseFormatSpecifier(fmt, spec)) {
                        // Found a format specifier
                        TypeParser parser = _typeParsers.First(tp => tp.Type == spec.Type);
                        if (parser.Parser(inp, spec))
                            count++;
                        else
                            break;
                    } else if (Char.IsWhiteSpace(fmt.Peek())) {
                        // Whitespace
                        inp.MovePastWhitespace();
                        fmt.MoveAhead();
                    } else if (fmt.Peek() == inp.Peek()) {
                        // Matching character
                        inp.MoveAhead();
                        fmt.MoveAhead();
                    } else break;	// Break at mismatch
                }

                // Return number of fields successfully parsed
                return count;
            }

            /// <summary>
            /// Attempts to parse a field format specifier from the format string.
            /// </summary>
            protected bool ParseFormatSpecifier(TextParser format, FormatSpecifier spec) {
                // Return if not a field format specifier
                if (format.Peek() != '%')
                    return false;
                format.MoveAhead();

                // Return if "%%" (treat as '%' literal)
                if (format.Peek() == '%')
                    return false;

                // Test for asterisk, which indicates result is not stored
                if (format.Peek() == '*') {
                    spec.NoResult = true;
                    format.MoveAhead();
                } else spec.NoResult = false;

                // Parse width
                int start = format.Position;
                while (Char.IsDigit(format.Peek()))
                    format.MoveAhead();
                if (format.Position > start)
                    spec.Width = int.Parse(format.Extract(start, format.Position));
                else
                    spec.Width = 0;

                // Parse modifier
                if (format.Peek() == 'h') {
                    format.MoveAhead();
                    if (format.Peek() == 'h') {
                        format.MoveAhead();
                        spec.Modifier = Modifiers.ShortShort;
                    } else spec.Modifier = Modifiers.Short;
                } else if (Char.ToLower(format.Peek()) == 'l') {
                    format.MoveAhead();
                    if (format.Peek() == 'l') {
                        format.MoveAhead();
                        spec.Modifier = Modifiers.LongLong;
                    } else spec.Modifier = Modifiers.Long;
                } else spec.Modifier = Modifiers.None;

                // Parse type
                switch (format.Peek()) {
                    case 'c':
                        spec.Type = Types.Character;
                        break;
                    case 'd':
                    case 'i':
                        spec.Type = Types.Decimal;
                        break;
                    case 'a':
                    case 'A':
                    case 'e':
                    case 'E':
                    case 'f':
                    case 'F':
                    case 'g':
                    case 'G':
                        spec.Type = Types.Float;
                        break;
                    case 'o':
                        spec.Type = Types.Octal;
                        break;
                    case 's':
                        spec.Type = Types.String;
                        break;
                    case 'u':
                        spec.Type = Types.Unsigned;
                        break;
                    case 'x':
                    case 'X':
                        spec.Type = Types.Hexadecimal;
                        break;
                    case '[':
                        spec.Type = Types.ScanSet;
                        format.MoveAhead();
                        // Parse scan set characters
                        if (format.Peek() == '^') {
                            spec.ScanSetExclude = true;
                            format.MoveAhead();
                        } else spec.ScanSetExclude = false;
                        start = format.Position;
                        // Treat immediate ']' as literal
                        if (format.Peek() == ']')
                            format.MoveAhead();
                        format.MoveTo(']');
                        if (format.EndOfText)
                            throw new Exception("Type specifier expected character : ']'");
                        spec.ScanSet = format.Extract(start, format.Position);
                        break;
                    default:
                        string msg = String.Format("Unknown format type specified : '{0}'", format.Peek());
                        throw new Exception(msg);
                }
                format.MoveAhead();
                return true;
            }

            /// <summary>
            /// Parse a character field
            /// </summary>
            private bool ParseCharacter(TextParser input, FormatSpecifier spec) {
                // Parse character(s)
                int start = input.Position;
                int count = (spec.Width > 1) ? spec.Width : 1;
                while (!input.EndOfText && count-- > 0)
                    input.MoveAhead();

                // Extract token
                if (count <= 0 && input.Position > start) {
                    if (!spec.NoResult) {
                        string token = input.Extract(start, input.Position);
                        if (token.Length > 1)
                            Results.Add(token.ToCharArray());
                        else
                            Results.Add(token[0]);
                    }
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Parse integer field
            /// </summary>
            private bool ParseDecimal(TextParser input, FormatSpecifier spec) {
                int radix = 10;

                // Skip any whitespace
                input.MovePastWhitespace();

                // Parse leading sign
                int start = input.Position;
                if (input.Peek() == '+' || input.Peek() == '-') {
                    input.MoveAhead();
                // Disable Octal et Hexadecimal format
                //} else if (input.Peek() == '0') {
                //    if (Char.ToLower(input.Peek(1)) == 'x') {
                //        radix = 16;
                //        input.MoveAhead(2);
                //    } else {
                //        radix = 8;
                //        input.MoveAhead();
                //    }
                }

                // Parse digits
                while (IsValidDigit(input.Peek(), radix))
                    input.MoveAhead();

                // Don't exceed field width
                if (spec.Width > 0) {
                    int count = input.Position - start;
                    if (spec.Width < count)
                        input.MoveAhead(spec.Width - count);
                }

                // Extract token
                if (input.Position > start) {
                    if (!spec.NoResult) {
                        if (spec.Type == Types.Decimal)
                            AddSigned(input.Extract(start, input.Position), spec.Modifier, radix);
                        else
                            AddUnsigned(input.Extract(start, input.Position), spec.Modifier, radix);
                    }
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Parse a floating-point field
            /// </summary>
            private bool ParseFloat(TextParser input, FormatSpecifier spec) {
                // Skip any whitespace
                input.MovePastWhitespace();

                // Parse leading sign
                int start = input.Position;
                if (input.Peek() == '+' || input.Peek() == '-')
                    input.MoveAhead();

                // Parse digits
                bool hasPoint = false;
                while (Char.IsDigit(input.Peek()) || input.Peek() == '.') {
                    if (input.Peek() == '.') {
                        if (hasPoint)
                            break;
                        hasPoint = true;
                    }
                    input.MoveAhead();
                }

                // Parse exponential notation
                if (Char.ToLower(input.Peek()) == 'e') {
                    input.MoveAhead();
                    if (input.Peek() == '+' || input.Peek() == '-')
                        input.MoveAhead();
                    while (Char.IsDigit(input.Peek()))
                        input.MoveAhead();
                }

                // Don't exceed field width
                if (spec.Width > 0) {
                    int count = input.Position - start;
                    if (spec.Width < count)
                        input.MoveAhead(spec.Width - count);
                }

                // Because we parse the exponential notation before we apply
                // any field-width constraint, it becomes awkward to verify
                // we have a valid floating point token. To prevent an
                // exception, we use TryParse() here instead of Parse().
                double result;

                // Extract token
                if (input.Position > start &&
                    double.TryParse(input.Extract(start, input.Position), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {
                    if (!spec.NoResult) {
                        if (spec.Modifier == Modifiers.Long ||
                            spec.Modifier == Modifiers.LongLong)
                            Results.Add(result);
                        else
                            Results.Add((float)result);
                    }
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Parse hexadecimal field
            /// </summary>
            protected bool ParseHexadecimal(TextParser input, FormatSpecifier spec) {
                // Skip any whitespace
                input.MovePastWhitespace();

                // Parse 0x prefix
                int start = input.Position;
                if (input.Peek() == '0' && input.Peek(1) == 'x')
                    input.MoveAhead(2);

                // Parse digits
                while (IsValidDigit(input.Peek(), 16))
                    input.MoveAhead();

                // Don't exceed field width
                if (spec.Width > 0) {
                    int count = input.Position - start;
                    if (spec.Width < count)
                        input.MoveAhead(spec.Width - count);
                }

                // Extract token
                if (input.Position > start) {
                    if (!spec.NoResult)
                        AddUnsigned(input.Extract(start, input.Position), spec.Modifier, 16);
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Parse an octal field
            /// </summary>
            private bool ParseOctal(TextParser input, FormatSpecifier spec) {
                // Skip any whitespace
                input.MovePastWhitespace();

                // Parse digits
                int start = input.Position;
                while (IsValidDigit(input.Peek(), 8))
                    input.MoveAhead();

                // Don't exceed field width
                if (spec.Width > 0) {
                    int count = input.Position - start;
                    if (spec.Width < count)
                        input.MoveAhead(spec.Width - count);
                }

                // Extract token
                if (input.Position > start) {
                    if (!spec.NoResult)
                        AddUnsigned(input.Extract(start, input.Position), spec.Modifier, 8);
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Parse a scan-set field
            /// </summary>
            protected bool ParseScanSet(TextParser input, FormatSpecifier spec) {
                // Parse characters
                int start = input.Position;
                if (!spec.ScanSetExclude) {
                    while (spec.ScanSet.Contains(input.Peek()))
                        input.MoveAhead();
                } else {
                    while (!input.EndOfText && !spec.ScanSet.Contains(input.Peek()))
                        input.MoveAhead();
                }

                // Don't exceed field width
                if (spec.Width > 0) {
                    int count = input.Position - start;
                    if (spec.Width < count)
                        input.MoveAhead(spec.Width - count);
                }

                // Extract token
                if (input.Position > start) {
                    if (!spec.NoResult) {
                        Results.Add(input.Extract(start, input.Position));
                        input.MoveAhead();
                    }
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Parse a string field
            /// </summary>
            private bool ParseString(TextParser input, FormatSpecifier spec) {
                // Skip any whitespace
                input.MovePastWhitespace();

                // Parse string characters
                int start = input.Position;
                while (!input.EndOfText && !Char.IsWhiteSpace(input.Peek()))
                    input.MoveAhead();

                // Don't exceed field width
                if (spec.Width > 0) {
                    int count = input.Position - start;
                    if (spec.Width < count)
                        input.MoveAhead(spec.Width - count);
                }

                // Extract token
                if (input.Position > start) {
                    if (!spec.NoResult)
                        Results.Add(input.Extract(start, input.Position));
                    return true;
                }
                return false;
            }

            // Determines if the given digit is valid for the given radix
            private bool IsValidDigit(char c, int radix) {
                int i = "0123456789abcdef".IndexOf(Char.ToLower(c));
                if (i >= 0 && i < radix)
                    return true;
                return false;
            }

            // Parse signed token and add to results
            private void AddSigned(string token, Modifiers mod, int radix) {
                object obj;
                if (mod == Modifiers.ShortShort)
                    obj = Convert.ToSByte(token, radix);
                else if (mod == Modifiers.Short)
                    obj = Convert.ToInt16(token, radix);
                else if (mod == Modifiers.Long ||
                    mod == Modifiers.LongLong)
                    obj = Convert.ToInt64(token, radix);
                else
                    obj = Convert.ToInt32(token, radix);
                Results.Add(obj);
            }

            // Parse unsigned token and add to results
            private void AddUnsigned(string token, Modifiers mod, int radix) {
                object obj;
                if (mod == Modifiers.ShortShort)
                    obj = Convert.ToByte(token, radix);
                else if (mod == Modifiers.Short)
                    obj = Convert.ToUInt16(token, radix);
                else if (mod == Modifiers.Long ||
                    mod == Modifiers.LongLong)
                    obj = Convert.ToUInt64(token, radix);
                else
                    obj = Convert.ToUInt32(token, radix);
                Results.Add(obj);
            }
        }

        /// <summary>
        /// sscanf with 1 result
        /// </summary>
        public static int sscanf<T>(String input, String format, ref T r) {
            var parser = new ScanFormatted();
            parser.Parse(input, format);
            var res = parser.Results.Count;
            if (res > 0)
                r = (T)parser.Results[0];
            return res;
        }

        /// <summary>
        /// sscanf with 2 results
        /// </summary>
        public static int sscanf<T1, T2>(String input, String format, ref T1 r1, ref T2 r2) {
            var parser = new ScanFormatted();
            parser.Parse(input, format);
            var res = parser.Results.Count;
            if (res > 0)
                r1 = (T1)parser.Results[0];
            if (res > 1)
                r2 = (T2)parser.Results[1];
            return res;
        }

        /// <summary>
        /// sscanf with 3 results
        /// </summary>
        public static int sscanf<T1, T2, T3>(String input, String format, ref T1 r1, ref T2 r2, ref T3 r3) {
            var parser = new ScanFormatted();
            parser.Parse(input, format);
            var res = parser.Results.Count;
            if (res > 0)
                r1 = (T1)parser.Results[0];
            if (res > 1)
                r2 = (T2)parser.Results[1];
            if (res > 2)
                r3 = (T3)parser.Results[2];
            return res;
        }

        /// <summary>
        /// sscanf with 4 results
        /// </summary>
        public static int sscanf<T1, T2, T3, T4>(String input, String format, ref T1 r1, ref T2 r2, ref T3 r3, ref T4 r4) {
            var parser = new ScanFormatted();
            parser.Parse(input, format);
            var res = parser.Results.Count;
            if (res > 0)
                r1 = (T1)parser.Results[0];
            if (res > 1)
                r2 = (T2)parser.Results[1];
            if (res > 2)
                r3 = (T3)parser.Results[2];
            if (res > 3)
                r4 = (T4)parser.Results[3];
            return res;
        }

        /// <summary>
        /// sscanf with 5 results
        /// </summary>
        public static int sscanf<T1, T2, T3, T4, T5>(String input, String format, ref T1 r1, ref T2 r2, ref T3 r3, ref T4 r4, ref T5 r5) {
            var parser = new ScanFormatted();
            parser.Parse(input, format);
            var res = parser.Results.Count;
            if (res > 0)
                r1 = (T1)parser.Results[0];
            if (res > 1)
                r2 = (T2)parser.Results[1];
            if (res > 2)
                r3 = (T3)parser.Results[2];
            if (res > 3)
                r4 = (T4)parser.Results[3];
            if (res > 4)
                r5 = (T5)parser.Results[4];
            return res;
        }

        /// <summary>
        /// sscanf with 6 results
        /// </summary>
        public static int sscanf<T1, T2, T3, T4, T5, T6>(String input, String format, ref T1 r1, ref T2 r2, ref T3 r3, ref T4 r4, ref T5 r5, ref T6 r6) {
            var parser = new ScanFormatted();
            parser.Parse(input, format);
            var res = parser.Results.Count;
            if (res > 0)
                r1 = (T1)parser.Results[0];
            if (res > 1)
                r2 = (T2)parser.Results[1];
            if (res > 2)
                r3 = (T3)parser.Results[2];
            if (res > 3)
                r4 = (T4)parser.Results[3];
            if (res > 4)
                r5 = (T5)parser.Results[4];
            if (res > 5)
                r6 = (T6)parser.Results[5];
            return res;
        }

    }
}
