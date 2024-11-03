using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Trigger : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Player_Entity player_Entity = other.GetComponent<Player_Entity>();

        if (player_Entity)
        {
            player_Entity.Death();
            return;
        }

        DragAndPush_Interactable dragAndPush_Interactable = other.GetComponent<DragAndPush_Interactable>();

        if (dragAndPush_Interactable)
        {
            dragAndPush_Interactable.Destroyed();
            return;
        }
    }
}
