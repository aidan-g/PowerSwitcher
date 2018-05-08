using Microsoft.Win32;
using System;

namespace PowerSwitcher.TrayApp.Services
{
    ////
    //  Code heavily inspired by https://github.com/File-New-Project/EarTrumpet/blob/master/EarTrumpet/Services/UserSystemPreferencesService.cs
    ////
    public static class UserSystemPreferencesService
    {
        const string KEY_NAME = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        public static bool IsTransparencyEnabled
        {
            get
            {
                return WithSubKey(RegistryHive.CurrentUser, RegistryView.Registry64, KEY_NAME, key =>
                {
                    if (key == null)
                    {
                        return false;
                    }
                    return (int)key.GetValue("EnableTransparency", 0) > 0;
                });
            }
        }

        public static bool UseAccentColor
        {
            get
            {
                return WithSubKey(RegistryHive.CurrentUser, RegistryView.Registry64, KEY_NAME, key =>
                 {
                     if (key == null)
                     {
                         return false;
                     }
                     return (int)key.GetValue("ColorPrevalence", 0) > 0;
                 });
            }
        }

        private static T WithSubKey<T>(RegistryHive hKey, RegistryView view, string name, Func<RegistryKey, T> func)
        {
            using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
            {
                using (var subKey = baseKey.OpenSubKey(name))
                {
                    return func(subKey);
                }
            }
        }
    }
}
