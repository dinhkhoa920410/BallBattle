using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public const int CYAN = 0;
    public const int BROWN = 1;
    public const int RED = 2;
    public const int YELLOW = 3;
    public const int BLUE = 4;
    public const int WHITE = 5;
    public const int BLACK = 6;
    public const int MAGENTA =7;
    public static Color getColorCode(int color){
        switch(color){
            case RED: return Color.red;
            case BLUE: return  Color.blue;
            case WHITE: return Color.white;
            case BROWN: return new Color(100f/255,75f/255,55f/255,1);
            case YELLOW: return Color.yellow;
            case CYAN: return Color.cyan;
            case MAGENTA: return Color.magenta;
            default: return Color.black;
        }
    }

    public static Color setAlpha(Color color, float alpha){
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static Vector3 scaleVector3X(Vector3 v3, float scaleRate){
        return new Vector3(scaleRate, v3.y, v3.z);
    }
}
