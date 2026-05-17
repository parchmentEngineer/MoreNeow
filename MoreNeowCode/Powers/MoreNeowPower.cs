using BaseLib.Abstracts;
using BaseLib.Extensions;
using MoreNeow.MoreNeowCode.Extensions;
using Godot;

namespace MoreNeow.MoreNeowCode.Powers;

public abstract class MoreNeowPower : CustomPowerModel
{
    //Loads from MoreNeow/images/powers/your_power.png
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}