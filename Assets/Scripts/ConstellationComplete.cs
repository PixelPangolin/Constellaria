using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationComplete : MonoBehaviour {

	public GameObject lineA;
	public GameObject lineB;
	public GameObject endImage;
	public Camera mainCamera;

    // We're adding a series of lines in Start
    public List<GameObject> correctLines;

    void Start()
    {
        correctLines = new List<GameObject>
        {
            lineA,
            lineB
        };
    }

    // Update is called once per frame
    void Update () {

        if (CheckAllCorrectLines())
		{
			endImage.SetActive(true);
			mainCamera.orthographicSize = (mainCamera.orthographicSize + 0.01f);
		}
	}

    private bool CheckAllCorrectLines()
    {
        if (correctLines.Count == 0)
        {
            Debug.Log("Error -there are no correct lines");
            return true;
        }
            
        for (int i = 0; i < correctLines.Count; i++)
        {
            if (correctLines[i].activeInHierarchy == false)
            {
                return false;
            }
        }
        return true;
    }
}
