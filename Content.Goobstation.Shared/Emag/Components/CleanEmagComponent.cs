namespace Content.Goobstation.Shared.Emag.Components;

/// <summary>
/// Marker component for emagged entities that can be cleaned
/// </summary>
[RegisterComponent]
public sealed partial class CleanEmagComponent : Component
{
    /// <summary>
    /// How long is the clean do after
    /// </summary>
    [DataField]
    public TimeSpan CleanDuration = TimeSpan.FromSeconds(5);
}
