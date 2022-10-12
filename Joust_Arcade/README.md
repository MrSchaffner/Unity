# Joust Arcade Game

The project was to build an anthology of classic games, using Unity and C# to make porting to multiple platforms easier. I chose to work on the 1982 arcade game Joust, because it was one of my favorites and I thought it would have enough complexity to keep me busy for the sprint duration. 

## Problems solved:

### Handling Moving Off-Screen 

As in many upright arcade games, the characters in Joust can go off-screen and appear on the other side. 

![Gif of Moving off-screen in Joust Game](https://github.com/MrSchaffner/Unity/blob/master/Images_Display/joust_images/thru_walls.gif)

When the player can jump through and reappear shortly after, the implementation is simple, as in PAC-Man. But in Joust, a player can be half on one side and half on the other, with enemy collisions possible on either side of the screen. To handle this, I created a clone that moves in tandem with the original. 
I then wrote code to jump the player back exactly one screen backward, every time it collided with an offscreen trigger (shown in blue).

![Gif of How Moving off-screen was done](https://github.com/MrSchaffner/Unity/blob/master/Images_Display/joust_images/thru_wall_reveal.gif)
<code>Teleporter Class:</code>
```csharp
public class WallDetection : MonoBehaviour
{
    private float adjustmentAmt = 9.436f; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            return;
        }
        float distance;
        if (this.tag == "LeftWall") distance = adjustmentAmt; else distance = -adjustmentAmt; 
            Vector2 pos = collision.GetComponent<Transform>().parent.position;
        collision.GetComponent<Transform>().parent.position = new Vector2(pos.x + distance, pos.y);

    }
}
```

### Multiple Player States

The Ostrich in Joust has two main states: Flying and Walking. A Finite State Machine seemed the best and most extensible solution. This means that the Steed class has an attribute called State and it can only be assigned one state at any given time. All behavior is handled not by the Steed class, but is instead sent to the current state.

<code>Steed Class</code>
```csharp
public class Steed_FSM : MonoBehaviour // MonoBehavior is the base class for all Unity classes
{
    public State CurrentState;

    public void Start()
    {
        this.TransitionToState(Walking);
    }

    public void TransitionToState(State state)
    {
        if (CurrentState != null) CurrentState.ExitState();
        CurrentState = state;
        CurrentState.EnterState();
    }

    public void Update(float move, bool flapInput) //sends inputs to State
    {
        CurrentState.Update(move,flapInput);
    }

    public void Flip()
    {
        facingR = !facingR;
        foreach (SpriteRenderer Char in spriteRenderer)
        {
            Char.flipX = !Char.flipX;
        }
    }
}

```
<code>Flying State Class</code>
```csharp
public class SteedFlyingState : State
{
    Steed_FSM player;
    public bool legsOut=true; //made public for debugging
    private List<GameObject> legsList = new List<GameObject>();

    public override void Update(float move, bool flapInput)
    {
        //Check if leg colliders should be extended
        bool shouldExtendLegs = (player.landingTrigger && player.rigidbody.velocity.y < 0); 
       ExtendLegs(shouldExtendLegs);
        
        if (flapInput) // move while in air
        {
            Flap();
            if (move != 0)
            {
                Vector2 v2 = new Vector2(move > 0 ? player.flyAccelPerFrame : -player.flyAccelPerFrame, 0.0f); //apply a consistent force for each flap
                player.rigidbody.AddForce(v2);
            }
        }

        //if looking wrong way, turn around
        if ((move < 0 && player.facingR) || (move > 0 && !player.facingR))
        {
            player.Flip(); //When flying, momentum is maintained.
        }
    } 
}
```

### Complex Walking Behavior

Traditional Joust is played on an upright arcade machine, using a joystick for horizontal movement and a single jump button. Though it would have been easier to accept keyboard arrow keys for input, I wanted to use Unity's variable Axis input manager, so that the game could easily be played with a USB joystick or similar input method. 

Below is the code that controls the entire walking sequence. I have also included the original, more verbose version. I found it interesting that switching to a less-intuitive order of checking conditionals produced much cleaner code. 

<code>This script checks for direction facing first, then checks for user input:</code>
```csharp
 public override void Update(float move, bool flapInput)
    {
        // JUMPING
        if (flapInput)
        {
            Flap(); 
            return;
        }
        if (move == 0f) //No Input - exit method
        {
            if (!player.reset)
            {
                player.reset = true; //this forces player to press joystick twice to turn around
            }
            return;
        }
        
        
        if(move > 0f == player.facingR) //facing direction of input, move player
        {
            MoveHorizontal();
            return;
        } else //facing wrong way, come to a stop
        {
            if (vel().x != 0f) //need to brake and STOP
            {
                player.rigidbody.velocity = new Vector2(0f, 0f);
                player.reset = false;
                return;
            } else if(player.reset) // input opposite as facing, turn around if reset
            {
                player.Flip();
                return;
            }
        }
    }
```
<CODE>This uglier, less efficient script checks for velocity then direction:</CODE>
```csharp
 public override void Update(float move, bool flapInput)
    {
        // JUMPING
        if (flapInput)
        {
            Flap(); 
            return;
        }
        if (move == 0f) //No Input - exit method
        {
            if (!player.reset)
            {
                player.reset = true; //this forces player to press joystick twice to turn around
            }
            return;
        }

        if (vel().x == 0) //not moving
        {
           if ((inputR == player.facingR)) //input same way as facing //player.JoystickReset && 
           {
               MoveHorizontal();
           }
           else
           {
               if (player.JoystickReset) // input opposite as facing, turn around if reset
               {
                   player.Flip();
               }
           }
        }
        else //  MOVING already
        {
           bool velR = vel().x > 0;
           player.JoystickReset = false;

           if (inputR == velR) // moving direction of input
           {
               MoveHorizontal();
           }
           else // Moving opposite direction of input BRAKE
           {
               player.rigidbody.velocity = new Vector2(0f, 0f);
           }
        }
    } //END update
```

### Code Reuse

In Joust, the enemies are exactly the same as the player in terms of their abilities and actions. This motivated my decision to make a universal Steed class. 
This would make for more reusable code, but required two smaller classes, one for the user, to receive input, and one for the enemies, to send their AI.

<code>Player MoveController takes input:</code>

```csharp
public class PlayerMoveController : MoveController
{
    public override void PlayerOnCollisionEnter2D(Collision2D collision) // called manually
    {
        if (collision.gameObject.tag == "Enemy")
        {
            game.CheckJoustResult(collision, player);
            return;
        }
    }

    void Update() // Update is called once per frame
    {
        bool flapInput = false;
        if (Input.GetButton("Jump"))
        {
            flapInput = true;
        }
        float move = Input.GetAxis("Horizontal"); // 0 if no input detected
        player.Update(move, flapInput);
    }
}
```
<code>Enemy MoveController AI:</code>
```csharp
public class AIMoveController : MoveController
{
    Steed_FSM enemy;
    bool facingR;
    float deltaTimeElapsed = 0f;

    public string[] cyclePhases = new string[3] {"destination", "changeDir", "justFly" };
    float[] cycleDurations = new float[3] { 5f, 10f, 15f };
    public int phaseIndex;
    float[] destinationList = new float[4] {-3, -1.5f, 1.3f, 3.4f }; //-3 is in the lava
    public int destIndex = 2;

    public float timeElapsedInCycle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        phaseIndex = 0;
        NextPhase();
        enemy = this.GetComponent<Steed_FSM>();
        facingR = enemy.facingR;
    }

    void MoveCycle()
    {
        timeElapsedInCycle += Time.deltaTime;
        if (timeElapsedInCycle > cycleDurations[phaseIndex]) //if phase has completed
        {
            phaseIndex = (phaseIndex + 1) % cycleDurations.Length; //loop through length
            if (phaseIndex == 0)
            {
                timeElapsedInCycle = 0f; //reset
            }
            NextPhase();
        }
    }

    void NextPhase()
    {
        switch (cyclePhases[phaseIndex])
        {
            case "destination":
                pickRandomDest();
                break;
            case "changeDir":
                enemy.Flip();
                break;
            case "justFly":
                // just do the same thing for a while
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveCycle();
        float move;
        bool flapInput = false;

        if (enemy.facingR)
        {
            move = 1f;
        }
        else
        {
            move = -1f;
        }
        flapInput = CheckIfFlap();

        enemy.Update(move, flapInput);

    } //end UPDATE()

    void pickRandomDest()
    {
        destIndex =  UnityEngine.Random.Range(1,4); //leave out zero index because it is lava
    }

    bool CheckIfFlap()
    {
        //simple AI - flap if destination is above, else dont
        if (enemy.transform.position.y < destinationList[destIndex])
            return true;
        else
            return false;
    }
}

```
## Final Product
In Joust, just like the Medieval contest on Horseback, you play as a knight carrying a long lance. In the arcade classic, you instead ride an ostrich, allowing you to take to the skies and spear your enemies from above. When you make contact with an opponent, whoever is higher is the victor and the other is defeated. 

<code>The playable game at the end of the Sprint:</code>

![Gif of Joust Game in Action](https://github.com/MrSchaffner/Unity/blob/master/Images_Display/joust_images/full_game.gif)
