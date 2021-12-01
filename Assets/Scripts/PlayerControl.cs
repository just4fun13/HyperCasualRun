using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Plane m_Plane;

    public delegate void PickedCoinEvent();
    public static event PickedCoinEvent PickedCoin;
    public delegate void LevelFinishedEvent();
    public static event LevelFinishedEvent LevelFinished;

    Vector3 destinationPoint;
    bool isMoving = false;
    float journeyTime = 0f;
    public void OnPointerDown(BaseEventData data)
    {
    }
    public void OnDrag(BaseEventData data)
    {
    }

    public void OnPointerUp(BaseEventData data)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //Initialise the enter variable

        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Ground")))
        {
            destinationPoint = hit.point;
            transform.LookAt(new Vector3(destinationPoint.x, transform.position.y, destinationPoint.z));
            rb.velocity = transform.forward * 2f;
            journeyTime = Time.time + (transform.position - destinationPoint).magnitude / 2f;
            if (!isMoving) StartCoroutine(MoveTo());
        }
    }

    IEnumerator MoveTo()
    {
        isMoving = true;
        animator.SetBool("moving", true);
        while (journeyTime > Time.time)
        yield return new WaitForEndOfFrame();
        rb.velocity = Vector3.zero;
        isMoving = false;
        animator.SetBool("moving", false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Coin"))
        {
            if (PickedCoin != null)
                PickedCoin();
            Destroy(col.gameObject);
        }
        if (col.gameObject.CompareTag("Finish"))
        {
            if (LevelFinished != null)
                LevelFinished();
            animator.Play("dance");
            rb.velocity = Vector3.zero;
        }
    }
}
