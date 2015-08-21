using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {

	protected Animator animator;                          //Variable of type Animator to store a reference to the enemy's Animator component.
	protected Transform target;                           //Transform to attempt to move toward each turn.

	void Awake() {

	}
	// Use this for initialization
	protected override void Start () {
		//Get and store a reference to the attached Animator component.
		animator = GetComponent<Animator> ();
		
		//Find the Player GameObject using it's tag and store a reference to its transform component.
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		GameManager.manager.enemies.Add (this);
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	//Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
	//See comments in MovingObject for more on how base AttemptMove function works.
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		//Call the AttemptMove function from MovingObject.
		base.AttemptMove <T> (xDir, yDir);
	}

	//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
	public void MoveEnemy ()
	{
		// This random number determines which direction our enemies will move in.
		// There are four possible numbers generated (0, 1, 2, 3) and thus four directions the enemy may move
		int ran = Random.Range (0, 4);
		//Declare variables for X and Y axis move directions, these range from -1 to 1.
		//These values allow us to choose between the cardinal directions: up, down, left and right.
		int xDir = 0;
		int yDir = 0;

		// These statements determine the enemy movement direction.
		// This is very basic and will need to be changed.
		if (ran == 0) {
			xDir = 1;
		}
		if (ran == 1) {
			xDir = -1;
		}
		if (ran == 2) {
			yDir = 1;
		}
		if (ran == 3) {
			yDir = -1;
		}

		//If the difference in positions is approximately zero (Epsilon) do the following:
		//if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
			
			//If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
		//	yDir = target.position.y > transform.position.y ? 1 : -1;
		
		//If the difference in positions is not approximately zero (Epsilon) do the following:
		//else
			//Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
		//	xDir = target.position.x > transform.position.x ? 1 : -1;
		
		//Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
		AttemptMove <Player> (xDir, yDir);
	}

	//OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
	//and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
	protected override void OnCantMove <T> (T component)
	{
		//Declare hitPlayer and set it to equal the encountered component.
		Player hitPlayer = component as Player;


		
		//Set the attack trigger of animator to trigger Enemy attack animation.
		//animator.SetTrigger ("enemyAttack");
		
	}

}
