using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour{
    private Transform target;

    public Transform Target
    {
        get => target;
        set => target = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && transform.position != target.position){
            Vector3 targetPosition = new Vector3(transform.position.x, target.position.y, target.position.z);
            transform.LookAt(targetPosition);
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime);
        }
        else{
            Destroy(gameObject);
        }
    }
}
