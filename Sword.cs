using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject wielder;
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
        if (other.CompareTag("Enemy") && wielder.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Enemy>())
            { other.gameObject.GetComponent<Enemy>().DealDamage(wielder.GetComponent<Knight>().GetHit()); }
            if (other.gameObject.GetComponent<CombatEnemy>())
            {
                other.gameObject.GetComponent<CombatEnemy>().DealDamage(wielder.GetComponent<Knight>().GetHit());
            }
        }
    }
}
