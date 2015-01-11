using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Marker_Plot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Marker[] Data = new Marker[] {  new Marker() { staticX=1000, staticY=0, staticZ=0, Visible=2 },     //defining X Axis
                                            new Marker() { staticX=0, staticY=1000, staticZ=0, Visible=3 },     //defining Y Axis
                                            new Marker() { staticX=0, staticY=0, staticZ=1000, Visible=4 },     //defining Z Axis
                                            new Marker() { staticX=-1000, staticY=0, staticZ=0, Visible=2 },    //predefining a Well Trajectory and Log
                                            new Marker() { staticX=0, staticY=-1000, staticZ=0, Visible=3 },
                                            new Marker() { staticX=0, staticY=0, staticZ=-1000, Visible=4 },
                                            new Marker() { staticX=-50, staticY=0, staticZ=100, Visible=10 },
                                            new Marker() { staticX=-50, staticY=0, staticZ=90, Visible=11 },
                                            new Marker() { staticX=-50, staticY=0, staticZ=80, Visible=12 },
                                            new Marker() { staticX=-50, staticY=0, staticZ=70, Visible=13 },
                                            new Marker() { staticX=-49, staticY=1, staticZ=60, Visible=14 },
                                            new Marker() { staticX=-48, staticY=2, staticZ=50, Visible=15 },
                                            new Marker() { staticX=-46, staticY=8, staticZ=35, Visible=16 },
                                            new Marker() { staticX=-34, staticY=16, staticZ=30, Visible=17 },
                                            new Marker() { staticX=10, staticY=40, staticZ=20, Visible=18 },
                                            new Marker() { staticX=100, staticY=96, staticZ=20, Visible=19 } };
            Application.Run(new Form1(Data));
        }
    }
    public class Marker : IEquatable<Marker>
    {
        public double staticX { get; set; }                                                                     //stores real X coordinate of Marker
        public double staticY { get; set; }                                                                     //stores real Y coordinate of Marker
        public double staticZ { get; set; }                                                                     //stores real Z coordinate of Marker
        public double dynamicX { get; set; }                                                                    //stores transformed X coordinate of Marker
        public double dynamicY { get; set; }                                                                    //stores transformed Y coordinate of Marker
        public double dynamicZ { get; set; }                                                                    //stores transformed Z coordiante of Marker
        public string Text { get; set; }                                                                        //stores Text associated with Marker
        public int Visible { get; set; }                                                                        //stores Visibility index of Marker ( -1 : invisible :: 0 : temporary :: 1 : permanent :: 2 : X axis :: 3 : Y axis :: 4 : Z axis :: rest : Well Log Value )
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            Marker objAsMarker = obj as Marker;
            if (objAsMarker == null)
                return false;
            return Equals(objAsMarker.Text);
        }
        public bool Equals(Marker that)
        {
            if (that == null)
                return false;
            return (this.staticX == that.staticX && this.staticY == that.staticY && this.staticZ == that.staticZ && this.Text.Equals(that.Text));
        }
        public override int GetHashCode()
        {
            return this.Visible;
        }

    }
}