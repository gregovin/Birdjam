using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsDetector : MonoBehaviour
{
    public LayerMask grabbable;
    public Text score;
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collided");
        if (((1<<other.gameObject.layer) & grabbable) == 1 << other.gameObject.layer)
        {
            score.text = "" + (int.Parse(score.text) + 100);
            Destroy(other.gameObject);
        }
    }
}
