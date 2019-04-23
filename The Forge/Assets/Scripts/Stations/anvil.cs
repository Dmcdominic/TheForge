using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anvil : MonoStation
{
    private int count = 0;
    private bool is_playing = false;

    private int player_num;
    public int button_mash_num;
	public int quickcraft_button_mash_num;

    public SpriteRenderer icon_normal;
    public SpriteRenderer icon_pressed;
    public SpriteRenderer bar_location;
    public spritesheet bar_sprites;
    private static readonly int progress_num = 10;

    public override void on_interact(player Player)
    {
        base.on_interact(Player);
        player_num = Player.index;
        is_playing = true;

    }

    private void Start()
    {
        bar_location.enabled = false;
        icon_normal.enabled = false;
        icon_pressed.enabled = false;
        count = 0;
    }

    // Called every frame
    protected override void Update()
    {
        base.Update();
        // Your stuff here
        if (input.p[player_num].hand_tool && is_playing)
        {
            count++;
			sound_manager.play_one_shot(sound_manager.instance.anvil_hit);
			//Debug.Log("count = " + count);
        }

		if (!is_playing) {
			return;
		}

		int current_button_mash_num = powerups_controller.has_powerup(user.team, powerups.quick_craft) ? quickcraft_button_mash_num : button_mash_num;
        if (count >= current_button_mash_num && is_playing) {
			complete_event();
		}

        if (is_playing)
        {
            bar_location.enabled = true;
            bar_location.sprite = bar_sprites.sprites[Mathf.FloorToInt((float)count / ((float)current_button_mash_num / (float)progress_num))];
        }

        if (input.p[player_num].hand_tool_down & is_playing)
        {
            icon_normal.enabled = false;
            icon_pressed.enabled = true;
        }
        else if (is_playing)
        {
            icon_normal.enabled = true;
            icon_pressed.enabled = false;
        }
    }

	// End the minigame
	private void complete_event(bool abort = false) {
		if (!abort) {
			complete_items_swap();
		}
		count = 0;
		is_playing = false;
		bar_location.sprite = bar_sprites.sprites[0];
		bar_location.enabled = false;
		icon_normal.enabled = false;
		icon_pressed.enabled = false;
	}

	// Reset the 
	public override void abort_items_swap() {
		complete_event(true);
		base.abort_items_swap();
	}
}