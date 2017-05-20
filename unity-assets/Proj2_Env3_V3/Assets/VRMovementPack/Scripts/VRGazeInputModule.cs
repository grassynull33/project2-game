using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

// To use:
// 1. Drag onto your EventSystem game object.
// 2. Move above any other Input Modules (eg: StandaloneInputModule & TouchInputModule) as they will fight over selections.
public class VRGazeInputModule : PointerInputModule {
    public RaycastResult CurrentRaycast;
    private PointerEventData pointerEventData;
    private GameObject currentLookAtHandler;

    public override void Process() {
        HandleLook();
        HandleSelection();
    }

    void HandleLook() {
        if (pointerEventData == null) {
            pointerEventData = new PointerEventData(eventSystem);
        }
        // fake a pointer always being at the center of the screen
        pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);
        pointerEventData.delta = Vector2.zero;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, raycastResults);
        CurrentRaycast = pointerEventData.pointerCurrentRaycast = FindFirstRaycast(raycastResults);
        ProcessMove(pointerEventData);
    }

    void HandleSelection() {
        if (pointerEventData.pointerEnter != null) {
            // if the ui receiver has changed, reset the gaze delay timer
            GameObject handler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(pointerEventData.pointerEnter);
            if (currentLookAtHandler != handler) {
                currentLookAtHandler = handler;
            }

            // if we have a handler and it's time to click, do it now
            if (currentLookAtHandler != null && (Input.GetButtonDown("Fire1"))) {
                ExecuteEvents.ExecuteHierarchy(currentLookAtHandler, pointerEventData, ExecuteEvents.pointerDownHandler);
                ExecuteEvents.ExecuteHierarchy(currentLookAtHandler, pointerEventData, ExecuteEvents.pointerUpHandler);
                ExecuteEvents.ExecuteHierarchy(currentLookAtHandler, pointerEventData, ExecuteEvents.pointerClickHandler);
            }
        }
        else {
            currentLookAtHandler = null;
        }
    }
}