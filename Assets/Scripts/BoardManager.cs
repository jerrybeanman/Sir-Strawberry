using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
	public Transform FloorTop;
	public Transform FloorTopLeft;
	public Transform FloorTopRight;
	public Transform FloorMiddle;
	public Transform FloorLeft;
	public Transform FloorRight;
	public Transform FloorBottom;
	public Transform FloorBottomRight;
	public Transform FloorBottomLeft;

	private const string ft = "8";
	private const string ftl = "7";
	private const string ftr = "9";
	private const string fm = "5";
	private const string fl = "4";
	private const string fr = "6";
	private const string fb = "2";
	private const string fbr = "3";
	private const string fbl = "1";

	private float height;
	private float width;


	void Awake() {
		LevelReader.instance.setLevel();
	}

	// Use this for initialization
	void Start () {
		height = LevelReader.instance.Level.Length;
		width = LevelReader.instance.Level[0].Length;
		createLevel ();
	}

	void createLevel() {
		for (int z = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				switch (LevelReader.instance.Level[z] [x]) {
				case ft:
					Instantiate (FloorTop, new Vector3 (x, -z, 0f), Quaternion.identity);
					break;
				case ftl:
					Instantiate (FloorTopLeft, new Vector3 (x, -z, 0f), Quaternion.identity);
					break;
				case ftr:
					Instantiate (FloorTopRight, new Vector3 (x, -z, 0f), Quaternion.identity);
					break;
				case fm:
					Instantiate (FloorMiddle, new Vector3 (x, -z, 0f), Quaternion.identity);
					break;
				case fl:
					Instantiate (FloorLeft, new Vector3 (x, -z, 0f), Quaternion.identity);
					break;
				case fr:
					Instantiate (FloorRight, new Vector3 (x, -z, 0f), Quaternion.identity);
					break;
				case fb:
					Instantiate (FloorBottom, new Vector3 (x, -z, 0f), Quaternion.identity);
					break;
				case fbr:
					Instantiate (FloorBottomRight, new Vector3 (x, -z, 0f), Quaternion.identity);
					break;
				case fbl:
					Instantiate (FloorBottomLeft, new Vector3 (x, -z, 0f), Quaternion.identity);
					break;
				}
			}
		}
	}
}
