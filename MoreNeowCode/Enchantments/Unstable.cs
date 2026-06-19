using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MoreNeow.MoreNeowCode.Enchantments;

public class Unstable : MoreNeowEnchantment
{
    //protected override void OnEnchant() => this.Card.AddKeyword(CardKeyword.Retain);
    
    private HashSet<CardModel>? _cardsToSkip;
    private HashSet<CardModel> CardsToSkip
    {
        get
        {
            this.AssertMutable();
            if (this._cardsToSkip == null)
                this._cardsToSkip = new HashSet<CardModel>();
            return this._cardsToSkip;
        }
    }
    
    //CardModel card1 = Card.Owner.RunState.CloneCard(card);
    //CardsToSkip.Add(card1);
    //CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card1, PileType.Deck, source: this));
    
    /*if (starterCard.Enchantment != null)
    {
        EnchantmentModel enchantment = (EnchantmentModel) starterCard.Enchantment.MutableClone();
        CardCmd.Enchant(enchantment, card, (Decimal) enchantment.Amount);
    }*/
    
    public override async Task AfterCardChangedPiles(CardModel newlyAddedCard, PileType oldPileType, AbstractModel? source)
    {
        CardPile pile = newlyAddedCard.Pile;
        if ((pile != null ? (pile.Type != PileType.Deck ? 1 : 0) : 1) != 0 || newlyAddedCard.Owner != this.Card.Owner || source != null || CardsToSkip.Remove(newlyAddedCard))
            return;
        if (newlyAddedCard.Title != Card.Title)
        {
            //CardModel copiedCard = newlyAddedCard.Owner.RunState.CreateCard(newlyAddedCard.CanonicalInstance, newlyAddedCard.Owner);
            CardModel copiedCard = Card.Owner.RunState.CloneCard(newlyAddedCard);
            if (ModelDb.Enchantment<Unstable>().CanEnchant(copiedCard))
            {
                if (newlyAddedCard.IsUpgraded)
                    CardCmd.Upgrade(copiedCard);

                CardCmd.Enchant<Unstable>(copiedCard, 1M);

                CardsToSkip.Add(copiedCard);
                await CardCmd.Transform(this.Card, copiedCard);
            }
        }
        
    }
}