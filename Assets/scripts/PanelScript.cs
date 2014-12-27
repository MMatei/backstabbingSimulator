using UnityEngine;
using System.Collections;

/* 
 * An abstract master class for all panels; all panels must implement _update,
 * so that GUIScript can call it before doing its own things.
 */
public abstract class PanelScript : MonoBehaviour {

    // must return whether the mouse click activated anything in the panel
    // so that GUIScript knows to ignore that click
    internal abstract bool _update();

    // check if mouse pointer inside rectangle
    protected bool contains(RectTransform rect, Vector3 point)
    {
        if (rect.position.x <= point.x && rect.position.y <= point.y &&
            rect.position.x + rect.rect.width >= point.x &&
            rect.position.y + rect.rect.height >= point.y)
            return true;
        return false;
    }
}
