using Content.Goobstation.Shared.Toys.Systems;

namespace Content.Goobstation.Shared.Toys.Components;

/// <summary>
/// See <see cref="ClownRecorderSystem"/>
/// </summary>
[RegisterComponent]
public sealed partial class ClownRecorderComponent : Component
{
    [DataField]
    public TimeSpan NormalDelay = TimeSpan.FromSeconds(30);

    [DataField]
    public TimeSpan EmaggedDelay = TimeSpan.FromSeconds(5);
}
