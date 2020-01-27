using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ConnectedWizzard : ConnectedPlayer
{
    public ConnectedWizzard(string id = "player2")
        : base (id)
    {

    }
    protected override void LoadSkills()
    {
        skill1 = new SkillTimer("Sprites/Menu/Skills/spr_skill_6");
        skill2 = new SkillTimer("Sprites/Menu/Skills/spr_skill_7");
        skill3 = new SkillTimer("Sprites/Menu/Skills/spr_skill_8");
    }

    protected override void LoadStats()
    {
        maxhealth = 60;
        MaxStamina = 120;
        playerType = PlayerType.Wizzard;
    }
}

