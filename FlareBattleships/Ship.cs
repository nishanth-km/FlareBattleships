using System;
using System.Collections.Generic;
using System.Text;

namespace FlareBattleships
{
    public class Ship
    {
        public int Length { get; set; }
        public ShipType ShipType { get; set; }

    }

    public enum ShipType
    {
        Destoryer,
        Submarine,
        Cruiser,
        Battleship,
        Carrier
    }
}
