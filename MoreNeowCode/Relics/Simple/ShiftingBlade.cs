using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MoreNeow.MoreNeowCode.Enchantments;

namespace MoreNeow.MoreNeowCode.Relics.Simple;


[Pool(typeof(EventRelicPool))]
public class ShiftingBlade : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Unstable>();
    public override bool HasUponPickupEffect => true;

    public async override Task AfterObtained()
    {
        //CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
        //foreach (CardModel card in (await CardSelectCmd.FromDeckForEnchantment(Owner, ModelDb.Enchantment<Unstable>(), 1, prefs)).ToList<CardModel>())
        //{
            
       //}
       
       //CardCreationOptions options1 = new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.Uniform, (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Common)).WithFlags(CardCreationFlags.NoUpgradeRoll);
       //List<CardModel> list = CardFactory.CreateForReward(Owner, 1, options1).Select((Func<CardCreationResult, CardModel>) (r => r.Card)).ToList();
       //if (list.Count <= 0)
       //    return;
       //CardModel card = list[0];
       
       CardModel card = Owner.RunState.CreateCard(GetStrikeForCharacter(this.Owner.Character), this.Owner);
       if (card != null)
       {
           if (ModelDb.Enchantment<Unstable>().CanEnchant(card))
           {
               CardCmd.Enchant<Unstable>(card, 1M);
               NRun? instance = NRun.Instance;
               if (instance != null)
                   instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node)NCardEnchantVfx.Create(card)!);
               CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck));
           }
       }
    }
    
    private static CardModel GetStrikeForCharacter(CharacterModel character)
    {
        return TestMode.IsOn && character is Deprived ? (CardModel) ModelDb.Card<StrikeIronclad>() : character.CardPool.AllCards.First<CardModel>((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Basic && c.Tags.Contains<CardTag>(CardTag.Strike)));
    }
    
}