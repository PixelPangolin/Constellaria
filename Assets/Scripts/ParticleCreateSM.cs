using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCreateSM : StateMachineBehaviour
{
    public GameObject particles;            // Prefab of the particle system to play in the state.

    // This will be called when the animator first transitions to this state.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        this.particles = (GameObject)Instantiate(Resources.Load("Prefabs/dustSystem"));
        Vector3 partTrans = new Vector3(animator.rootPosition.x, animator.rootPosition.y - 1);
        this.particles.transform.position = partTrans;

        if (stateInfo.IsName("RunStopLeft")){
            this.particles.transform.rotation = Quaternion.Euler(-20, 90, 0);
        }
        else
        {
            this.particles.transform.rotation = Quaternion.Euler(-20, -90, 0);
        }
    }

}