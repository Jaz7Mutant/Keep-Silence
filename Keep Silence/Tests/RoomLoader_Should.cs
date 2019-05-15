using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Keep_Silence.Tests
{
    [TestFixture]
    class RoomLoader_Should
    {
        private static readonly List<string> TestRoomNames = new List<string> {"TestRoom_1"};

        [Test]
        public void ReadAllRooms()
        {
            Assert.AreEqual(ParseRooms(TestRoomNames).Count, TestRoomNames.Count);
        }

        [Test]
        public void CompareNameAndMap()
        {
            var roomNames = ParseRooms(TestRoomNames).Keys;
            Assert.AreEqual(roomNames, TestRoomNames);
        }

        [Test]
        public void CheckMonsters()
        {
            var monsters = ParseRooms(TestRoomNames).Values.First().Monsters;
            Assert.AreEqual(monsters[0].Position, new Point(1,3));
            Assert.AreEqual(monsters[1].Position, new Point(2,3));
        }

        [Test]
        public void CheckNoMonsters()
        {
            var rooms = ParseRooms(new List<string> { "TestRoom_NoMonsters" });
            Assert.AreEqual(rooms.Values.First().Monsters.Count, 0);
        }

        [Test]
        public void TestDoor()
        {
            var rawDoor = ParseRooms(TestRoomNames).Values.First().Map[1, 2];
            Assert.True(rawDoor is Door);
            var door = (Door) rawDoor;
            Assert.AreEqual(door.IsOpen, true);
            Assert.AreEqual(door.Illumination, 0);
            Assert.AreEqual(door.NextRoomName, "door");
            Assert.AreEqual(door.NextRoomStartPosition, new Point(1, 1));
        }

        [Test]
        public void TestChest()
        {
            var rawChest = ParseRooms(TestRoomNames).Values.First().Map[2, 2];
            Assert.True(rawChest is Chest);
            var chest = (Chest) rawChest;
            Assert.AreEqual(chest.DeltaPlayerHealthPoints, 10);
            Assert.AreEqual(chest.DeltaPlayerFlashlightPoints, 20);
            Assert.AreEqual(chest.DeltaFlashlightRadius, 30);
            Assert.AreEqual(chest.DoorToUnlock, new Point(-1,-1));
            Assert.AreEqual(chest.Message, "TestMessage");
        }

        [Test]
        public void TestRightEnvironmentRecognition()
        {
            var map = ParseRooms(TestRoomNames).Values.First().Map;
            Assert.True(map[0,0] is Darkness);
            Assert.True(map[1,1] is Wall);
            Assert.True(map[1,2] is Door);
            Assert.True(map[2,2] is Chest);
        }

        private static Dictionary<string,Room> ParseRooms(List<string> roomNames)
        {
            RoomLoader.Rooms = new DirectoryInfo(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Tests"));
            RoomLoader.RoomFileNames = roomNames;
            return RoomLoader.GetRoomsFromDirectory();
        }
    }
}
