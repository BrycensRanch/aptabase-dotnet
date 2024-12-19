using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Aptabase.Core;

internal class SystemInfo
{
    private static readonly string _pkgVersion = typeof(AptabaseClient).Assembly
        .GetCustomAttribute<AssemblyFileVersionAttribute>()!.Version;

    public bool IsDebug { get; set; }
    public string OsName { get; }
    public string OsVersion { get; }
    public string DeviceModel { get; }
    public string SdkVersion { get; }
    public string Locale { get; }
    public string AppVersion { get; }
    public string AppBuildNumber { get; }

    public SystemInfo()
	{
        OsName = GetOsName();
        OsVersion = GetOsVersion();
        DeviceModel = GetDeviceModel();
        SdkVersion = $"{Assembly.GetExecutingAssembly().GetName()}@{_pkgVersion}";
        Locale = Thread.CurrentThread.CurrentCulture.Name;
        AppVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? string.Empty;
        AppBuildNumber = Assembly.GetEntryAssembly()?.GetName().Version?.Build.ToString() ?? string.Empty;
    }
        
    internal static bool IsInDebugMode(Assembly? assembly)
    {
        if (assembly == null)
            return false;

        var attributes = assembly.GetCustomAttributes(typeof(DebuggableAttribute), false);
        if (attributes.Length > 0)
        {
            if (attributes[0] is DebuggableAttribute debuggable)
                return (debuggable.DebuggingFlags & DebuggableAttribute.DebuggingModes.Default) == DebuggableAttribute.DebuggingModes.Default;
            else
                return false;
        }
        else
            return false;
    }

    private static string GetOsName()
    {
        if (OperatingSystem.IsAndroid())
            return "Android";

        if (OperatingSystem.IsWindows())
            return "Windows";
    
        
        if (OperatingSystem.IsMacOS()) return "macOS";
        if (OperatingSystem.IsTvOS()) return "tvOS";
        if (OperatingSystem.IsWatchOS()) return "watchOS";
        // iPadOS is bundled as iOS as well. It's hard to tell the difference from pure .NET
        if (OperatingSystem.IsIOS()) return "iOS";

        if (OperatingSystem.IsLinux()) return DistroDetect.GetLinuxDistro();
        if (OperatingSystem.IsFreeBSD()) return "FreeBSD";


        return string.Empty;
    }

    private static string GetDeviceModel()
    {
        return "";
    }

    private static string GetOsVersion()
    {
#if MACCATALYST
        var osVersion = Foundation.NSProcessInfo.ProcessInfo.OperatingSystemVersion;
        return $"{osVersion.Major}.{osVersion.Minor}.{osVersion.PatchVersion}";
#else
        return OperatingSystem.IsLinux() ? DistroDetect.GetLinuxDistroVersion() : Environment.OSVersion.Version.ToString();
#endif
    }
}

