using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using OnlineTestForCLanguage.Authorization;
using System.Linq;

namespace OnlineTestForCLanguage.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class OnlineTestForCLanguageNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("HomePage"),
                        url: "",
                        icon: "fas fa-home",
                        requiresAuthentication: true
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Users,
                        L("Users"),
                        url: "Users",
                        icon: "fas fa-users",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Roles,
                        L("Roles"),
                        url: "Roles",
                        icon: "fas fa-tired",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                            )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Exams,
                        L("Exams"),
                        url: "Exams",
                        icon: "fas fa-umbrella",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Exams)
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Papers,
                        L("Papers"),
                        url: "Papers",
                        icon: "fas fa-weight",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Papers)
                    )
                )
              .AddItem(
                    new MenuItemDefinition(
                        PageNames.Tests,
                        L("Tests"),
                        url: "Tests",
                        icon: "fas fa-vials",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tests)
                    )
                )
               .AddItem(
                    new MenuItemDefinition(
                        PageNames.TestCounts,
                        L("TestCounts"),
                        url: "TestCounts",
                        icon: "fas fa-vials",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_TestCounts)
                    )
                )
                //.AddItem(
                //    new MenuItemDefinition(
                //        PageNames.About,
                //        L("About"),
                //        url: "About",
                //        icon: "fas fa-info-circle"
                //    )
                //)
                //.AddItem( // Menu items below is just for demonstration!
                //    new MenuItemDefinition(
                //        "MultiLevelMenu",
                //        L("MultiLevelMenu"),
                //        icon: "fas fa-circle"
                //    ).AddItem(
                //        new MenuItemDefinition(
                //            "AspNetBoilerplate",
                //            new FixedLocalizableString("ASP.NET Boilerplate"),
                //            icon: "far fa-circle"
                //        ).AddItem(
                //            new MenuItemDefinition(
                //                "AspNetBoilerplateHome",
                //                new FixedLocalizableString("Home"),
                //                url: "https://aspnetboilerplate.com?ref=abptmpl",
                //                icon: "far fa-dot-circle"
                //            )
                //        )
                //    ).AddItem(
                //        new MenuItemDefinition(
                //            "AspNetZero",
                //            new FixedLocalizableString("ASP.NET Zero"),
                //            icon: "far fa-circle"
                //        ).AddItem(
                //            new MenuItemDefinition(
                //                "AspNetZeroHome",
                //                new FixedLocalizableString("Home"),
                //                url: "https://aspnetzero.com?ref=abptmpl",
                //                icon: "far fa-dot-circle"
                //            )
                //        )
                //    )
                //)
                ;
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, OnlineTestForCLanguageConsts.LocalizationSourceName);
        }
    }
}
