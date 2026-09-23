using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Movement_variables
    public float moveSpeed;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    #endregion

    #region Attack_variables
    public float explosionDamage;
    public float explosionRadius;
    public GameObject explosionObject;
    #endregion

    #region Physics_components
    Rigidbody2D EnemyRB;
    #endregion

    #region Unity_functions

    private void Awake() {
        EnemyRB = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        /* TODO 2.1: Call Move() if player is !null */
        if (player == null) {
            return;
        }
        Move();
    }
    #endregion

    #region Movement_functions
    private void Move()
    { 
        /* TODO 2.1: Move the enemy towards the player */
        Vector2 direction = player.position - transform.position;
        EnemyRB.velocity = direction.normalized * moveSpeed;
    }
    #endregion

    #region Attack_functions
    private void Explode()
    {
        /* TODO 2.2: Explode should Debug.Log("Tons of Damage") if the player is within explosionRadius. 
            To simulate a explosion, the enemy game object should be destroyed and spawn the explosionObj prefab in its place. 
            NOTE: You will NOT be implementing the damage in this task, print out damage using Debug.Log() in place of where the damage function would be called.
            We will implement the damage in task 3.2.
            IMPORTANT: Destroy() should be the LAST function executed. Once a game object is destroyed, it will not execute any code beyond that line. 
        */
        RaycastHit2D[] objectsInRadius = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);
        foreach (RaycastHit2D hit in objectsInRadius) {
            if (hit.transform.CompareTag("Player")) 
            {
                Debug.Log("tons of damage");
                Instantiate(explosionObject, transform.position, transform.rotation);
                hit.transform.GetComponent<PlayerController>().TakeDamage(explosionDamage);
                FindFirstObjectByType<AudioManager>().Play("Explosion");
                Destroy(this.gameObject);
            }
        }
        
        /* TODO 3.2: Call the TakeDamage() function inside of the player's PlayerController script using
            the "hit" reference variable. */
        

    }

    private void OnCollisionEnter2D(Collision2D other) {
       /* TODO 2.2: Call Explode() if enemy comes in contact with player */
       if (other.transform.CompareTag("Player")) {
            Explode();
       }
    }
    #endregion

    #region Health_functions
    public void TakeDamage(float value)
    {
       /* TODO 3.1: Adjust currHealth when the enemy takes damage
        IMPORTANT: What happens when the enemy's health reaches 0? */
        if (value >= currHealth) {
            currHealth = 0;
            Die();
        } else {
            currHealth -= value;
        }
        Debug.Log("current health: " + currHealth.ToString());
        FindFirstObjectByType<AudioManager>().Play("EnemyHurt");
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
    #endregion
}