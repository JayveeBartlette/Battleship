using BattleShip;

namespace BattleShipTest
{
    public class CoordinateTest
    {
        [Fact]
        public void Coordinates_WithSameValues_ShouldBeEqual()
        {
            var ccord1 = new Coordinate(1, 1);
            var ccord2 = new Coordinate(1, 1);
            Assert.Equal(ccord1, ccord2);
        }

        [Fact]
        public void Coordinates_WithDifferentValues_ShouldNotBeEqual()
        {
            var ccord1 = new Coordinate(1, 1);
            var ccord2 = new Coordinate(2, 2);
            Assert.NotEqual(ccord1, ccord2);
        }

        [Fact]
        public void Coordinates_ShouldHaveSameHashCode_WhenEqual()
        {
            var ccord1 = new Coordinate(1, 1);
            var ccord2 = new Coordinate(1, 1);
            Assert.Equal(ccord1.GetHashCode(), ccord2.GetHashCode());
        }

        [Fact]
        public void Coordinates_ShouldHaveDifferentHashCode_WhenNotEqual()
        {
            var ccord1 = new Coordinate(1, 1);
            var ccord2 = new Coordinate(2, 2);
            Assert.NotEqual(ccord1.GetHashCode(), ccord2.GetHashCode());
        }
    }
    public class ShipTest
    {
        [Fact]
        public void Ship_ShouldBeDestroyed_WhenAllCoordinatesAreHit()
        {
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);

            ship.TakeHit(new Coordinate(1, 1));
            ship.TakeHit(new Coordinate(1, 2));
            ship.TakeHit(new Coordinate(1, 3));

            Assert.True(ship.IsDestroyed());
        }

        [Fact]
        public void Ship_ShouldNotBeDestroyed_WhenNotAllCoordinatesAreHit()
        {
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);

            ship.TakeHit(new Coordinate(1, 1));
            ship.TakeHit(new Coordinate(1, 2));

            Assert.False(ship.IsDestroyed());
        }

        [Fact]
        public void Ship_ShoulBePlacedCorrectly()
        {
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);

            Assert.True(ship.IsLocated(coordinates[0]));
        }

        [Fact]
        public void Ship_ShouldReturnTrue_IsLocated_WhenCoordinatesMatch()
        {
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);
            Assert.True(ship.IsLocated(new Coordinate(1, 1)));
        }

        [Fact]
        public void Ship_ShouldReturnFalse_IsLocated_WhenCoordinatesDoNotMatch()
        {
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);
            Assert.False(ship.IsLocated(new Coordinate(0, 0)));
        }
    }

    public class BoardTest
    {
        [Fact]
        public void Board_ShouldAllowShipPlacement_WhenValidCoordinates()
        {
            var board = new Board();
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);
            var result = board.PositionShip(ship);
            Assert.True(result.isValid);
        }
        [Fact]
        public void Board_ShouldNotAllowShipPlacement_WhenInvalidCoordinates()
        {
            var board = new Board();
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);
            board.PositionShip(ship);
            var result = board.PositionShip(ship);
            Assert.False(result.isValid);
        }

        [Fact]
        public void Board_ShouldRegisterAttack_WhenValidCoordinates_AndReturnTrueWhenHit()
        {
            var board = new Board();
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);
            board.PositionShip(ship);
            var result = board.RegisterAttack(new Coordinate(1, 1));
            Assert.True(result);
        }

        [Fact]
        public void Board_ShouldRegisterAttack_WhenValidCoordinates_AndReturnFalseWhenNotHit()
        {
            var board = new Board();
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);
            board.PositionShip(ship);
            var result = board.RegisterAttack(new Coordinate(1, 0));
            Assert.False(result);
        }

        [Fact]
        public void Board_ShouldReturnTrue_WhenAllShipsSunk()
        {
            var board = new Board();
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);
            board.PositionShip(ship);
            board.RegisterAttack(new Coordinate(1, 1));
            board.RegisterAttack(new Coordinate(1, 2));
            board.RegisterAttack(new Coordinate(1, 3));
            Assert.True(board.AllShipsDestroyed());
        }
    }

    public class PlayerTest
    {
        [Fact]
        public void Player_ShouldHaveName()
        {
            var player = new Player("Jayvee", 2);
            Assert.Equal("Jayvee", player.Name);
        }
        [Fact]
        public void Player_ShouldHaveBoard()
        {
            var player = new Player("Jayvee", 2);
            Assert.NotNull(player.PlayerBoard);
        }
        [Fact]
        public void Player_ShouldHaveShipCount()
        {
            var player = new Player("Jayvee", 5);
            Assert.Equal(5, player.InitialShipCount);
        }

        [Fact]
        public void Player_ShouldPlaceShip()
        {
            var player = new Player("Jayvee", 2);
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);
            var result = player.PlaceShip(ship);
            Assert.True(result.isValid);
        }

        [Fact]
        public void Player_ShouldNotPlaceShip_WhenInvalidCoordinates()
        {
            var player = new Player("Jayvee", 2);
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 1),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);
            var result = player.PlaceShip(ship);
            Assert.False(result.isValid);
        }

        [Fact]
        public void Player_ShouldNotPlaceShip_When2ShipsOverlap()
        {
            var player = new Player("Jayvee", 2);
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);
            player.PlaceShip(ship);
            var secondPlacement = player.PlaceShip(ship);
            Assert.False(secondPlacement.isValid);
        }

        [Fact]
        public void Player_ShouldNotPlaceShip_WhenPositionOutofBounds()
        {
            var player = new Player("Jayvee", 2);
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 11)
            };
            var ship = new Ship(coordinates);
            var result = player.PlaceShip(ship);
            Assert.False(result.isValid);
        }

        [Fact]
        public void Player_CanAttackOpponent_AndHit()
        {
            var player1 = new Player("Jayvee", 2);
            var coordinates1 = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship1 = new Ship(coordinates1);
            player1.PlaceShip(ship1);

            var player2 = new Player("Jayvee", 2);
            var coordinates2 = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship2 = new Ship(coordinates2);
            player2.PlaceShip(ship2);

            var hit = player1.Attack(new Coordinate(1,1), player2);
            Assert.True(hit);
        }

        [Fact]
        public void Player_CanAttackOpponent_AndMiss()
        {
            var player1 = new Player("Jayvee", 2);
            var coordinates1 = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship1 = new Ship(coordinates1);
            player1.PlaceShip(ship1);
            var player2 = new Player("AI", 2);
            var coordinates2 = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship2 = new Ship(coordinates2);
            player2.PlaceShip(ship2);
            var hit = player1.Attack(new Coordinate(0, 0), player2);
            Assert.False(hit);
        }

        [Fact]
        public void Player_ShouldLose_WhenAllShipsSunk()
        {
            var player = new Player("Jayvee", 2);
            var coordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(1, 3)
            };
            var ship = new Ship(coordinates);
            player.PlaceShip(ship);
            player.PlayerBoard.RegisterAttack(new Coordinate(1, 1));
            player.PlayerBoard.RegisterAttack(new Coordinate(1, 2));
            player.PlayerBoard.RegisterAttack(new Coordinate(1, 3));
            Assert.True(player.HasLost());
        }
    }
}
