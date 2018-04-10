using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDel : MonoBehaviour {

    private ParticleSystem ps;
     
     ////////////////////////////////////////////////////////////////
     
    void Start()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
