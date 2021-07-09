using SovComBankTest.Utils;
using Xunit;

namespace SovComBankTest.ModuleTests
{
    public class TransliterationTests
    {
        [Fact]
        public void GivenTheCyrillicMessage_WhenTranslating_TranslationResultMustBeEqual()
        {
            var check = "Эй, жлоб! Где туз? Прячь юных съёмщиц в шкаф.";

            var result = Transliteration.CyrillicToLatin(check);

            Assert.Equal("Ei, zhlob! Gde tuz? Priach' iunyikh s'iomshchits v shkaf.", result);
        }
    }
}