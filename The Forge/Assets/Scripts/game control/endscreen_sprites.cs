using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endscreen_sprites : MonoBehaviour
{
    // Public GameObject sprites for players (end screen)
    public GameObject Eitri1;
    public GameObject Eitri2;
    public GameObject Brokkr1;
    public GameObject Brokkr2;

    private static SpriteRenderer Eit1;
    private static SpriteRenderer Eit2;
    private static SpriteRenderer Bro1;
    private static SpriteRenderer Bro2;

    private static Sprite brokkr1;
    private static Sprite brokkr2;
    private static Sprite brokkr3;
    private static Sprite brokkr4;

    private static Sprite eitri1;
    private static Sprite eitri2;
    private static Sprite eitri3;
    private static Sprite eitri4;

    private static bool EitriHere = false;
    private static bool BrokkrHere = false;

    // Start is called before the first frame update
    void Start()
    {
        Bro1 = Brokkr1.GetComponent<SpriteRenderer>();
        Bro2 = Brokkr2.GetComponent<SpriteRenderer>();
        Eit1 = Eitri1.GetComponent<SpriteRenderer>();
        Eit2 = Eitri2.GetComponent<SpriteRenderer>();

        brokkr1 = Resources.Load<Sprite>("Assets/Art/dwarves/Dwarf 1/dwarf_sprite_green_1_idle_0");
        brokkr2 = Resources.Load<Sprite>("Assets/Art/dwarves/Dwarf 2/dwarf_sprite_green_2_idle_0");
        brokkr3 = Resources.Load<Sprite>("Assets/Art/dwarves/Dwarf 3/dwarf_sprite_green_3_idle_0");
        brokkr4 = Resources.Load<Sprite>("Assets/Art/dwarves/Dwarf 4/dwarf_sprite_green_4_idle_0");

        eitri1 = Resources.Load<Sprite>("Assets/Art/dwarves/Dwarf 1/dwarf_sprite_blue_1_idle_0");
        eitri2 = Resources.Load<Sprite>("Assets/Art/dwarves/Dwarf 2/dwarf_sprite_blue_2_idle_0");
        eitri3 = Resources.Load<Sprite>("Assets/Art/dwarves/Dwarf 3/dwarf_sprite_blue_3_idle_0");
        eitri4 = Resources.Load<Sprite>("Assets/Art/dwarves/Dwarf 4/dwarf_sprite_blue_4_idle_0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Set the player models (end screen)
    public static void set_player_models()
    {
        for (int i = 0; i <= dwarf_spawner.dwarves.Length; i++)
        {
            if (dwarf_spawner.dwarves[i].team == 0 && BrokkrHere == false)
            {
                int index = dwarf_spawner.dwarves[i].get_anim_palette_index();
                set_brokkr_sprite(Bro1, index);
                BrokkrHere = true;
            }
            else if (dwarf_spawner.dwarves[i].team == 0 && BrokkrHere == true)
            {
                int index = dwarf_spawner.dwarves[i].get_anim_palette_index();
                set_brokkr_sprite(Bro2, index);
            }
            else if (dwarf_spawner.dwarves[i].team == 1 && EitriHere == false)
            {
                int index = dwarf_spawner.dwarves[i].get_anim_palette_index();
                set_eitri_sprite(Eit1, index);
                EitriHere = true;
            }
            else if (dwarf_spawner.dwarves[i].team == 1 && EitriHere == true)
            {
                int index = dwarf_spawner.dwarves[i].get_anim_palette_index();
                set_eitri_sprite(Eit2, index);
            }
        }
    }

    // Set the Brokkr sprites
    private static void set_brokkr_sprite(SpriteRenderer bro, int index)
    {
        if (index == 1)
        {
            bro.sprite = brokkr1;
        } else if (index == 2)
        {
            bro.sprite = brokkr2;
        } else if (index == 3)
        {
            bro.sprite = brokkr3;
        } else if (index == 4)
        {
            bro.sprite = brokkr4;
        }
    }

    // Set the Eitri sprites
    private static void set_eitri_sprite(SpriteRenderer eit, int index)
    {
        if (index == 5)
        {
            eit.sprite = eitri1;
        } else if (index == 6)
        {
            eit.sprite = eitri2;
        } else if (index == 7)
        {
            eit.sprite = eitri3;
        } else if (index == 8)
        {
            eit.sprite = eitri4;
        }
    }
}
