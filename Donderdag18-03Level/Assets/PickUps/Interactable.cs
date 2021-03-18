using UnityEngine;
using UnityEngine.AI;

/*	
	This component is for all objects that the player can
	interact with such as enemies, items etc. It is meant
	to be used as a base class.
*/

public class Interactable : MonoBehaviour
{

	private GameObject player;
	public float radius = 3f;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		Debug.Log(player);
	}

	public virtual void Update()
	{
		if ((player.transform.position - this.transform.position).sqrMagnitude < radius * radius)
		{
			Action();
		}
	}


	// This method is meant to be overwritten
	public virtual void Interact()
	{

	}

	public virtual void Action()
	{
		if (Input.GetKey(KeyCode.E)) // close and pick up (P pressed)
		{
			Debug.Log("E pressed");
			Interact();
		}
	}

}