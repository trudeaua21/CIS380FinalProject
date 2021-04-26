using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    bool haveKey;
    bool haveSkill;
    public GameObject keySprite;
    public GameObject swordSkill;
    // Start is called before the first frame update
    void Start()
    {
        haveKey = false;
        haveSkill = false;
        keySprite.SetActive(false);
        swordSkill.SetActive(false);
        Debug.Log("Inventory started properly");
    }

    // Update is called once per frame
    void playerGetKey()
    {
        haveKey = true;
        keySprite.SetActive(true);
    }

    private void playerGetSkill()
    {
        haveSkill = true;
        swordSkill.SetActive(true);
    }
}
