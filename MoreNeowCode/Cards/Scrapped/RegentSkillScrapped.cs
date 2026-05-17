using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoreNeow.MoreNeowCode.Cards.Scrapped;


[Pool(typeof(DeprecatedCardPool))]
public class RegentSkillScrapped : MoreNeowCard
{
    public RegentSkillScrapped() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self) { }
    public override int CanonicalStarCost => 1;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6m, ValueProp.Move), new PowerVar<VulnerablePower>(1), new PowerVar<WeakPower>(1)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        //Creature target = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
        //if (target == null)
            //return;
        foreach (Creature target in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<WeakPower>(choiceContext, target, DynamicVars.Weak.BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3);
}