using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace InputSystemActionPrompts
{
    /// <summary>
    /// Get all bindings for a device and update a list of Images with each binding.
    /// Disable any gameobjects that do no have a corresponding binding.
    /// Eg, For when binding arrow keys, WASD and D-Pad.
    /// </summary>
    public class PromptIconSet : MonoBehaviour
    {
        /// <summary>
        /// This should be the full path, including binding map and action, eg "Player/Move"
        /// </summary>
        [SerializeField] private string m_Action = "Player/Move";
        
        /// <summary>
        /// The image to apply the prompt sprite to
        /// </summary>
        public List<Image> Images;

        [SerializeField] private bool _setNativeSize = true;
        
        void Start()
        {
            RefreshIcons();
            // Listen to device changing
            InputDevicePromptSystem.OnActiveDeviceChanged+= DeviceChanged;
        }
        
        private void OnDestroy()
        {
            // Remove listener
            InputDevicePromptSystem.OnActiveDeviceChanged-= DeviceChanged;
        }
        
        /// <summary>
        /// Called when active input device changed
        /// </summary>
        /// <param name="obj"></param>
        private void DeviceChanged(InputDevice device)
        {
            RefreshIcons();
        }

        /// <summary>
        /// Sets the icon for the current action
        /// </summary>
        private void RefreshIcons()
        {
            var sourceSprites=InputDevicePromptSystem.GetActionPathBindingSprites(m_Action);
            if (sourceSprites == null)
            {
                Images.ForEach(i => i.gameObject.SetActive(false));
                return;
            }

            for (int i = 0; i < Images.Count; i++)
            {
                if (i < sourceSprites.Count)
                {
                    var image = Images[i];
                    // Not the best for canvas performance as enabling/disabling the gameObject will rebuild the canvas
                    // Probs ok though as we'll only be doing this occasionally when the input changes
                    image.gameObject.SetActive(true);
                    image.sprite = sourceSprites[i];
                    if (_setNativeSize)
                        image.SetNativeSize();
                }
                else
                {
                    Images[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
