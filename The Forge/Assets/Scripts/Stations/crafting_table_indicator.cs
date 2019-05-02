using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crafting_table_indicator : MonoBehaviour
{
    public GameObject indic;
    public GameObject bar;

    public GameObject center;
    public GameObject edge_left;
    public GameObject edge_right;

    public SpriteRenderer icon_normal;
    public SpriteRenderer icon_pressed;

    public SpriteRenderer bar_location;
    public spritesheet bar_sprites;
    private static readonly int progress_num = 10;
    public int indicator_speed;
    private int speed_factor;

    private int total_num;
    private int boost_size;
    private int miss_num;

    private int direction;
    private int index;

    private int counter;
    private int miss_counter;
    private bool in_center;
    private bool is_playing;

    private bool been_hit;

	// Component references
	private crafting_table crafting_Table;

    public bool finished = false;

    private void Start()
    {
        direction = 1;
        counter = 0;
        speed_factor = 4;
		crafting_Table = GameObject.Find("crafting_table").GetComponent<crafting_table>();
		total_num = crafting_Table.total_num;
        miss_num = crafting_Table.miss_num;
        boost_size = crafting_Table.boost_size;
        bar_location.enabled = false;
        icon_normal.enabled = false;
        icon_pressed.enabled = false;
    }

    void Update()
    {
        // Your stuff here
        finished = false;
        indic.transform.position += Vector3.right * direction * Time.deltaTime * (indicator_speed/speed_factor);

        is_playing = GameObject.Find("crafting_table").GetComponent<crafting_table>().is_playing;
		int player_num = crafting_Table.player_num;

		if (in_center && input.p[player_num].hand_tool && is_playing)
        {
            if (!been_hit)
            {
                counter += boost_size;
                been_hit = true;
            }
        }
        if (is_playing && (counter >= total_num || miss_counter > miss_num))
        {

            counter = 0;
            miss_counter = 0;
            finished = true;
        }
        if (!in_center)
        {
            been_hit = false;
            //lose some progress if hit outside the red zone
            if (input.p[player_num].hand_tool)
            {
                counter -= boost_size;
                if (counter < 0)
                    counter = 0;
            }
        }
        if (!is_playing)
        {
            counter = 0;
            miss_counter = 0;
        }

        //display the progress bar
        if (is_playing)
        {
            bar_location.enabled = true;
            index = Mathf.FloorToInt((float)counter / ((float)total_num / (float)progress_num));
            bar_location.sprite = bar_sprites.sprites[index];
        }

        //display the icon
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        in_center = false;
        if (((collision.name == "edge_right") || (collision.name == "edge_left")) && is_playing)
        {
            direction = -direction;
            miss_counter += 1;
        }
        if ((collision.name == "center"))
        {
            in_center = true;
        }
    }
}
