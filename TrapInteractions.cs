using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TrapInteractions : MonoBehaviour
{
    Rigidbody playerRB;
    GameObject player;
    GameObject[] trap;
    Collider[] trap_cols;

    bool jumperFlag = false;

    public GameObject pathFollower;
    PathFollower script;

    Node[] PathNode;

    public int trapInt_CurrentNode;

    public bool isCollided = false;

    PlayerRagdoll ragdollActivate;

    public Quaternion originialRotation;


    void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        ragdollActivate = GetComponent<PlayerRagdoll>();

        script = pathFollower.GetComponent<PathFollower>();

        PathNode = script.GetComponentsInChildren<Node>();
    }
    // Start is called before the first frame update
    void Start()
    {


        originialRotation = playerRB.transform.rotation;





        ragdollActivate.SetActiveRagdollPhysics(false);




    }





    void OnTriggerEnter(Collider other)
    {

        //if (!isTrapTriggered)
        //{
        if (other.gameObject.tag == "Trap"|| other.gameObject.tag == "turningTrap" || other.gameObject.tag == "loopTrap")
        {
            // playerRB.transform.DOPunchPosition(new Vector3(0, 0, 100f * Time.deltaTime), 1, 5, 1, false);
            //playerRB.transform.DOPunchRotation(new Vector3(0, 0, 100f * Time.deltaTime), 1, 10, 1);
            // isTrapTriggered = true;
            // trapInt_CurrentNode = script.CurrentNode;

            /* player.GetComponent<Animator>().enabled = false;
             playerRB.detectCollisions = true;    
             playerRB.DOJump(PathNode[(script.CurrentNode) - 2].transform.position, 0.75f, 2, 0.315f, false).SetEase(script.anim_ease)

                 .OnStart(() =>
                 {

                     script.trapCollided = false;
                     pathFollower.GetComponent<PathFollower>().CheckNode();
                     script.trapCollided = true;

                 })
                 .OnPlay(()=> {

                 script.CurrentNode--;

                 }

                 )

                 .OnComplete(() =>
                 {

                     pathFollower.GetComponent<PathFollower>().CheckNode();
                     script.trapCollided = false;
                 });

             isCollided = true;*/

            script.isGrounded = false;
            pathFollower.GetComponent<PathFollower>().enabled = false;
            ragdollActivate.SetActiveRagdollPhysics(true);



            playerRB.AddForce(new Vector3(Random.Range(-4000f,-3000), 0, Random.Range(-4000f, 4000f)) + new Vector3(0, 4000f, 0));
            playerRB.AddTorque(new Vector3(0, 100f, 0));


            if (playerRB.velocity == Vector3.zero)
            {
                Debug.Log("velocity ifine girdi.");
                StartCoroutine(SpawnAtLastCheckpoint());




            }




            //playerRB.AddForce(-transform.forward*4000f+ new Vector3(0,4000f,0));





        }

        else if (other.gameObject.tag == "Jumper")
        {
            var meshJumper = other.gameObject.GetComponentInChildren<MeshRenderer>();
            var meshJumperY = meshJumper.bounds.size.y;                       // CALCULATION OF THE COMPRESS RATIO 
            var meshCompressDiff = (meshJumperY)- (meshJumperY * 0.2f);    //
            var meshOriginalScaleY = other.gameObject.transform.localScale.y;
            var meshCompressedScaleY = other.gameObject.transform.localScale.y * 0.2f;

            var playerRBmoveInY = meshCompressDiff; // INIT DIFFERENCE TO MOVEMENT IN Y FOR RB
            var playerOriginPosY = playerRB.transform.position.y;
            var playerMoveToInY = playerOriginPosY - playerRBmoveInY;

            var jumperRB=other.GetComponent<Rigidbody>();
            var originY = other.gameObject.transform.localScale.y;
            // other.gameObject.transform.DOScaleY(0.398f,0.35f).OnComplete(()=> { });

            pathFollower.GetComponent<PathFollower>().enabled = false;

            Sequence jumperSeq = DOTween.Sequence();

            if (!jumperFlag)
            {
                jumperSeq
                    .Append(other.gameObject.transform.DOScaleY(meshCompressedScaleY, 0.15f).SetEase(Ease.OutQuart))
                    .Join(playerRB.DOMoveY(playerMoveToInY, 0.15f).SetEase(Ease.OutQuart))

                    .Append(other.gameObject.transform.DOScaleY(meshOriginalScaleY, 0.12f).SetEase(Ease.InQuart))
                    .Join(playerRB.DOMoveY(playerOriginPosY, 0.12f).SetEase(Ease.InQuart))
                   
                    .Append(DOTweenModulePhysics.DOJump(playerRB, PathNode[(script.CurrentNode)+2].transform.position, 6f, 1, 0.50f, false)

                                                                .OnStart(() => { script.isGrounded = false; script.trapCollided = true; script.CheckNode(); })
                                                                .OnComplete(() => { script.CurrentNode+=2; script.isGrounded = true; script.trapCollided = false; script.CheckNode(); }))
                    
                    .OnStart(()=> {  })
                    .OnUpdate(()=> { jumperFlag = true; })
                    .OnComplete(()=> {
                        jumperFlag = false;
                        
                        pathFollower.GetComponent<PathFollower>().enabled = true;
                    })
                
                ;
            }
            
           
            
        }
        //}



    }

    void OnTriggerExit(Collider other)
    {
        //  if (other.gameObject.tag == "Trap"/*&& !isTrapLeft*/)
        // {
        //     DOTweenModulePhysics.DOJump(playerRB,PathNode[(script.CurrentNode)-1].transform.position  , 0.75f, 2, 0.315f, false).SetEase(script.anim_ease);
        //     Debug.Log("previous node:"+ (script.CurrentNode-1));
        // isTrapLeft = true;

        //  }
        //trapInt_CurrentNode = trapInt_CurrentNode - 1;
        /*if (other.gameObject.tag == "Trap")
        {
            isCollided = false;
            script.trapCollided = false;
        }*/





    }

    void OnTriggerStay(Collider other)
    {
        /*if (other.gameObject.tag == "Trap")
        {
            isCollided = true;
        }


        Debug.Log("isCollided:(stay)" + isCollided);*/
    }



    IEnumerator SpawnAtLastCheckpoint()
    {
        yield return new WaitForSeconds(3);

        playerRB.transform.position = script.lastCheckPoint.transform.position;
        transform.rotation = originialRotation;

        ragdollActivate.SetActiveRagdollPhysics(false);

        Debug.Log("Couroutine claısıyor.");

        script.CurrentNode = script.lastCheckPoint.transform.GetSiblingIndex();

        jumperFlag = false;

        //playerRB.DOLookAt(PathNode[script.CurrentNode+1].transform.position-transform.position,0,AxisConstraint.None,null);

        script.CheckNode();

        script.isGrounded = true;

        playerRB.Sleep();

        pathFollower.GetComponent<PathFollower>().enabled = true;

    }
}
