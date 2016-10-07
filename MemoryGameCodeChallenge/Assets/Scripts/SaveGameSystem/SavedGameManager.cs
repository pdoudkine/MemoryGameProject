using UnityEngine;
using System.Collections;
using System.IO;

//class that handles saved games
public class SavedGameManager : MonoBehaviour {

	public static SavedGameManager instance;
	public bool startWithLoad = false;
	public bool savedGameExists;
	FileInfo gameFile;
	//at game launch, check to see if we have a saved game
	void Start()
	{
		if (instance != null)
			Destroy (instance);
		instance = this;
		if (CheckSavedGame ())
			savedGameExists = true;
		else
			savedGameExists = false;
	
	}
	//searches directory for a save game file then returns wether it exists or not
	private bool CheckSavedGame()
	{
		DirectoryInfo dirInfo = new DirectoryInfo (Application.persistentDataPath); 
		FileInfo[] files = dirInfo.GetFiles ("*.txt");
		foreach (FileInfo file in files) {
			if (file.Name == "GIP.txt") {
				gameFile = file;
				return true;
			}
		}
		return false;

	}

	public void DeleteSavedGame()
	{
		if (gameFile != null)
			startWithLoad = false;
		startWithLoad = false;
	}

	public void SetStartWithLoad()
		{
			startWithLoad = true;
		}

}


