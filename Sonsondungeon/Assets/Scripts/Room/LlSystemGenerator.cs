using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LlSystemGenerator : MonoBehaviour
{
	public Rrule[] Rrules;
	public string rootSentence;
	[Range(0,10)]
	public int iterationLimit = 1;

	private void Start()
	{
		Debug.Log(GenerateSentence());
	}

	public string GenerateSentence(string word =null)
	{
		if (word==null)
		{
			word = rootSentence;
			
		}
		return GrowRecursive(word);
	}
	private string GrowRecursive(string word, int iterationIndex = 0)
	{
		if(iterationIndex >= iterationLimit )
		{
			return word;
		}
		StringBuilder newWord = new StringBuilder();

		foreach(var c in word)
		{
			newWord.Append(c);
			ProcessRulesRecursivelly(newWord, c, iterationIndex);
		}
		return newWord.ToString();

	}
	private void ProcessRulesRecursivelly(StringBuilder newWord, char c, int iterationIndex)
	{
		foreach (var Rrule in Rrules)
		{
			if (Rrule.letter == c.ToString())
			{
				newWord.Append(GrowRecursive(Rrule.GetResult(), iterationIndex +1));
			}
		}
	}
}
