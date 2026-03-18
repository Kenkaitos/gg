using Content.Goobstation.Common.Emag.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Common.Emag;

/// <summary>
/// Raised on the entity when it's cleanable emag is cleaned 
/// </summary>
/// <param name="User">The entity</param>
/// <param name="EmagType">The type of emag that was cleaned</param>
/// <param name="Handled">If event is handled by previous system</param>
[ByRefEvent]
public record struct EmagCleanedEvent(EntityUid User, ProtoId<EmagTypePrototype> EmagType, bool Handled = false);
