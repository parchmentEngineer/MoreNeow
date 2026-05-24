using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoreNeow.MoreNeowCode.Powers;

public class BrawlPower : MoreNeowPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command.Attacker != Owner)
            return;
        await CreatureCmd.GainBlock(Owner,command.Results.SelectMany(r => r).Sum(r => r.TotalDamage + r.OverkillDamage) * Amount, ValueProp.Unpowered, null);
        //await CreatureCmd.GainBlock(Owner, command.Results.Sum(r => r.TotalDamage + r.OverkillDamage) * Amount, ValueProp.Unpowered, null);
        await PowerCmd.Remove(this);
    }
    
    /*
    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Enemy)
            return;
        //await PowerCmd.Remove(this);
    }*/
}