using System.Text;

namespace Bank.Utils;

/// <summary>
/// Обработчик кодировки транслитерации
/// </summary>
internal class CyrillicToLatinFallbackBuffer : EncoderFallbackBuffer
{
    private readonly Dictionary<char, string> _table;
    private int _bufferIndex;
    private string _buffer;
    private int _leftToReturn;

    internal CyrillicToLatinFallbackBuffer(Dictionary<char, string> table) =>
        (_table, _bufferIndex, _leftToReturn) = (table, -1, -1);

    /// <inheritdoc />
    public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index) => false;

    /// <inheritdoc />
    public override bool Fallback(char charUnknown, int index)
    {
        if (!charUnknown.IsCyrillicChar()) return false;

        _buffer = _table[charUnknown];
        _leftToReturn = _buffer.Length - 1;
        _bufferIndex = -1;
        return true;
    }

    /// <inheritdoc />
    public override char GetNextChar()
    {
        if (_leftToReturn < 0)
            return '\u0000';

        _leftToReturn--;
        _bufferIndex++;
        return _buffer[_bufferIndex];
    }

    /// <inheritdoc />
    public override bool MovePrevious()
    {
        if (_bufferIndex <= 0) return false;

        _bufferIndex--;
        _leftToReturn++;
        return true;
    }

    /// <inheritdoc />
    public override int Remaining => _leftToReturn;
}
