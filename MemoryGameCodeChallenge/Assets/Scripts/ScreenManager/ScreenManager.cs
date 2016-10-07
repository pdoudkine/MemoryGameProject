using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScreenManager : MonoBehaviour {


	public static ScreenManager instance;

	public UIScreen openingScreen;

	private Dictionary<Type, UIScreen> screens;

	private Stack<UIScreen> screenStack;

	// Use this for initialization
	void Awake () {
		instance = this;

		//We assign values
		screens = new Dictionary<Type, UIScreen> ();
		screenStack = new Stack<UIScreen> ();

		//Looping through all screens that are children of the ScreenManager
		foreach (UIScreen screen in GetComponentsInChildren<UIScreen>(true)) {
			//Adding them to our Dictionary and disable everything
			screens.Add (screen.GetType(), screen);
			screen.gameObject.SetActive(false);
		}
		//Showing our opening screen
		// Push<MainMenu> (); is the same as Push (typeof(MainMenu));

		Push (openingScreen.GetType ());
	}

	//This is a generic method! We can replace T with a Type
	public T Push <T>() where T : UIScreen
	{
		//This converts the Generic T to an object of the type 'Type'
		Type screenType = typeof(T);

		return Push (screenType) as T;
	}

	private UIScreen Push (Type screenType) {

		//We access our Dictionary and provide the Type, so that it returns the object of this Type
		//E.g. we provide GameScreen, it returns the UIScreen-object which has the GameScreen component
		UIScreen newScreen = screens [screenType];

		if (newScreen.hidePreviousScreen) {
			//We disable the previous screen
			if (screenStack.Count != 0) {
				UIScreen currentTopScreen = screenStack.Peek ();
				currentTopScreen.gameObject.SetActive (false);
			}
		}

		
		newScreen.gameObject.SetActive (true);
		screenStack.Push (newScreen);
		//We return the screen that was just pushed onto the Stack
		return newScreen;
	}

	public void Pop () {
		if (screenStack.Count <= 1) {
			Debug.LogError("You can't Pop the last UIScreen from the stack!");
			return;
		}
		//Disabling the screen that's currently on top
		UIScreen currentTopScreen = screenStack.Pop ();
		currentTopScreen.gameObject.SetActive (false);

		//Enabling the screen that's on top after we disabled to currentTopScreen
		UIScreen newTopScreen = screenStack.Peek ();
		newTopScreen.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	public void ResetGame () {
		foreach (UIScreen screen in screenStack) {
			if (screen.GetType () != typeof(MenuScreen)) {
				Debug.Log ("POP");
				Debug.Log (screen.GetType());
				Pop ();
			}
		}
	}
}













