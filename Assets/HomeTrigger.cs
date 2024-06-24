using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(GameManager.ins.butterfliesLeft <= 0)
            {
                GameManager.ins.beatGame = true;
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (GameManager.ins.butterfliesLeft <= 0)
            {
                GameManager.ins.beatGame = true;
            }
        }
    }
}
