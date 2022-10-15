using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IASlug : MonoBehaviour
{
    public Transform enemie;
    public SpriteRenderer enemieSprite;
    public Transform[] position;
    public float speed;
    public bool isRight;
    public bool visivel;
    private int idTarget = 1;
    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        enemie.position = position[0].position;
        idTarget = 1;

    }

    // Update is called once per frame
    void Update()
    {
        visivel = enemie != null ? enemieSprite.isVisible : null as Transform;


        //Debug.Log(visivel);


        if (enemie == null)
        {
            return;
        }

        if (enemie != null && visivel)
        {

            visivel = true;
            isActive = true;
        }

        if (isActive)
        {
            enemie.position = Vector3.MoveTowards(enemie.position, position[idTarget].position, speed * Time.deltaTime);

            if (enemie.position == position[idTarget].position)
            {
                idTarget += 1;
                if (idTarget == position.Length)
                {
                    idTarget = 0;
                }
            }

            if (position[idTarget].position.x < enemie.position.x && isRight)
            {
                Flip();
            }
            else if (position[idTarget].position.x > enemie.position.x && isRight == false)
            {
                Flip();
            }

        }
        else
        {
            return;
        }
    }
    
    void Flip()
    {

        isRight = !isRight;
        enemieSprite.flipX = !enemieSprite.flipX;
    }
}
