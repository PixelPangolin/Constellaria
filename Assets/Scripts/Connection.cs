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
 	        this.beam = pivot.transform.GetChild(0);
			
        	this.pivot.transform.parent = gameObject.transform;
			//beam.GetComponent<Renderer>().material.color = Random.ColorHSV();
			this.length = getDistance ();
			this.beam.localScale = new Vector3 (this.length,0.2f, 1);
			this.beam.position = Vector3.MoveTowards (this.beam.position, new Vector3 (this.beam.position.x, this.beam.position.y, 1), 1);
			this.pivot.transform.position = new Vector3 ((this.start.transform.position.x+this.end.transform.position.x)/2, (this.start.transform.position.y +this.end.transform.position.y)/2, this.length / 2);

			float degreesToRotate = Mathf.Rad2Deg*Mathf.Atan2(this.end.transform.position.y - this.start.transform.position.y, this.end.transform.position.x - this.start.transform.position.x)%180;
			//not possible to return values outside of 180 to -180
			//degrees has to be within 90 to -90 to have the right side up, and we have to handle angles sharper than 45, as they can't be walked up.
			if (degreesToRotate < -90) {//If degress < -90, add 180
				degreesToRotate = degreesToRotate+180;
			}
			else if (degreesToRotate > 90) {//If degress > 90, subtract 180
				degreesToRotate = degreesToRotate-180;
			}

			if (Mathf.Abs (degreesToRotate) >= 45) {//If the angle is more than 45, disable collision.
				//TODO: decide if this case should be a ladder
				beam.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			pivot.transform.rotation = Quaternion.Euler (0, 0, degreesToRotate);
		if (this.goal) {
			Material m_Material = this.beam.gameObject.GetComponent<Renderer> ().material;

			m_Material.SetColor ("_EmissionColor", Color.yellow);
			//m_Material.color = Color.cyan;
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

	// Sets this connection to be a goal connection, changes aspects about the connection as a result

	public void setGoal(){
		this.goal = true;
		//Transform b = this.beam;
		//GameObject bOL = b.gameObject;
		//Material m_Material = this.beam.gameObject.GetComponent<Renderer>().material;
		//m_Material.color = Color.cyan;
	}

}
