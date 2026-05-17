using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MoreNeow.MoreNeowCode.Cards;

namespace MoreNeow.MoreNeowCode.Relics.Simple;


[Pool(typeof(EventRelicPool))]
public class NeowsEmbrace : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<NeowsRebirth>()];
    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        List<CardPileAddResult> newCard = new List<CardPileAddResult>(1);
        CardModel card = Owner.RunState.CreateCard<NeowsRebirth>(Owner);
        newCard.Add(await CardPileCmd.Add(card, PileType.Deck));
        CardCmd.PreviewCardPileAdd(newCard, 2f);
    }
}