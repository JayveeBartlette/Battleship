namespace BattleShip
{
    public interface IPlayer
    {
        public string Name { get; }
        public int InitialShipCount { get;}
        public IBoard PlayerBoard { get; }

        public (bool isValid, string message) PlaceShip(Ship ship);
        public bool Attack(Coordinate attackCoord, BasePlayer opponent);
        public bool HasLost();
    }

    public abstract class BasePlayer : IPlayer
    {
        public string Name { get; protected set; }
        public int InitialShipCount { get; protected set; }

        public IBoard PlayerBoard { get; private set; }

        protected BasePlayer(string name, int shipCount)
        {
            Name = name;
            InitialShipCount = shipCount;
            PlayerBoard = new Board();
        }

        public abstract bool Attack(Coordinate attackCoord, BasePlayer opponent);

        public bool HasLost() => PlayerBoard.AllShipsDestroyed();

        public abstract (bool isValid, string message) PlaceShip(Ship ship);
    }

    public class Player : BasePlayer
    {
        public Player(string name, int shipCount = 2) : base(name, shipCount) 
        {
            Name = name;
            InitialShipCount = shipCount;
        }

        public override (bool isValid, string message) PlaceShip(Ship ship)
        {
           return PlayerBoard.PositionShip(ship);
        }
        public override bool Attack(Coordinate attackCoord, BasePlayer opponent)
        {
            return opponent.PlayerBoard.RegisterAttack(attackCoord);
        }
    }
}
