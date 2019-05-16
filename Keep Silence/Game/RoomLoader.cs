using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;

namespace Keep_Silence
{
    public static class RoomLoader
    {
        public static DirectoryInfo Rooms = new DirectoryInfo(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "Rooms"));

        public static List<string> RoomFileNames = new List<string>
        {
            "1",
            "2",
            //"3",
            //"4",
            //"5",
        };

        public static Dictionary<string, Room> GetRoomsFromDirectory()
        {
            var roomsWithNames = new Dictionary<string, Room>();
            foreach (var roomFileName in RoomFileNames)
            {
                var file = Rooms.GetFiles($"{roomFileName}.txt", SearchOption.AllDirectories).Single();
                var roomData = File.ReadAllText(file.FullName).Split('%');
                roomsWithNames.Add(roomFileName, CreateRoom(roomData, roomFileName));
            }

            return roomsWithNames;
        }

        private static Room CreateRoom(string[] roomData, string roomFileName)
        {
            var mapRows = roomData[0].Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            var doorsData = roomData[1].Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            var chestsData = roomData[2].Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            var monstersData = roomData[3].Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            if (mapRows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong room map '{roomFileName}'");
            var map = new IEnvironment[mapRows[0].Length, mapRows.Length];
            var chestsCounter = 0;
            var doorsCounter = 0;
            for (var x = 0; x < mapRows[0].Length; x++)
            for (var y = 0; y < mapRows.Length; y++)
            {
                map[x, y] = CreateCreatureBySymbol(mapRows[y][x]);
                if (map[x, y] is Door door)
                {
                    var doorData = doorsData[doorsCounter].Split('\t');
                    door.IsOpen = bool.Parse(doorData[0]);
                    door.NextRoomName = doorData[1];
                    var pointData = doorData[2].Split();
                    door.NextRoomStartPosition = new Point(int.Parse(pointData[0]), int.Parse(pointData[1]));
                    doorsCounter++;
                }

                if (map[x, y] is Chest chest)
                {
                    var chestData = chestsData[chestsCounter].Split('\t');
                    chest.DeltaPlayerHealthPoints = int.Parse(chestData[0]);
                    chest.DeltaPlayerFlashlightPoints = int.Parse(chestData[1]);
                    chest.DeltaFlashlightRadius = int.Parse(chestData[2]);
                    var pointData = chestData[3].Split();
                    chest.DoorToUnlock = new Point(int.Parse(pointData[0]), int.Parse(pointData[1]));
                    chest.Message = chestData[4];
                    chestsCounter++;
                }
            }

            var monsters = monstersData
                .Select(rawMonster => rawMonster.Split())
                .Select(monsterData => new Monster
                    {Position = new Point(int.Parse(monsterData[0]), int.Parse(monsterData[1]))})
                .ToList();

            return new Room {Map = map, Monsters = monsters};
        }

        private static IEnvironment CreateCreatureBySymbol(char c)
        {
            switch (c)
            {
                case '.':
                    return new Darkness();
                case '#':
                    return new Wall();
                case ' ':
                    return new Terrain();
                case 'D':
                    return new Door();
                case 'C':
                    return new Chest();
                default:
                    throw new Exception($"wrong character for IEnvironment {c}");
            }
        }
    }
}