using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Cards.Mocks;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.TestSupport;
using MoreNeow.MoreNeowCode.Cards.AltStarters;
using MoreNeow.MoreNeowCode.Cards.Scrapped;

namespace MoreNeow.MoreNeowCode.Relics.Complex;

[Pool(typeof(EventRelicPool))]
public class UnfamiliarDeckbox : MoreNeowRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override bool HasUponPickupEffect => true;

    private static Dictionary<ModelId, List<ModelId>> CharacterDecks = new();

    public static void AddCharacterDeck(ModelId character, ModelId card1, ModelId card2)
    {
        CharacterDecks[character] = new List<ModelId>() { card1, card2 };
    }

    public static bool DoesCharacterHaveDeck(CharacterModel character)
    {
        return CharacterDecks.ContainsKey(character.Id);
    }
    
    public override async Task AfterObtained()
    {

        ModelId attackCardId = null;
        ModelId skillCardId = null;
        List<ModelId> starterCards = new();
        if (CharacterDecks.TryGetValue(Owner.Character.Id, out starterCards))
        {
            attackCardId = starterCards[0];
            skillCardId = starterCards[1];
        }

        /*if (Owner.Character is Ironclad)
        {
            attackCard = ModelDb.Card<IroncladAttack>();
            skillCard = ModelDb.Card<IroncladSkill>();
        }
        if (Owner.Character is Silent)
        {
            attackCard = ModelDb.Card<SilentAttack>();
            skillCard = ModelDb.Card<SilentSkill>();
        }
        if (Owner.Character is Regent)
        {
            attackCard = ModelDb.Card<RegentAttack>();
            skillCard = ModelDb.Card<RegentSkill>();
        } 
        if (Owner.Character is Necrobinder)
        {
            attackCard = ModelDb.Card<NecrobinderAttack>();
            skillCard = ModelDb.Card<NecrobinderSkill>();
        }

        if (Owner.Character is Defect)
        {
            attackCard = ModelDb.Card<DefectAttack>();
            skillCard = ModelDb.Card<DefectSkill>();
        }*/

        if (attackCardId is null || skillCardId is null)
            return;

        List<CardModel> cardsToReturn = new List<CardModel>();
        foreach (CardModel card in (IEnumerable<CardModel>)PileType.Deck.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            if (card.Rarity == CardRarity.Basic)
            {
                await CardPileCmd.RemoveFromDeck(card);
            }
            else
            {
                cardsToReturn.Add(card);
                await CardPileCmd.RemoveFromDeck(card);
            }
        }

        
        List<CardPileAddResult> results = new List<CardPileAddResult>();
        for (int i = 0; i < 2; ++i)
        {
            CardModel card = Owner.RunState.CreateCard(GetStrikeForCharacter(this.Owner.Character), this.Owner);
            results.Add(await CardPileCmd.Add(card, PileType.Deck));
        }
        for (int i = 0; i < 3; ++i)
        {
            CardModel card = Owner.RunState.CreateCard(ModelDb.GetById<CardModel>(attackCardId), Owner);
            results.Add(await CardPileCmd.Add(card, PileType.Deck));
        }
        for (int i = 0; i < 2; ++i)
        {
            CardModel card = Owner.RunState.CreateCard(GetDefendForCharacter(this.Owner.Character), this.Owner);
            results.Add(await CardPileCmd.Add(card, PileType.Deck));
        }
        for (int i = 0; i < 3; ++i)
        {
            CardModel card = Owner.RunState.CreateCard(ModelDb.GetById<CardModel>(skillCardId), Owner);
            results.Add(await CardPileCmd.Add(card, PileType.Deck));
        }
        foreach (CardModel card in cardsToReturn)
        {
            CardModel newCard = Owner.RunState.CreateCard(ModelDb.GetById<CardModel>(card.Id), Owner);
            results.Add(await CardPileCmd.Add(newCard, PileType.Deck));
        }
        
        //CardCmd.PreviewCardPileAdd(results, 2f, style: CardPreviewStyle.MessyLayout);
        foreach(CardPileAddResult toPreview in results)
        {
            CardCmd.PreviewCardPileAdd(toPreview, style: CardPreviewStyle.MessyLayout);
            await Cmd.CustomScaledWait(0.1f, 0.2f);
        }
    }
    
    private static CardModel GetStrikeForCharacter(CharacterModel character)
    {
        return TestMode.IsOn && character is Deprived ? (CardModel) ModelDb.Card<StrikeIronclad>() : character.CardPool.AllCards.First<CardModel>((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Basic && c.Tags.Contains<CardTag>(CardTag.Strike)));
    }
    
    private static CardModel GetDefendForCharacter(CharacterModel character)
    {
        return TestMode.IsOn && character is Deprived ? (CardModel) ModelDb.Card<DefendIronclad>() : character.CardPool.AllCards.First<CardModel>((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Basic && c.Tags.Contains<CardTag>(CardTag.Defend)));
    }
}