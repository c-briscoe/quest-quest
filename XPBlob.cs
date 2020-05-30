using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPBlob : MonoBehaviour
{
    public int xpValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Knight>().GetXP(xpValue);
            Destroy(gameObject);
        }
    }
}
