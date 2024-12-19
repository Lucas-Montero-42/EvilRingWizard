using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : ScriptableObject
{
    public Sprite ringTexture;
    public GameObject ringModel;
    public enum Shape
    {
        Single,
        Double,
        Three,
        Knuckes,
        Long,
        LShape,
        LInverse

    }
    public Shape shape;
}
