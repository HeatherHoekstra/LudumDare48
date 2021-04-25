using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Animator playerAnim;
    public GameObject rope;
    public GameObject ropeBroken;


    public void PlayButton()
    {
        rope.SetActive(false);
        ropeBroken.SetActive(true);
        playerAnim.SetTrigger("Drop");
    }
}
