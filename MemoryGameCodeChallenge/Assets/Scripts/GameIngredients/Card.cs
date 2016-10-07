using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

//Card class that holds all information of a card such as color, rotation,face exposed, etc
//Fires off an event when clicked
public class Card : MonoBehaviour, IPointerClickHandler {

	//event for when the card is clicked
	private Action<Card> clickCallback;
	//if the cards match partner was found
	public bool matched= false;
	//facing direction of the card
	public enum CardState
	{
		UP = 0, 
		DOWN = 1
	};
	//card color
	public enum CardType
	{
		BLACK = 0,
		BLUE = 1,
		CYAN = 2,
		GRAY = 3,
		GREEN = 4,
		MAGENTA = 5,
		RED = 6,
		YELLOW = 7		
	}
	//colors for front and back face
	public Color frontFaceColor;
	public Color backFaceColor;
	//image to hold card face color
	private Image cardFace;
	//card animator
	private Animator anim;
	//current facing direction state
	public CardState state;
	//current color of the card
	public CardType type;


	void Start () {
		//delegating function when card is clicked
		clickCallback = GameManager.instance.OnCardClicked;
		//intialization of variables
		cardFace = GetComponentsInChildren<Image> ()[1];
		if (!matched) {
			state = CardState.DOWN;
			anim = GetComponentInChildren<Animator> ();
			anim.SetBool ("startGame", false);
			StartCoroutine (StartGame ());
		}
		switch (type) {
		case CardType.BLACK:
			backFaceColor = Color.black;
			break;
		case CardType.BLUE:
			backFaceColor = Color.blue;
			break;
		case CardType.CYAN:
			backFaceColor = Color.cyan;
			break;
		case CardType.GRAY:
			backFaceColor = Color.gray;
			break;
		case CardType.GREEN:
			backFaceColor = Color.green;
			break;
		case CardType.MAGENTA:
			backFaceColor = Color.magenta;
			break;
		case CardType.RED:
			backFaceColor = Color.red;
			break;
		case CardType.YELLOW:
			backFaceColor = Color.yellow;
			break;
		}
	}

	void OnEnable()
	{
		GameManager.ResetCards += SetStateDown;

	}

	void OnDisable()
	{
		GameManager.ResetCards -= SetStateDown;
	}

	void Update () {
		//setting the cards color based on its rotation
		if (cardFace.gameObject.transform.rotation.eulerAngles.y < 90f) {
			cardFace.color = frontFaceColor;
		} else {
			cardFace.color = backFaceColor;
		}
	}
	//capturing pointer click events
	public void OnPointerClick(PointerEventData e)
	{
		//if cards can currently be clicked and the card is face down
		//play an animation and change its parameters to the face up state
		if (GameManager.instance.allowCardSelection && state == CardState.DOWN) {
			clickCallback (this);
			if (state == CardState.DOWN) {
				anim.SetBool ("isFaceUp", true);
				state = CardState.UP;
			} 

		}
	}

	//Coroutine used to play animation at start of game
	IEnumerator StartGame()
	{
		yield return new WaitForSeconds (3f);
		anim.SetBool ("startGame", true);
	}

	//change card to face down state
	private void SetStateDown()
	{
		if (!matched) {
			anim.SetBool ("isFaceUp", false);
			state = CardState.DOWN;
			//StartCoroutine (EnableCardClick ());
		}
	}
	//Saving card data to a data holder class
	public SaveableItemData GetData()
	{
		SaveableItemData data = new SaveableItemData ();
		data.name = this.name;
		data.rotation = this.transform.rotation;
		data.type = (int)this.type;
		data.state = (int)this.state;
		data.matched = this.matched;
		return data;
	}
	//setting card data from a passed data holder
	public void SetData(SaveableItemData data)
	{
		this.transform.rotation = data.rotation;
		this.type = (CardType)data.type;
		this.state = (CardState)data.state;
		this.matched = data.matched;
	}

}
