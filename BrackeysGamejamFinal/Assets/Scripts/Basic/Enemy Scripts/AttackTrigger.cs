using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public static Action OnEPress;

    [Header("Transition Values")]
    public string attackScene = "AttackScene";
    public SceneTransition sceneTransition;

    [HideInInspector]
    public bool interacting = false, press = false;

    void Update()
    {
        if (interacting) { return; }

        if (!press) { return; }

        if (Input.GetKeyDown(KeyCode.E))
        {
            interacting = true;
            sceneTransition.FadeTo(attackScene);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            press = true;

            //save the game object as the last enemy
            Enemy enemy = gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.AssignAsLastEnemy();
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            press = false;

            //set the last enemy as empty
            EnemySave.Instance.AssignLastEnemy(null);
        }
    }
}
