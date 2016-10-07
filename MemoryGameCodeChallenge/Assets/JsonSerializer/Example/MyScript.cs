using UnityEngine;
using System.Collections;

public class MyScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		Data d = new Data ();
		d.foo = "bar";

		string s = Json.Serialize (d);
		Debug.Log (s);

		Data d2 = Json.Deserialize<Data> (s);
		Debug.Log (d2.nestedObj.fooArray [1]);

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
