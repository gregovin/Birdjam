using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D player;
    public StealthScript s;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(player == other) s.Seen(other.gameObject);
    }
}
