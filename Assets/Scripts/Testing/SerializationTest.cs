using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializationTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Piece p = new Piece("nich", 1, 2, 3, false);
        p.SetRace(Enums.Race.Elf);
        Debug.Log(p.GetName());
        Debug.Log(p.GetRace());
        var b = Serialization.serializeObject(p);
        Piece np = (Piece) Serialization.deserializeData(b);

        Debug.Log(np.GetName());
        Debug.Log(np.GetRace());
    }
}
