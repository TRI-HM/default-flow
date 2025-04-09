using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotating : MonoBehaviour
{
    public bool horizontal = true;
    public bool vertical = true;
    public bool deep = true;

    void Update()
    {
        if (horizontal)
        {
            this.transform.Rotate(Vector3.up * Time.deltaTime * 50);
        }
        if (vertical)
        {
            this.transform.Rotate(Vector3.right * Time.deltaTime * 70);
        }
        if (deep)
        {
            this.transform.Rotate(Vector3.forward * Time.deltaTime * 50);
        }
    }
}
