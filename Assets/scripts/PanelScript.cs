﻿using UnityEngine;
using System.Collections;

public class PanelScript : MonoBehaviour {

    private GameObject panel;
    private RectTransform textureRect;
    private RectTransform closeButton;
    internal bool isDragging = false;
    private Vector3 lastMousePosition;

	// Use this for initialization
	void Start () {
        panel = gameObject;
        textureRect = (RectTransform)panel.transform;
        closeButton = (RectTransform)panel.transform.GetChild(2).transform;
	}
	
	// we need this to be called at precise moments, so we cannot rely on the default Update function
	internal void _update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 debugWorldClick = Camera.main.camera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(debugWorldClick);
            if (panel != null)
            {
                if (contains(closeButton, Input.mousePosition))
                {
                    panel.SetActive(false);// hide panel
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