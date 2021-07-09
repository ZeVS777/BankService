using System;
using System.Collections.Generic;
using System.Text;

namespace SovComBankTest.Utils
{
    public static class CharExtension
    {
        public static bool IsCyrillicChar(this char ch) => ch switch
        {
            'Ё' or 'ё' => true,
            >= 'А' and <= 'я' => true,
            _ => false
        };
    }

    public sealed class Transliteration
    {
        public static string CyrillicToLatin(ReadOnlySpan<char> message)
        {
            var encoding = Encoding.GetEncoding("us-ascii", new CyrillicToLatinFallback(),
                DecoderFallback.ExceptionFallback);

            var encoder = encoding.GetEncoder();
            var byteCount = encoder.GetByteCount(message, false);
            var bytes = new byte[byteCount];
            encoder.GetBytes(message, bytes, false);

            var decoder = encoding.GetDecoder();
            var charCount = decoder.GetCharCount(bytes, 0, byteCount);
            var chars = new char[charCount];
            decoder.GetChars(bytes, 0, byteCount, chars, 0);

            return new string(chars);
        }
    }

    internal class CyrillicToLatinFallback : EncoderFallback
    {
        private readonly Dictionary<char, string> _table;

        public CyrillicToLatinFallback()
        {
            #region charTable

            _table = new Dictionary<char, string>
            {
                {'А', "A"},
                {'Б', "B"},
                {'В', "V"},
                {'Г', "G"},
                {'Д', "D"},
                {'Е', "E"},
                {'Ё', "IO"},
                {'Ж', "Zh"},
                {'З', "Z"},
                {'И', "I"},
                {'Й', "I"},
                {'К', "K"},
                {'Л', "L"},
                {'М', "M"},
                {'Н', "N"},
                {'О', "O"},
                {'П', "P"},
                {'Р', "R"},
                {'С', "S"},
                {'Т', "T"},
                {'У', "U"},
                {'Ф', "F"},
                {'Х', "Kh"},
                {'Ц', "Ts"},
                {'Ч', "Ch"},
                {'Ш', "Sh"},
                {'Щ', "Shch"},
                {'Ь', "'"},
                {'Ы', "Ye"},
                {'Ъ', "'"},
                {'Э', "E"},
                {'Ю', "Iu"},
                {'Я', "Ia"},
                {'а', "a"},
                {'б', "b"},
                {'в', "v"},
                {'г', "g"},
                {'д', "d"},
                {'е', "e"},
                {'ё', "io"},
                {'ж', "zh"},
                {'з', "z"},
                {'и', "i"},
                {'й', "i"},
                {'к', "k"},
                {'л', "l"},
                {'м', "m"},
                {'н', "n"},
                {'о', "o"},
                {'п', "p"},
                {'р', "r"},
                {'с', "s"},
                {'т', "t"},
                {'у', "u"},
                {'ф', "f"},
                {'х', "kh"},
                {'ц', "ts"},
                {'ч', "ch"},
                {'ш', "sh"},
                {'щ', "shch"},
                {'ь', "'"},
                {'ы', "yi"},
                {'ъ', "'"},
                {'э', "e"},
                {'ю', "iu"},
                {'я', "ia"}
            };

            #endregion
        }

        public override EncoderFallbackBuffer CreateFallbackBuffer() => new CyrillicToLatinFallbackBuffer(_table);

        public override int MaxCharCount => 4; // Maximum is "Shch" and "shch"
    }

    internal class CyrillicToLatinFallbackBuffer : EncoderFallbackBuffer
    {
        private readonly Dictionary<char, string> _table;
        private int _bufferIndex;
        private string _buffer;
        private int _leftToReturn;

        internal CyrillicToLatinFallbackBuffer(Dictionary<char, string> table) =>
            (_table, _bufferIndex, _leftToReturn) = (table, -1, -1);

        public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index) => false;

        public override bool Fallback(char charUnknown, int index)
        {
            if (!charUnknown.IsCyrillicChar()) return false;

            _buffer = _table[charUnknown];
            _leftToReturn = _buffer.Length - 1;
            _bufferIndex = -1;
            return true;
        }

        public override char GetNextChar()
        {
            if (_leftToReturn < 0)
                return '\u0000';

            _leftToReturn--;
            _bufferIndex++;
            return _buffer[_bufferIndex];
        }

        public override bool MovePrevious()
        {
            if (_bufferIndex <= 0) return false;

            _bufferIndex--;
            _leftToReturn++;
            return true;
        }

        public override int Remaining => _leftToReturn;
    }
}