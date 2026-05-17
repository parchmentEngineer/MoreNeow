using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace MoreNeow.MoreNeowCode.Relics.Complex;

[Pool(typeof(EventRelicPool))]
public class SpectersGrin : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Apparition>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new MaxHpVar(18)];
    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(),  Owner.Creature, DynamicVars.MaxHp.BaseValue, false);
        List<CardPileAddResult> newCard = new List<CardPileAddResult>(1);
        CardModel card = Owner.RunState.CreateCard<Apparition>(Owner);
        newCard.Add(await CardPileCmd.Add(card, PileType.Deck));
        CardCmd.PreviewCardPileAdd(newCard, 2f);
    }
}