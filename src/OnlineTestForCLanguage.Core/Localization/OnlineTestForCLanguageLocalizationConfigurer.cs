using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace OnlineTestForCLanguage.Localization
{
    public static class OnlineTestForCLanguageLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(OnlineTestForCLanguageConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(OnlineTestForCLanguageLocalizationConfigurer).GetAssembly(),
                        "OnlineTestForCLanguage.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
