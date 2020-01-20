using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Warrior : Player
{

    public Warrior(bool host = true, string id = "player")
    : base(host, id)
    {

    }
    protected override void LoadStats()
    {
        playerType = PlayerType.Warrior;

        speed = 400;
        maxhealth = 120;
        MaxStamina = 150;
        staminatimerreset = 1.0f;
        addstaminatimerreset = 0.03f;
    }

    protected override void LoadSkills()
    {
        skill1 = new CloseAttack("Sprites/Menu/Skills/spr_skill_0", 1);
        skill2 = new Block("Sprites/Menu/Skills/spr_skill_4", 2);
        skill3 = new Dodge("Sprites/Menu/Skills/spr_skill_5", 3);
    }
}

