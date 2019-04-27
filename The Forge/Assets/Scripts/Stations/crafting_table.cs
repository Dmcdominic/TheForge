using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crafting_table : MonoStation
{
    public GameObject indic;
    public GameObject bar;

    public bool is_playing = false;
    public int player_num;
    public int total_num;
    public int boost_size;

    private bool is_finished = false;


    public override void on_interact(player Player)
    {
        base.on_interact(Player);
        // todo - play minigame?
        player_num = Player.index;
        is_playing = true;
    }

    private void Start()
    {
        indic.SetActive(false);
        bar.SetActive(false);
        is_playing = false;
    }


    // Called every frame
    protected override void Update()
    {
        base.Update();
        // Your stuff here
        is_finished = false;

        if (is_playing)
        {
            indic.SetActive(true);
            bar.SetActive(true);
            is_finished = GameObject.Find("indicator").GetComponent<crafting_table_indicator>().finished;
            Debug.Log(is_finished);
            Debug.Log("been heeeeeeeeere");
            if (is_finished)
            {
                complete_items_swap();
                is_playing = false;
                is_finished = false;

            }
        }
        else
        {
            indic.SetActive(false);
            bar.SetActive(false);
        }

    }



}


   
