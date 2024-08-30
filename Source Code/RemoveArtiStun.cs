using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using Steamworks;

#pragma warning disable CS0618

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
// Only stuffs I've changed is in the [Start]:[End] bracket
// [Start]
namespace RemoveArtiStun; // Name space thingy, idk it was auto-completed by visual studio

[BepInPlugin("thdilos.removeartistun", "Remove Artificer Stun", "1.0.0")] // The plugin info, first string is [authorname].[modid], second string is [Mod Name], third is [version] 

public partial class RemoveArtiStun : BaseUnityPlugin
{
    private MenuTab options; // The remix menu instance [Null]
    public RemoveArtiStun() // When this mod is initialised, assign the remix menu instance
    {
        options = new MenuTab(this);
    }
    private void OnEnable()
    {
        On.RainWorld.OnModsInit += RainWorldOnOnModsInit; // Idk this line, it's from template // Prob registering this mod into the mod loader?
    }

    private bool IsInit; // idk
    private void RainWorldOnOnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self) // idk // prob execute the function when the mod is initialised
    {
        orig(self);
        try // Ok this one I did myself, try to register the options[remix menu] into the game Remix Menu
        {
            MachineConnector.SetRegisteredOI("thdilos.removeartistun", options); // This single line does the trick, it's from rainworld modding wiki
        }
        catch (Exception e)
        {
            Logger.LogError(e);  // this log is put under \Rain World\BepInEx\LogOutput.log
        }
        try
        {
            if (IsInit) return;
        
            //Your hooks go here // <- I didn't write this comment
            On.Player.Stun += (orig, self, st) => // Ok this one is achieving the main function // I've never seen this structure b4 ngl. wtf is a += () =>
            // But it's like this: On.[The Class you want to modify].[The specific method you want to modify] += (orig, self, The Variable 1 [in this case is st, stun duration], [If have] The variable 2...) =>
            // The Stun function in vanilla game is like, Stun(int st), where st is the stun duration
            {
                if (outerScopeTester() && self.SlugCatClass == MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName.Artificer) // To test you are Artificer and you have tick the checkbox in the remix menu
                {
                    return; // If satisfied both, we do not do the Stun
                }
                else
                {
                    orig(self, st); // Else, stun you like the original game
                }
            };
            On.RainWorldGame.ShutDownProcess += RainWorldGameOnShutDownProcess; // idk
            On.GameSession.ctor += GameSessionOnctor; // idk
            
            IsInit = true; // idk
        }
        catch (Exception ex) // log probably idk
        {
            Logger.LogError(ex);
            throw;
        }
    }
    // ok this one I did
    // Problem is when you initialise, by referring to the remix menu option directly
    // Your change will not be affected because it's already permanently written into the code
    // So I think of a method
    // To dynamically referring this single function I wrote
    // That everytime a stun is triggered, it will call this function to determine whether you have the mod enabled
    
    // There's prob better ways than doing this, but I find this method myself and Im very proud >:3
    private bool outerScopeTester()
    {
        return options.enable_remove_stun.Value;
    }
    // [End]
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
