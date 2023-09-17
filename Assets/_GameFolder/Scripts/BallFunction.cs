using DG.Tweening;
using UnityEngine;
using System.Collections;

public class BallFunction : MonoBehaviour
{
    [SerializeField] private float throwforce = 10f;
    [SerializeField] private float timeAnim;
    [SerializeField] private CircleCollider2D col;
    [SerializeField] private Rigidbody2D ballrb;
    [SerializeField] private Transform PlayerHand;
    [SerializeField] private bool isBusy;
    [SerializeField] private bool isFixedOnTarget;
    [SerializeField] private bool isReturning;
    [SerializeField] private bool isInPlayerHand;
    [SerializeField] private Vector2 targetClickPosition;
    [SerializeField] private Vector2 mousePosition;
    private Vector2 atualPosition;
    private Vector2 initialPosition;
    private float distanceBetween;

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

        if (isBusy)  //se a booleana estiver ativa
        {
            distanceBetween = Vector2.Distance(atualPosition, targetClickPosition); //verificará a distancia entre os 2 vetores

            if(distanceBetween <= 0.1f) //se esses vetores estiverem proximos um do outro, rodará as instruções
            { 
                col.isTrigger = false;
                isFixedOnTarget = true;
                Debug.Log("Chegou e fixou");
                isBusy = false;
            }
        }
        else if (isReturning)
        {
            distanceBetween = Vector2.Distance(atualPosition, initialPosition);

            if(distanceBetween <= 0.1f)
            {
                isInPlayerHand = true;
                Debug.Log("Parou na mão");
                isReturning = false;
                col.isTrigger = true;
            }
        }


        
        if (Input.GetButtonDown("Fire1") && !isBusy && !isReturning)
        {
            if (isInPlayerHand)
            {
                mousePosition = Input.mousePosition;
                isBusy = true;
                targetClickPosition = Camera.main.ScreenToWorldPoint(mousePosition); //identifica que o target vai ser baseado na tela de onde esta a camera para o mundo
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
        if (isReturning)
        {
            ReturnBall();
        }
        
    }

    void ThrowBall()
    {
        //Debug.Log("Ta Indo");
        isInPlayerHand = false;
        ballrb.position = Vector2.Lerp(atualPosition, targetClickPosition, Time.deltaTime * throwforce);
        ballrb.transform.parent = null;
    }

    void ReturnBall()
    {
        //Debug.Log("Ta retornando");
        isFixedOnTarget = false;
        transform.SetParent(PlayerHand);
        transform.position = Vector2.Lerp(atualPosition, initialPosition, Time.deltaTime * throwforce);
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D ballCollision)
    {
        if (ballCollision.CompareTag("Enemy") && col.isTrigger)
        {
            Debug.Log("Virou colisor");
            col.isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D ballColision)
    {
        if(ballColision.collider.CompareTag("Player") || ballColision.collider.CompareTag("Enemy"))
        {
            StartCoroutine(BallScaleAnim(timeAnim));
        }
    }

    private IEnumerator BallScaleAnim(float time)
    {
        transform.DOScale(1.4f, time);
        yield return new WaitForSeconds(time);
        transform.DOScale(0.7f, time);
    }
}
