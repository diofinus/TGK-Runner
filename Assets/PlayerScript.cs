using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	// Use this for initialization
	public float jumpForce;
	public int plusDistance;
	public float intervalPlusDistance;
	[HideInInspector]
	public bool jump = false; //condition to jump
	public GameObject textGameOver;
	public GameObject textDistance;
	private Transform groundCheck;

	private bool grounded = false; //condition when grounded

	[HideInInspector]
	public bool gameOver = false;

	private int distance;

	void Start () {
		groundCheck = transform.Find("GroundCheck");
		InvokeRepeating("tambahDistance",0,intervalPlusDistance);
	
	}

	void tambahDistance()
	{
		if(gameOver==false)
		{
			distance+=plusDistance;
			textDistance.GetComponent<TextMesh>().text = distance.ToString();
		}

	}
	// Update is called once per frame
	void Update () {
		/*Debug.Log(Time.deltaTime*50);
		Debug.Log( "distance = "+distance);*/

		Debug.Log(grounded);
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		if( ( Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump") ) && grounded==true)
		{
			//can jump if grounded and mouse click
			jump = true;
		}
		if(this.transform.renderer.IsVisibleFrom(Camera.main)==false)
		{
			textGameOver.GetComponent<MeshRenderer>().enabled=true;
			distance=0;
			gameOver=true;
		}
		if(gameOver==true && Input.GetMouseButtonUp(0))
		{

			gameOver=false;
			Application.LoadLevel("runner");
		}
	
	}
	void FixedUpdate()
	{
		if(jump)
		{
			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f,jumpForce));
			jump = false;
		}
	}
}
