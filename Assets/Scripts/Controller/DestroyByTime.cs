using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    public int waitFor;

	// Use this for initialization
	void Start () {
        StartCoroutine(Destroy());
	}
	
	IEnumerator Destroy()
    {
        yield return new WaitForSeconds(waitFor);
        Destroy(this);
    }
}
