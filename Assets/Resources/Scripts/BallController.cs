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

    private bool IsClicked;
    private float BallForce;
    private Transform DirectionTrans;

    private Ray MouseRay;
    private Vector3 DestPos;
    private RaycastHit HitInfo;
    private IEnumerator DirectionCoroutine;
   
    private void Awake()
    {
        DirectionCoroutine = DirectionRotate();
        DirectionTrans = DirectionUI.GetComponent<Transform>();
    }

    private IEnumerator DirectionRotate()
    {
        while(true)
        {
            BallForce = (DestPos - transform.position).magnitude;

            Vector3 _destpos = DestPos;
            Vector3 _mid = (DestPos + transform.position)/2;
            Vector3 _scale = DirectionTrans.localScale;
            _mid.y = 1f;
            _destpos.y = 1f;
            _scale.y = BallForce;

            transform.LookAt(_destpos);
            DirectionTrans.position = _mid;
            DirectionTrans.localScale = _scale;
            
            yield return null;
        }
    }

    public void MoveBallByMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DirectionUI.SetActive(true);
            StartCoroutine(DirectionCoroutine);
        }

        if (Input.GetMouseButtonUp(0))
        {
            DirectionUI.SetActive(false);
            StopCoroutine(DirectionCoroutine);
            BallBody.velocity = (DestPos - transform.position).normalized * BallForce;
        }

        if (Input.GetMouseButton(0))
        {
            MouseRay = MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(MouseRay.origin, MouseRay.direction, out HitInfo, 100f))
                DestPos = HitInfo.point;
        }
    }
}
