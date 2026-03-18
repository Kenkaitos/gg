using Content.Goobstation.Shared.Emag.Components;
using Content.Shared.Emag.Components;
using Content.Shared.Interaction;
using Robust.Shared.Prototypes;
using Content.Shared.Emag.Systems;
using Content.Shared.Popups;
using Content.Shared.DoAfter;
using Content.Goobstation.Common.Emag.Prototypes;
using Content.Goobstation.Common.Emag;

namespace Content.Goobstation.Shared.Emag.Systems;

public sealed partial class CleanEmagSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly EmagSystem _emag = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CleanEmagComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<EmaggedComponent, CleaningEmaggedDeviceDoAfterEvent>(OnCleaningEmaggedDevice);
    }

    private void OnAfterInteract(Entity<CleanEmagComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Target == null || !args.CanReach || args.Handled)
            return;

        if (!HasComp<EmaggedComponent>(args.Target) || !_emag.CheckProtoId(args.Target.Value, "Jestographic"))
            return;

        var doAfter = new DoAfterArgs(EntityManager, args.User, ent.Comp.CleanDuration, new CleaningEmaggedDeviceDoAfterEvent(), args.Target, args.Target)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            BreakOnHandChange = true
        };

        _doAfter.TryStartDoAfter(doAfter);

        _popup.PopupPredicted(Loc.GetString("emag-cleaning", ("device", args.Target)), args.User, args.User);

        args.Handled = true;
    }

    private void OnCleaningEmaggedDevice(Entity<EmaggedComponent> ent, ref CleaningEmaggedDeviceDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        ProtoId<EmagTypePrototype>? cleanableEmag = null;

        foreach (var emagType in ent.Comp.EmagTypeList)
        {
            if (!_proto.TryIndex(emagType, out var proto) || !proto.IsCleanable)
                continue;

            cleanableEmag = emagType;
            break;
        }

        if (cleanableEmag == null)
            return;

        if (!ent.Comp.EmagTypeList.Remove(cleanableEmag.Value))
            return;

        Dirty(ent);
        var ev = new EmagCleanedEvent(args.User, cleanableEmag.Value);
        RaiseLocalEvent(ent.Owner, ref ev);

        args.Handled = true;
    }
}
