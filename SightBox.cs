using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightBox : MonoBehaviour
{
    [Tooltip("The enemy this box is used to detect for")]
    public Enemy thisEnem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { thisEnem.DetectPlayer(other.gameObject); }
    }
}
