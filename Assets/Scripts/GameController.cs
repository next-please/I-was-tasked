using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text timer;
    public int elapsedTicks = 0;
    public int ticksPerSecond = 50;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        elapsedTicks += 1;
        float elapsedSeconds = elapsedTicks / ticksPerSecond;
        timer.text = elapsedSeconds.ToString();
    }
}
