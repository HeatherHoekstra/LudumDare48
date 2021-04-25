using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{

    public IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(10);

            DestroyGameObject();       
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
