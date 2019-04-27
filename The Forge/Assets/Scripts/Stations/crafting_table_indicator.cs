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

    private int total_num;
    private int boost_size;

    private int direction;

    private int counter;
    private bool in_center;
    private int player_num;
    private bool is_playing;

    public bool finished = false;

    private void Start()
    {
        direction = 1;
        counter = 0;
        total_num = GameObject.Find("crafting_table").GetComponent<crafting_table>().total_num;
        player_num = GameObject.Find("crafting_table").GetComponent<crafting_table>().player_num;
        boost_size = GameObject.Find("crafting_table").GetComponent<crafting_table>().boost_size;
    }

    void Update()
    {
        // Your stuff here
        finished = false;
        Debug.Log("beeeeeeeeeeen here");

        indic.transform.position += Vector3.right * direction * Time.deltaTime;

        is_playing = GameObject.Find("crafting_table").GetComponent<crafting_table>().is_playing;
      
        if (in_center && input.p[player_num].hand_tool && is_playing)
        {
            counter += boost_size;

        }
        if (is_playing && counter > total_num)
        {

            counter = 0;
            finished = true;
        }

        if (is_playing == false)
        {
            counter = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        in_center = false;

        if (((collision.name == "edge_right") || (collision.name == "edge_left")) && is_playing)
        {
            direction = -direction;
            counter += 1;
        }
        if ((collision.name == "center"))
        {
            in_center = true;
        }


    }
}
