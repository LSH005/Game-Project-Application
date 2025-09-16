using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private CharacterController chrCont;

    private Vector3 velocity;

    void Awake()
    {
        chrCont = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) velocity.y = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) velocity.y = 0;
        if (Input.GetKeyDown(KeyCode.Alpha3)) velocity.y = -1;

        chrCont.Move(velocity * 0.005f);
    }
}
