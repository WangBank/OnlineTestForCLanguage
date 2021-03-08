using System.Collections.Generic;

namespace OnlineTestForCLanguage.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
