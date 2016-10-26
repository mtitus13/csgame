using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace csgame {
    enum Direction {
        north = 0,
        east,
        south,
        west
    }
    class Room {
        public static Direction[] directions = { Direction.north, Direction.east, Direction.south, Direction.west };
        public static Direction oppositeDirection(Direction dir) {
            Direction ret = Direction.north;
            switch(dir) {
                case Direction.north:
                    ret = Direction.south;
                    break;
                case Direction.east:
                    ret = Direction.west;
                    break;
                case Direction.west:
                    ret = Direction.east;
                    break;
                default:
                    break;
            }
            return ret;
        }


        public Dictionary<Direction, int> exits;
        public bool cleanup = false;
        public bool draw = true;

        public int numExits {
            get {
                int val = 0;
                foreach(Direction dir in directions) {
                    if (exits[dir] != 0) val++;
                }
                return val;
            }
        }

        public const int height= 32;
        public const int width = 32;
        public static Color wallColor = Color.DarkGreen;
        public static Color bgColor = Color.Black;

        public Room() {
            exits = new Dictionary<Direction, int>();
            foreach(Direction dir in directions) {
                exits[dir] = 0;
            }
        }
        public void paint(int x, int y, Graphics g) {
            if (!draw) {
                g.FillRectangle(new SolidBrush(wallColor), new Rectangle(x + 1, y + 1, width, height));
                return;
            }
            g.FillRectangle(new SolidBrush(bgColor), new Rectangle(x+1, y+1, width, height));
            if (exits[Direction.north] == 0) {
                g.DrawLine(new Pen(wallColor), x, y, x + width, y);
            }
            if(exits[Direction.east] == 0) {
                g.DrawLine(new Pen(wallColor), x + width, y, x + width, y + height);
            }
            if(exits[Direction.south] == 0) {
                g.DrawLine(new Pen(wallColor), x, y + height, x + width, y + height);
            }
            if(exits[Direction.west] == 0) {
                g.DrawLine(new Pen(wallColor), x, y, x, y + height);
            }
        }

        public override string ToString() {
            string ret = "[";
            List<int> roomExits = new List<int>(4);
            foreach(Direction d in directions) {
                roomExits.Add(exits[d]);
            }
            ret += string.Join(",", roomExits);
            ret += "]";
            return ret;
        }
    }
}
