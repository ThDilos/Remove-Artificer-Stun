using Menu.Remix.MixedUI;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Text;
using System.Diagnostics;
using Menu.Remix.MixedUI.ValueTypes;

namespace RemoveArtiStun
{
    public class MenuTab : OptionInterface
    {
        // Declare toggle-able elements
        public readonly Configurable<bool> remove_global_stun;
        public readonly Configurable<bool> remove_explosion_stun;
        public readonly Configurable<bool> remove_electric_stun;
        public readonly Configurable<bool> remove_blunt_stun;
        public readonly Configurable<bool> apply_to_exp_perk;

        // CheckBoxes Modification
        private OpCheckBox exp;
        private OpCheckBox ele;
        private OpCheckBox blu;
        private OpCheckBox glo;

        public MenuTab(RemoveArtiStun plugin) 
        // Register toggle-able elements
        // I have no idea what is string Key in Configurable. I just anyhow fill in keys. But most definitely you cannot repeat them
        {
            remove_global_stun = this.config.Bind<bool>("removeartistun_remove_global_stun", false);
            remove_explosion_stun = this.config.Bind<bool>("removeartistun_remove_explosion_stun", true);
            remove_electric_stun = this.config.Bind<bool>("removeartistun_remove_electric_stun", false);
            remove_blunt_stun = this.config.Bind<bool>("removeartistun_remove_blunt_stun", false);
            apply_to_exp_perk = this.config.Bind<bool>("removeartistun_apply_to_exp_perk", false);
        }
        public override void Initialize()
        {
            // base.Initialize(); // DO NOT WRITE THIS LINE, BERAK EVERYTHING
            OpTab leTab = new OpTab(this, "Silly Tab Name That youre not gonna see :3"); // Declare a OpTab, the String is only useful if you have multiple tabs.
            this.Tabs = new OpTab[] // Some how you need to do this? this.Tabs = new OpTab[] {(Your OpTabs)};
            {
                leTab
            };

            exp = new OpCheckBox(remove_explosion_stun, 270f, 470f);
            ele = new OpCheckBox(remove_electric_stun, 270f, 440f);
            blu = new OpCheckBox(remove_blunt_stun, 270f, 410f);
            glo = new OpCheckBox(remove_global_stun, 270f, 380f);

            // Use an array for better management i guess
            UIelement[] UIarrayOptions = new UIelement[]
            {
                new OpLabel(200f, 550f, "Mod Setting frfr", true),
                new OpLabel(55f, 500f, "Remove Stun from damage type:", true),
                new OpLabel(55f, 470f, "- Explosives", false), // Not for other slugcats in Exp mode
                exp,
                new OpLabel(55f, 440f, "- Electricity (Thrown JellyFish)", false), // JellyFish on water / Craft Electric Spear / Holding Small Centipede is not included :(
                ele,
                new OpLabel(55f, 410f, "- Blunt Force (Rock)", false),
                blu,
                new OpLabel(55f, 380f, "- EVERYTHING (Override above 3)", false), // Includes Literally Everything
                glo,

                new OpLabel(55f, 350f, "Apply the above to Expedition Explosive Immunity Perk"),
                new OpCheckBox(apply_to_exp_perk, 390f, 347f),

                new OpLabel(20f, 250f, "Side Note: The above damage type only consider those that do damages, which means:" +
                "\nHarmless stuns like \"Holding a small centipede\" or \"Crafting an electric spear\"" +
                "\nwill still stun you. (Because no damage)"),
                new OpLabel(20f, 190f, "Also, disappointingly, other slugcats in Expedition will still take Explosion Stun even with the Explosion\nImmunity Perk," +
                " the stun function is hard coded into the basic explosion.", false),
                new OpLabel(20f, 160f, "I've attempted to modify it but failed horribly. (Skill issue)", false),
                new OpLabel(20f, 140f, "Of course if you ticked EVERYTHING, you'll never be stunned (except when grabbed by other creature).", false),

                new OpLabel(new UnityEngine.Vector2(200f,100f),new UnityEngine.Vector2(5f,5f), "random floating label", FLabelAlignment.Center, false, null),
                new OpLabel(new UnityEngine.Vector2(300f,10f),new UnityEngine.Vector2(5f,5f), "Bottom Text", FLabelAlignment.Center, false, null),
                new OpLabel(new UnityEngine.Vector2(530f,336f),new UnityEngine.Vector2(5f,5f), ":3", FLabelAlignment.Center, false, null),
            };
            leTab.AddItems(UIarrayOptions);
        }

        private void OptionsMenu_OnConfigChanged()
        {
            throw new System.NotImplementedException();
        }

        public override void Update() // Override the 3 if the big one is toggled on
        {
            if (glo.GetValueBool())
            {
                exp.SetValueBool(false);
                ele.SetValueBool(false);
                blu.SetValueBool(false);
                exp.greyedOut = true;
                ele.greyedOut = true;
                blu.greyedOut = true;
            }
            else
            {
                exp.greyedOut = false;
                ele.greyedOut = false;
                blu.greyedOut = false;
            }
        }
    }
}