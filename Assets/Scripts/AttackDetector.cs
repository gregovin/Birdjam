using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackDetector : MonoBehaviour
{
    public Collider2D player;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if(player == other) SceneManager.LoadScene("Title Screen");
    }
}
