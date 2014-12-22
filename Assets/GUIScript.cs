﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GUIScript : MonoBehaviour {

	private Texture2D[] banner;
	private Texture2D guiTexture;
	private byte[,] mapMatrix;
	private float mapWorldWidth, mapWorldHeight, mapScreenWidth, mapScreenHeight;
	private int currentSelected = 0;
	private Rect guiRect, bannerRect;
	public GUIStyle style;
	Vector2 scrollPosition;
	// Use this for initialization
	void Start () {
		//processProvincesBmp ();
		readLordsInfo ();
		int[] dim = new int[2];
		mapMatrix = readMapMatrix (dim);
		mapScreenWidth = dim [0];//screen coordinates
		mapScreenHeight = dim [1];
		GameObject map = GameObject.Find ("Map");
		mapWorldWidth = map.renderer.bounds.size.x;//world coordinates
		mapWorldHeight = map.renderer.bounds.size.y;
		guiTexture = Resources.Load("parchment_texture") as Texture2D;

		// Initialize texture rectangles, proportional to screen size
		guiRect = new Rect (0, Screen.height - 255, Screen.width, 255);
		bannerRect = new Rect (0, Screen.height - 255, 255, 255);
	}

	void OnGUI() {
		if (currentSelected != 0) {
			GUI.DrawTexture (guiRect, guiTexture);
			GUI.DrawTexture (bannerRect, banner [currentSelected - 1]);
			//GUI.Box(new Rect(300, Screen.height-255, 255, 255), "Testing this yonder text area.   "+banner.ToString()+" "+banner+"- each lord can raise from a province: Prov.manpower * Prov.percentOwnedBy(A) soldiers; (evidently, this decreases tax income*)\n	- you can demobilise a militia anywhere, but if done outside your lands, a portion of your troops (proportional to the distance to your home province) will not reapear in their home province's manpower. (for coding purposes, we will always keep the recruits from province A into unit A) Regardless, you cannot raise levies again for two weeks.\n	- lords can also maintain a retinue of knights - creating 1 knight costs 1 gold; recruited units appear in a new unit in the Lord's home province", style);
			/*GUILayout.BeginArea (new Rect(300, Screen.height-255, 255, 255));
			scrollPosition = GUILayout.BeginScrollView (scrollPosition, GUILayout.Width (255), GUILayout.Height (255));
			/*changes made in the below 2 lines */
			/*GUILayout.Box("a\nb\n;c\nd\ne\nf\ng\nh\ni\nj\nk\nl\nm\nk\nnn\nx\ny\nz\nq\nr\np\no\nu\nt"); // just your message as parameter.
			
			GUILayout.EndScrollView ();
			
			GUILayout.EndArea();*/
			GUI.DrawTexture(new Rect(300,Screen.height - 255, 225, 225), banner[currentSelected-1]);
			GUI.DrawTexture(new Rect(600,Screen.height - 255, 200, 200), banner[currentSelected-1]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//button values are 0 for left button, 1 for right button, 2 for the middle button.
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mouseWorldPoint = Camera.main.camera.ScreenToWorldPoint(Input.mousePosition);
			int x = (int)(((mouseWorldPoint.x + mapWorldWidth/2) / mapWorldWidth) * mapScreenWidth);
			int y = (int)(((-mouseWorldPoint.y + mapWorldHeight/2) / mapWorldHeight) * mapScreenHeight);
			currentSelected = mapMatrix[y, x];
		}
	}

	private void readLordsInfo() {
		StreamReader reader = new StreamReader ("Assets/lords.txt");
		int n = Convert.ToInt32(reader.ReadLine ());
		banner = new Texture2D[n];
		for (int i = 0; i < n; i++) {
			banner[i] = Resources.Load("banners/" + reader.ReadLine()) as Texture2D;
		}
	}

	public static byte[,] readMapMatrix(int[] dimensions)
	{
		BinaryReader file = new BinaryReader(new FileStream("Assets/map.bin", FileMode.Open));
		int w = file.ReadInt32();
		int h = file.ReadInt32();
		dimensions[0] = w;
		dimensions[1] = h;
		int p = 0;
		byte[,] mapMatrix = new byte[h, w];
		byte[] data = file.ReadBytes(h * w);
		for (int i = 0; i < h; i++)
			for (int j = 0; j < w; j++)
				mapMatrix [i, j] = data [p++];
		return mapMatrix;
	}

	/*  functia creeaza mapMatrix care contine informatii despre carei provincii ii apartine pixelul i, j
		si apoi scrie matricea in fisierul map.bin
		pt a determina acest lucru, parcurgem bmp-ul pixel cu pixel
		fiecarei culori diferite intalnite ii asignam un nr diferit si fiecarui pixel ii asignam nr corespunzator
		astfel, dintr-o matrice de culori => o matrice de id-uri de provincii
		(PS: id-ul descoperit aici va trebui sa fie asignat provinciei in provinces.txt) */
	private void processProvincesBmp() {
		Texture2D texture = Resources.Load("provinces") as Texture2D;
		Color[] pixels = texture.GetPixels ();
		List<Color> colors = new List<Color>();
		int w = texture.width;
		int h = texture.height;
		int p = 0;//the current position in the pixels array
		byte[,] mapMatrix = new byte[h, w];
		for (int y = 0; y < h; y++) {//lines
			for (int x = 0; x < w; x++) {//columns
				Color c = pixels[p++];
				int k = colors.IndexOf(c);
				if (k == -1)
				{//am descoperit culoare noua => o adaugam la lista (cu un index/id mai mare, evident)
					//Debug.Log("new color at " + x + " " + (h-1-y) + " " + c);
					k = colors.Count;
					colors.Add(c);
				}
				// WARNING! we write at h-1-y, because Unity reads a bmp from the bottom
				// as opposed to the normal way, reading from the top
				mapMatrix[h-1-y, x] = (byte)k;
			}
		}
		BinaryWriter file = new BinaryWriter(new FileStream("Assets/map.bin", FileMode.OpenOrCreate));
		file.Write(w);
		file.Write(h);
		for (int i = 0; i < h; i++)
			for (int j = 0; j < w; j++)
				file.Write(mapMatrix[i, j]);
	}
}