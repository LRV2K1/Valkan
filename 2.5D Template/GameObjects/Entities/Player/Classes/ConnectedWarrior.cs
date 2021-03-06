﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ConnectedWarrior : ConnectedPlayer
{
    public ConnectedWarrior(string id = "player2")
       : base(id)
    {

    }
    protected override void LoadSkills()
    {
        skill1 = new SkillTimer("Sprites/Menu/Skills/spr_skill_0");
        skill2 = new SkillTimer("Sprites/Menu/Skills/spr_skill_4");
        skill3 = new SkillTimer("Sprites/Menu/Skills/spr_skill_5");
    }

    protected override void LoadStats()
    {
        maxhealth = 100;
        MaxStamina = 150;
        playerType = PlayerType.Warrior;
    }
}

