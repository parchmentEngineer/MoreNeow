using BaseLib.Config;

namespace MoreNeow.MoreNeowCode;

public class MoreNeowConfig : SimpleModConfig
{
    
    [ConfigHoverTip]
    public static bool AlwaysOfferNewOption { get; set; } = true;
    [ConfigHoverTip]
    public static bool AlwaysOfferDeckbox { get; set; } = false;
    [ConfigHoverTip]
    public static bool OnlyOfferNewOptions { get; set; } = false;
}