using System.Text;

namespace Bank.Utils;

/// <summary>
/// Кодировка сообщения в латинские символы
/// </summary>
internal class CyrillicToLatinFallback : EncoderFallback
{
    #region charTable

    private static readonly Dictionary<char, string> Table = new()
    {
        {'А', "A"},
        {'Б', "B"},
        {'В', "V"},
        {'Г', "G"},
        {'Д', "D"},
        {'Е', "E"},
        {'Ё', "E"},
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
        {'Ы', "Y"},
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
        {'ё', "e"},
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
        {'ы', "y"},
        {'ъ', "'"},
        {'э', "e"},
        {'ю', "iu"},
        {'я', "ia"}
    };

    #endregion

    /// <inheritdoc />
    public override EncoderFallbackBuffer CreateFallbackBuffer() => new CyrillicToLatinFallbackBuffer(Table);

    /// <inheritdoc />
    public override int MaxCharCount => 4; // Maximum is "Shch" and "shch"
}
