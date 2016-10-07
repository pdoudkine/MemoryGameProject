using UnityEngine;
using System.Collections;

public class MenuScreen : UIScreen {

	public void OnStartButton () {
		ScreenManager.instance.Push<GameScreen> ();
	}

}
