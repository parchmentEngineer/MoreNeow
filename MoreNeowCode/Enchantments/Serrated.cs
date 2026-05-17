using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MoreNeow.MoreNeowCode.Enchantments;

public class Serrated : MoreNeowEnchantment
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    protected override void OnEnchant() => this.Card.AddKeyword(CardKeyword.Exhaust);
    public override bool HasExtraCardText => true;
    public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Attack;
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await PowerCmd.Apply<VulnerablePower>(choiceContext, Card.TargetType == TargetType.AllEnemies ? Card.CombatState.HittableEnemies : (IEnumerable<Creature>) new List<Creature>([cardPlay.Target]), 2, Card.Owner.Creature, Card);
    }
}