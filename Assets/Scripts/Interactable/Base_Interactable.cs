using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Interactable : MonoBehaviour
{
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Interacted(Player_Interact player_Interact)
    {
        
    }

    public virtual void StopInteract(Player_Interact player_Interact)
    {

    }

    #region Getting Variables

    public BoxCollider BoxCollider()
    {
        return boxCollider;
    }

    #endregion
}
