using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Marker_Plot
{
    public partial class Form1 : Form
    {
        List <Marker> Data;                                                                                         //stores all data ( Markers, Axes and Well Log along Trajectory )
        int angleX, angleY, angleZ, planeX, planeY, planeZ, Xplane, Yplane, Zplane, pointSize, depth, dataCount;    //refer to update() method for variable use
        double magnification;                                                                                       //refer to update() method for variable use
        Color[] colors;                                                                                             //stores Colors of pens used for plotting Well Log along Trajectory
        Form2 dialog1;                                                                                              //Add/Remove Marker dialog box
        Form3 dialog2;                                                                                              //Edit Marker dialog box
        public Form1(Marker[] array)
        {
            this.Data = new List<Marker>();
            foreach (Marker data in array)
                this.Data.Add(new Marker { staticX = data.staticX, staticY = data.staticY, staticZ = data.staticZ, Visible = data.Visible });
            this.dataCount = this.Data.Count;
            this.colors = new Color[] { Color.Violet, Color.White, Color.Red, Color.Blue, Color.Green,
                                        Color.DarkGreen, Color.DarkOliveGreen, Color.DarkSeaGreen, Color.ForestGreen, Color.Green,
                                        Color.GreenYellow, Color.LawnGreen, Color.LightGreen, Color.LightSeaGreen, Color.LimeGreen,
                                        Color.MediumSeaGreen, Color.MediumSpringGreen, Color.PaleGreen, Color.SeaGreen, Color.SpringGreen };
            InitializeComponent();
            this.update();
            this.Refresh();
            this.dialog1 = new Form2();
            this.dialog2 = new Form3();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            string print;                                                                                           //stores Text associated with Marker for printing
            for (var i = 6; i < this.dataCount - 1; ++i)                                                            //Paints Well Log along Trajectory
                e.Graphics.DrawLine(new Pen(colors[this.Data[i].Visible], 1 + this.pointSize / 10),
                    new Point() { X = (int)this.Data[i].dynamicX, Y = 400 - (int)this.Data[i].dynamicY },
                    new Point() { X = (int)this.Data[i + 1].dynamicX, Y = 400 - (int)this.Data[i + 1].dynamicY });
            foreach (Marker point in this.Data)                                                                     //Paints Temporary Markers, Permanent Markers and Axes ( refer to Marker Class for Visibility index )
                if (point.Visible == 0)
                {
                    e.Graphics.FillEllipse(Brushes.Violet, (int)point.dynamicX, 400 - (int)point.dynamicY, this.pointSize, this.pointSize);
                    print = point.Text;
                    if (this.checkBox1.Checked)
                        print += " (" + point.staticX + "," + point.staticY + "," + point.staticZ + ")";            //adds Coordinates to Marker Text if Show Coordinates is checked
                    e.Graphics.DrawString(print, new Font("Arial", 2 * this.pointSize), Brushes.Violet,
                        new PointF((float)(point.dynamicX + this.pointSize), (float)(400 - point.dynamicY + this.pointSize)));
                }
                else if (point.Visible == 1)
                {
                    e.Graphics.FillEllipse(Brushes.White, (int)point.dynamicX, 400 - (int)point.dynamicY, this.pointSize, this.pointSize);
                    print = point.Text;
                    if (this.checkBox1.Checked)                                                                     //adds Coordinates to Marker Text if Show Coordinates is checked
                        print += " (" + point.staticX + "," + point.staticY + "," + point.staticZ + ")";
                    e.Graphics.DrawString(print, new Font("Arial", 2 * this.pointSize), Brushes.White,
                        new PointF((float)(point.dynamicX + this.pointSize), (float)(400 - point.dynamicY + this.pointSize)));
                }
                else if (point.Visible == 2)
                    e.Graphics.DrawLine(new Pen(Color.Red, 1 + this.pointSize / 10), new Point(200, 200),
                        new Point((int)point.dynamicX, 400 - (int)point.dynamicY));
                else if (point.Visible == 3)
                    e.Graphics.DrawLine(new Pen(Color.Blue, 1 + this.pointSize / 10), new Point(200, 200),
                        new Point((int)point.dynamicX, 400 - (int)point.dynamicY));
                else if (point.Visible == 4)
                    e.Graphics.DrawLine(new Pen(Color.Green, 1 + this.pointSize / 10), new Point(200, 200),
                        new Point((int)point.dynamicX, 400 - (int)point.dynamicY));
        }
        private void turn(int angle, int axis)                                                                      //rotates the Marker Plot about an axis by a specified angle in degrees according to standard mathematical formulae
        {
            var axis1 = (axis + 1) % 3;
            var axis2 = (axis + 2) % 3;
            double angleRadians = Math.PI / 180 * angle;
            double[] pseudoPoint;
            foreach (Marker point in this.Data)
            {
                pseudoPoint = new double[] { point.dynamicX, point.dynamicY, point.dynamicZ };
                var newAxis1 = pseudoPoint[axis1] * Math.Cos(angleRadians) - pseudoPoint[axis2] * Math.Sin(angleRadians);
                var newAxis2 = pseudoPoint[axis2] * Math.Sin(angleRadians) + pseudoPoint[axis2] * Math.Cos(angleRadians);
                pseudoPoint[axis1] = newAxis1;
                pseudoPoint[axis2] = newAxis2;
                point.dynamicX = pseudoPoint[0];
                point.dynamicY = pseudoPoint[1];
                point.dynamicZ = pseudoPoint[2];
            }
        }
        private void update()
        {
            this.angleX = (int)numericUpDown1.Value;                                                                //stores the angle in degrees by which Plot is rotated about X Axis
            this.angleY = (int)numericUpDown2.Value;                                                                //stores the angle in degrees by which Plot is rotated about Y Axis
            this.angleZ = (int)numericUpDown3.Value;                                                                //stores the angle in degrees by which Plot is rotated about Z Axis
            this.planeX = (int)numericUpDown4.Value;                                                                //stores the minimum value of Visible Markers in Section cut along X Axis
            this.planeY = (int)numericUpDown5.Value;                                                                //stores the minimum value of Visible Markers in Section cut along Y Axis
            this.planeZ = (int)numericUpDown6.Value;                                                                //stores the minimum value of Visible Markers in Section cut along Z Axis
            this.Xplane = (int)numericUpDown12.Value;                                                               //stores the maximum value of Visible Markers in Section cut along X Axis
            this.Yplane = (int)numericUpDown11.Value;                                                               //stores the maximum value of Visible Markers in Section cut along Y Axis
            this.Zplane = (int)numericUpDown10.Value;                                                               //stores the maximum value of Visible Markers in Section cut along Z Axis
            this.magnification = (double)numericUpDown7.Value;                                                      //stores the value by which the Plot is Magnified
            this.pointSize = (int)numericUpDown8.Value;                                                             //stores the value by which Markers are Magnified
            this.depth = (int)numericUpDown9.Value;                                                                 //stores the depth after transformation at which Markers are visible
            foreach (Marker point in this.Data)                                                                     //initialises dynamic variables before transformation from static variables
            {
                point.dynamicX = point.staticX;
                point.dynamicY = point.staticY;
                point.dynamicZ = point.staticZ;
                if (point.Visible == -1)
                    point.Visible = 1;
            }
            this.turn(angleX, 0);                                                                                   //transforms Markers by turning about X Axis
            this.turn(angleY, 1);                                                                                   //transforms Markers by turning about Y Axis
            this.turn(angleZ, 2);                                                                                   //transforms Markers by turning about Z Axis
            double maxZ;
            if (this.dataCount < this.Data.Count)
                maxZ = this.Data[this.dataCount].dynamicZ;
            else
                maxZ = -100;
            foreach (Marker point in this.Data)                                                                     //applies Isotropic Transformation from 3D data to 2D plot according to standard mathematical formulae
            {
                var newX = (point.dynamicX - point.dynamicZ) / Math.Sqrt(2);
                var newY = (point.dynamicX + 2 * point.dynamicY + point.dynamicZ) / Math.Sqrt(6);
                var newZ = (point.dynamicX - point.dynamicY + point.dynamicZ) / Math.Sqrt(3);
                point.dynamicX = newX;
                point.dynamicY = newY;
                point.dynamicZ = newZ;
                point.dynamicX = 200 + (1 + this.magnification / 10) * point.dynamicX;
                point.dynamicY = 200 + (1 + this.magnification / 10) * point.dynamicY;
                if (point.Visible < 2)
                    maxZ = Math.Max(maxZ, point.dynamicZ);
            }
            for (var i = this.dataCount; i < this.Data.Count; ++i)                                                  //Bubble Sorting transformed data to display Markers by depth
            {
                for (var j = this.dataCount; j < this.Data.Count - i - 1; ++j)
                    if (this.Data[j].dynamicZ > this.Data[j + 1].dynamicZ)
                    {
                        var swap=this.Data[j];
                        this.Data[j]=this.Data[j+1];
                        this.Data[j+1]=swap;
                    }
                if (this.Data[this.Data.Count - 1 - i + this.dataCount].dynamicZ + this.depth > maxZ ||
                    this.Data[this.Data.Count - 1 - i + this.dataCount].staticX > this.Xplane || this.Data[this.Data.Count - 1 - i + this.dataCount].staticX < this.planeX ||
                    this.Data[this.Data.Count - 1 - i + this.dataCount].staticY > this.Yplane || this.Data[this.Data.Count - 1 - i + this.dataCount].staticY < this.planeY ||
                    this.Data[this.Data.Count - 1 - i + this.dataCount].staticZ > this.Zplane || this.Data[this.Data.Count - 1 - i + this.dataCount].staticZ < this.planeZ)
                    this.Data[this.Data.Count - 1 - i + this.dataCount].Visible = -1;                               //setting Marker Visibility according to Sections cut by user
            }
        }
        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            this.update();
            this.Refresh();
        }
        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            dialog1.ShowDialog();
            if (dialog1.DialogResult == DialogResult.Yes)                                                           //adds new Permanent Marker
            {
                Marker newPoint = new Marker { staticX = (int)dialog1.numericUpDown1.Value, staticY = (int)dialog1.numericUpDown2.Value,
                    staticZ = (int)dialog1.numericUpDown3.Value, Visible = 1, Text = dialog1.textBox1.Text };
                if (!this.Data.Contains(newPoint))                                                                  //checks for existing duplicate Marker
                {
                    this.Data.Add(newPoint);
                    this.update();
                    this.Refresh();
                }
                else
                    MessageBox.Show("That marker already exists.", "Uh-oh");
            }
            else if (dialog1.DialogResult == DialogResult.Retry)                                                    //adds new Temporary Marker
            {
                Marker newPoint = new Marker { staticX = (int)dialog1.numericUpDown1.Value, staticY = (int)dialog1.numericUpDown2.Value,
                    staticZ = (int)dialog1.numericUpDown3.Value, Visible = 0, Text = dialog1.textBox1.Text };
                if (!this.Data.Contains(newPoint))                                                                  //checks for existing duplicate Marker
                {
                    this.Data.Add(newPoint);
                    this.update();
                    this.Refresh();
                    this.Data.Remove(newPoint);
                }
                else
                    MessageBox.Show("That marker already exists.", "Uh-oh");
            }
            else if (dialog1.DialogResult == DialogResult.No)                                                       //deletes existing Marker
            {
                Marker newPoint = new Marker { staticX = (int)dialog1.numericUpDown1.Value, staticY = (int)dialog1.numericUpDown2.Value,
                    staticZ = (int)dialog1.numericUpDown3.Value, Visible = 1, Text = dialog1.textBox1.Text };
                if (this.Data.Contains(newPoint))                                                                   //checks for existance of Marker
                {
                    this.Data.Remove(newPoint);
                    this.update();
                    this.Refresh();
                }
                else
                    MessageBox.Show("That marker does not exist.", "Uh-oh");
            }
        }
        private void button3_MouseClick(object sender, MouseEventArgs e)
        {
            dialog2.ShowDialog();
            if (dialog2.DialogResult == DialogResult.Yes)                                                           //Permanently changes Marker
            {
                Marker oldPoint = new Marker { staticX = (int)dialog2.numericUpDown1.Value, staticY = (int)dialog2.numericUpDown2.Value,
                    staticZ = (int)dialog2.numericUpDown3.Value, Visible = 1, Text = dialog2.textBox1.Text };
                Marker newPoint = new Marker { staticX = (int)dialog2.numericUpDown6.Value, staticY = (int)dialog2.numericUpDown5.Value,
                    staticZ = (int)dialog2.numericUpDown4.Value, Visible = 1, Text = dialog2.textBox2.Text };
                if (this.Data.Contains(oldPoint))                                                                   //checks for existance of Marker
                {
                    this.Data.Add(newPoint);
                    this.Data.Remove(oldPoint);
                    this.update();
                    this.Refresh();
                }
                else
                    MessageBox.Show("That marker does not exist.", "Uh-oh");
            }
            else if (dialog2.DialogResult == DialogResult.Retry)                                                    //Temporarily changes Marker
            {
                Marker oldPoint = new Marker { staticX = (int)dialog2.numericUpDown1.Value, staticY = (int)dialog2.numericUpDown2.Value,
                    staticZ = (int)dialog2.numericUpDown3.Value, Visible = 0, Text = dialog2.textBox1.Text };
                Marker newPoint = new Marker { staticX = (int)dialog2.numericUpDown6.Value, staticY = (int)dialog2.numericUpDown5.Value,
                    staticZ = (int)dialog2.numericUpDown4.Value, Visible = 0, Text = dialog2.textBox2.Text };
                if (this.Data.Contains(oldPoint))                                                                   //checks for existance of Marker
                {
                    this.Data.Remove(oldPoint);
                    this.Data.Add(oldPoint);
                    this.Data.Add(newPoint);
                    this.update();
                    this.Refresh();
                    this.Data.Remove(oldPoint);
                    this.Data.Remove(newPoint);
                    oldPoint.Visible = 1;
                    this.Data.Add(oldPoint);
                }
                else
                    MessageBox.Show("That marker does not exist.", "Uh-oh");
            }
        }
        private void valueChanged(object sender, EventArgs e)
        {
            this.update();
            this.Refresh();
        }
    }
}