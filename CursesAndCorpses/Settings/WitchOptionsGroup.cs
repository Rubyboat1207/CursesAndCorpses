// using System;
// using MiraAPI.GameOptions;
// using MiraAPI.GameOptions.Attributes;

// public class WitchOptionsGroup : AbstractOptionGroup
// {

//     public override string GroupName => "Witch Settings";

//     public override Type AdvancedRole => typeof(WitchRole);

//     [ModdedNumberOption("Curse Cooldown", min:5f, max:45f)]
//     public float CurseCooldown {get; set;} = 15f;
    

//     [ModdedNumberOption("Curses Per Game", min:0f, max:15f)]
//     public int CurseCount {get; set;} = 0;
// }