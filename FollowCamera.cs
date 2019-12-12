using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    public float damping = 1;
    Vector3 offset;
    public float position_Yoffset=10f;

    public GameObject pathFollower;
    PathFollower script;
    Node[] PathNode;

    void Start()
    {
        offset = transform.position - target.transform.position;

        script = pathFollower.GetComponent<PathFollower>();

        PathNode = script.GetComponentsInChildren<Node>();

        position_Yoffset = transform.position.y - target.transform.position.y;
    }


    void LateUpdate()
    {
        Vector3 desiredPosition = target.transform.position + offset;
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        Vector3 node_y = Vector3.Lerp(transform.position, new Vector3(0,PathNode[script.CurrentNode].transform.position.y + position_Yoffset,0), Time.deltaTime * damping);

        transform.position = new Vector3(position.x,node_y.y, position.z);
        
       

       
        
        

        //transform.LookAt(target.transform.position);
    }
}