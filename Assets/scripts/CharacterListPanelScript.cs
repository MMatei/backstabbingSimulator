using UnityEngine;
using System.Collections;

public class CharacterListPanelScript : PanelScript
{

    private GameObject panel;
    private RectTransform textureRect;
    private RectTransform closeButton;
    private RectTransform scrollView, panelSV, arrynButton, baratheonButton, greyjoyButton, lannisterButton, martellButton, starkButton, tyrellButton;
    internal bool isDragging = false;
    private Vector3 lastMousePosition;

    // Use this for initialization
    void Start()
    {
        panel = gameObject;
        textureRect = (RectTransform)panel.transform;
        closeButton = (RectTransform)panel.transform.FindChild("CloseButton").transform;
        scrollView = (RectTransform)panel.transform.FindChild("ScrollView").transform;
        panelSV = (RectTransform)scrollView.transform.FindChild("Panel").transform;

        arrynButton = (RectTransform)panelSV.transform.FindChild("Arryn").transform;
        baratheonButton = (RectTransform)panelSV.transform.FindChild("Baratheon").transform;
        greyjoyButton = (RectTransform)panelSV.transform.FindChild("Greyjoy").transform;
        lannisterButton = (RectTransform)panelSV.transform.FindChild("Lannister").transform;
        martellButton = (RectTransform)panelSV.transform.FindChild("Martell").transform;
        starkButton = (RectTransform)panelSV.transform.FindChild("Stark").transform;
        tyrellButton = (RectTransform)panelSV.transform.FindChild("Tyrell").transform;
    }

    // called by GUIScript, once per frame
    override internal bool _update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (panel != null)
            {
                if (contains(closeButton, Input.mousePosition))
                {
                    panel.SetActive(false);// hide panel
                    return true;
                }
                
                if (contains(arrynButton, Input.mousePosition))
                {
                    GUIScript.characterPanel.SetActive(true);// hide panel
                    return true;
                }
                
                if (contains(baratheonButton, Input.mousePosition))
                {
                    GUIScript.characterPanel.SetActive(true);// hide panel
                    return true;
                }
                
                if (contains(greyjoyButton, Input.mousePosition))
                {
                    GUIScript.characterPanel.SetActive(true);// hide panel
                    return true;
                }

                if (contains(lannisterButton, Input.mousePosition))
                {
                    GUIScript.characterPanel.SetActive(true);// hide panel
                    return true;
                }

                if (contains(martellButton, Input.mousePosition))
                {
                    GUIScript.characterPanel.SetActive(true);// hide panel
                    return true;
                }

                if (contains(starkButton, Input.mousePosition))
                {
                    GUIScript.characterPanel.SetActive(true);// hide panel
                    return true;
                }

                if (contains(tyrellButton, Input.mousePosition))
                {
                    GUIScript.characterPanel.SetActive(true);// hide panel
                    return true;
                }
                
                if (contains(textureRect, Input.mousePosition))
                {
                    isDragging = false;
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
