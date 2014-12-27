using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProvincePanelScript : PanelScript {

    private GameObject panel;
    private RectTransform panelRect;
    private RawImage banner;
    private Text textArea;

	// Use this for initialization
	void Start () {
        panel = gameObject;
        panelRect = (RectTransform)panel.transform;
        banner = panelRect.FindChild("OwnerBanner").GetComponent<RawImage>();
        textArea = panelRect.FindChild("Text").GetComponent<Text>();
	}
	
	// called by GUIScript, once per frame
    override internal bool _update()
    {
        return contains(panelRect, Input.mousePosition);
	}

    internal void setInformation(Province prov)
    {
        banner.texture = prov.owner.banner;
        textArea.text = prov.owner.name;
    }
}
