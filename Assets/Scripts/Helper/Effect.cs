using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public string name;

    public string description;

    public float chance;

    public float effectPercent;
}

public class Serendipity : Effect
{
    public Serendipity()
    {
        name = "Serendipity";
        description = "increasing the amount of coins earned";
        chance = 0.25f;
        effectPercent = 0.35f;
    }
}

public class Assassination : Effect
{
    public Assassination()
    {
        name = "Assassination";
        description = "increasing the chance of meeting bosses";
        chance = 0.45f;
        effectPercent = 0.55f;
    }
}
