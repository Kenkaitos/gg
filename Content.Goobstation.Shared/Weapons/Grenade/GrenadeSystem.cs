using Content.Goobstation.Shared.Boomerang;
using Content.Shared.Emag.Systems;
using Content.Goobstation.Common.Emag;

namespace Content.Goobstation.Shared.Weapons.Grenade;
public sealed partial class GrenadeSystem : EntitySystem
{
    [Dependency] private readonly EmagSystem _emag = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GrenadeComponent, GotEmaggedEvent>(OnGotEmagged);
        SubscribeLocalEvent<GrenadeComponent, EmagCleanedEvent>(OnEmagCleaned);

    }

    private void OnGotEmagged(Entity<GrenadeComponent> ent, ref GotEmaggedEvent args)
    {
        if (!_emag.CompareProtoId(args.Type, "Jestographic"))
            return;

        if (_emag.CheckProtoId(ent.Owner, "Jestographic"))
            return;

        EnsureComp<BoomerangComponent>(ent.Owner);

        args.Handled = true;
    }

    private void OnEmagCleaned(Entity<GrenadeComponent> ent, ref EmagCleanedEvent args)
    {
        if (args.Handled)
            return;

        if (!HasComp<BoomerangComponent>(ent.Owner))
            return;

        RemComp<BoomerangComponent>(ent.Owner);

        args.Handled = true;
    }
}
