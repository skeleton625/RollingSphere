using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject SplitBlock;
    [SerializeField]
    private string SensibleTag;
    [SerializeField]
    private Animator BlockAnim;
    private UIManager UManager;
    private BlockGenerator BG;

    private void OnEnable()
    {
        BlockAnim.Play("Block_Up", 0, 0.25f);
    }

    private void Start()
    {
        BG = BlockGenerator.instance;
        UManager = UIManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(SensibleTag))
        {
            SplitBlock.transform.position = transform.position;
            SplitBlock.SetActive(true);
            BG.PushPreBlock(gameObject);
            UManager.PlusScore();
            gameObject.SetActive(false);
        }
    }
}
