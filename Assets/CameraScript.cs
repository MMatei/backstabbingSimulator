using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	// Screen coordinates ( (0, 0) to (screenW, screenH) )
	private int screenMarginTop, screenMarginLeft, screenMarginBottom, screenMarginRight;
	// Below - scene coordinates - the map and camera begin centered in (0, 0)
	private float speed = 2f;//the camera's movement speed
	private float verticalRadius, horizontalRadius;//the distance from camera center to its bounds
	private float cameraBoundTop, cameraBoundLeft, cameraBoundBottom, cameraBoundRight;//the coords of the map bounds

	// Use this for initialization
	void Start () {
		// These are the margins in which the mouse pointer has to be in order
		// to scroll in that particular direction
		screenMarginTop = (int)(Screen.height * 0.1);
		screenMarginLeft = (int)(Screen.width * 0.1);
		screenMarginBottom = (int)(Screen.height * 0.9);
		screenMarginRight = (int)(Screen.width * 0.9);

		// Get camera radius
		verticalRadius = Camera.main.camera.orthographicSize;
		horizontalRadius = verticalRadius * Screen.width / Screen.height;
		// Get map bounds and compute the camera bounds
		// basically, it's the edge of the map, minus the radius the camera covers
		GameObject map = GameObject.Find ("Map");
		cameraBoundTop = map.renderer.bounds.size.y / 2 - verticalRadius;
		cameraBoundBottom = -cameraBoundTop;
		cameraBoundRight = map.renderer.bounds.size.x / 2 - horizontalRadius;
		cameraBoundLeft = - cameraBoundRight;
	}

	// Less than update
	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
	}
	// Update is called once per frame (which can mean lots of times)
	void Update () {
		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;
		Vector3 pos = transform.position;
		if (mouseX < screenMarginLeft)
			pos.x -= speed * Time.deltaTime;
		if (mouseX > screenMarginRight)
			pos.x += speed * Time.deltaTime;
		if (mouseY < screenMarginTop)
			pos.y -= speed * Time.deltaTime;
		if (mouseY > screenMarginBottom)
			pos.y += speed * Time.deltaTime;
		// Ensure that the camera does not exceed the bounds
		pos.x = Mathf.Clamp (pos.x, cameraBoundLeft, cameraBoundRight);
		pos.y = Mathf.Clamp (pos.y, cameraBoundBottom, cameraBoundTop);
		transform.position = pos;
	}
}
