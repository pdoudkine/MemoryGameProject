using UnityEngine;
using System.Collections;

public class Data
{
	public string foo;
	public Data2 nestedObj = new Data2 ();
}

public class Data2
{
	public string[] fooArray = new string[] {
		"bar",
		"bar2"
	};
}
