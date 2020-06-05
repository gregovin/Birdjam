using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogDetector : MonoBehaviour
{
    public Collider2D player;
    public DogScript s;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (player == other) s.Seen(other.gameObject);
    }
}
