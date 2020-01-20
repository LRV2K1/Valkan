using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Warrior : Player
{
    protected override void LoadStats()
    {
        playerType = PlayerType.Warrior;

        speed = 400;
        maxhealth = 100;
        MaxStamina = 150;
        staminatimerreset = 1.5f;
        addstaminatimerreset = 0.05f;
    }

    protected override void LoadSkills()
    {
        skill1 = new CloseAttack("Sprites/Menu/Skills/spr_skill_0");
        skill2 = new Block("Sprites/Menu/Skills/spr_skill_4");
        skill3 = new Dodge("Sprites/Menu/Skills/spr_skill_5");
    }
}

