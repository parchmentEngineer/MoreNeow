using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace MoreNeow.MoreNeowCode.Relics.Complex;


[Pool(typeof(EventRelicPool))]
public class AntikytheraFragment : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        for (int i = 0; i < 5; i++)
        {
            CardCreationOptions options = new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.Uniform, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Common)).WithFlags(CardCreationFlags.NoUpgradeRoll);
            List<CardModel> list = CardFactory.CreateForReward(Owner, 1, options).Select<CardCreationResult, CardModel>((Func<CardCreationResult, CardModel>) (r => r.Card)).ToList<CardModel>();
            if (list.Count <= 0)
                return;
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(list[0], PileType.Deck));
        }
    }
}