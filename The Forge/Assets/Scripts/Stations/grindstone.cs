using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class grindstone : MonoStation {

    // Public fields
    public GameObject completed_indicator;

    // Private vars
    private player current_owner;
    private bool finished_grinding;

    private int index;

    private TextMeshPro player_indicator;


    // Init
    private void Awake() {
        player_indicator = completed_indicator.GetComponentInChildren<TextMeshPro>();

        current_owner = null;
        finished_grinding = false;
        completed_indicator.SetActive(false);
    }

    public override void on_interact(player Player) {
        if (current_owner == null) {
            base.on_interact(Player);
            take_ingredients_only();
            start_grinding(Player);
        } else if (Player == current_owner) {
            user = Player;
            complete_items_swap();
            finished_grinding = false;
            current_owner = null;
            completed_indicator.SetActive(false);
        } else {
            Debug.LogError("Unexpected interaction with forge");
        }
    }

    // Start the grindstone
    private void start_grinding(player Player)
    {
        index = Player.index;
        float x_input = input.p[index].h_axis;
        float y_input = input.p[index].v_axis;

        current_owner = Player;
        finished_grinding = false;
        int counter = 0;
        int cycle = 0;
        for (int i = 0; i <= 3; i++) {
            if (y_input > 0.1) {
                cycle++;
                print("UP");
                if (x_input > 0.1) {
                    cycle++;
                    print("RIGHT");
                    if (y_input < -0.1) {
                        cycle++;
                        print("DOWN");
                        if (x_input < -0.1) {
                            counter++;
                            print("LEFT");
                            print(counter);
                            cycle = 0;
                            if (counter == 3) {
                                counter = 0;
                                on_done_grinding();
                            }
                        }
                    } else {
                        cycle = 0;
                    }
                } else {
                    cycle = 0;
                }
            } else {
                cycle = 0;
            }
        }
    }

    // Called when the product is done grinding
    private void on_done_grinding() {
        finished_grinding = true;
        completed_indicator.SetActive(true);
        player_indicator.text = player.get_indicator_string(current_owner.index);
        sound_manager.update_loop(sound_manager.instance.furnace_loop, false);
    }

    // More specific can_interact
    public override bool can_interact(player Player)
    {
        return (current_owner == null && base.can_interact(Player)) || (current_owner == Player && finished_grinding);
    }
}
