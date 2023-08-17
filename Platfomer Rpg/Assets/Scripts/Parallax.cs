using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    GameObject cam;
    [SerializeField] float prallaxValue;
    float xPos;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMove = cam.transform.position.x * prallaxValue;
        transform.position=new Vector3 (xPos+distanceToMove, transform.position.y);
        
    }
}
