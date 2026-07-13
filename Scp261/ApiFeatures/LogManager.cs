using System;
using LabApi.Features.Console;

namespace Scp261.ApiFeatures;

internal static class LogManager
{
    private static string PluginName => Scp261.Singleton?.Name ?? "VendingMachine";

    public static void Info(string message, ConsoleColor color = ConsoleColor.Cyan)
    {
        Logger.Raw($"[INFO] [{PluginName}] {message}", color);
    }

    public static void Warn(string message)
    {
        Logger.Warn(message);
    }

    public static void Error(string message, ConsoleColor color = ConsoleColor.Red)
    {
        Logger.Raw($"[ERROR] [{PluginName}] {message}", color);
    }
}