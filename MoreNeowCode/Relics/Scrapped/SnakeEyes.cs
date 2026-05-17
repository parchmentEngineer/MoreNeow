namespace MoreNeow.MoreNeowCode.Relics.Scrapped;


/*[Pool(typeof(EventRelicPool))]
public class SnakeEyes : MoreNeowRelic
{
    private int _timesUsed;
    
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Slither>();
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(6)];

    public override bool IsUsedUp => this.TimesUsed >= DynamicVars.Cards.IntValue;
    public override bool ShowCounter => this.DynamicVars.Cards.IntValue > 0 && this.TimesUsed < this.DynamicVars.Cards.IntValue;
    public override int DisplayAmount => this.DynamicVars.Cards.IntValue - this.TimesUsed;
    
    [SavedProperty]
    public int TimesUsed
    {
        get => this._timesUsed;
        set
        {
            this.AssertMutable();
            this._timesUsed = value;
            this.InvokeDisplayAmountChanged();
            this.CheckIfUsedUp();
        }
    }
    
    private void CheckIfUsedUp()
    {
        if (!this.IsUsedUp)
            return;
        this.Status = RelicStatus.Disabled;
    }
    
    public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewards, CardCreationOptions options)
    {
        if (player != this.Owner || this.TimesUsed >= this.DynamicVars.Cards.IntValue)
            return false;
        Slither canonicalSlither = ModelDb.Enchantment<Slither>();
        List<CardCreationResult> list = cardRewards.Where(r => canonicalSlither.CanEnchant(r.Card)).ToList();
        if (list.Count == 0)
            return false;
        CardCreationResult? cardCreationResult = Owner.RunState.Rng.Niche.NextItem(list);
        if (cardCreationResult == null)
            return false;
        CardModel card = Owner.RunState.CloneCard(cardCreationResult.Card);
        CardCmd.Enchant<Slither>(card, 1);
        cardCreationResult.ModifyCard(card, this);
        return true;
    }
    
    public override Task AfterModifyingCardRewardOptions()
    {
        if (this.TimesUsed >= this.DynamicVars.Cards.IntValue)
            return Task.CompletedTask;
        ++this.TimesUsed;
        return Task.CompletedTask;
    }
}*/