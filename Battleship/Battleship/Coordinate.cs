using System;
using System.Collections.Generic;
namespace BattleShip
{
    public struct Coordinate
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Coordinate(int row, int col)
        {
            Row = row;
            Col = col;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Coordinate coor)
            {
                return Row == coor.Row && Col == coor.Col;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }
    }
}
