using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PathFollower : MonoBehaviour
{
    public AnimationCurve anim_ease;

    TweenCallback groundCallback;
    Node[] PathNode;
    GameObject Player; //object which moves along the path
    Rigidbody playerRB;
    public float MoveSpeed; //speed along the path
    float Timer; //default timer
    public int CurrentNode; // 
    public Vector3 CurrentPositonHolder;
    // Start is called before the first frame update
    int tempNode;//a temporary node to hold previous node
    Animator player_animController;
    public bool isGrounded = true;

    public GameObject targetObject;
    public TrapInteractions trap_Script;

    public bool trapCollided = false;

    PlayerRagdoll playerRagdoll;

    public Node lastCheckPoint;

    void Awake()
    {
      
    }

    void Start()
    {
       trap_Script = targetObject.GetComponent<TrapInteractions>();


        /*Player = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < Player.Length; i++)
        {
            playerRB = Player[i].GetComponent<Rigidbody>();
            player_animController = Player[i].GetComponent<Animator>();
        }*/

        Player = targetObject;
        playerRB = Player.GetComponent<Rigidbody>();
        player_animController = Player.GetComponent<Animator>();
       
        PathNode = GetComponentsInChildren<Node>();
        CheckNode();

        playerRagdoll = playerRB.GetComponent<PlayerRagdoll>();
    }

    public void CheckNode()
    {
        if (CurrentNode < PathNode.Length - 1)
        {
            Timer = 0;
        }

        if (PathNode[CurrentNode].tag == "Checkpoint")
        {
            lastCheckPoint = PathNode[CurrentNode];
            Debug.Log("Last Checkpoint Position:"+lastCheckPoint.transform.position);
        }

        CurrentPositonHolder = PathNode[CurrentNode].transform.position;//currentnode position to holder


    }

    public void RenewBackNode()
    {
        CurrentPositonHolder = PathNode[CurrentNode - 1].transform.position;//currentnode position to holder
    }


    void DrawLine()//point index 0 to 1, 1 to 2....
    {
        for (int i = 0; i < PathNode.Length; i++)
        {
            if (i < PathNode.Length - 1)
            {
                Debug.DrawLine(PathNode[i].transform.position, PathNode[i + 1].transform.position, Color.green);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        player_animController.SetTrigger("Idle 1");
        DrawLine();

        Timer += Time.deltaTime * MoveSpeed;

        
            if (Vector3.Distance(Player.transform.position,CurrentPositonHolder)>1f)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.touches[0];



                    if (touch.phase == TouchPhase.Began)
                    {
                        //g.transform.position = Vector3.Lerp(g.transform.position, CurrentPositonHolder, Timer);
                        //g.transform.position = CurrentPositonHolder;

                        playerRagdoll.SetActiveRagdollPhysics(false);

                        var dir = PathNode[CurrentNode].transform.position - Player.transform.position; //a vector pointing from pointA to pointB
                        var rot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z), Vector3.up); //calc a rotation that



                        //if (g.transform.position.y == 0f)
                        //if (g.transform.position.y == PathNode[tempNode].transform.position.y)// check if player has landed    

                        if (isGrounded && trap_Script.isCollided == false)
                        {
                            //player_animController.SetTrigger("Jump");
                            Player.transform.DORotateQuaternion(rot, 0.5f);

                        isGrounded = false;

                            DOTweenModulePhysics.DOJump(playerRB, CurrentPositonHolder, 2f, 1, 0.25f, false)
                                //.SetEase(anim_ease)
                                .OnStart(() => { trapCollided = true;  })
                                .OnComplete(() => { isGrounded = true; trapCollided = false; CheckNode(); Player.transform.DORotateQuaternion(rot, 0.5f);    });

                            //DOTweenModulePhysics.DOLookAt(playerRB,CurrentPositonHolder,0.25f,AxisConstraint.None,Vector3.zero);


                        }
                       


                    }
                }

            }

            else
            {
                if (CurrentNode < PathNode.Length - 1)
                {

                    CurrentNode++;

                    CheckNode();
                    tempNode = CurrentNode;

                    if (CurrentNode == PathNode.Length - 1)
                    {
                    SceneManager.LoadScene("level__2", LoadSceneMode.Single);
                    /* DOTweenModulePhysics.DOJump(playerRB, PathNode[0].transform.position, 1.5f, 1, 0.315f, false).OnStart(() => { isGrounded = false; }).OnComplete(() => { isGrounded = true; });
                     CurrentNode = 0;
                     CurrentPositonHolder = PathNode[CurrentNode].transform.position;
                     //CurrentNode -= 2;*/
                }
                }


            }

        
        Debug.Log("Grounded:" + isGrounded);
    }

    public void LoadLevel1()
    {    
        SceneManager.LoadScene("level__1", LoadSceneMode.Single);
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene("level__2", LoadSceneMode.Single);
    }



}
