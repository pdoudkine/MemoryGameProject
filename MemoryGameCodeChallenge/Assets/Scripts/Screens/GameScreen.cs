using UnityEngine;
using System.Collections;
using System.IO;


public class GameScreen : UIScreen
{
	//game screen which instantiates game prefab and manages enabling or disabling the game
	public GameManager gamePrefab;

	string saveName = "/GIP.txt";
	private GameManager currentGame;

	void OnEnable ()
	{
		//create the gameplay prefab, parent it and centre it
		currentGame = Instantiate (gamePrefab);
		currentGame.gameObject.transform.SetParent (this.transform);
		currentGame.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);


	}



	void OnDisable ()
	{
		//We check to see if currentGame is null, 
		//because when we close the game, the GameManager is destroyed before the GameScreen
		//save game when player quits
		if (currentGame != null) {
			string path = Application.persistentDataPath + saveName;
			StreamWriter sw = File.CreateText (path);
			sw.Write (currentGame.GetData ());
			sw.Close ();
			Destroy (currentGame.gameObject);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Q)) {
			ScreenManager.instance.Push<Popup> ().Init ("Are you sure you want to quit?", OnExitPopupConfirm);
		}

	}

	void OnExitPopupConfirm ()
	{
		ScreenManager.instance.Pop ();
	}




}
