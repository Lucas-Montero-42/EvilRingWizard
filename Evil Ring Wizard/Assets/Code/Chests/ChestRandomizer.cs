using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRandomizer : MonoBehaviour
{
    public RingItem[] commonRings;
    public RingItem[] rareRings;
    public RingItem[] legendaryRings;
    private UseChest chest;
    [Range(0,2)]
    public int ChestQuality = 0;
    public void Awake()
    {
        AssignRing();
    }
    private void AssignRing()
    {
        int roll = Random.Range(1, 21); // Rango de 1 a 20
        RingItem selectedRing;

        if (ChestQuality == 0) // 70% común, 25% raro, 5% legendario
        {
            if (roll <= 14) // 70% (14/20)
                selectedRing = commonRings[Random.Range(0, commonRings.Length)];
            else if (roll <= 19) // 25% (5/20)
                selectedRing = rareRings[Random.Range(0, rareRings.Length)];
            else // 5% (1/20)
                selectedRing = legendaryRings[Random.Range(0, legendaryRings.Length)];
        }
        else if (ChestQuality == 1) // 20% común, 60% raro, 20% legendario
        {
            if (roll <= 4) // 20% (4/20)
                selectedRing = commonRings[Random.Range(0, commonRings.Length)];
            else if (roll <= 16) // 60% (12/20)
                selectedRing = rareRings[Random.Range(0, rareRings.Length)];
            else // 20% (4/20)
                selectedRing = legendaryRings[Random.Range(0, legendaryRings.Length)];
        }
        else // ChestQuality == 2 -> 10% común, 40% raro, 50% legendario
        {
            if (roll == 1 || roll == 2) // 10% (2/20)
                selectedRing = commonRings[Random.Range(0, commonRings.Length)];
            else if (roll <= 10) // 40% (8/20)
                selectedRing = rareRings[Random.Range(0, rareRings.Length)];
            else // 50% (10/20)
                selectedRing = legendaryRings[Random.Range(0, legendaryRings.Length)];
        }

        chest.SetInnateRing(selectedRing);
    }
}
