using UnityEngine;

public class HotelSafe2 : BaseSafeHandler
{
    public string keyCode;

    public int maxCodeSize = 6;
    public int minCodeSize = 4;

    Animator doorAnimator;

    // Use this for initialization
    void Start () {

        doorAnimator = GameObject.Find("SafeDoor").GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

   

    private void InitialiseKeypad()
    {
        //Get the keys of the keypad
        if (keypad != null)
        {
            foreach (Transform key in keypad.transform)
            {
                ButtonHandler buttonHandler = key.GetComponentInChildren<ButtonHandler>();
                buttonHandler.OnButtonPressed += HandleKeyPress;
            }
        }
    }

    private void HandleKeyPress(ButtonHandler.KeyCodes key)
    {
        if (key == ButtonHandler.KeyCodes.Clear)
        {
            Clear();
        }
        else if (key == ButtonHandler.KeyCodes.Key)
        {
            Enter();
        }
        else
        {
            NumberPressed((int)key);
        }


    }

    public override void Enter()
    {
        if (keyCode == "3131")
        {
            //Unlock();
            doorAnimator.Play("OpenDoor", -1, 0f);

        }
        else
        {
            Debug.Log("틀림~.~");
        }
    }

    public override bool IsLocked()
    {
        return (!string.IsNullOrEmpty(keyCode));
    }


}
