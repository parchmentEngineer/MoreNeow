using BaseLib.Abstracts;
using BaseLib.Extensions;
using MoreNeow.MoreNeowCode.Extensions;

namespace MoreNeow.MoreNeowCode.Enchantments;

public class MoreNeowEnchantment : CustomEnchantmentModel
{
    protected override string CustomIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
}