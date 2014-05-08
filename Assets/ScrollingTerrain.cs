using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Parallax scrolling script that should be assigned to a layer
/// </summary>
public class ScrollingTerrain : MonoBehaviour
{
	/// <summary>
	/// Scrolling speed
	/// </summary>
	public Vector2 speed = new Vector2(10, 10);
	
	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);
	
	/// <summary>
	/// Movement should be applied to camera
	/// </summary>
	public bool isLinkedToCamera = false;
	
	/// <summary>
	/// 1 - Background is infinite
	/// </summary>
	public bool isLooping = false;

	public float diffIncrease;
	//spawn terrain configuration
	public float minSpawnX;
	public float maxSpawnX;
	public float minSpawnY;
	public float maxspawnY;

	public int minBlock;
	public int maxBlock;

	//randomize number to determine if there's cliff or not
	public int minObstacle;
	public int maxObstacle;

	/// <summary>
	/// 2 - List of children with a renderer.
	/// </summary>
	private List<Transform> backgroundPart;
	private float positionY;
	private int currRandom;
	private bool bolehRandom = true;
	private PlayerScript playerScript;
	private float defaultSpeed;
	// 3 - Get all the children
	void Start()
	{
		Debug.Log("start");
		defaultSpeed = speed.x;
		playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
		// For infinite background only
		if (isLooping)
		{
			// Get all the children of the layer with a renderer
			backgroundPart = new List<Transform>();
			
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				
				// Add only the visible children
				if (child.renderer != null)
				{
					backgroundPart.Add(child);
				}
			}
			
			// Sort by position.
			// Note: Get the children from left to right.
			// We would need to add a few conditions to handle
			// all the possible scrolling directions.
			backgroundPart = backgroundPart.OrderBy(
				t => t.position.x
				).ToList();
		}
		positionY = backgroundPart.FirstOrDefault().position.y;
	}
	
	void Update()
	{
		if(playerScript.gameOver==true)
		{
			speed.x=defaultSpeed;
		}
		//Debug.Log (speed.x);
		// Movement
		Vector3 movement = new Vector3(
			speed.x * direction.x,
			speed.y * direction.y,
			0);
		
		movement *= Time.deltaTime;
		transform.Translate(movement);

		speed.x+=diffIncrease/100;
		// Move the camera
		if (isLinkedToCamera)
		{
			Camera.main.transform.Translate(movement);
		}
		
		// 4 - Loop
		if (isLooping)
		{
			// Get the first object.
			// The list is ordered from left (x position) to right.
			Transform firstChild = backgroundPart.FirstOrDefault();
			
			if (firstChild != null)
			{
				// Check if the child is already (partly) before the camera.
				// We test the position first because the IsVisibleFrom
				// method is a bit heavier to execute.
				if (firstChild.position.x < -10/*Camera.main.transform.position.x*/)
				{
					// If the child is already on the left of the camera,
					// we test if it's completely outside and needs to be
					// recycled.
					if (firstChild.renderer.IsVisibleFrom(Camera.main) == false)
					{
						// Get the last child position.
						Transform lastChild = backgroundPart.LastOrDefault();
						Vector3 lastPosition = lastChild.transform.position;
						Vector3 lastSize = (lastChild.renderer.bounds.max - lastChild.renderer.bounds.min);
						
						int tes = Random.Range(minObstacle,maxObstacle);
						if(tes==1)
						{
							float randomX = Random.Range(minSpawnX,maxSpawnX);
							float randomY = Random.Range(minSpawnY,maxspawnY);
							currRandom= Random.Range(minBlock,maxBlock);

							// Set the position of the recyled one to be AFTER
							// the last child.
							// Note: Only work for horizontal scrolling currently.
							firstChild.position = new Vector3(lastPosition.x + lastSize.x + randomX, positionY + randomY, firstChild.position.z);
							
							// Set the recycled child to the last position
							// of the backgroundPart list.
							backgroundPart.Remove(firstChild);
							backgroundPart.Add(firstChild);
						}
						else
						{
							/*float randomX = Random.Range(minSpawnX,maxSpawnX);
							float randomY = Random.Range(minSpawnY,maxspawnY);
							currRandom= Random.Range(minBlock,maxBlock);*/
							
							// Set the position of the recyled one to be AFTER
							// the last child.
							// Note: Only work for horizontal scrolling currently.
							firstChild.position = new Vector3(lastPosition.x + lastSize.x, lastPosition.y, firstChild.position.z);
							
							// Set the recycled child to the last position
							// of the backgroundPart list.
							backgroundPart.Remove(firstChild);
							backgroundPart.Add(firstChild);
						}

					/*	if(bolehRandom ==true)
						{

							bolehRandom = false;
						}*/

						/*if(currRandom>1)
						{

								Debug.Log("SpawnTerrainDouble");
								//Debug.Log(lastSize.x);
								firstChild.position = new Vector3(lastPosition.x + lastSize.x,lastPosition.y, firstChild.position.z);
								
								// Set the recycled child to the last position
								// of the backgroundPart list.
								backgroundPart.Remove(firstChild);
								backgroundPart.Add(firstChild);
								firstChild = backgroundPart.FirstOrDefault();
								lastChild = backgroundPart.LastOrDefault();
								lastPosition = lastChild.transform.position;
								lastSize = (lastChild.renderer.bounds.max - lastChild.renderer.bounds.min);
								currRandom--;
							

						}
						else
						{
							bolehRandom = true;
						}*/

						// Set the position of the recyled one to be AFTER
						// the last child.
						// Note: Only work for horizontal scrolling currently.
						//firstChild.position = new Vector3(lastPosition.x + lastSize.x + randomX, positionY + randomY, firstChild.position.z);
						
						// Set the recycled child to the last position
						// of the backgroundPart list.
						/*backgroundPart.Remove(firstChild);
						backgroundPart.Add(firstChild);*/
					}
				}
			}
		}
	}
}
