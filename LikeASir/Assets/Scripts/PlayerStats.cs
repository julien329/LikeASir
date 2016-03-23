using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    enum ColorChoice
    {
        GREY,
        BLUE,
        CYAN,
        GREEN,
        MAGENTA,
        RED,
        WHITE,
        YELLOW

    }
    
    public Color[] colors;
    public Color[] playerColors;
   // GameObject UIObjects;
    //IntroMenuWindow introMenu;

    void Start()
    {
        //introMenu = UIObjects.GetComponent<IntroMenuWindow>();
        playerColors = new Color[4];
        for(int i =0; i < 4; i++)
            playerColors[i] = Color.gray;
    }

	public Color nextColor(Color color, int playerNum)
    {
        int i = 0;

        //Find current color index
        while (i < colors.Length)
        {
            if (colors[i].Equals(color))
                break;
            i++;
        }

        //If last color is reached, next one is the first one
        if (i == colors.Length)
            i = 1;

        //Iterates through the colors to find the first free one, looping around;
        while(colorIsInUse(colors[i]))
        {
            i++;
            if (i == colors.Length)
                i = 1;
        }

        playerColors[playerNum-1] = colors[i];
        return colors[i];
    }
	
    //Checks if the color is currently used by the player.
	bool colorIsInUse(Color color)
    {
        foreach (Color x in playerColors)
            if (x.Equals(color))
                return true;
        return false;
    }
}
