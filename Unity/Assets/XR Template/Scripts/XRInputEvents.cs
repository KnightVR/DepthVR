using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class XRInputEvents : MonoBehaviour
{
    public UnityEvent leftPrimaryPressed;
    public UnityEvent leftSecondaryPressed;
    public UnityEvent rightPrimaryPressed;
    public UnityEvent rightSecondaryPressed;
    //TODO add release events

    private bool lastLeftPrimaryPressing = false;
    private bool lastRightPrimaryPressing = false;
    private bool lastLeftSecondaryPressing = false;
    private bool lastRightSecondaryPressing = false;

    enum ButtonState { unchanged, released, pressed , error};

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice leftController, rightController;
        bool leftControllerFound = FindFirstDevice(out leftController, InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller);
        bool rightControllerFound = FindFirstDevice(out rightController, InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller);
        if (leftControllerFound)
        {
            InvokeControllerEvents(leftController, ref lastLeftPrimaryPressing, ref lastLeftSecondaryPressing, leftPrimaryPressed, leftSecondaryPressed);
        }
        if (rightControllerFound)
        {
            InvokeControllerEvents(rightController, ref lastRightPrimaryPressing, ref lastRightSecondaryPressing, rightPrimaryPressed, rightSecondaryPressed);
        }
    }

    void InvokeControllerEvents(InputDevice controller, ref bool lastPrimaryPressing, ref bool lastSecondaryPressing, in UnityEvent primaryEvent, in UnityEvent secondaryEvent)
    {
        /// <summary>
        /// Invoke Unity events for controller
        /// </summary>
        /// <param controller>
        ///     Input device to read states from
        /// </param>
        /// <param lastPrimaryPressing>
        ///     Previous state of primary button (should be global to keep track of button state between frames)
        /// </param>
        /// <param lastSecondaryPressing>
        ///     Previous state of secondary button (should be global to keep track of button state between frames)
        /// </param>
        /// <param primaryEvent>
        ///     Event to invoke when primary button is pressed
        /// </param>
        /// <param secondaryEvent>
        ///     Event to invoke when secondary button is pressed
        /// </param>

        ButtonState primaryButtonState = GetControllerButtonPressed(controller, CommonUsages.primaryButton, ref lastPrimaryPressing);
        ButtonState secondaryButtonState = GetControllerButtonPressed(controller, CommonUsages.secondaryButton, ref lastSecondaryPressing);
        if (primaryButtonState == ButtonState.pressed)
            primaryEvent.Invoke();
        if (secondaryButtonState == ButtonState.pressed)
            secondaryEvent.Invoke();
    }

    ButtonState GetControllerButtonPressed(InputDevice controller, InputFeatureUsage<bool> button, ref bool previousButtonPressing)
    {
        /// <summary>
        /// Gets state of button on controller (only if state has changed from false->true this frame)
        /// </summary>
        /// <param inputDevice>
        ///     Input device to read button from
        /// </param>
        /// <param button>
        ///     Button on controller to read
        ///     For example the primary button:
        ///         CommonUsages.primaryButton
        /// </param>
        /// <param previousButtonState>
        ///     Previous state of button (should be global to keep track of button state between frames)
        /// </param>
        /// <returns>state of button: 0 = state not changed, 1 = pressed, 2 = released </returns>

        bool buttonPressing;
        ButtonState newButtonState = ButtonState.error;
        if (controller.TryGetFeatureValue(button, out buttonPressing))
        {
            if (buttonPressing != previousButtonPressing)
            {
                if (buttonPressing)
                {
                    newButtonState = ButtonState.pressed;
                } else
                {
                    newButtonState = ButtonState.released;
                }
            } else
            {
                newButtonState = ButtonState.unchanged;
            }
            previousButtonPressing = buttonPressing;
        }
        else
        {
            Debug.Log("Failed to get feature value for controller");
        }
        return newButtonState;
    }

    bool FindFirstDevice(out InputDevice inputDevice, InputDeviceCharacteristics characteristics)
    {
        /// <summary>
        /// Finds the first device that matches the specifed characterists
        /// </summary>
        /// <param inputDevice>
        ///     Input device to write to (should be ignored if this function returns false)
        /// </param>
        /// <param characteristics>
        ///     Characterists device must match (see example below for details).
        ///     For example the characterists for left controller: 
        ///         InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller
        /// </param>
        /// <returns>true if at least one controller is found</returns>

        List<InputDevice> inputDevices = new List<InputDevice>();
        bool devicesFound = FindDevices(out inputDevices, characteristics);
        if (devicesFound)
        {
            inputDevice = inputDevices[0];
        }
        else
        {
            inputDevice = new InputDevice();
            Debug.Log("No device found with the characterists specified");
        }
        return devicesFound;
    }

    bool FindDevices(out List<InputDevice> inputDevices, InputDeviceCharacteristics characteristics)
    {
        /// <summary>
        /// Finds the first device that matches the specifed characterists
        /// </summary>
        /// <param inputDevices>
        ///     List of Input devices to write to (should be ignored if this function returns false)
        /// </param>
        /// <param characteristics>
        ///     Characterists devices must match (see example below for details).
        ///     For example the characterists for left controller: 
        ///         InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller
        /// </param>
        /// <returns>true if at least one controller is found</returns>

        inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(characteristics, inputDevices);
        bool devicesFound = inputDevices.Count > 0;
        if (!devicesFound)
            Debug.Log("No devices found with the characterists specified");
        return devicesFound;
    }
}
