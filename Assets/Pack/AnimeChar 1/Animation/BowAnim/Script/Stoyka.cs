using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoyka : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            



            anim.SetTrigger("Bow");




        }
    }
}
