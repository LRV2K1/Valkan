﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ConnectedBard : ConnectedPlayer
{
    public ConnectedBard(string id = "player2")
      : base(id)
    {

    }
    protected override void LoadSkills()
    {
        skill1 = new SkillTimer("Sprites/Menu/Skills/spr_skill_9");
        skill2 = new SkillTimer("Sprites/Menu/Skills/spr_skill_2");
        skill3 = new SkillTimer("Sprites/Menu/Skills/spr_skill_1");
    }

    protected override void LoadStats()
    {
        maxhealth = 40;
        MaxStamina = 80;
        playerType = PlayerType.Bard;
    }
}

