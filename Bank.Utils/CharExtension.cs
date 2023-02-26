namespace Bank.Utils;

/// <summary>
/// Метод расширения операции над символами
/// </summary>
public static class CharExtension
{
    /// <summary>
    /// Проверка принадлежности символа алфавиту кириллицы
    /// </summary>
    /// <param name="ch">Символ для проверки</param>
    /// <returns><see langwword="true"/>, если символ из алфавита кириллицы, иначе <see langwword="false"/></returns>
    public static bool IsCyrillicChar(this char ch) => ch switch
    {
        'Ё' or 'ё' => true,
        >= 'А' and <= 'я' => true,
        _ => false
    };
}
