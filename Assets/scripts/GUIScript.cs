using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GUIScript : MonoBehaviour {

    // Game data
	private Province[] province;

	private byte[,] mapMatrix;
	private float mapWorldWidth, mapWorldHeight, mapScreenWidth, mapScreenHeight;
	private int currentSelected = 0;
	Vector2 scrollPosition;

    // --- the UI panels and their associated scripts ---
    internal static GameObject characterPanel, provincePanel, characterListPanel;
    private PanelScript characterPanelScript;
    private ProvincePanelScript provincePanelScript;
    private CharacterListPanelScript characterListPanelScript;

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

        // Find the UI panels
        characterPanel = GameObject.Find("CharacterPanel");
        characterPanelScript = characterPanel.GetComponent<PanelScript>();
        provincePanel = GameObject.Find("ProvincePanel");
        provincePanelScript = provincePanel.GetComponent<ProvincePanelScript>();
        characterListPanel = GameObject.Find("CharacterListPanel");
        characterListPanelScript = characterListPanel.GetComponent<CharacterListPanelScript>();
        characterPanel.SetActive(false);
        provincePanel.SetActive(false);
        characterListPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        // First, we call the updates of the UI panels
        // we do this because we need to know whether the mouse click is located inside a panel
        // if so, we needn't treat that click here
        bool ignoreClick = false;
        ignoreClick |= characterPanelScript._update();
        ignoreClick |= provincePanelScript._update();
        ignoreClick |= characterListPanelScript._update();

		//button values are 0 for left button, 1 for right button, 2 for the middle button.
		if (Input.GetMouseButtonDown (0)) {
            if (!ignoreClick) // clicked on the map
            {
                Vector3 mouseWorldPoint = Camera.main.camera.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)(((mouseWorldPoint.x + mapWorldWidth / 2) / mapWorldWidth) * mapScreenWidth);
                int y = (int)(((-mouseWorldPoint.y + mapWorldHeight / 2) / mapWorldHeight) * mapScreenHeight);
                currentSelected = mapMatrix[y, x];
                Debug.Log(currentSelected);
                if (currentSelected != 0)
                {
                    // show panel and set relevant information
                    provincePanel.SetActive(true);
                    // selected - 1 because the numbering starts from 0, yet province 0 is no man's land / empty
                    provincePanelScript.setInformation(province[currentSelected-1]);
                }
                else
                {// hide panel
                    provincePanel.SetActive(false);
                }
            }
		}
	}

	private void readLordsInfo() {
		StreamReader reader = new StreamReader ("Assets/lords.txt");
		int n = Convert.ToInt32(reader.ReadLine ());
        province = new Province[n];
		for (int i = 0; i < n; i++) {
            String[] piece = reader.ReadLine().Split(';');
            province[i] = new Province();
			province[i].owner = new State(piece[0], Resources.Load("banners/" + piece[1]) as Texture2D);
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
