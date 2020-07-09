using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using static Kbg.NppPluginNET.PluginInfrastructure.Win32.Win32Keyboard;

namespace NPPGames.Core
{
    public class Keyboard
    {
        public Keyboard()
        {
            CurrentKeys = new List<VirtualKeyStates>();
            KeyListeners = new List<VirtualKeyStates>();
            InputProcessor = new InputProcessor();
        }
        private readonly InputProcessor InputProcessor;
        public void ListenToAllKeys()
        {
            foreach (var ckey in Enum.GetValues(typeof(VirtualKeyStates)))
            {
                KeyListeners.Add((VirtualKeyStates)ckey);
            }
        }
        public void ListenTo(VirtualKeyStates keyStates)
        {
            if (!KeyListeners.Contains(keyStates))
                KeyListeners.Add(keyStates);
        }
        private List<VirtualKeyStates> KeyListeners { get; set; }

        private List<VirtualKeyStates> CurrentKeys { get; set; }

        private ProcessKeysResult ProcessedKeys;

        public bool IsKeyDown(VirtualKeyStates key)
        {
            /*
              if (null != CurrentKeys)
                 return CurrentKeys.Contains(key);
             return false;
             */
            if (null != CurrentKeys)
                return CurrentKeys.Contains(key);
            return false;
        }

        public bool IsNewKeyDown(VirtualKeyStates key)
        {
            /*
              if (null != CurrentKeys)
                 return CurrentKeys.Contains(key);
             return false;
             */
            if (null != ProcessedKeys)
                return ProcessedKeys.NewKeysDown.Contains(key);
            return false;
        }

        public IEnumerable<VirtualKeyStates> GetNewKeysDown()
        {
            return ProcessedKeys?.NewKeysDown;
        }

        public void Update()
        {
            //CurrentKeys = InputProcessor.ProcessInput();
            //if(null != CurrentKeys)
            //{

            //}
            CurrentKeys.Clear();
            foreach (var keycandidate in KeyListeners)
            {
                if (Win32.Win32Keyboard.IsKeyPressed(keycandidate))
                    CurrentKeys.Add(keycandidate);
            }
            ProcessedKeys = InputProcessor.ProcessKeys(CurrentKeys);
        }
    }
}
