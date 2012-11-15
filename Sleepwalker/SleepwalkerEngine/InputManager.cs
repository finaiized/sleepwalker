using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace SleepwalkerEngine
{
    /// <summary>
    /// Handles input. 
    /// 
    /// This class provides an easy way to query the state of key bindings, which extend over keys by allowing multiple bindings to an event.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// The list of key bindings.
        /// </summary>
        private List<KeyBindings> keyBindings = new List<KeyBindings>();

        /// <summary>
        /// Add a key binding to the input manager.
        /// </summary>
        /// <param name="name">The name of the key binding to add</param>
        public void AddKeyBinding(string name)
        {
            keyBindings.Add(new KeyBindings(name));
        }

        /// <summary>
        /// Get the key binding with the provided name.
        /// </summary>
        /// <param name="name">The name of the key binding.</param>
        /// <returns>A key binding with the provided name.</returns>
        public KeyBindings this[string name]
        {
            get
            {
                return keyBindings.Find(a => a.Name == name);
            }
        }

        /// <summary>
        /// Update the input manager.
        /// </summary>
        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            foreach (var binding in keyBindings)
            {
                binding.Update(keyState, mouseState);
            }
        }
    }
}
