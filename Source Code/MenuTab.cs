using Menu.Remix.MixedUI;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Text;
using System.Diagnostics;

namespace RemoveArtiStun
{
    public class MenuTab : OptionInterface
    {  
        public readonly Configurable<bool> enable_remove_stun;
        private int killCount = 0;
        public MenuTab(RemoveArtiStun plugin) 
        {
            enable_remove_stun = this.config.Bind<bool>("removeartistun_enable_remove_stun", true);
        }
        public override void Initialize()
        {
            OpTab leTab = new OpTab(this, "Silly Tab Name That youre not gonna see :3");
            this.Tabs = new OpTab[]
            {
                leTab
            };

            UIelement[] UIarrayOptions = new UIelement[]
            {
                new OpLabel(200f, 550f, "Mod Setting frfr", true),
                new OpLabel(55f, 500f, "The only tab with the only checkbox controlling the only function :3", false),
                new OpLabel(55f, 480f, "Remove Stun For Artificer", false),
                new OpCheckBox(enable_remove_stun, 220f, 475f),
                new OpLabel(new UnityEngine.Vector2(200f,200f),new UnityEngine.Vector2(5f,5f), "random floating text", FLabelAlignment.Center, false, null),
                new OpLabel(new UnityEngine.Vector2(30f,350f),new UnityEngine.Vector2(5f,5f), ":3", FLabelAlignment.Center, false, null),
            };
            leTab.AddItems(UIarrayOptions);
        }

        private void OptionsMenu_OnConfigChanged()
        {
            throw new System.NotImplementedException();
        }
    }
}