using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject BlockField;
    [SerializeField]
    private GameObject SplitBlock;
    [SerializeField]
    private string SensibleTag;
    [SerializeField]
    private Animator BlockAnim;

    private UIManager UManager;
    private GameManager GManager;

    private void OnEnable()
    {
        BlockAnim.Play("Block_Up", 0, 0.25f);
    }

    private void Start()
    {
        GManager = GameManager.instance;
        UManager = UIManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(SensibleTag))
        {
            SplitBlock.transform.position = transform.position;
            SplitBlock.SetActive(true);

            GManager.PushPreBlock(gameObject);
            UManager.RenewScore(1);
            gameObject.SetActive(false);
        }
        else if (other.tag.Equals("Block"))
            GManager.ReGenerateBlock(BlockField);
    }
}
