using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour {


	void Start () {
		if (SavedGameManager.instance.savedGameExists) {
			this.GetComponent<Button> ().interactable = true;
		} else
			this.GetComponent<Button> ().interactable = false;
	}
	

}
