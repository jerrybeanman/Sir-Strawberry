﻿using UnityEngine;
using System.Collections;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
	public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.
	//public int pointsPerFood = 10;              //Number of points to add to player food points when picking up a food object.
	//public int pointsPerSoda = 20;              //Number of points to add to player food points when picking up a soda object.
	//public int wallDamage = 1;                  //How much damage a player does to a wall when chopping it.
	
	
	private Animator animator;                  //Used to store a reference to the Player's animator component.
	//private int food;                           //Used to store player food points total during level.
	private int health;
	private Vector3 position;
	//private Transform transform;
	private bool isMoving = false;
	private Vector2 touchOrigin = -Vector2.one;
	
	
	//Start overrides the Start function of MovingObject
	protected override void Start ()
	{
		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();

		position = transform.position;
		//transform = transform;
		
		//Get the current food point total stored in GameManager.instance between levels.
		//food = GameManager.instance.playerFoodPoints;
		//print (food);
		
		//Call the Start function of the MovingObject base class.
		base.Start ();
	}
	
	
	//This function is called when the behaviour becomes disabled or inactive.
	private void OnDisable ()
	{
		//When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
		//GameManager.instance.playerFoodPoints = food;
	}
	
	
	private void Update ()
	{
		int up = 0;
		int down = 0;
		int left = 0;
		int right = 0;

		if (isMoving) {
			return;
		}
		isMoving = true;
		//If it's not the player's turn, exit the function.
		//if(!GameManager.instance.playersTurn) return;
		int horizontal = 0;     //Used to store the horizontal move direction.
		int vertical = 0;       //Used to store the vertical move direction.
		
		#if UNITY_EDITOR
		//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
		horizontal = (int)(Input.GetAxisRaw ("Horizontal"));

		//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
		vertical = (int)(Input.GetAxisRaw ("Vertical"));


		//Check if moving horizontally, if so set vertical to zero.
		if (horizontal != 0) {
			vertical = 0;
		}
		/*
		if (vertical != 0) {
			horizontal = 0;
		}
		*/

		if(horizontal == 0 && vertical == 0) {
			//if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdleForward")) {
			//	animator.SetTrigger("playerIdle");
			//}
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveRight")) {
				animator.SetTrigger("playerRightIdle");
			}
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveLeft")) {
				animator.SetTrigger("playerLeftIdle");
			}
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveUp")) {
				animator.SetTrigger("playerUpIdle");
			}
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveDown")) {
				animator.SetTrigger("playerDownIdle");
			}
		} 
		if(horizontal == 1) {
			if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveRight")) {
				animator.SetTrigger("playerRight");
			}
		}
		if(horizontal == -1) {
			if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveLeft")) {
				animator.SetTrigger("playerLeft");
			}
		}
		if(vertical == 1) {
			if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveForward")) {
				animator.SetTrigger("playerUp");
			}
		}
		if(vertical == -1) {
			if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveBack")) {
				animator.SetTrigger("playerDown");
			}
		}
		#endif

		#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		//Check if Input has registered more than zero touches
		if (Input.touchCount > 0)
		{
			//Store the first touch detected.
			Touch myTouch = Input.touches[0];
			
			//Check if the phase of that touch equals Began
			if (myTouch.phase == TouchPhase.Began)
			{
				//If so, set touchOrigin to the position of that touch
				touchOrigin = myTouch.position;
			}
			
			//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
			else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
			{
				//Set touchEnd to equal the position of this touch
				Vector2 touchEnd = myTouch.position;
				
				//Calculate the difference between the beginning and end of the touch on the x axis.
				float x = touchEnd.x - touchOrigin.x;
				
				//Calculate the difference between the beginning and end of the touch on the y axis.
				float y = touchEnd.y - touchOrigin.y;
				
				//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
				touchOrigin.x = -1;
				
				//Check if the difference along the x axis is greater than the difference along the y axis.
				if (Mathf.Abs(x) > Mathf.Abs(y))
					//If x is greater than zero, set horizontal to 1, otherwise set it to -1
					horizontal = x > 0 ? 1 : -1;
				else
					//If y is greater than zero, set horizontal to 1, otherwise set it to -1
					vertical = y > 0 ? 1 : -1;
			}
		}
		
		#endif 
		//End of mobile platform dependendent compilation section started above with #elif

		
		//Check if we have a non-zero value for horizontal or vertical
		if (horizontal != 0 || vertical != 0) {
			//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
			//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
			AttemptMove<TestImoveable> (horizontal, vertical);
		}
		
		StartCoroutine (WaitForMovement ());
		
	}
	
	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		//Every time player moves, subtract from food points total.
		//food--;
		
		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		base.AttemptMove <T> (xDir, yDir);
		
		//Hit allows us to reference the result of the Linecast done in Move.
		RaycastHit2D hit;
		
		//If Move returns true, meaning Player was able to move into an empty space.
		if (Move (xDir, yDir, out hit)) 
		{
			GameManager.manager.health++;
			//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
		}
		
		//Since the player has moved and lost food points, check if the game has ended.
		CheckIfGameOver ();
		
		//Set the playersTurn boolean of GameManager to false now that players turn is over.
		//GameManager.instance.playersTurn = false;
	}
	
	
	//OnCantMove overrides the abstract function OnCantMove in MovingObject.
	//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
	protected override void OnCantMove <T> (T component)
	{
		//Set hitWall to equal the component passed in as a parameter.
		//Wall hitWall = component as Wall;
		
		//Call the DamageWall function of the Wall we are hitting.
		//hitWall.DamageWall (wallDamage);
		
		//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
		//animator.SetTrigger ("playerChop");
	}
	
	
	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other)
	{
		//Check if the tag of the trigger collided with is Exit.
		if(other.tag == "Exit")
		{
			//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
			Invoke ("Restart", restartLevelDelay);
			
			//Disable the player object since level is over.
			enabled = false;
		}
		
		//Check if the tag of the trigger collided with is Food.
		else if(other.tag == "Food")
		{
			//Add pointsPerFood to the players current food total.
			//food += pointsPerFood;
			
			//Disable the food object the player collided with.
			other.gameObject.SetActive (false);
		}
		
		//Check if the tag of the trigger collided with is Soda.
		else if(other.tag == "Soda")
		{
			//Add pointsPerSoda to players food points total
			//food += pointsPerSoda;
			
			
			//Disable the soda object the player collided with.
			other.gameObject.SetActive (false);
		}
	}
	
	
	//Restart reloads the scene when called.
	private void Restart ()
	{
		//Load the last scene loaded, in this case Main, the only scene in the game.
		Application.LoadLevel (Application.loadedLevel);
	}
	
	
	//LoseFood is called when an enemy attacks the player.
	//It takes a parameter loss which specifies how many points to lose.
	public void LoseFood (int loss)
	{
		//Set the trigger for the player animator to transition to the playerHit animation.
		animator.SetTrigger ("playerHit");
		
		//Subtract lost food points from the players total.
		//food -= loss;
		
		//Check to see if game has ended.
		CheckIfGameOver ();
	}
	
	
	//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
	private void CheckIfGameOver ()
	{
		//Check if food point total is less than or equal to zero.
		/*
		if (food <= 0) 
		{
			
			//Call the GameOver function of GameManager.
			//GameManager.instance.GameOver ();
		}
		*/
	}

	IEnumerator WaitForMovement() {
		yield return new WaitForSeconds (0.1f);
		isMoving = false;
	}
}