using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MoreNeow.MoreNeowCode.Relics.Simple;


[Pool(typeof(EventRelicPool))]
public class ShatteredPane : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        List<CardPoolModel> charPools = Owner.UnlockState.CharacterCardPools.ToList<CardPoolModel>();
        if (charPools.Count > 1)
            charPools.Remove(Owner.Character.CardPool);
        CardCreationOptions options1 = new CardCreationOptions(charPools, CardCreationSource.Other, CardRarityOddsType.Uniform, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Uncommon)).WithFlags(CardCreationFlags.NoUpgradeRoll);
        List<CardModel> list = CardFactory.CreateForReward(Owner, 3, options1).Select((Func<CardCreationResult, CardModel>) (r => r.Card)).ToList();
        if (list.Count <= 0)
            return;
        CardModel card = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), (IReadOnlyList<CardModel>)list, Owner, true);
        if (card != null)
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck));
        }
    }
}