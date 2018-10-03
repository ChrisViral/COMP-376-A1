using UnityEngine;

public class Done_RandomRotator : MonoBehaviour 
{
	public float tumble;
	
	void Start ()
	{
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
	}
}