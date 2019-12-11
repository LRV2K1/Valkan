namespace Server_or_Client_Prototype_3._0
{
    public class Player
    {
        int iD;
        int positionX;
        int positionY;
        bool visible;

        public int ID { get { return iD; } set { value = iD; } }
        public int PositionX { get { return positionX; } set { positionX = value ; } }
        public int PositionY { get { return positionY; } set { positionY = value; } }
        public bool Visible { get { return visible; } set { visible = value; } }

        public Player(int iD, int positionX = 300, int positionY = 300, bool visible = true)
        {
            this.iD = iD;
            this.positionX = positionX;
            this.positionY = positionY;
            this.visible = visible;
        }
    }
}
