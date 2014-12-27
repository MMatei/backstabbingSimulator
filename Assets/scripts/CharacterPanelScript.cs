using UnityEngine;
using System.Collections;

public class CharacterPanelScript : PanelScript {

    private GameObject panel;
    private RectTransform textureRect;
    private RectTransform closeButton;
    internal bool isDragging = false;
    private Vector3 lastMousePosition;

	// Use this for initialization
	void Start () {
        panel = gameObject;
        textureRect = (RectTransform)panel.transform;
        closeButton = (RectTransform)panel.transform.FindChild("CloseButton").transform;
	}
	
	// called by GUIScript, once per frame
	override internal bool _update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (panel != null)
            {
                if (contains(closeButton, Input.mousePosition))
                {
                    panel.SetActive(false);// hide panel
                    return true;
                }
                if (contains(textureRect, Input.mousePosition))
                {
                    isDragging = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
            isDragging = false;
        if (isDragging)
        {
            Vector3 pos = textureRect.position;
            pos += (Input.mousePosition - lastMousePosition);
            textureRect.position = pos;
        }
        lastMousePosition = Input.mousePosition;
        return isDragging;
	}
}
