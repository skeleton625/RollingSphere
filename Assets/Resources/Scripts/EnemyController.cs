using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameManager GManager;
    private void Start()
    {
        GManager = GameManager.instance;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            GManager.IsGameOver = true;
            Destroy(collision.gameObject);
        }
    }
}
