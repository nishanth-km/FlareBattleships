using System;
using System.Collections.Generic;
using System.Text;

namespace FlareBattleships
{
    public class BoardCell
    {
        public CellStatus CellStatus { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public Ship Ship { get; set; }
    }
    public enum CellStatus
    {
        Empty,
        Occupied,
        Hit,
        Missed
    }
}
