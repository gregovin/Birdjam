using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthScript : MonoBehaviour
{
    public Collider2D[] colliders;
    float timeKeeper = 0;
    int colliderChoosen = 0;
    Animator controller;
    // Start is called before the first frame update
    void Start()
    {
        colliders[colliderChoosen].enabled = false;
        controller = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        controller.SetBool("Looking", !controller.GetBool("Looking"));
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("it works");
    }
    void FixedUpdate()
    {
        timeKeeper += Time.fixedDeltaTime;
        if (timeKeeper > 5)
        {
            colliders[colliderChoosen].enabled = true;
            colliderChoosen = (colliderChoosen + 1) % colliders.Length;
            colliders[colliderChoosen].enabled = false;
            timeKeeper = 0;
        }
    }
}
