using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FlareBattleships
{
    public class Board
    {
        public List<BoardCell> BoardCells { get; set; }
        public bool HasLost
        {
            get
            {
                if (BoardCells.Any(c => c.CellStatus.Equals(CellStatus.Occupied)) || BoardCells.All(c => c.CellStatus.Equals(CellStatus.Empty)))
                {
                    return false;
                }// for newly created baord -> all are empty and no ship is added.
                return true;
            }
        }

        public Board()
        {
            BoardCells = new List<BoardCell>();
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    BoardCells.Add(new BoardCell
                    {
                        CellStatus = CellStatus.Empty,
                        XCoordinate = i,
                        YCoordinate = j
                    });
                }
            }
        }

        public void Addship(int xCoordinate, int yCoordinate, Orientation orientation, Ship ship)
        {
            var isValid = Validate(xCoordinate, yCoordinate, orientation, ship);
            if (!isValid)
                throw new ArgumentException("Invalid Co-ordinates or Co-ordinates already occupied");
            if (orientation == Orientation.Vertical)
            {
                for (int i = yCoordinate; i < yCoordinate + ship.Length; i++)
                {
                    BoardCells.Where(b => b.XCoordinate == xCoordinate && b.YCoordinate == i).ToList().ForEach(x => { x.CellStatus = CellStatus.Occupied; x.Ship = ship; });
                }
            }
            else
            {
                for (int i = xCoordinate; i < xCoordinate + ship.Length; i++)
                {
                    BoardCells.Where(b => b.XCoordinate == i && b.YCoordinate == yCoordinate).ToList().ForEach(x => { x.CellStatus = CellStatus.Occupied; x.Ship = ship; });
                }
            }

        }
        public bool Validate(int xCoordinate, int yCoordinate, Orientation orientation, Ship ship)
        {

            if (orientation == Orientation.Vertical)
            {
                for (int i = yCoordinate; i < yCoordinate + ship.Length; i++)
                {
                    if (!BoardCells.Any(b => b.CellStatus == CellStatus.Empty && b.XCoordinate == xCoordinate && b.YCoordinate == i) || xCoordinate > 10 || i > 10)
                        return false;
                }
            }
            else
            {
                for (int i = xCoordinate; i < xCoordinate + ship.Length; i++)
                {
                    if (!BoardCells.Any(b => b.CellStatus == CellStatus.Empty && b.XCoordinate == i && b.YCoordinate == yCoordinate) || yCoordinate > 10 || i > 10)
                        return false;
                }
            }
            return true;
        }

        public AttackResult Attack(int xCoordinate, int yCoordinate)
        {
            var cell = BoardCells.Where(b => b.XCoordinate == xCoordinate && b.YCoordinate == yCoordinate).FirstOrDefault();
            if (cell != null)
            {
                switch(cell.CellStatus)
                {
                    case CellStatus.Empty: cell.CellStatus = CellStatus.Missed;
                        return AttackResult.Miss;
                    case CellStatus.Occupied: cell.CellStatus = CellStatus.Hit;
                        return AttackResult.Hit;
                }
            }
            throw new ArgumentException("Invalid co-ordinates");
        }
    }
}
