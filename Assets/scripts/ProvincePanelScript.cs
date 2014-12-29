using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProvincePanelScript : PanelScript {

    private GameObject panel;
    private RectTransform panelRect;
    private RectTransform viewLordsButton;
    private RawImage banner;
    private Text textArea;

	// Use this for initialization
	void Start () {
        panel = gameObject;
        panelRect = (RectTransform)panel.transform;
        banner = panelRect.FindChild("OwnerBanner").GetComponent<RawImage>();
        textArea = panelRect.FindChild("Text").GetComponent<Text>();
        viewLordsButton = (RectTransform)panel.transform.FindChild("ViewLordsButton").transform;
	}
	
	// called by GUIScript, once per frame
    override internal bool _update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (/*panel != null*/true) // If province panel is inactive, the character list panel will not be able to create a char panel; need attention
            {
                if (contains(viewLordsButton, Input.mousePosition))
                {
                    GUIScript.characterListPanel.SetActive(true);
                    return true;
                }
            }
        }
        return contains(panelRect, Input.mousePosition);
	}

    internal void setInformation(Province prov)
    {
        banner.texture = prov.owner.banner;
        textArea.text = prov.owner.name;
    }
}
