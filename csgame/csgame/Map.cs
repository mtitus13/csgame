using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace csgame {
    class Map {
        public List<List<Room>> rooms;
        private int _rows, _cols;

        public int rows {
            get {
                return _rows;
            }
        }

        public int cols {
            get {
                return _cols;
            }
        }

        public Map(int rows, int cols) {
            _rows = rows;
            _cols = cols;
            clear();
        }

        public void clear() {
            rooms = new List<List<Room>>();
            for (int r = 0; r < _rows; r++) {
                List<Room> row = new List<Room>();
                for (int c = 0; c < _cols; c++) {
                    row.Add(new Room());
                }
                rooms.Add(row);
            }
        }

        public void generate() {
            clear();

            List<Point> inRooms, edgeRooms, outRooms;
            inRooms = new List<Point>();
            edgeRooms = new List<Point>();
            outRooms = new List<Point>();
            
            for(int r = 0; r < _rows; r++) {
                for(int c = 0; c < _cols; c++) {
                    if(r == 0 && c == 0) {
                        inRooms.Add(new Point(r, c));
                    } else if(r + c == 1) {
                        edgeRooms.Add(new Point(r, c));
                    } else {
                        outRooms.Add(new Point(r, c));
                    }
                }
            }

            Point nextRoom, room;
            List<Direction> possibleRooms;
            Direction linkDirection;
            Random rand = new Random();
            Dictionary<Direction, Point> directions = new Dictionary<Direction, Point>();
            directions[Direction.north] = new Point(-1, 0);
            directions[Direction.east] = new Point(0, 1);
            directions[Direction.south] = new Point(1, 0);
            directions[Direction.west] = new Point(0, -1);
            while(edgeRooms.Count > 0) {
                nextRoom = edgeRooms[rand.Next(edgeRooms.Count)];
                possibleRooms = new List<Direction>();

                foreach (KeyValuePair<Direction, Point> kvp in directions) {
                    room = new Point(nextRoom.X + kvp.Value.X, nextRoom.Y + kvp.Value.Y);
                    if (room.X >= 0 && room.X < _rows && room.Y >= 0 && room.Y < _cols && inRooms.Contains(room)) {
                        possibleRooms.Add(kvp.Key);
                    }
                }
                
                if(possibleRooms.Count > 0) {
                    linkDirection = possibleRooms[rand.Next(possibleRooms.Count)];
                    room = directions[linkDirection];
                    room = new Point(nextRoom.X + room.X, nextRoom.Y + room.Y);
                    rooms[nextRoom.X][nextRoom.Y].exits[linkDirection] = 1;
                    rooms[room.X][room.Y].exits[Room.oppositeDirection(linkDirection)] = 1;

                    foreach(KeyValuePair<Direction, Point> kvp in directions) {
                        room = new Point(nextRoom.X + kvp.Value.X, nextRoom.Y + kvp.Value.Y);
                        if(outRooms.Contains(room)) {
                            outRooms.Remove(room);
                            edgeRooms.Add(room);
                        }
                    }
                    edgeRooms.Remove(nextRoom);
                    inRooms.Add(nextRoom);
                }
            }
            
        }

        public void cleanUpDeadEnds() {
            Dictionary<Direction, Point> directions = new Dictionary<Direction, Point>();
            directions[Direction.north] = new Point(-1, 0);
            directions[Direction.east] = new Point(0, 1);
            directions[Direction.south] = new Point(1, 0);
            directions[Direction.west] = new Point(0, -1);

            List<Point> deadEndRooms = new List<Point>();
            for (int r = 0; r < _rows; r++) {
                for (int c = 0; c < _cols; c++) {
                    Room mapRoom = rooms[r][c];
                    if (mapRoom.numExits < 3) mapRoom.cleanup = true;
                    if (mapRoom.numExits < 2) deadEndRooms.Add(new Point(r, c));
                }
            }

            foreach (Point deadEndRoomPoint in deadEndRooms) {
                Room deadEndRoom = rooms[deadEndRoomPoint.X][deadEndRoomPoint.Y];
                Point nextRoomPoint = deadEndRoomPoint;
                while (deadEndRoom.cleanup) {
                    foreach (KeyValuePair<Direction, Point> kvp in directions) {
                        if (deadEndRoom.exits[kvp.Key] != 0) {
                            deadEndRoom.exits[kvp.Key] = 0;
                            deadEndRoom.draw = false;
                            nextRoomPoint = new Point(nextRoomPoint.X + kvp.Value.X, nextRoomPoint.Y + kvp.Value.Y);
                            deadEndRoom = rooms[nextRoomPoint.X][nextRoomPoint.Y];
                            deadEndRoom.exits[Room.oppositeDirection(kvp.Key)] = 0;
                            break;
                        }
                    }
                }
            }
        }
    }
}
