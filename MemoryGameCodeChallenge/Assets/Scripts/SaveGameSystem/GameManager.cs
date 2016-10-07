using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
//Class to manage interactions within the actual game screen
public class GameManager : MonoBehaviour {
	//object to hold all the cards
	[SerializeField]
	public GameObject cardHolder;
	//list of card game objects to be shuffled
	public List<GameObject> cardDeck;
	private int deckSize = 15;
	public static GameManager instance;
	//used to toggle card interactability
	public bool allowCardSelection = false;
	//currently selected (face up) cards
	List<Card> selectedCards;
	//event to reset cards back down to face down state
	public static event Action ResetCards;

	//create singleton when game starts and start game
	void OnEnable()
	{
		if (instance != null)
			{
				Destroy(instance);
			}
		instance = this;
		GameStart ();

	}

	void GameStart()
	{
		//if a game is to be loaded, load the save file and create a set of card objects
		//that inherent the save data
		selectedCards = new List<Card> ();
		if (SavedGameManager.instance.startWithLoad) {
			DirectoryInfo dirInfo = new DirectoryInfo (Application.persistentDataPath); 
			FileInfo[] files = dirInfo.GetFiles ("*.txt");
			foreach (FileInfo file in files) {
				if (file.Name == "GIP.txt") {
					StreamReader sr = file.OpenText ();
					string gameDataString = sr.ReadToEnd ();
					sr.Close ();
					SetData (gameDataString);
				}
			}
			//if a new game, create a list of card objects and set them to a fresh game state
		} else {
			List<GameObject> deck = new List<GameObject> ();
			Instantiate (cardHolder);
			Debug.Log ("HIT");
			int color = 0;
			int counter = 0;
			GameObject tempCard;
			for (int i = 0; i <= deckSize; i++) {
				Debug.Log (i);
				//deck [i].type = (Card.CardType)color;
				tempCard = Instantiate (Resources.Load<GameObject> ("Card"));
//			tempCard.transform.SetParent (cardHolder.transform);
				tempCard.GetComponent<Card> ().type = (Card.CardType)color;
				deck.Add (tempCard);
				counter++;
				if (counter == 2) {
					counter = 0;
					color++;
				}
			}
			deck = Shuffle (deck);
			for (int i = 0; i <= deck.Count - 1; i++) {
				deck [i].transform.SetParent (cardHolder.transform);
			}
		}


		StartCoroutine (ProcessCardSelection ());
	}

	public void EnableSelection()
	{
		allowCardSelection = true;
	}
	//function that activates when a card is clicked
	public void OnCardClicked(Card card)
	{
		
			allowCardSelection = false;
			selectedCards.Add (card);

	}
	//process two selected cards and see if they match
	//if they do, set them to be permanently face up and be listed as matched.  Then check to see if 
	//all cards are matched
	//if they do not, flip them back over

	IEnumerator ProcessCardSelection()
	{
		yield return new WaitForSeconds (4f);
		allowCardSelection = true;
		while (true) {
			if (selectedCards.Count == 2) {
				if (selectedCards [0].type == selectedCards [1].type) {
					Debug.Log ("Matched");
					selectedCards [0].matched = true;
					selectedCards [1].matched = true;
					selectedCards.Clear ();
					yield return CheckWinCondition ();
					allowCardSelection = true;
				} else {
					allowCardSelection = false;
					Debug.Log ("Two Cards Selected");
					selectedCards.Clear ();
					yield return new WaitForSeconds (1f);
					ResetCards ();
					yield return new WaitForSeconds (1.1f);
					allowCardSelection = true;
				}
			} else if (selectedCards.Count == 1) {
				yield return new WaitForSeconds (1f);
				allowCardSelection = true;
			}
			yield return null;
		}
	}

	//check matched state of each card to see if player won
	//if player won show end screen, if not continue
	IEnumerator CheckWinCondition()
	{

		bool playerWon = true;
		foreach (Card c in cardHolder.GetComponentsInChildren<Card>()) {
			playerWon = (playerWon && c.matched);
		}
		if (playerWon) {
			ScreenManager.instance.Push<EndScreen> ();
		}
		yield return null;
	}
	//shuffle the cards in the deck and return the shuffled deck
	private List<GameObject> Shuffle(List<GameObject> shuffleList)
	{
		int n = shuffleList.Count - 1;
		for (int i = 0; i < n; i++) {
			int r = i + (int)(UnityEngine.Random.Range(0f,1f) * (n-i));
			GameObject temp = shuffleList[r];
			shuffleList[r] = shuffleList[i];
			shuffleList[i] = temp;
		}
		return shuffleList;
	}
	//get game saved data and serialize it
	public string GetData()
	{
		GameData data = new GameData ();
		foreach (Card item in GetComponentsInChildren<Card>()) {
			SaveableItemData itemData = item.GetData ();
			data.itemList.Add (itemData);
		}
		return Json.Serialize (data);
	}
	//set game data based on saved data
	public void SetData(string dataString)
	{
		GameData data = Json.Deserialize<GameData> (dataString);
		foreach (SaveableItemData itemData in data.itemList) {
			Card item = Instantiate (Resources.Load<Card> ("Card"));
			item.name = itemData.name;
			item.transform.SetParent (cardHolder.transform);
			item.SetData (itemData);
		}
	}
}
