using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    
	void Update () {
#if UNITY_IOS
        //HandleTouchscreenInput();
#endif
        
#if UNITY_EDITOR
        HandleEditorInputs();
#endif
    }

    void HandleTouchscreenInput()
    {
        // TODO: Handle touch input for iphone
        throw new System.Exception("UNIMPLEMENTED: You must handle the touchscreen input!");
    }

    void HandleEditorInputs()
    {
        // If mouse clicked on screen
        if (Input.GetMouseButtonDown(0))
        {
            ScreenTap(Input.mousePosition);
        }
    }

    void ScreenTap(Vector2 tapPosition)
    {
        RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(tapPosition), Vector2.zero);

        if (ray.collider != null)
        {
            Tile tile = ray.collider.GetComponent<Tile>();
            if (tile != null)
            {
                GameplayManager.Inst.TileTap(tile);
            }
        }
    }
}
