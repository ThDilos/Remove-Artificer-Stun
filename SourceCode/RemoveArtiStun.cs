using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using Steamworks;
using Expedition;
using Mono.Cecil.Cil;
using MonoMod.Cil;

#pragma warning disable CS0618

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace RemoveArtiStun;

[BepInPlugin("thdilos.removeartistun", "Remove Artificer Stun", "1.0.0")]

public partial class RemoveArtiStun : BaseUnityPlugin
{
    private MenuTab options;
    public RemoveArtiStun()
    {
        options = new MenuTab(this);
    }
    private void OnEnable()
    {
        On.RainWorld.OnModsInit += RainWorldOnOnModsInit;
    }

    private bool IsInit;
    private void RainWorldOnOnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);
        try
        {
            MachineConnector.SetRegisteredOI("thdilos.removeartistun", options);
        }
        catch (Exception e)
        {
            Logger.LogError(e);
        }
        try
        {
            if (IsInit) return;

            //Your hooks go here
            On.Player.Stun += (orig, self, st) =>
            {
                if (options.remove_global_stun.Value && // If you removed all stun AND
                (self.SlugCatClass == MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName.Artificer || // You are Artificer OR
                (ModManager.Expedition && self.room.game.rainWorld.ExpeditionMode && ExpeditionGame.activeUnlocks.Contains("unl-explosionimmunity") && options.apply_to_exp_perk.Value) // You are in Expedition, You have Explosion Immunity, You ticked apply remove stun to the perk in remix menu
                ))
                {
                    return;
                }
                else
                {
                    orig(self, st);
                }
            };
            // Source-Specific Stun // Very Limited
            On.Creature.Violence += (orig, self, source, directionAndMomentum, hitChunk, hitAppendage, type, damage, stunBonus) =>
            {
                if (self is Player && ( // Activate on Player
                    (self as Player).SlugCatClass == MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName.Artificer || // You are Artificer OR
                    (ModManager.Expedition && self.room.game.rainWorld.ExpeditionMode && ExpeditionGame.activeUnlocks.Contains("unl-explosionimmunity") && options.apply_to_exp_perk.Value)) && // Expedition is Enabled in Remix Menu AND You are in Expedition Mode AND You activated Explosion Immunity Perk AND You ticked "Apply" in my Remix Menu
                    outerScopeTester(type)) // Pass the External Damage type check
                {
                    orig(self, source, directionAndMomentum, hitChunk, hitAppendage, type, damage, 0.0f); // Take no Stun
                }
                else
                {
                    orig(self, source, directionAndMomentum, hitChunk, hitAppendage, type, damage, stunBonus); // Take Stun
                }
            };

            // ILHooking
            //IL.Explosion.Update += (il) =>
                //{
                    //ILCursor c = new ILCursor(il);
                    //c.GotoNext(
                        //MoveType.After,
                        //x => x.MatchLdarg(0),
                        //x => x.MatchLdfld(typeof(float).GetField(nameof(Explosion.minStun))),
                        //x => x.MatchLdcR4(0f),
                        //x => x.MatchLdcR4(0.5f)
                        //);
                    //c.MoveAfterLabels();
                    //c.Next.Operand = 0f;
                //};
            // This shi is not working at all :(

            On.RainWorldGame.ShutDownProcess += RainWorldGameOnShutDownProcess;
            On.GameSession.ctor += GameSessionOnctor;


            
            IsInit = true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }

    // There's prob better ways than doing this, but I find this method myself and Im very proud >:3
    // External Damage type check
    private bool outerScopeTester(Creature.DamageType type)
    {
        if (options.remove_global_stun.Value)
        {
            return false; // Neglect this part of function if you already removed all the stun sources
        }
        if (type == Creature.DamageType.Explosion) // ScavengerBomb / FireEgg / SingularityBomb etc.
        {
            return options.remove_explosion_stun.Value;
        }
        else if (type == Creature.DamageType.Electric) // JellyFish 
        {
            return options.remove_electric_stun.Value; // SmallCentipede is not included
        }
        else if (type == Creature.DamageType.Blunt) // Rock etc.
        {
            return options.remove_blunt_stun.Value;
        }
        else // Fall damage is not a valid Damage type, it is hard coded into the Player class, need to use the ILHook, which is far more complicated
        {
            return false;
        }
    }
    
    private void RainWorldGameOnShutDownProcess(On.RainWorldGame.orig_ShutDownProcess orig, RainWorldGame self)
    {
        orig(self);
        ClearMemory();
    }
    private void GameSessionOnctor(On.GameSession.orig_ctor orig, GameSession self, RainWorldGame game)
    {
        orig(self, game);
        ClearMemory();
    }

    #region Helper Methods

    private void ClearMemory()
    {
        //If you have any collections (lists, dictionaries, etc.)
        //Clear them here to prevent a memory leak
        //YourList.Clear();
    }

    #endregion
}
