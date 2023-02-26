using System.Text;

namespace Bank.Utils;

/// <summary>
/// Методы транслитерации
/// </summary>
public sealed class Transliteration
{
    /// <summary>
    /// Преобразовать строку в латинские символы
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <returns>Сообщение, состоящее только из латинских символов</returns>
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
