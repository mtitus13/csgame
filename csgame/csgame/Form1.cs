using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csgame {
    public partial class Form1 : Form {
        private Map maze;
        private bool newMaze = true;
        public Form1() {
            InitializeComponent();
            maze = new Map(32,32);
            maze.generate();

            mapPanel.Width = Width;
            mapPanel.Height = Height - 64;
            mapPanel.Location = new Point(Location.X, Location.Y + 64);
            this.Invalidate();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e) {
            
        }

        private void mapPanel_Paint(object sender, PaintEventArgs e) {
            int top = 0;
            int left = 0;
            Graphics g = e.Graphics;
            // g.FillRectangle(new SolidBrush(Room.bgColor), new Rectangle(left, top, mazeWidth, mazeHeight));
            int x = left;
            int y = top;
            foreach (List<Room> mapRow in maze.rooms) {
                foreach (Room room in mapRow) {
                    room.paint(x, y, g);
                    x += Room.width;
                    if (x > mapPanel.Width) break;
                }
                x = left;
                y += Room.height;
                if (y > mapPanel.Height) break;
            }
        }

        private void mapPanel_Click(object sender, EventArgs e) {
            if (newMaze) {
                maze.cleanUpDeadEnds();
                newMaze = false;
            } else {
                maze = new Map(32, 32);
                maze.generate();
                newMaze = true;
            }
            mapPanel.Invalidate();
        }
    }
}
