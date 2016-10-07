using UnityEngine;
using System.Collections;
using System;
using FullSerializer;

public class Json
{
	private static readonly fsSerializer _serializer = new fsSerializer ();
		
	public static string Serialize (object value)
	{
		// serialize the data
		fsData data;
		_serializer.TrySerialize (value.GetType (), value, out data).AssertSuccessWithoutWarnings ();
			
		// emit the data via JSON
		return fsJsonPrinter.CompressedJson (data);
	}
		
	public static T Deserialize <T> (string serializedState) where T : class, new()
	{
		// step 1: parse the JSON data
		fsData data = fsJsonParser.Parse (serializedState);
			
		// step 2: deserialize the data
		T deserialized = new T ();
		_serializer.TryDeserialize<T> (data, ref deserialized).AssertSuccessWithoutWarnings ();
			
		return deserialized as T;
	}

	public static object Deserialize (Type type, string serializedState)
	{
		fsData data = fsJsonParser.Parse (serializedState);
		object deserialized = null;
		_serializer.TryDeserialize (data, type, ref deserialized);
		return deserialized;
	}

}
