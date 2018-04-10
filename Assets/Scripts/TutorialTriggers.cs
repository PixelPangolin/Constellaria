// This code activates tutorial messages when trigger volumes are entered
// reference https://answers.unity.com/questions/1315037/object-appear-and-disappear-code.html
// reference Andrew Crowe for guidance
// reference Other team members' scripts
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggers : MonoBehaviour
{
    public List<GameObject> ActivateThenDeactivate;
    public List<GameObject> ActivateOnEnter;
    public List<GameObject> DeactivateOnEnter;
    public List<GameObject> ActivateOnExit;
    public List<GameObject> DeactivateOnExit;
    public GameObject player;

    // Checks that the player entered the trigger
    void OnTriggerEnter2D(Collider2D player)
    {
        // sets the tutorial sprite to active
        foreach (GameObject each in ActivateThenDeactivate) {
            each.SetActive(true);
        }
        foreach (GameObject each in ActivateOnEnter)
        {
            each.SetActive(true);
        }
        foreach (GameObject each in DeactivateOnEnter)
        {
            each.SetActive(false);
        }

    }
    // Checks that the player left the trigger
    void OnTriggerExit2D(Collider2D player)
    {
        // sets the tutorial sprite to inactive
        foreach (GameObject each in ActivateThenDeactivate)
        {
            each.SetActive(false);
        }
        foreach (GameObject each in DeactivateOnExit)
        {
            each.SetActive(false);
        }
        foreach (GameObject each in ActivateOnExit)
        {
            each.SetActive(true);
        }
    }


}

