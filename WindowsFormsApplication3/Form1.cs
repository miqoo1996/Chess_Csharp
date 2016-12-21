using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// 
/// https://github.com/miqoo1996/Chess_Csharp
/// 
/// @author Miqayel Ishkhanyan: <miqoo1996@mail.ru>, <miqoo1996@gmail.com>
/// @author phone: <+37495336290>, <+37494336290>
/// 
/// @copy 22.12.2016
/// 
/// start date 17.12.2016 11:00 ---- end date 22.12.2016 3:10
/// 
/// </summary>
namespace InterfacesForChess
{
    interface IChessGetter
    {
        int getRandomNumber(int a, int b);
        PictureBox getItemByName(string name);
        PictureBox getItem(int i, int j);
        PictureBox getRandomArea(List<string> areaBusi = null);
        string getImageName(int index, int areaNumber);
    }

    interface IChessSetter
    {
        void setItem(int i, int j, Image image = null);
        void setActiveFigure(string name);
        void setFigure(int type, string name);
    }

    interface IChessAddMethod
    {
        void addClickMethodFigureDynamic(string element, PictureBox area);
        void addDynamicClick(string area);
    }

    interface IChess
    {
        List<string> appendElements(List<string> elements, List<string> areaBusi);
        bool showArea(int index, int areaNumber, Color color);
        void clearChessArea();
    }
}

namespace WindowsFormsApplication3
{
    using InterfacesForChess;
    
    public partial class Form1 : Form, IChessGetter, IChessSetter, IChessAddMethod, IChess
    {
        /// <summary>
        /// example: pictureBoxD1 ----> picture Box name (for call the figure)
        /// picture box name prefix
        /// </summary>
        private string pictureBoxNamePrefix = "pictureBox";  // picture box name prefix ( before name )

        /// <summary>
        /// project directory path
        /// </summary>
        protected string path = Environment.CurrentDirectory + "\\..\\..";

        /// <summary>
        /// chess area names
        /// </summary>
        protected string[] arrChessAreaNumbers = { "A", "B", "C", "D", "E", "F", "G", "H" };

        /// <summary>
        /// active figures
        /// 
        /// 5 white figur
        /// </summary>
        protected List<string> activeFigures = new List<string> { "wk.gif", "wn.gif", "wp.gif", "wq.gif", "wr.gif" };

        /// <summary>
        /// 6 white figur
        /// </summary>
        protected List<string> whiteFigures = new List<string> { "wk.gif", "wn.gif", "wp.gif", "wp.gif", "wq.gif", "wr.gif" };

        /// <summary>
        /// 11 black figur
        /// </summary>
        protected List<string> blackFigures = new List<string> { "bb.gif", "bk.gif", "bn.gif", "bn.gif", "bp.gif", "bp.gif", "bp.gif", "bp.gif", "bp.gif", "bq.gif", "br.gif" };

        /// <summary>
        /// array area busis
        /// </summary>
        protected List<string> areaBusi = new List<string>();

        /// <summary>
        /// array active figure
        /// 
        /// </summary>
        protected PictureBox activeFigure;

        /// <summary>
        /// chess moves history
        /// 
        /// for reset chess box
        /// 
        /// List data for reset old view
        /// 
        /// </summary>
        private List<KeyValuePair<string, string>> chessMovesHistory = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// CHESS_BOX
        /// 
        /// origin
        /// 
        /// </summary>
        private PictureBox CHESS_BOX = null;

        Random rnd = new Random();

        /// <summary>
        /// figure class
        /// 
        /// for get more information the figure
        /// 
        /// param type Class (Object)
        /// 
        /// </summary>
        Figure fg = new Figure();


        /// <summary>
        /// 
        /// Form1 __magic method
        /// 
        /// </summary>
        public Form1()
        {
            // Initialize component
            InitializeComponent();
            // Initialize method
            init();
        }

        /// <summary>
        /// 
        /// Initialize method with constructor
        /// 
        /// </summary>
        private void init()
        {          
            clearChessArea();
            CHESS_BOX = pictureBox1;
            fg.setStepCounts(0);

            // new figures for event click
            setActiveFigure("wb.gif");

            // new figure
            setFigure(1, "wb.gif");
            setFigure(1, "wb.gif");
        }

        /// <summary>
        /// creates a number between a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int getRandomNumber(int a, int b)
        {
            int randomNumber = rnd.Next(a, b + 1);
            return randomNumber;
        }

        /// <summary>
        /// get item by name (picture box name)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PictureBox getItemByName(string name)
        {
            return ((PictureBox)this.Controls[name]);
        }

        /// <summary>
        /// get item
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public PictureBox getItem(int i, int j)
        {
            return ((PictureBox)getItemByName(pictureBoxNamePrefix + arrChessAreaNumbers[i] + (j + 1).ToString()));
        }

        /// <summary>
        /// get random area
        /// 
        /// if in the area exist figure not returned(Recursive Method)
        /// 
        /// </summary>
        /// <param name="areaBusi"></param>
        /// <returns>area(Recursive Method)</returns>
        public PictureBox getRandomArea(List<string> areaBusi = null)
        {
            int randomNumber1 = getRandomNumber(0, 7);
            int randomNumber2 = getRandomNumber(0, 7);

            PictureBox area = getItem(randomNumber1, randomNumber2);

            if (areaBusi != null)
            {
                bool isAppendElement = areaBusi.Contains(area.Name) == false;

                if (isAppendElement == false)
                {
                    return ((PictureBox)getRandomArea(areaBusi));
                }
            }

            return area;
        }

                /// <summary>
        /// get image name the area
        /// </summary>
        /// <param name="index"></param>
        /// <param name="areaNumber"></param>
        /// <returns></returns>
        public string getImageName(int index, int areaNumber)
        {
            string imageName = "";

            try
            {
                string area = String.Concat(pictureBoxNamePrefix, arrChessAreaNumbers[index], areaNumber);
                int bIndex = Array.IndexOf(areaBusi.ToArray(), area);
                string imagePath = getItemByName(areaBusi[bIndex]).ImageLocation;
                string[] imagePathArr = imagePath.Split('\\');
                imageName = imagePathArr[imagePathArr.Length - 1];
            }
            catch (Exception imgExp)
            {

            }

            return imageName;
        }

        /// <summary>
        /// set item
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="image"></param>
        public void setItem(int i, int j, Image image = null)
        {
            ((PictureBox)this.Controls[pictureBoxNamePrefix + arrChessAreaNumbers[i] + (j + 1).ToString()]).Image = image;
        }

        /// <summary>
        /// add new active figure
        /// </summary>
        /// <param name="name"></param>
        public void setActiveFigure(string name)
        {
            this.activeFigures.Add(name);
        }

        /// <summary>
        /// add new figure
        /// </summary>
        /// <param name="name"></param>
        public void setFigure(int type, string name)
        {
            if(type == 1)
            {
                this.whiteFigures.Add(name);
            }
            else if(type == 2)
            {
                 this.blackFigures.Add(name);
            }
        }

        /// <summary>
        /// clear chess area
        /// </summary>
        public void clearChessArea()
        {
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    setItem(i, j, null);
                }
            }
        }

        /// <summary>
        /// add click method figure dynamic
        /// </summary>
        /// <param name="element"></param>
        /// <param name="area"></param>
        public void addClickMethodFigureDynamic(string element, PictureBox area)
        {
            bool isActiveFigures = activeFigures.Contains(element) == true;
            
            if (isActiveFigures)
            {
                switch (element)
                {
                    case "wk.gif":
                        area.Click += new EventHandler(buttonWk_Click);
                        break;
                    case "wn.gif":
                        area.Click += new EventHandler(buttonWn_Click);
                        break;
                    case "wb.gif":
                        area.Click += new EventHandler(buttonWb_Click);
                        break;
                    case "wp.gif":
                        area.Click += new EventHandler(buttonWp_Click);
                        break;
                    case "wq.gif":
                        area.Click += new EventHandler(buttonWq_Click);
                        break;
                    case "wr.gif":
                        area.Click += new EventHandler(buttonWr_Click);
                        break;
                }
            }
        }

        /// <summary>
        /// append elements
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="areaBusi"></param>
        /// <returns></returns>
        public List<string> appendElements(List<string> elements, List<string> areaBusi)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                PictureBox area = getRandomArea(areaBusi);

                area.Load(String.Concat(path, "\\", elements[i]));
                areaBusi.Add(area.Name);

                addClickMethodFigureDynamic(elements[i], area);
            }
            return areaBusi;
        }

        /// <summary>
        /// reset chess box
        /// </summary>
        protected void resetChessBox()
        {
            fg.setStepCounts(0);
            labelStepCount.Text = "0";

            foreach (KeyValuePair<string, string> listAres in chessMovesHistory)
            {
                if (listAres.Value == "Peru")
                {
                    getItemByName(listAres.Key).BackColor = Color.Peru;
                }
                else if (listAres.Value == "BurlyWood")
                {
                    getItemByName(listAres.Key).BackColor = Color.BurlyWood;
                }
            }
        }

        /// <summary>
        /// add new method click
        /// </summary>
        /// <param name="area"></param>
        public void addDynamicClick(string area)
        {
            getItemByName(area).Click += new EventHandler(blackFigureRm_Click);
        }

        /// <summary>
        /// show the figure area with the color (if area exist by the name)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="areaNumber"></param>
        /// <param name="color"></param>
        /// <returns>bool true or false</returns>
        public bool showArea(int index, int areaNumber, Color color)
        {
            bool result = false;
           
            try
            {
                // get area name
                string area = String.Concat(pictureBoxNamePrefix, arrChessAreaNumbers[index], areaNumber);

                // get image name
                string imageName = getImageName(index, areaNumber);

                // if the area no exist figure
                if (areaBusi.Contains(area) == false || blackFigures.Contains(imageName) == true)
                {
                    // set color and area name
                    chessMovesHistory.Add(new KeyValuePair<string, string>(area, getItemByName(area).BackColor.Name));
                    // change color
                    getItemByName(area).BackColor = color;
                    fg.addStepCounts(1);

                    result = blackFigures.Contains(imageName) == false;

                    if(result == false && imageName != "")
                    {
                        addDynamicClick(area);
                    }
                }
            }
            catch (Exception exp)
            {
                // error
                // exp.Message for view
            }

            return result;
        }

        /// <summary>
        /// identify which button was clicked and perform necessary actions
        /// black figure click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void blackFigureRm_Click(object sender, EventArgs e)
        {
            PictureBox figure = sender as PictureBox;

            if (activeFigure.Name != "")
            {
                fg.setFigure(activeFigure);

                string areaName = fg.getAreaName();
                int areaNumber = fg.getAreaNumber();
                int index = Array.IndexOf(arrChessAreaNumbers, areaName);
                string imageName = getImageName(index, areaNumber);

                addClickMethodFigureDynamic(imageName, figure);

                /////////////////////////////// < ::before > //////////////////////////////////

                var image = activeFigure.Image;

                activeFigure.Image = null;
                figure.Image = image;

                resetChessBox();

                areaBusi.Remove(activeFigure.Name);
                // clear|remove
                activeFigure = new PictureBox();
                labelStepCount.Text = "0";
                fg.setStepCounts(0);
            }
        }

        /// <summary>
        /// identify which button was clicked and perform necessary actions
        /// wk.gif click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonWk_Click(object sender, EventArgs e)
        {
            PictureBox figure = sender as PictureBox;
            fg.setFigure(figure);

            activeFigure = figure;

            string areaName = fg.getAreaName();
            int areaNumber = fg.getAreaNumber();
            int index = Array.IndexOf(arrChessAreaNumbers, areaName);

            resetChessBox();

            showArea(index, areaNumber + 1, Color.Red);
            showArea(index, areaNumber - 1, Color.Red);

            showArea(index + 1, areaNumber, Color.Red);
            showArea(index - 1, areaNumber, Color.Red);

            showArea(index + 1, areaNumber + 1, Color.Red);
            showArea(index + 1, areaNumber - 1, Color.Red);
            showArea(index - 1, areaNumber - 1, Color.Red);
            showArea(index - 1, areaNumber + 1, Color.Red);
        }

        /// <summary>
        /// identify which button was clicked and perform necessary actions
        /// wn.gif click event
        /// 
        /// Max allowed steps count 8 (2 LEFT, 2 RIGHT, 2 TOP, 2 BOTTOM steps)
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonWn_Click(object sender, EventArgs e)
        {
            PictureBox figure = sender as PictureBox;
            fg.setFigure(figure);

            activeFigure = figure;

            string areaName = fg.getAreaName();
            int areaNumber = fg.getAreaNumber();
            int index = Array.IndexOf(arrChessAreaNumbers, areaName);

            resetChessBox();

            showArea(index + 1, areaNumber + 2, Color.Red);
            showArea(index + 1, areaNumber - 2, Color.Red);
            showArea(index + 2, areaNumber + 1, Color.Red);
            showArea(index + 2, areaNumber - 1, Color.Red);
            showArea(index - 1, areaNumber + 2, Color.Red);
            showArea(index - 1, areaNumber - 2, Color.Red);
            showArea(index - 2, areaNumber + 1, Color.Red);
            showArea(index - 2, areaNumber - 1, Color.Red);
        }
        
        /// <summary>
        /// identify which button was clicked and perform necessary actions
        /// wb.gif click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonWb_Click(object sender, EventArgs e)
        {
            PictureBox figure = sender as PictureBox;
            fg.setFigure(figure);

            activeFigure = figure;

            string areaName = fg.getAreaName();
            int areaNumber = fg.getAreaNumber();
            int index = Array.IndexOf(arrChessAreaNumbers, areaName);

            resetChessBox();

            bool toLeft = true;
            bool toRight = true;
            bool toTop = true;
            bool toBotom = true;

            for (int i = 0; i < 8; i++)
            {
                if (toLeft)
                {
                    toLeft = showArea(index + i + 1, areaNumber + i + 1, Color.Red);
                }
                if (toRight)
                {
                    toRight = showArea(index + i + 1, areaNumber - i - 1, Color.Red);
                }
                if (toTop)
                {
                    toTop = showArea(index - i - 1, areaNumber + i + 1, Color.Red);
                }
                if (toBotom)
                {
                    toBotom = showArea(index - i - 1, areaNumber - i - 1, Color.Red);
                }
            }
        }

        /// <summary>
        /// identify which button was clicked and perform necessary actions
        /// wp.gif click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonWp_Click(object sender, EventArgs e)
        {
            PictureBox figure = sender as PictureBox;
            fg.setFigure(figure);

            activeFigure = figure;

            string areaName = fg.getAreaName();
            int areaNumber = fg.getAreaNumber();
            int index = Array.IndexOf(arrChessAreaNumbers, areaName);

            resetChessBox();

            string TopimageName1 = getImageName(index, areaNumber + 1);

            if (TopimageName1 == "")
            {
                showArea(index, areaNumber + 1, Color.Red);
            }

            string imageName1 = getImageName(index + 1, areaNumber + 1);
            string imageName2 = getImageName(index - 1, areaNumber + 1);

            if (fg.isBlack(imageName1))
            {
                showArea(index + 1, areaNumber + 1, Color.Red);
            }
            if (fg.isBlack(imageName2))
            {
                showArea(index - 1, areaNumber + 1, Color.Red);
            }


            if (areaNumber == 2)
            {
                string TopimageName2 = getImageName(index, areaNumber + 2);

                if (TopimageName1 == "" && TopimageName2 == "")
                {
                    showArea(index, areaNumber + 2, Color.Red);
                }

                string imageName3 = getImageName(index + 1, areaNumber + 2);
                string imageName4 = getImageName(index - 1, areaNumber + 2);

                if (fg.isBlack(imageName3))
                {
                    showArea(index + 1, areaNumber + 2, Color.Red);
                }
                if (fg.isBlack(imageName4))
                {
                    showArea(index - 1, areaNumber + 2, Color.Red);
                }
            }
        }

        /// <summary>
        /// identify which button was clicked and perform necessary actions
        /// wq.gif click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonWq_Click(object sender, EventArgs e)
        {
            PictureBox figure = sender as PictureBox;
            fg.setFigure(figure);

            activeFigure = figure;

            string areaName = fg.getAreaName();
            int areaNumber = fg.getAreaNumber();
            int index = Array.IndexOf(arrChessAreaNumbers, areaName);

            resetChessBox();

            bool toLeft = true;
            bool toRight = true;
            bool toTop = true;
            bool toBotom = true;

            bool toLeft1 = true;
            bool toRight1 = true;

            bool toTop1 = true;
            bool toBotom1 = true;
            bool toTop2 = true;
            bool toBotom2 = true;

            for (int i = 0; i < 8; i++)
            {
                if (toLeft)
                {
                    toLeft = showArea(index + i + 1, areaNumber + i + 1, Color.Red);
                }
                if (toRight)
                {
                    toRight = showArea(index + i + 1, areaNumber - i - 1, Color.Red);
                }
                if (toTop)
                {
                    toTop = showArea(index - i - 1, areaNumber + i + 1, Color.Red);
                }
                if (toBotom)
                {
                    toBotom = showArea(index - i - 1, areaNumber - i - 1, Color.Red);
                }

                if (toLeft1)
                {
                    toLeft1 = showArea(index + i + 1, areaNumber, Color.Red);
                }

                if (toRight1)
                {
                    toRight1 = showArea(index - i - 1, areaNumber, Color.Red);
                }

                if (toTop1)
                {
                    toTop1 = showArea(index, areaNumber + i + 1, Color.Red);                    
                }
                if(toTop2)
                {
                    toTop2 = showArea(index, areaNumber + i - 1, Color.Red);
                }
                if (toBotom1)
                {
                    toBotom1 = showArea(index, areaNumber - i + 1, Color.Red);                    
                }
                if(toBotom2)
                {
                    toBotom2 = showArea(index, areaNumber - i - 1, Color.Red);
                }
            }
        }

        /// <summary>
        /// identify which button was clicked and perform necessary actions
        /// wr.gif click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonWr_Click(object sender, EventArgs e)
        {
            PictureBox figure = sender as PictureBox;
            fg.setFigure(figure);

            activeFigure = figure;

            string areaName = fg.getAreaName();
            int areaNumber = fg.getAreaNumber();
            int index = Array.IndexOf(arrChessAreaNumbers, areaName);

            resetChessBox();
            
            bool toLeft1 = true;
            bool toRight1 = true;

            bool toTop1 = true;
            bool toBotom1 = true;
            bool toTop2 = true;
            bool toBotom2 = true;

            for (int i = 0; i < 8; i++)
            {
                if (toLeft1)
                {
                    toLeft1 = showArea(index + i + 1, areaNumber, Color.Red);
                }

                if (toRight1)
                {
                    toRight1 = showArea(index - i - 1, areaNumber, Color.Red);
                }

                if (toTop1)
                {
                    toTop1 = showArea(index, areaNumber + i + 1, Color.Red);
                }
                if (toTop2)
                {
                    toTop2 = showArea(index, areaNumber + i - 1, Color.Red);
                }
                if (toBotom1)
                {
                    toBotom1 = showArea(index, areaNumber - i + 1, Color.Red);
                }
                if (toBotom2)
                {
                    toBotom2 = showArea(index, areaNumber - i - 1, Color.Red);
                }
            }
        }

        /// <summary>
        /// click method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRandomSort_Click(object sender, EventArgs e)
        {
            clearChessArea();

            resetChessBox();

            List<string> _areaBusi = new List<string>();

            _areaBusi = appendElements(whiteFigures, _areaBusi);
            _areaBusi = appendElements(blackFigures, _areaBusi);

            this.areaBusi = _areaBusi;
        }

        /// <summary>
        /// click method
        /// write the active figurs steps count
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCountSteps_Click(object sender, EventArgs e)
        {
            labelStepCount.Text = fg.getStepCount().ToString();
        }
    }
}
