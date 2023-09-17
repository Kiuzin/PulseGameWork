using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFunction : MonoBehaviour
{
    [SerializeField] private float throwforce = 10f;
    [SerializeField] private CircleCollider2D col;
    [SerializeField] private Rigidbody2D ballrb;
    [SerializeField] private Vector2 target;
    [SerializeField] private Transform PlayerHand;
    [SerializeField] private bool isBusy;
    [SerializeField] private bool isFixedOnTarget;
    [SerializeField] private bool isReturning;
    [SerializeField] private bool isInPlayerHand;
    [SerializeField] private Vector2 atualPosition;
    [SerializeField] private Vector2 worldPosition;
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] private float distanceBetween;

    private void Awake()
    {

        Debug.Log(transform.parent.position);
        ballrb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        isFixedOnTarget = false;
        isBusy = false;
        isReturning = false;
        isInPlayerHand = true;
    }

    private void Update()
    {
        initialPosition = PlayerHand.position;
        atualPosition = transform.position;  //identifica onde o objeto está       

        //Debug.Log(target);  //debug

        if (atualPosition == target)  //se a posição do objeto for igual a do target, rodará as instruçoes
        {
            //col.isTrigger = false;
            isFixedOnTarget = true;
            isBusy = false;
        }
        else if (atualPosition == initialPosition)
        {
            isReturning = false;
            isInPlayerHand = false;
        }
        
        if (Input.GetButtonDown("Fire1") && !isBusy && !isReturning)
        {
            if (isInPlayerHand)
            {
                worldPosition = Input.mousePosition;
                isBusy = true;
                target = Camera.main.ScreenToWorldPoint(worldPosition); //identifica que o target vai ser baseado na tela de onde esta a camera para o mundo
            }
            else if (isFixedOnTarget)
            {
                isReturning = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isBusy)
        {
            ThrowBall();
        }
        if (isFixedOnTarget)
        {
            Debug.Log("Ta fixo");
            
            col.isTrigger = false;
        }
        if (isReturning)
        {
            ReturnBall();
            //StartCoroutine(testeNumber());
        distanceBetween = Vector2.Distance(atualPosition, initialPosition);
        }
        if(isInPlayerHand)
        {
            isReturning = false;
            Debug.Log("Ta Na mão do player");
            col.isTrigger = true;
        }
        
    }

    void ThrowBall()
    {
        Debug.Log("Ta Indo");
        isInPlayerHand = false;
        ballrb.position = Vector2.Lerp(atualPosition, target, Time.deltaTime * throwforce);
        ballrb.transform.parent = null;
    }

    void ReturnBall()
    {
        Debug.Log("Ta retornando");
        isFixedOnTarget = false;
        transform.SetParent(PlayerHand);
        transform.position = Vector2.Lerp(atualPosition, initialPosition, Time.deltaTime * throwforce);
        col.isTrigger = true;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
