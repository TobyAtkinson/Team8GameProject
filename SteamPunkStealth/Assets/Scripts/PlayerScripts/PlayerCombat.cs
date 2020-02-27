using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public GameObject sword;
    private Animator swordAnim;

    public enum swordState
    {
        Idle,
        FirstSwing,
        SecondSwing,
        Blocking
    }

    public bool windowForSecondSwing = false;

    public swordState currentSwordState = swordState.Idle;

    void Start()
    {
        swordAnim = sword.GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(currentSwordState == swordState.Idle)
            {
                //swordAnim.enabled = false;
                //swordAnim.enabled = true;
                // start first swing
                windowForSecondSwing = false;
                currentSwordState = swordState.FirstSwing;
                StartCoroutine(FirstSwing());

            }
            else if (currentSwordState == swordState.FirstSwing && windowForSecondSwing == true)
            {
                //swordAnim.enabled = false;
                //swordAnim.enabled = true;
                //start second swing
                currentSwordState = swordState.SecondSwing;
                windowForSecondSwing = false;
                StartCoroutine(SecondSwing());
            }
            else
            {
                // player is already doing something
            }
        }
        if (Input.GetMouseButton(1))
        {
            if(currentSwordState == swordState.Idle)
            {
                windowForSecondSwing = false;
                currentSwordState = swordState.Blocking;
                StartCoroutine(Block());
            }
            
        }
    }

    IEnumerator FirstSwing()
    {
        swordAnim.Play("SwordAttack1");
        yield return new WaitForSeconds(0.25f);
        // Activate kill barrier
        yield return new WaitForSeconds(0.15f);
        windowForSecondSwing = true;
        if(currentSwordState == swordState.FirstSwing)
        {
            yield return new WaitForSeconds(0.15f);
            // Deactive kill barrier
            yield return new WaitForSeconds(0.15f);
            windowForSecondSwing = false;
            yield return new WaitForSeconds(0.15f);
            yield return new WaitForSeconds(0.75f);
            windowForSecondSwing = false;
            currentSwordState = swordState.Idle;
        }
        
        

        //0.70 left
    }
    IEnumerator SecondSwing()
    {
        swordAnim.Play("SwordAttack2");
        windowForSecondSwing = false;
        yield return new WaitForSeconds(0.25f);
        // Activate kill barrier
        windowForSecondSwing = false;
        yield return new WaitForSeconds(0.30f);
        // Deactive kill barrier
        windowForSecondSwing = false;
        yield return new WaitForSeconds(0.3f);
        windowForSecondSwing = false;
        yield return new WaitForSeconds(0.75f);
        windowForSecondSwing = false;
        currentSwordState = swordState.Idle;

        //0.70 left
    }
    IEnumerator Block()
    {
        swordAnim.Play("SwordBlock");
        // Activate block
        yield return new WaitForSeconds(0.75f);
        // Deactive Block
        yield return new WaitForSeconds(0.50f);
        currentSwordState = swordState.Idle;

    }
}
