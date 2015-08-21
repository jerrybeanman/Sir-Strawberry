using UnityEngine;
using System.Collections;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
	private Animator animator;                  //Used to store a reference to the Player's animator component.
	private int health;
	public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.
    public static Player instance;
	
	
	
    public PlayerStat playerStat;
	private Vector3 position;
	//private Transform transform;
	private Vector2 touchOrigin = -Vector2.one;


    protected void Awake()
    {
        // Singleton design pattern. If gameManager doesnt exist, this becomes our manager.
        // If it does exist, do not overwrite it.
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

	//Start overrides the Start function of MovingObject
	protected override void Start ()
	{
		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();
		
		//Call the Start function of the MovingObject base class.
		base.Start ();
	}
	
	
	
	
	private void Update ()
	{
		if (!GameManager.manager.playersTurn)
			return;

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

		// This block controls the player animations. Im sure it can be done more effectively in the future. 
		if(horizontal == 0 && vertical == 0) {
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
		// If the player is moving right
		if(horizontal == 1) {
			if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveRight")) {
				animator.SetTrigger("playerRight");
			}
		}
		// If the player is moving left
		if(horizontal == -1) {
			if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveLeft")) {
				animator.SetTrigger("playerLeft");
			}
		}
		// If the player is moving up (or forward)
		if(vertical == 1) {
			if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveForward")) {
				animator.SetTrigger("playerUp");
			}
		}
		// If the player is moving down (or back)
		if(vertical == -1) {
			if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMoveBack")) {
				animator.SetTrigger("playerDown");
			}
		}
		#endif

		// Code for mobile. HAS NOT BEEN TESTED

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
	}
	
	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove <T> (int xDir, int yDir)
	{		
		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		base.AttemptMove <T> (xDir, yDir);
		
		//Hit allows us to reference the result of the Linecast done in Move.
		RaycastHit2D hit;
		
		//If Move returns true, meaning Player was able to move into an empty space.
		if (Move (xDir, yDir, out hit)) 
		{

			//Call sound effects as needed
		}

		//Set the playersTurn boolean of GameManager to false now that players turn is over.
		GameManager.manager.playersTurn = false;
	}
	
	// Not currently used.

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
	
	// Not currently used.

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other)
	{
		//Check if the tag of the trigger collided with is Exit.
		if(other.tag == "Exit")
		{
			//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
			//Invoke ("Restart", restartLevelDelay);
			
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
	
	// Not currently used.

	//Restart reloads the scene when called.
	private void Restart ()
	{
		//Load the last scene loaded, in this case Main, the only scene in the game.
		Application.LoadLevel (Application.loadedLevel);
	}
	
	// Not currently used.

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
	
	// Not currently used.

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

    public string StatToString()
    {
        string s = "";
        s += "<b><color=#4DA4BF>" + "HP: " + "</color></b>" + playerStat.CurrentHp + "/" + playerStat.MaxHp + "\n\n";
        s += "<b><color=#4DA4BF>" + "MP: " + "</color></b>" + playerStat.CurrentMp + "/" + playerStat.MaxMp + "\n\n";
        s += "<b><color=#4DA4BF>" + "ATK: " + "</color></b>" + playerStat.AtkPower + "\n\n";
        s += "<b><color=#4DA4BF>" + "SPD: " + "</color></b>" + playerStat.AtkSpeed + "\n\n";
        return s;

    }
    [System.Serializable]
    public class PlayerStat
    {
        public float CurrentHp;
        public float MaxHp;
        public float CurrentMp;
        public float MaxMp;
        public float AtkPower;
        public float AtkSpeed;
    }

}