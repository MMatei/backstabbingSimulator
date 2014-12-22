using UnityEngine;
using System.Collections;

public class CharacterPanelScript : MonoBehaviour {

    private GameObject panel;
    private RectTransform textureRect;
    private RectTransform closeButton;
    internal bool isDragging = false;
    private Vector3 lastMousePosition;

	// Use this for initialization
	void Start () {
        panel = GameObject.Find("CharacterPanel");
        textureRect = (RectTransform)panel.transform;
        closeButton = (RectTransform)GameObject.Find("Button").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (panel != null)
            {
                if (contains(closeButton, Input.mousePosition))
                {
                    Destroy(panel);
                    return;
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
	}

    private bool contains(RectTransform rect, Vector3 point)
    {
        if (rect.position.x <= point.x && rect.position.y <= point.y &&
            rect.position.x + rect.rect.width >= point.x &&
            rect.position.y + rect.rect.height >= point.y)
            return true;
        return false;
    }
}
