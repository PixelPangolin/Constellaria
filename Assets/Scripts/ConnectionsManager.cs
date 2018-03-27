using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ConnectionsManager : MonoBehaviour {
	
    public List<Node> nodes;
    public List<Vector2> goalConnectedNodes;
	public List<Connection> playerConnections;

    

	void Start () {
		if (nodes.Count == 0) {
			throw new Exception ("No Nodes Added");
		}
		if(goalConnectedNodes.Count == 0){
			throw new Exception ("No goal Connections set");
		}
			
	}

    public void SwitchConnectionStateBetweenNodes(Node a, Node b)
    {
        Connection c = GetConnection(a, b);
        if (c == null)
        {
			// Removes oldest connection if we are at the max connections
			/*
			if (playerConnections.Count >= maxTotalConnections) {
				RemoveOldest ();
			}
*/			bool removeConnectA = RemoveExtraConnection (a);
			bool removeConnectB = RemoveExtraConnection (b);
			if (removeConnectA||removeConnectB) {
				//One of the nodes had extra connections
				//SOUND: A link Being Cut
				GameObject.Find("GameController").GetComponent<AudioController>().playMakeLine();
			} else{
				//No connection was broken
				//SOUND: Making a new Link
				GameObject.Find("GameController").GetComponent<AudioController>().playBreakLine();
			}
			GameObject beam = new GameObject ("beamOLight");
			beam.transform.parent = transform;
			beam.AddComponent<Connection> ();
			Connection con = beam.GetComponent<Connection> ();
			con.setNodes (a, b);

			// Check if connection is part of the goal and set it's to be a goal connection if so
			if (CheckInGoal (a,b)) {
				//print ("In goal");
				con.setGoal ();
			}
			playerConnections.Add (con);
			CheckForWin ();
        } 
		/* Removes nodes when re-traversed
		else
        {
            playerConnections.Remove(c);
            c.Delete();
        } */
    }

	public void RemoveOldest(){
		Connection first = playerConnections [0];
		playerConnections.Remove(first);
		first.Delete();
	}

	public bool RemoveExtraConnection(Node a){
		List<Connection> NodeAConnections = new List<Connection>();
		for (int i = 0; i < playerConnections.Count; i++){
			Connection c = playerConnections[i];

			if (((c.start == a) || (c.end == a)))
			{
				NodeAConnections.Add (c);
			}
		}
		//print (NodeAConnections.Count);
		//print (a.GetMaxConnections ());
		if (NodeAConnections.Count >= a.GetMaxConnections()) {
			Connection first = NodeAConnections [0];
			playerConnections.Remove (first);
			first.Delete ();
			return true;
		} else {
			return false;}
	}

    public Connection GetConnection(Node a, Node b)
    {
        // A better way to do this might be to make a hashmap of <tuple(nodes) -> connection>
        // Although Unity's version of .NET doesn't actually have a tuple data type.
        for (int i = 0; i < playerConnections.Count; i++)
        {
            Connection c = playerConnections[i];
            if (((c.start == a) && (c.end) == b) ||
                ((c.start == b) && (c.end) == a))
            {
                return c;
            }
        }
        return null;
    }

	public bool CheckInGoal(Node a, Node b){
		int correctConnections = 0;
		for (int i = 0; i < goalConnectedNodes.Count; i++) {
			Node goalStart = nodes[(int)goalConnectedNodes[i].x];
			Node goalEnd = nodes[(int)goalConnectedNodes[i].y];
			if (((goalStart == a) && (goalEnd == b))
				|| ((goalStart == b) && (goalEnd == a))){
				return true;
			}
		}
		return false;
	}


	// Checks whether all connections that are in "goal connections" have been made
	// If all goal connections are created then run the level complete script. 
	public void CheckForWin(){

		int correctConnections = 0;
		for (int i = 0; i < goalConnectedNodes.Count; i++) {
			Node a = nodes[(int)goalConnectedNodes[i].x];
			Node b = nodes[(int)goalConnectedNodes[i].y];
			for (int j = 0; j < playerConnections.Count; j++){
				Connection c = playerConnections[j];
				if (((c.start == a) && (c.end) == b) ||
					((c.start == b) && (c.end) == a))
				{
					correctConnections++;
					break;
				}
				
			}
		}
		//print ("Number of Correct Connections : " + correctConnections);

		if (correctConnections == goalConnectedNodes.Count) {
			gameObject.GetComponent<LevelComplete> ().EndLevel ();
		}
	}
}
