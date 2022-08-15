using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlareBattleships;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

namespace FlareBattleships.Test
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void CreateBoardTest()
        {
            //create board and board should have 10x10 cells.
            var expectedBoard = new Board();

            Assert.IsNotNull(expectedBoard);
            Assert.IsTrue(expectedBoard.BoardCells.Count == 100);
        }

        [TestMethod]
        public void AddShip_ValidTest()
        {
            //valid co-ordinates and no-ships already placed there.
            var actualBoard = new Board();
            int xCoordinate = 1;
            int yCoordinate = 1;
            Orientation orientation = Orientation.Vertical;
            var ship = new Ship
            {
                Length = 2,
                ShipType = ShipType.Battleship
            };

            //add ship updates boardcells with cell status - Empty, occupied, miss, hit
            var expectedBoard = new Board();
            if (orientation == Orientation.Vertical)
            {
                for (int i = yCoordinate; i < yCoordinate + ship.Length; i++)
                {
                    expectedBoard.BoardCells.Where(b => b.XCoordinate == xCoordinate && b.YCoordinate == i).ToList().ForEach(x => { x.CellStatus = CellStatus.Occupied; x.Ship = ship; });
                }
            }
            else
            {
                for (int i = xCoordinate; i < xCoordinate + ship.Length; i++)
                {
                    expectedBoard.BoardCells.Where(b => b.XCoordinate == i && b.YCoordinate == yCoordinate).ToList().ForEach(x => { x.CellStatus = CellStatus.Occupied; x.Ship = ship; });
                }
            }

            actualBoard.Addship(xCoordinate, yCoordinate, orientation, ship);

            Assert.IsTrue(CompareBoard(expectedBoard, actualBoard));

        }

        [TestMethod]
        public void ValdiateAddShip_ValidCoordinatesTest()
        {
            var board = new Board();
            var expectedResult = true;
            int xCoordinate = 1;
            int yCoordinate = 9;
            Orientation orientation = Orientation.Vertical;
            var ship = new Ship
            {
                Length = 2,
                ShipType = ShipType.Battleship
            };

            var actualResult = board.Validate(xCoordinate, yCoordinate, orientation, ship);

            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void ValdiateAddShip_ValidCoordinatesTest2()
        {
            var board = new Board();
            var expectedResult = true;
            int xCoordinate = 1;
            int yCoordinate = 10;
            Orientation orientation = Orientation.Vertical;
            var ship = new Ship
            {
                Length = 1,
                ShipType = ShipType.Battleship
            };

            var actualResult = board.Validate(xCoordinate, yCoordinate, orientation, ship);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void ValdiateAddShip_InValidCoordinatesTest()
        {
            var board = new Board();
            var expectedResult = false;
            int xCoordinate = 1;
            int yCoordinate = 10;
            Orientation orientation = Orientation.Vertical;
            var ship = new Ship
            {
                Length = 2,
                ShipType = ShipType.Battleship
            };

            var actualResult = board.Validate(xCoordinate, yCoordinate, orientation, ship);

            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void ValdiateAddShip_InValidCoordinatesTest2()
        {
            var board = new Board();
            var expectedResult = false;
            int xCoordinate = 11;
            int yCoordinate = 1;
            Orientation orientation = Orientation.Vertical;
            var ship = new Ship
            {
                Length = 1,
                ShipType = ShipType.Battleship
            };

            var actualResult = board.Validate(xCoordinate, yCoordinate, orientation, ship);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void ValdiateAddShip_InValidCoordinatesTest3()
        {
            var board = new Board();
            var expectedResult = false;
            int xCoordinate = 1;
            int yCoordinate = 13;
            Orientation orientation = Orientation.Vertical;
            var ship = new Ship
            {
                Length = 1,
                ShipType = ShipType.Battleship
            };

            var actualResult = board.Validate(xCoordinate, yCoordinate, orientation, ship);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid Co-ordinates or Co-ordinates already occupied")]
        public void AddShip_InValidOccupiedTest()
        {
            var board = new Board();
            var ship = new Ship
            {
                Length = 2,
                ShipType = ShipType.Battleship
            };

            board.Addship(1, 1, Orientation.Vertical, ship);

            board.Addship(1, 2, Orientation.Horizontal, ship);
        }

        [TestMethod]
        public void TakeAttack_ValidHitTest()
        {
            var board = new Board();
            var ship = new Ship
            {
                Length = 2,
                ShipType = ShipType.Battleship
            };

            board.Addship(1, 1, Orientation.Vertical, ship);

            board.Addship(3, 5, Orientation.Horizontal, ship);

            var actualResult = board.Attack(1, 1);
            var expectedResult = AttackResult.Hit;

            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TakeAttack_ValidMissTest()
        {
            var board = new Board();
            var ship = new Ship
            {
                Length = 2,
                ShipType = ShipType.Battleship
            };

            board.Addship(1, 1, Orientation.Vertical, ship);

            board.Addship(3, 5, Orientation.Horizontal, ship);

            var actualResult = board.Attack(3, 1);
            var expectedResult = AttackResult.Miss;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid co-ordinates")]
        public void TakeAttack_InValidTest()
        {
            var board = new Board();
            var ship = new Ship
            {
                Length = 2,
                ShipType = ShipType.Battleship
            };

            board.Addship(1, 1, Orientation.Vertical, ship);

            board.Addship(3, 5, Orientation.Horizontal, ship);

            var actualResult = board.Attack(11, 1);
            var expectedResult = AttackResult.Miss;

            Assert.AreEqual(expectedResult, actualResult);

        }

        [TestMethod]
        public void GetLostStatus_LostTest()
        {
            var board = new Board();
            var ship = new Ship
            {
                Length = 2,
                ShipType = ShipType.Battleship
            };

            board.Addship(1, 1, Orientation.Vertical, ship);

            board.Addship(3, 5, Orientation.Horizontal, ship);
            board.Attack(1, 1);
            board.Attack(1, 2);
            board.Attack(3, 5);
            board.Attack(4, 5);

            Assert.AreEqual(board.HasLost, true);
        }
        [TestMethod]
        public void GetLostStatus_PlayingTest()
        {
            var board = new Board();
            var ship = new Ship
            {
                Length = 2,
                ShipType = ShipType.Battleship
            };

            board.Addship(1, 1, Orientation.Vertical, ship);

            board.Addship(3, 5, Orientation.Horizontal, ship);

            board.Attack(5, 1);
            board.Attack(5, 2);
            board.Attack(1, 5);
            board.Attack(4, 5);
            Assert.AreEqual(board.HasLost, false);
        }

        public bool CompareBoard(Board expectedBoard, Board actualBoard)
        {
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    var ecell = expectedBoard.BoardCells.Where(c => c.XCoordinate == i && c.YCoordinate == j).FirstOrDefault();
                    var acell = actualBoard.BoardCells.Where(c => c.XCoordinate == i && c.YCoordinate == j).FirstOrDefault();

                    if (ecell.CellStatus != acell.CellStatus || ecell.Ship != acell.Ship)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
