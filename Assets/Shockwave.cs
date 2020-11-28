using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour{
    
    private void Awake(){
        
    }

    void Update(){
        transform.localScale += new Vector3(0.0035f, 0.0035f, 0.0035f);

        if (transform.localScale.x > 0.4f){
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }

    public IEnumerator startShockwave(float range){
        bool stop = true;
        gameObject.SetActive(true);
        while (stop){
            transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);

            if (transform.localScale.x > 1f){
                stop = false;
            }
        }
        yield break;
    }
}