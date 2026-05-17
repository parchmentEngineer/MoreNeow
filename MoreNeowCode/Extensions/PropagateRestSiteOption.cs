using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MoreNeow.MoreNeowCode.Enchantments;
using MoreNeow.MoreNeowCode.Relics.Simple;

namespace MoreNeow.MoreNeowCode.Extensions;

public class PropagateRestSiteOption(Player owner) : RestSiteOption(owner)
{
    public override string OptionId => "PROPAGATE";

    public override async Task<bool> OnSelect()
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1)
        {
            Cancelable = true,
            RequireManualConfirmation = true
        };
        IEnumerable<CardModel> cardModels = await CardSelectCmd.FromDeckForEnchantment(Owner, ModelDb.Enchantment<Serrated>(), 1, prefs);
        foreach (CardModel card in cardModels)
        {
            CardCmd.Enchant<Serrated>(card, 1M);
            NRun? instance = NRun.Instance;
            if (instance != null)
                instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) NCardEnchantVfx.Create(card)!);
        }
        
        this.Owner.GetRelic<JaggedCoral>().Flash();
        return true;
    }

    public override Task DoLocalPostSelectVfx(CancellationToken ct = default (CancellationToken))
    {
        NDebugAudioManager.Instance?.Play("SOTE_SFX_SleepBlanket_v1.mp3", 0.5f, PitchVariance.Small);
        return Task.CompletedTask;
    }

    public override Task DoRemotePostSelectVfx()
    {
        NDebugAudioManager.Instance?.Play("SOTE_SFX_SleepBlanket_v1.mp3", 0.5f, PitchVariance.Small);
        NRestSiteRoom instance = NRestSiteRoom.Instance;
        NRestSiteCharacter parent = instance != null ? instance.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => c.Player == this.Owner)) : (NRestSiteCharacter) null;
        parent?.Shake();
        NRelicFlashVfx child = NRelicFlashVfx.Create((RelicModel) ModelDb.Relic<Shovel>());
        if (child == null)
            return Task.CompletedTask;
        if (parent != null)
            parent.AddChildSafely((Node) child);
        child.Position = Vector2.Zero;
        return Task.CompletedTask;
    }
}