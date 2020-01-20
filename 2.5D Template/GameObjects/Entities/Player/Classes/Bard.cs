using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Bard : Player
{
    public Bard(bool host = true, string id = "player")
        : base(host, id)
    {

    }

    protected override void LoadStats()
    {
        playerType = PlayerType.Bard;

        speed = 450;
        maxhealth = 40;
        MaxStamina = 80;
        staminatimerreset = 0.75f;
        addstaminatimerreset = 0.01f;
    }

    protected override void LoadSkills()
    {
        skill1 = new ProjectileAttack("Sprites/Menu/Skills/spr_skill_9", 1, "Sprites/Items/Projectiles/spr_rock", 1, "Sprites/Items/Particles/spr_rock_explosion@4", 1, 5);
        skill2 = new SpeedBuff("Sprites/Menu/Skills/spr_skill_2", 2, "Sprites/Items/Particles/spr_stamina@4");
        skill3 = new AreaHeal("Sprites/Menu/Skills/spr_skill_1", 3, "Sprites/Items/Particles/spr_heal@6");
    }
}

