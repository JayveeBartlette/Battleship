
namespace BattleShip
{
    public interface IShip
    {
        public List<Coordinate> GetPosition();
        public void TakeHit(Coordinate hit);
        public bool IsDestroyed();
        public bool IsLocated(Coordinate coordinate);
    }
    public class Ship: IShip
    {
        private List<Coordinate> Coordinates { get; set; }
        private HashSet<Coordinate> Hits = new HashSet<Coordinate>();
        public Ship(List<Coordinate> coordinates)
        {
            Coordinates = coordinates;
        }

        public List<Coordinate> GetPosition()
        {
            return Coordinates;
        }

        public void TakeHit(Coordinate hit)
        {
            if (Coordinates.Contains(hit))
            {
                Hits.Add(hit);
            }
        }
        
        public bool IsDestroyed()
        {
            return Hits.Count == Coordinates.Count;
        }

        public bool IsLocated(Coordinate coordinate)
        {
            return Coordinates.Contains(coordinate);
        }
    }
}
