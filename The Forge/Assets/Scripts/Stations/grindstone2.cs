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

    public override void on_interact(player Player)
    {
        base.on_interact(Player);
        player_num = Player.index;
        is_playing = true;
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
            sound_manager.update_loop(sound_manager.instance.whetstone_loop, true);

            float x_input = input.p[player_num].h_axis;
            float y_input = input.p[player_num].v_axis;

            if (y_input >= 0.7f)
            {
                if (prev == null)
                {
                    cycle += 1;
                    prev = "Up";
                } else if (cycle == 4 && prev == "Left")
                {
                    count += 1;
                    if (count == 3)
                    {
                        complete_items_swap();
                        cycle = 0;
                        count = 0;
                        is_playing = false;
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
}
