using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_Interact : Base_Interactable
{
    public override void Interacted(Player_Interact player_Interact)
    {
        base.Interacted(player_Interact);

        Game_Manager.instance.setting_Manager.OpenSetting();
    }
}
