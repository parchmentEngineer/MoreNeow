namespace MoreNeow.MoreNeowCode.Relics.Scrapped;

/*[Pool(typeof(EventRelicPool))]
public class ChromaticFragment : MoreNeowRelic
{
    private const string _characterKey = "Character";
    private ModelId? _characterId;
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override bool HasUponPickupEffect => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("Character")];

    private CharacterModel? Character => !(this.CharacterId != null) ? null : ModelDb.GetById<CharacterModel>(this.CharacterId);
    public override LocString Title => Character == null ? new LocString("relics", Id.Entry + ".title") : new LocString("relics", $"{Id.Entry}.{Character.Id.Entry}.title");
    
    [SavedProperty]
    public ModelId? CharacterId
    {
        get => this._characterId;
        set
        {
            this.AssertMutable();
            this._characterId = value;
            ((StringVar) this.DynamicVars["Character"]).StringValue = this.Character.Title.GetFormattedText();
        }
    }

    public override async Task AfterObtained()
    {
        CardCreationOptions options1 = new CardCreationOptions((IEnumerable<CardPoolModel>) Character.CardPool, CardCreationSource.Other, CardRarityOddsType.RegularEncounter);
        List<CardModel> options = CardFactory.CreateForReward(Owner, 2, options1).Select((Func<CardCreationResult, CardModel>) (c => c.Card)).ToList();
        CardModel? chosenCard = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), options, Owner, true);
        if (chosenCard != null)
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(chosenCard, PileType.Deck));
        foreach (CardModel card in options)
        {
            if (card != chosenCard)
                Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(Owner.NetId).CardChoices.Add(new CardChoiceHistoryEntry(card, false));
        }
    }
}*/