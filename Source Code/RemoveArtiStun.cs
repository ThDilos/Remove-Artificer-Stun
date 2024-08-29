using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using Steamworks;

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
                if (outerScopeTester() && self.SlugCatClass == MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName.Artificer)
                {
                    return;
                }
                else
                {
                    orig(self, st);
                }
            };

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
    private bool outerScopeTester()
    {
        return options.enable_remove_stun.Value;
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
