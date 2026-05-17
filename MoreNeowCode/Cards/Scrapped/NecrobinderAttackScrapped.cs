using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoreNeow.MoreNeowCode.Cards.Scrapped;


[Pool(typeof(DeprecatedCardPool))]
public class NecrobinderAttackScrapped : MoreNeowCard
{
    public NecrobinderAttackScrapped() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m, ValueProp.Move), new DynamicVar("Increase", 1M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Ethereal)];
    
    private Decimal _extraDamage;
    
    private Decimal ExtraDamage
    {
        get => this._extraDamage;
        set
        {
            this.AssertMutable();
            this._extraDamage = value;
        }
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        Decimal increase = DynamicVars["Increase"].BaseValue;
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue += increase;
        ExtraDamage += increase;
        CardCmd.ApplyKeyword(this, CardKeyword.Ethereal);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3);
}