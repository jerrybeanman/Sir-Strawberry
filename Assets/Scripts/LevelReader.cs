using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class LevelReader : MonoBehaviour {
	public static LevelReader instance;
	public string[][] Level;
	private string Difficulty;
	private string Map;
	private int[] maps;

	void Awake()
	{
		//singleton design pattern
		if(instance == null)
			instance = this;
		else if(instance != this)
			Destroy (gameObject);
		
		//since player inventory should persist over scenes, this object should not be destroyed on load
		DontDestroyOnLoad(gameObject);
	}
	
	// Reads our level text file and stores the information in a jagged array, then returns that array
	string[][] readFile(TextAsset t){
		string text = t.text;
		string[] lines = Regex.Split(text, "\r\n");
		int rows = lines.Length;
		
		string[][] levelBase = new string[rows][];
		for (int i = 0; i < lines.Length; i++)  {
			string[] stringsOfLine = Regex.Split(lines[i], " ");
			levelBase[i] = stringsOfLine;
		}
		return levelBase;
	}

	public void setLevel() {
		TextAsset text = (TextAsset)Resources.Load ("level1", typeof(TextAsset));				//Load the file from the Resources folder
		Level = readFile (text);		//Read the text file and assign back into two dimensional array
	}
	
	
	void Start(){
		//Instantiate (canvasUI);
		//Instantiate (world);
	}
}
