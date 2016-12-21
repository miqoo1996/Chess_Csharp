using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;

namespace WindowsFormsApplication3
{
    class Figure
    {
        private PictureBox figure;

        protected int stepCounts = 0;


        public void setFigure(PictureBox figure)
        {
            this.figure = figure;
        }

        public void setStepCounts(int count = 0)
        {
            this.stepCounts = count;
        }

        public void addStepCounts(int count = 0)
        {
            this.stepCounts += count;
        }


        public int getStepCount()
        {
            return this.stepCounts;
        }

        public PictureBox getFigure()
        {
            return ((PictureBox) this.figure);
        }

        public string getName()
        {
            return getFigure().Name;
        }

        public string getAreaName()
        {
            string figureName = getName();

            // Instantiate the regular expression object.
            Regex r = new Regex(@"(.?)\d", RegexOptions.IgnoreCase);
            Match m = r.Match(figureName);

            Regex r1 = new Regex(@"\w{1}", RegexOptions.IgnoreCase);
            Match m2 = r1.Match(m.Value);

            return m2.Value;
        }

        public int getAreaNumber()
        {
            string figureName = getName();

            // Instantiate the regular expression object.
            Regex r = new Regex(@"\d", RegexOptions.IgnoreCase);
            Match m = r.Match(figureName);

            return Int32.Parse(m.Value);
        }

        public bool isBlack(string figureName)
        {
            // Instantiate the regular expression object.
            Regex r = new Regex(@"[a-z]{1}", RegexOptions.IgnoreCase);
            Match m = r.Match(figureName);
           
            return m.Value == "b" ? true : false;
        }
    }
}
