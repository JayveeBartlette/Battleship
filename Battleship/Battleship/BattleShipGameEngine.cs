namespace BattleShip
{
    public class BattleShipGameEngine
    {
        private BasePlayer currentPlayer, opponentPlayer;
        public void StartGame()
        {

            SetupGamePlayers();
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"<<|Battleship|))~");
                opponentPlayer.PlayerBoard.Display(false);

                var attackCoordinate = GetInputAttackFromPlayer(currentPlayer);
                bool hit = currentPlayer.Attack(attackCoordinate, opponentPlayer);
                 Console.Clear();
                Console.WriteLine($"Attack coordinate:{attackCoordinate.Row},{attackCoordinate.Col}");
                Console.WriteLine(hit ? "Hit!" : "Miss");

                if (opponentPlayer.HasLost())
                {
                    Console.WriteLine($"{currentPlayer.Name} Wins!");
                    Console.WriteLine("Result board:");
                    opponentPlayer.PlayerBoard.Display(true);
                    break;
                }

                opponentPlayer.PlayerBoard.Display(false);
                Console.WriteLine($"{currentPlayer.Name} press [enter] to end your turn.");
                Console.ReadLine();

                // Swap players
                var temp = currentPlayer;
                currentPlayer = opponentPlayer;
                opponentPlayer = temp;
            }
        }

        private void SetupGamePlayers()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Battleship!");

            Console.WriteLine("Player 1, Enter a name");
            Player Player1 = new Player(Console.ReadLine());
            Console.WriteLine("Player 2, Enter a name");
            Player Player2 = new Player(Console.ReadLine());
            Console.Clear();

            Console.WriteLine($"{Player1.Name}, place your ships on the board.");
            PlayerShipPlacement(Player1);
            Player1.PlayerBoard.Display(true);
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine($"{Player2.Name}, place your ships on the board.");
            PlayerShipPlacement(Player2);
            Player2.PlayerBoard.Display(true);
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine("All ships placed. Let the game begin!");

            var firstTurn = new Random().Next(1, 2);
            currentPlayer = firstTurn == 1 ? Player1 : Player2;
            opponentPlayer = firstTurn == 1 ? Player2 : Player1;
            Console.WriteLine($"Player {currentPlayer.Name} goes first.");
            Console.ReadLine();
        }

        private Coordinate GetInputAttackFromPlayer(BasePlayer Player)
        {
            while (true)
            {
                Console.WriteLine($"{Player.Name}, enter your attack coordinates (row and column) Ex: 0,1");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid input coordinates. Please enter two numbers separated by a space.");
                    continue;
                }
                var nodes = input.Split(',');
                if (nodes.Length == 2 &&
                    int.TryParse(nodes[0], out int row) &&
                    int.TryParse(nodes[1], out int col))
                {
                    return new Coordinate(row, col);
                }
                Console.WriteLine("Invalid input coordinates. Please enter two numbers separated by a ',' EX: 0,1.");
            }
        }

        private void PlayerShipPlacement(BasePlayer player)
        {
            var remainingShips = player.InitialShipCount;
            while (remainingShips > 0)
            {
                Console.WriteLine($"{player.Name}, You have {remainingShips} ships left to place.");
                var ship = SetShipCoordinates();
                var shipPlaced = player.PlaceShip(ship);
                if (shipPlaced.isValid)
                {
                    remainingShips--;
                }
                else
                {
                    Console.WriteLine(shipPlaced.message);
                    Console.WriteLine("Please try again. Press [enter] to continue");
                }
            }
        }

        private Ship SetShipCoordinates()
        {
            Console.WriteLine("Enter the coordinates for your ship: ex 0,0 1,1 2,2 (3 tile ship)");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Invalid input. Ex 0,0 1,1 2,2 (3 tile ship)");
                return SetShipCoordinates();
            }

            var coordinates = new List<Coordinate>();
            var sets = input.Split(' ');
            foreach (var set in sets)
            {
                var pair = set.Split(',');
                if (pair.Length == 2 &&
                int.TryParse(pair[0], out int row) &&
                int.TryParse(pair[1], out int col))
                {
                     coordinates.Add(new Coordinate(row, col));
                }
            }
            if(sets.Count()!= coordinates.Count)
            {
                Console.WriteLine("Invalid input. Ex 0,0 1,1 2,2 (3 tile ship)");
                return SetShipCoordinates();
            }

            return new Ship(coordinates);
        }
    }
}
