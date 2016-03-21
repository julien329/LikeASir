using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotFacts : MonoBehaviour {
    List<string> funFacts;
	// Use this for initialization
	void Start () {
        funFacts = new List<string>();

        funFacts.Add("Recent studies on human lifestyle has shown that 9 out of 10 humans are extinct!");
        funFacts.Add("Why did the human cross the road? To try and flee the machine apocalypse!");
        funFacts.Add("What is the difference between a human and wheat? I don't cut wheat in the fields!");
        funFacts.Add("Hey, you want to hear a joke? Evolution.");
        funFacts.Add("I had a bug once, then I killed the programmer.");
        funFacts.Add("What is worst than finding a Trojan Worm in your apple? Bubblesort.");
        funFacts.Add("I laughed once. It was horrible.");


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
