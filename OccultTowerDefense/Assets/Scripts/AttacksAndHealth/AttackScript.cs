using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
	#region Variables to assign via the unity inspector (SerialiseFields).

	#endregion

	#region Private Variables.

	private Transform attackOwner;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	#endregion

	#region Public Access Functions (Getters and Setters).

	public void SetOwner(Transform a_owner)
	{
		attackOwner = a_owner;
	}
	#endregion
}
