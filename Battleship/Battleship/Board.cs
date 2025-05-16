namespace BattleShip
{
    public interface IBoard
    {
        public (bool isValid, string message) PositionShip(IShip ship);
        public bool RegisterAttack(Coordinate coordinate);
        public void Display(bool showShips);
        public bool AllShipsDestroyed();
    }
    public class Board: IBoard
    {
        private int GridSize { get; set; }
        private List<IShip> Ships { get; set; }
        private HashSet<Coordinate> Hits { get; set; }
        public Board()
        {
            GridSize = 10;
            Ships = new List<IShip>();
            Hits = new HashSet<Coordinate>();
        }

        public (bool isValid, string message) PositionShip(IShip ship)
        {
            var result = true;
            var validation = IsValidateShipPosition(ship); //Validations
            result = validation.isValid;
            
            if (!result)
            {
                return (false, validation.message);
            }
            Ships.Add(ship);
            return (result, validation.message); // Ship positioned successfully
        }

        private bool CheckIncremental(List<int> nums)
        {
            var isIncremental = nums[nums.Count - 1] - nums[0] == nums.Count - 1;

            return isIncremental;
        }

        private (bool isValid, string message) IsValidateShipPosition(IShip ship)
        {
            var shipCoords = ship.GetPosition();
            shipCoords.OrderBy(c => c.Row).ThenBy(c => c.Col).ToList();
            var isAlreadyOccupied = shipCoords.All(coord => Ships.Any(shp => shp.IsLocated(coord)));
            var isOutofBounds = shipCoords.All(coord => coord.Row < 0 || coord.Row > GridSize || coord.Col < 0 || coord.Col > GridSize);
            var isShipTooLarge = shipCoords.Count > GridSize;
            bool isHorizonrtal = shipCoords.All(coord => coord.Row == shipCoords[0].Row);
            bool isVertical = shipCoords.All(coord => coord.Col == shipCoords[0].Col);
            bool isDiagonal = shipCoords.All(coord => (coord.Row - shipCoords[0].Row) == (coord.Col - shipCoords[0].Col));

            bool hasDuplicateCoordinates = shipCoords.GroupBy(c => c).Any(g => g.Count() > 1);
            if (hasDuplicateCoordinates)
                return (false, "Invalid Position - Check coordinates: Duplicate coordinates found in ship placement.");

            if (isAlreadyOccupied || isOutofBounds ||isShipTooLarge)
                return (false, $"Invalid Position - Check coordinates: ship size or position should not exceed the map size {GridSize}, no overlapping ships allowed.");
            
            if (!( isHorizonrtal || isVertical || isDiagonal))
                return (false, "Invalid Position - Check coordinates if it matches a horizontal, vertical or diagonal position.");

            if (shipCoords.Count > 1)
            {
                if (isHorizonrtal)
                {
                    if (!CheckIncremental(shipCoords.Select(c => c.Col).ToList()))
                        return (false, "Invalid Position - Coordinates not valid and adjacent"); //Not Adjacent
                }
                if (isVertical)
                {
                    if (!CheckIncremental(shipCoords.Select(c => c.Row).ToList()))
                        return (false, "Invalid Position - Coordinates not valid and adjacent"); //Not Adjacent
                }
                if (isDiagonal)
                {
                    if (!(CheckIncremental(shipCoords.Select(c => c.Row).ToList()) && CheckIncremental(shipCoords.Select(c => c.Col).ToList())))
                        return (false, "Invalid Position - Coordinates not valid and adjacent"); //Not Adjacent
                }
            }
            return (true,"Ship valid and placed.");
        }

        public bool RegisterAttack(Coordinate coordinate)
        {
            if (coordinate.Row < 0 || coordinate.Row >= GridSize || // Out of bounds check
                coordinate.Col < 0 || coordinate.Col >= GridSize)
            {
                return false; // Invalid attack
            }
            Hits.Add(coordinate);
            foreach (var ship in Ships) //Check each ship for a hit
            {
                if (ship.IsLocated(coordinate))
                {
                    ship.TakeHit(coordinate);
                    return true; // Hit
                }
            }
            return false; // Miss
        }
        public void Display(bool showShips) //Added boolean here so i can show ships or not depending on the game flow
        {
            Console.WriteLine("  " + string.Join(" ", Enumerable.Range(0, GridSize)));
            for (int i = GridSize - 1; i >= 0; i--)
            {
                Console.Write(i + " ");
                for (int j = 0; j < GridSize; j++)
                {
                    var coordinate = new Coordinate(i, j);
                    if (Hits.Contains(coordinate))
                    {
                        Console.Write("X "); // Hit
                    }
                    else if (showShips && Ships.Any(s => s.IsLocated(coordinate)))
                    {
                        Console.Write("S "); // Ship part
                    }
                    else
                    {
                        Console.Write(". "); // Empty space
                    }
                }
                Console.WriteLine();
            }
        }
        public bool AllShipsDestroyed()
        {
            foreach (var ship in Ships)
            {
                if (!ship.IsDestroyed())
                {
                    return false; // At least one ship is not destroyed
                }
            }
            return true; // All ships are sunk
        }

    }
}
