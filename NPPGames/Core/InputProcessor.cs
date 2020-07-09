using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using static Kbg.NppPluginNET.PluginInfrastructure.Win32.Win32Keyboard;

namespace NPPGames.Core
{
    public class InputProcessor
    {
        public InputProcessor()
        {
            Keys = new List<VirtualKeyStates>();
            ProcessedKeys = new List<VirtualKeyStates>();
        }
        public List<VirtualKeyStates> Keys { get; set; }
        private List<VirtualKeyStates> ProcessedKeys { get; set; }

        public static List<VirtualKeyStates> ProcessInputForMessagePupmpedApplication()
        {
            byte[] keys = new byte[256];

            if(Win32.GetKeyboardState(keys))
            {
                List<VirtualKeyStates> downKeys = new List<VirtualKeyStates>();
                
                foreach (var ckey in Enum.GetValues(typeof(VirtualKeyStates)))
                {
                    var selectedKey = keys[(int)ckey];
                    if((selectedKey & 0x80) != 0)
                    {
                        downKeys.Add((VirtualKeyStates)ckey);
                    }
                }

                if(downKeys.Count > 0)
                {
                    return downKeys;
                }
            }
            return null;
        }

        public static List<VirtualKeyStates> ProcessInput()
        {
            List<VirtualKeyStates> downKeys = new List<VirtualKeyStates>();
            foreach (var ckey in Enum.GetValues(typeof(VirtualKeyStates)))
            {
                var selectedKey = Win32.Win32Keyboard.GetAsyncKeyState((VirtualKeyStates)ckey);
                if ((selectedKey & 0x80) != 0)
                {
                    downKeys.Add((VirtualKeyStates)ckey);
                }
            }

            return null;
        }

        public ProcessKeysResult ProcessKeys(List<VirtualKeyStates> newKKeys)
        {
            var newKeysDown = ProcessedKeys.Except(newKKeys);
            var newKeysUp = newKKeys.Except(ProcessedKeys);

            ProcessedKeys = new List<VirtualKeyStates>(newKKeys);

            return new ProcessKeysResult()
            {
                NewKeysDown = newKeysDown,
                NewKeysUp = newKeysUp
            };

        }
    }

    public class ProcessKeysResult
    {
        public IEnumerable<VirtualKeyStates> NewKeysDown { get; set; }
        public IEnumerable<VirtualKeyStates> NewKeysUp { get; set; }
    }
}
