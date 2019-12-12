using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    float maxJumpHeight = 3.0f;
    float groundHeight;
    Vector3 groundPos;
    float jumpSpeed = 7.0f;
    float fallSpeed = 12.0f;
    public bool inputJump = false;
    public bool grounded = true;

    Animator Chr00_aController;

    
    public float thrust=5f;
    

    // Start is called before the first frame update
    void Start()
    {
        Chr00_aController = GetComponent<Animator>();

        groundPos = transform.position;
        groundHeight = transform.position.y;
        maxJumpHeight = transform.position.y + maxJumpHeight;

        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            

            if (touch.phase == TouchPhase.Began)
            {
                //Chr00_aController.Set[Bool](isIdle, true);

               


                if (grounded)
                {
                    groundPos = transform.position;
                    inputJump = true;
                    Chr00_aController.SetTrigger("Jump");
                    
                    StartCoroutine("Jump_p");
                    
                }



            }
           
        }

        if (transform.position == groundPos)
            grounded = true;
        else
            grounded = false;

        if (grounded)
        {
            Chr00_aController.SetTrigger("Idle");
        }
    }


    IEnumerator Jump_p()
    {
        
        while (true)
        {
            if (transform.position.y >= maxJumpHeight)
                inputJump = false;
            if (inputJump)
            {               
                transform.Translate(Vector3.up * jumpSpeed * Time.smoothDeltaTime);
                transform.position += transform.forward * 0.5f;
            }
            else if (!inputJump)
            {
                transform.Translate(Vector3.down * fallSpeed * Time.smoothDeltaTime);
                if (transform.position.y < groundPos.y)
                {
                    
                    //transform.position = groundPos;
                    StopAllCoroutines();

                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    
}