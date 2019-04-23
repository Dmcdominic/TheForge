using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grindstone2 : MonoStation
{
    private int count = 0;
    private int cycle = 0;
    private string prev = null;
    private bool is_playing = false;

    private int player_num;

	// Static settings
	private static readonly int counts_required = 3;
	private static readonly int quickcraft_counts_required = 1;

    public override void on_interact(player Player)
    {
        base.on_interact(Player);
        player_num = Player.index;
        is_playing = true;
		sound_manager.update_loop(sound_manager.instance.whetstone_loop, true);
	}

    private void Start()
    {
        count = 0;
        cycle = 0;
        prev = null;
    }

    // Called every frame
    protected override void Update()
    {
        base.Update();
        // Your stuff here
        if (is_playing)
        {
            float x_input = input.p[player_num].h_axis;
            float y_input = input.p[player_num].v_axis;

			int current_counts_required = powerups_controller.has_powerup(user.team, powerups.quick_craft) ? quickcraft_counts_required : counts_required;

			if (y_input >= 0.7f)
            {
                if (prev == null)
                {
                    cycle += 1;
                    prev = "Up";
                } else if (cycle == 4 && prev == "Left")
                {
                    count += 1;
                    if (count >= current_counts_required)
                    {
                        complete_items_swap();
                        cycle = 0;
                        count = 0;
                        is_playing = false;
						prev = null;
						sound_manager.update_loop(sound_manager.instance.whetstone_loop, false);
					}
                    else
                    {
                        cycle = 1;
                        prev = "Up";
                    }
                }
            } else if (x_input >= 0.7f && prev == "Up")
            {
                cycle += 1;
                prev = "Right";
            } else if (y_input <= -0.7f && prev == "Right")
            {
                cycle += 1;
                prev = "Down";
            } else if (x_input <= -0.7f && prev == "Down")
            {
                cycle += 1;
                prev = "Left";
            }
        }
    }

	public override void abort_items_swap() {
		base.abort_items_swap();
		cycle = 0;
		count = 0;
		is_playing = false;
		prev = null;
		sound_manager.update_loop(sound_manager.instance.whetstone_loop, false);
	}
}
