using static Kbg.NppPluginNET.PluginInfrastructure.Win32.Win32Keyboard;

namespace NPPGames.Core.UserControls
{
    public class Textbox : Sprite
    {
        //const string data = @"|_______________________________________";
        public Textbox(GameData gameData, Scene scene, int width) : base(gameData, scene, width, 1)
        {
            var data = "|".PadRight(width, '_');
            base.SetData(data);
            Value = "";
            Enabled = true;
        }
        public string Value { get; set; }
        public bool Enabled { get; set; }
        public override void Update()
        {
            base.Update();

            if (Enabled)
            {
                var newkeys = GameData.Keyboard.GetNewKeysDown();
                foreach (var key in newkeys)
                {
                    if ((key > VirtualKeyStates.VK_HELP && key < VirtualKeyStates.VK_LWIN))
                        Value = $"{Value}{key.ToString().Replace("VK_", "")}";
                    else if (key == VirtualKeyStates.VK_SPACE)
                        Value += " ";
                    else if (key == VirtualKeyStates.VK_BACK && Value.Length > 0)
                        Value = Value.Substring(0, Value.Length - 1);
                }

                base.SetData((Value + "|").PadRight(Bounds.Size.Width - 1, '_'));
            }
        }
    }
}
