using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crafting_table : MonoStation
{
    public GameObject indic;
    public GameObject bar;
    public GameObject indicator_reset;

    public SpriteRenderer icon_normal;
    public SpriteRenderer icon_pressed;
    public SpriteRenderer bar_location;

    public bool is_playing = false;
    public int player_num;
    public int total_num;
    public int miss_num;
    public int boost_size;

    private bool is_finished = false;

    public override void on_interact(player Player)
    {
        base.on_interact(Player);
        // todo - play minigame?
        player_num = Player.index;
        is_playing = true;
        sound_manager.update_loop(sound_manager.instance.crafting_table_loop, true);
    }

    private void Start()
    {
        indic.SetActive(false);
        bar.SetActive(false);
        bar_location.enabled = false;
        icon_normal.enabled = false;
        icon_pressed.enabled = false;
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
            if (is_finished)
            {
                complete_event();
            }
        }
        else
        {
            indic.SetActive(false);
            bar.SetActive(false);
        }

    }

    // End the minigame
    private void complete_event(bool abort = false)
    {
        if (!abort)
        {
            complete_items_swap();
        }
        bar_location.enabled = false;
        icon_normal.enabled = false;
        icon_pressed.enabled = false;
        indic.transform.position = indicator_reset.transform.position;
        is_playing = false;
        is_finished = false;
        sound_manager.update_loop(sound_manager.instance.crafting_table_loop, false);
    }

    // Reset the minigame when you want to abort_items_swap
    public override void abort_items_swap()
    {
        complete_event(true);
        base.abort_items_swap();
    }

}
