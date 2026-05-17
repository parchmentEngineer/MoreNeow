using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoreNeow.MoreNeowCode.Cards.Scrapped;


[Pool(typeof(DefectCardPool))]
public class DefectAttack : MoreNeowCard
{
    public DefectAttack() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5m, ValueProp.Move),
    ];

    //protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];
    
    protected override bool ShouldGlowGoldInternal => this.HasStatusesInHand;
    
    private bool HasStatusesInHand
    {
        get
        {
            foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards)
            {
                if (card.Type == CardType.Status)
                    return true;
            }

            return false;
        }
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        if (!HasStatusesInHand)
            return;
        foreach (OrbModel orb in Owner.PlayerCombatState.OrbQueue.Orbs)
        {
            await OrbCmd.Passive(choiceContext, orb, null);
        }
    }
    
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3M);
}