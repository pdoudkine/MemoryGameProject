using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Popup : UIScreen {

	public Text messageTextfield;

	private Action confirmCallback;
	//callback for cancel button function
	private Action cancelCallback;

	void OnEnable () {
		Time.timeScale = 0;
	}

	void OnDisable () {
		Time.timeScale = 1;
	}
	//added a second callback for cancel button in case special functionality was needed
	public void Init (string message, Action _confirmCallback = null, Action _cancelCallback = null) {
		messageTextfield.text = message;
		confirmCallback = _confirmCallback;
		cancelCallback = _cancelCallback;

	}

	public void OnConfirmButton () {
		ScreenManager.instance.Pop ();
		if (confirmCallback != null) {
			confirmCallback ();
		}
	}
	//gets called when cancel button is pressed
	public void OnCancelButton () {
		ScreenManager.instance.Pop ();
		//if cancel button has special functionality do this
		if (cancelCallback != null) {
			cancelCallback();
		}

	}
}
