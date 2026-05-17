using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves.Runs;
using MoreNeow.MoreNeowCode.Extensions;

namespace MoreNeow.MoreNeowCode.Relics.Simple;

[Pool(typeof(EventRelicPool))]
public class PicnicBasket : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private bool _used;
    [SavedProperty]
    public bool Used
    {
        get => this._used;
        set
        {
            this.AssertMutable();
            this._used = value;
            this.InvokeDisplayAmountChanged();
            this.CheckIfUsedUp();
        }
    }
    
    public void CheckIfUsedUp()
    {
        if (!this.IsUsedUp)
            return;
        this.Status = RelicStatus.Disabled;
    }
    
    public override bool ShowCounter => true;
    public override int DisplayAmount => this.Used ? 0 : 1;
    public override bool IsUsedUp => this.Used;

    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != this.Owner || this.Used)
            return false;
        options.Add(new FeastRestSiteOption(player));
        return true;
    }
}