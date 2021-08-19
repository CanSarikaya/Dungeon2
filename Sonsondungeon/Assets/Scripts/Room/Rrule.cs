using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralRoom")]
public class Rrule : ScriptableObject
{
   public string letter;
   [SerializeField]
   private string[] results = null;
   public string GetResult()
   {
		  return results[0];
		   
   }
}
