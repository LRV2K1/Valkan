using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class EnemyInteraction
{
    static void Main(string[] args)
    {
        Enemy enemy = new Enemy();
        Player player = new Player();
        Player.Attack();       
        enemy.CheckDie();
        Console.WriteLine(enemy.Health);
        Console.ReadLine();
    }
}

public class Enemy
{
    protected int health;
    protected bool die, dead;
    public Enemy()
    {
        dead = false;
        health = 20;
    }

    public void CheckDie()
    {
        if (health <= 0)
        {
            die = true;
            Console.WriteLine("Die" + die);
        }
        else
        {
            Console.WriteLine("Alive");
        }
    }

    public int Health
    {
        set { health = value; }
        get { return health; }
    }
}

public class Player
{
    protected int health;
    public Player()
    {
        health = 30;
    }

    public static Enemy Attack()
    {
        Enemy health = new Enemy();
        health.Health = 0;
        return health;
    }
}



