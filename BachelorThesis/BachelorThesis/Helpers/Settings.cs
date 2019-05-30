// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace BachelorThesis.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
{
    private static ISettings AppSettings
    {
        get
        {
            return CrossSettings.Current;
        }
    }

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private const string LoginKey = "logged_in";
    private const string GuestKey = "guest";
    private const string UserKey = "user";
    private const string CompilerUidKey = "compiler_uid";
    private static readonly string SettingsDefault = string.Empty;

    #endregion

    public static bool IsLoggedIn
    {
        get
        {
            return AppSettings.GetValueOrDefault(LoginKey, false);
        }
        set
        {
            AppSettings.AddOrUpdateValue(LoginKey, value);
        }
    }

    public static string CompilerUid
    {
        get
        {
            return AppSettings.GetValueOrDefault(CompilerUidKey, null);
        }
        set
        {
            AppSettings.AddOrUpdateValue(CompilerUidKey, value);
        }
    }

    public static bool IsGuest
    {
        get
        {
            return AppSettings.GetValueOrDefault(GuestKey, false);
        }
        set
        {
            AppSettings.AddOrUpdateValue(GuestKey, value);
        }
    }

    public static string UserSettings
    {
        get
        {
            return AppSettings.GetValueOrDefault(UserKey, SettingsDefault);
        }
        set
        {
            AppSettings.AddOrUpdateValue(UserKey, value);
        }
    }

    public static string GeneralSettings
    {
        get
        {
            return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
        }
        set
        {
            AppSettings.AddOrUpdateValue(SettingsKey, value);
        }
    }
}
}
