using System;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace Scp261;

internal class Scp261 : Plugin<Configs>
{
    public static Scp261 Singleton { get; private set; }
    public override string Author => "MedveMarci";
    public override string Name => "SCP-261";
    public override string Description => "SCP-261 is a vending machine that can dispense various items and effects.";
    public override Version Version { get; } = new(1, 0, 0, 0);
    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

    public override void Enable()
    {
        Singleton = this;
        EventHandler.EnableEvents();
    }

    public override void Disable()
    {
        EventHandler.DisableEvents();
        Singleton = null;
    }
}