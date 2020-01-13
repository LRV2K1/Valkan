using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class GameObject : IGameLoopObject
{
    protected GameObject parent;
    protected GameObjectLibrary library;
    protected Vector2 position, velocity;
    protected int layer;
    protected string id;
    protected bool visible;

    public GameObject(int layer = 0, string id = "")
    {
        this.id = id;
        if (this.id == "")
        {
            this.id = GameEnvironment.RandomID;
        }
        this.layer = layer;
        position = Vector2.Zero;
        velocity = Vector2.Zero; 
        visible = true;
    }

    public virtual void HandleInput(InputHelper inputHelper)
    {
    }

    public virtual void Update(GameTime gameTime)
    {
        position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
    }

    public virtual void Reset()
    {
        visible = true;
    }

    public virtual void RemoveSelf()
    {

    }

    public virtual Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    public virtual Vector2 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    public virtual Vector2 GlobalPosition
    {
        get
        {
            if (parent != null)
            {
                return parent.GlobalPosition + Position;
            }
            else
            {
                return Position;
            }
        }
    }

    public virtual GameObjectList RootList
    {
        get
        {
            if (GameWorld != null)
            {
                return GameWorld.RootList;
            }
            else
            {
                return null;
            }
        }
    }

    //the gameworld is a library
    public GameObjectLibrary GameWorld
    {
        get { return library; }
        set { library = value; }
    }

    public virtual int Layer
    {
        get { return layer; }
        set { layer = value; }
    }

    public virtual GameObject Parent
    {
        get { return parent; }
        set
        {
            parent = value;
            if (parent != null)
            {
                GameWorld = parent.GameWorld;
            }
        }
    }

    public string Id
    {
        get { return id; }
    }

    public bool Visible
    {
        get { return visible; }
        set { visible = value; }
    }

    public virtual Rectangle BoundingBox
    {
        get
        {
            return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, 0, 0);
        }
    }
}