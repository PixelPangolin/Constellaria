using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Connection : MonoBehaviour {

    public Node start;
    public Node end;
	public bool goal = false;

    private GameObject pivot;
    private Transform beam;
    
    private float length;
    private float opp;
    private float adj;


	// Use this for initialization
	void Start () {
        this.pivot = (GameObject)Instantiate(Resources.Load("Prefabs/beamPivot"));
        beam = pivot.transform.GetChild(0);
        this.pivot.transform.parent = gameObject.transform;
        //beam.GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
	
	// Update is called once per frame
	void Update () {
		if (goal == false) {
			this.length = getDistance ();
        
			beam.localScale = new Vector3 (1, 0.2f, this.length);
			beam.localPosition = new Vector3 (0, 0, this.length / 2);
			beam.position = Vector3.MoveTowards (beam.position, new Vector3 (beam.position.x, beam.position.y, 1), 1);
			pivot.transform.position = start.transform.position;
			pivot.transform.LookAt (end.transform);
		} else {
		}
    }

    public float getDistance()
    {
        adj = Mathf.Abs(end.transform.position.x - start.transform.position.x);
        opp = Mathf.Abs(end.transform.position.y - start.transform.position.y);
        float hyp = Mathf.Sqrt(Mathf.Pow(opp, 2) + Mathf.Pow(adj, 2));
        return hyp;
    }

    public void setNodes(Node a, Node b)
    {
        this.start = a;
        this.end = b;
    }

    public void Delete()
    {
        Destroy(this.pivot.transform.parent.gameObject);
        Destroy(this);
    }

	public void SetGoalConnection(){
		this.goal = true;
	}
}
