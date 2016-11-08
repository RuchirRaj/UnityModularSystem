﻿// Defines modular classes here
// Author: Heavyskymobile - Rozx
// Date: 2016-11-05
// Version 0.1a

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ModularSystem{

	
	
	[System.Serializable]
	public class PartSet{
		
		public bool isActivate;
		
		public string name;
		
		public Transform attachTransform;
		
		public List<Part> partList = new List<Part>();
	}
	
	
	
	[System.Serializable]
	public class Part{
		
		public string name;
		
		public Vector3 position;
		public Vector3 rotation;
		public Vector3 scale;
		
		public GameObject prefab;
		
		// Spawn weight
		public int weight;
		
	}
	

	public enum StartingMethod{
		
		Awake, OnCall
	}
	

	public enum RandomSeedMode{
		
		Default, Manual, PositionBased
	}
	
	
	
	
	/// <summary>
    /// Static class to improve readability
    /// Example:
    /// <code>
    /// var selected = WeightedRandomizer.From(weights).TakeOne();
    /// </code>
    /// 
    /// </summary>
	
	[System.Serializable]
	public static class WeightedRandomizer
	{
		public static WeightedRandomizer<R> From<R>(Dictionary<R, int> spawnRate)
		{
			return new WeightedRandomizer<R>(spawnRate);
		}
	}
	
	[System.Serializable]
	public class WeightedRandomizer<T>
	{
		//public System.Random _random = new System.Random();
		private Dictionary<T, int> _weights;
		
        /// <summary>
        /// Instead of calling this constructor directly,
        /// consider calling a static method on the WeightedRandomizer (non-generic) class
        /// for a more readable method call, i.e.:
        /// 
        /// <code>
        /// var selected = WeightedRandomizer.From(weights).TakeOne();
        /// </code>
        /// 
        /// </summary>
        /// <param name="weights"></param>
		public WeightedRandomizer(Dictionary<T, int> weights)
		{
			_weights = weights;
		}
		
        /// <summary>
        /// Randomizes one item
        /// </summary>
        /// <param name="spawnRate">An ordered list withe the current spawn rates. The list will be updated so that selected items will have a smaller chance of being repeated.</param>
        /// <returns>The randomized item.</returns>
		public T TakeOne()
		{
            // Sorts the spawn rate list
			var sortedSpawnRate = Sort(_weights);
			
            // Sums all spawn rates
			int sum = 0;
			foreach (var spawn in _weights)
			{
				sum += spawn.Value;
			}
			
            // Randomizes a number from Zero to Sum
			//int roll = _random.Next(0, sum);
			int roll = Random.Range(0, sum);
			
            // Finds chosen item based on spawn rate
			T selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
			foreach (var spawn in sortedSpawnRate)
			{
				if (roll < spawn.Value)
				{
					selected = spawn.Key;
					break;
				}
				roll -= spawn.Value;
			}
			
            // Returns the selected item
			return selected;
		}
		

		private List<KeyValuePair<T, int>> Sort(Dictionary<T, int> weights)
		{
			var list = new List<KeyValuePair<T, int>>(weights);
			
            // Sorts the Spawn Rate List for randomization later
			list.Sort(
				delegate(KeyValuePair<T, int> firstPair,
				KeyValuePair<T, int> nextPair)
				{
					return firstPair.Value.CompareTo(nextPair.Value);
				}
			);
			
			return list;
		}
	}


}