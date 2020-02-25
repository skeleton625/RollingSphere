using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private GameObject DirectionUI;
    [SerializeField]
    private Rigidbody BallBody;
    [SerializeField]
    private Camera MainCamera;
    [SerializeField]
    private float BallForce;

    private bool IsCharConnect;
    private Transform DirectionTrans;

    private Ray MouseRay;
    private Vector3 DestPos;
    private RaycastHit HitInfo;
    private IEnumerator DirectionCoroutine;
    
    // Start is called before the first frame update
    void Awake()
    {
        DirectionCoroutine = DirectionRotate();
        DirectionTrans = DirectionUI.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveBallByMouse();
    }

    private void MoveBallByMouse()
    {
        if(Input.GetMouseButtonDown(0))
        {
            DirectionUI.SetActive(true);
            StartCoroutine(DirectionCoroutine);
        }
        if(Input.GetMouseButton(0))
        {
            MouseRay = MainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(MouseRay.origin, MouseRay.direction, out HitInfo, 100f))
            {
                if (HitInfo.transform.tag.Equals("Ground"))
                    DestPos = HitInfo.point;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            DirectionUI.SetActive(false);
            StopCoroutine(DirectionCoroutine);
            BallBody.velocity = (DestPos - transform.position).normalized * BallForce;
        }
            
    }

    private IEnumerator DirectionRotate()
    {
        while(true)
        {
            Vector3 _xz = DestPos;
            _xz.y = 1f;

            transform.LookAt(_xz);
            yield return null;
        }
    }
}
