using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public int speed;
    public int i;
    private bool teleup, teledown;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float movehor = input.p[i].h_axis;

        float movever = input.p[i].v_axis;
        

        if (movever > 0)
        {
            if (!teleup)
                movever = movever * 40;
            else
                movever = 0;
            teleup = true;
            teledown = false;
        }
        else if (movever < 0)
        {
            if (!teledown)
                movever = movever * 40;
            else
                movever = 0;
            teledown = true;
            teleup = false;
        }
        else
        {
            movever = 0;
        }

        Vector2 movement = new Vector2(movehor, movever);

       
        transform.Translate(movement/ 10 * speed);

    }


}
