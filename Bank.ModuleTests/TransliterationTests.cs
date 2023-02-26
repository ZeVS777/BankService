using Bank.Utils;
using Xunit;

namespace Bank.ModuleTests;

/// <summary>
/// Тесты транслитерации
/// </summary>
public class TransliterationTests
{
    /// <summary>
    /// Проверка транслитерации
    /// </summary>
    [Fact]
    public void GivenTheCyrillicMessage_WhenTranslating_TranslationResultMustBeEqual()
    {
        const string check = "Эй, жлоб! Где туз? Прячь юных съёмщиц в шкаф.";

        var result = Transliteration.CyrillicToLatin(check);

        Assert.Equal("Ei, zhlob! Gde tuz? Priach' iunykh s'emshchits v shkaf.", result);
    }
}
