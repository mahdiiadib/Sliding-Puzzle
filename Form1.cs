using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixTheCat
{
    public partial class Form1 : Form
    {
        PictureBox[,] picBoxes = new PictureBox[3, 3];
        Bitmap[,] images = new Bitmap[3, 3];
        string[,] tags = new string[3, 3];
        int clicks = 0;


        public Form1()
        {
            InitializeComponent();
            FillForm();
            Shuffle();
        }


        void FillForm()
        {
            picBoxes[0, 0] = pictureBox00;
            picBoxes[0, 1] = pictureBox01;
            picBoxes[0, 2] = pictureBox02;
            picBoxes[1, 0] = pictureBox10;
            picBoxes[1, 1] = pictureBox11;
            picBoxes[1, 2] = pictureBox12;
            picBoxes[2, 0] = pictureBox20;
            picBoxes[2, 1] = pictureBox21;
            picBoxes[2, 2] = pictureBox22;

            images[0, 0] = Properties.Resources.solid_color_image;
            images[0, 1] = Properties.Resources.c2;
            images[0, 2] = Properties.Resources.c3;
            images[1, 0] = Properties.Resources.c4;
            images[1, 1] = Properties.Resources.c5;
            images[1, 2] = Properties.Resources.c6;
            images[2, 0] = Properties.Resources.c7;
            images[2, 1] = Properties.Resources.c8;
            images[2, 2] = Properties.Resources.c9;

            tags[0, 0] = "T00";
            tags[0, 1] = "T01";
            tags[0, 2] = "T02";
            tags[1, 0] = "T10";
            tags[1, 1] = "T11";
            tags[1, 2] = "T12";
            tags[2, 0] = "T20";
            tags[2, 1] = "T21";
            tags[2, 2] = "T22";

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    picBoxes[i, j].Image = images[i, j];
                    picBoxes[i, j].Tag = tags[i, j];
                }
            }
        }


        bool CheckWinner()
        {
            //MessageBox.Show("Winner checking");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (tags[i, j] != picBoxes[i, j].Tag.ToString()) return false;
                }
            }
            return true;
        }


        void Shuffle()
        {
            SwapPicBoxImage(ref pictureBox00, ref pictureBox01);
            SwapPicBoxImage(ref pictureBox01, ref pictureBox11);
            SwapPicBoxImage(ref pictureBox11, ref pictureBox21);


            //Random rnd = new Random();
            //int swaps = rnd.Next(10, 21);
            ////MessageBox.Show($"{swaps}");

            //for (int i = 1; i <= swaps; i++)
            //{
            //    int row1 = rnd.Next(0, 3);
            //    int row2 = rnd.Next(0, 3);
            //    int col1 = rnd.Next(0, 3);
            //    int col2 = rnd.Next(0, 3);
            //    //MessageBox.Show($"{row1}, {col1}, {row2}, {col2}");
            //    SwapPicBoxImage(ref picBoxes[row1, col1], ref picBoxes[row2, col2]);
            //}

            clicks = 0;
            lblClicks.Text = $"Clicks: {clicks}";
        }


        void SwapPicBoxImage(ref PictureBox a, ref PictureBox b)
        {
            Image tmp; string stmp;

            tmp = a.Image; stmp = a.Tag.ToString();
            a.Image = b.Image; a.Tag = b.Tag;
            b.Image = tmp; b.Tag = stmp;
        }


        bool isValid(int i, int j)
        {
            //MessageBox.Show($"{i}, {j}: {i >= 0 && j >= 0 && i < 3 && j < 3}");
            return i >= 0 && j >= 0 && i < 3 && j < 3;
        }


        void GoodClickWorks(int x, int y)
        {
            picBoxes[x, y].Size = new Size(105, 105);
            lblClicks.Text = $"Clicks: {++clicks}";
            bool win = CheckWinner();
            if (win)
            {
                DialogResult dr = MessageBox.Show($"Congratulations! You fixed the cat in {clicks} clicks!\nDo you wish to play again?", "Winner", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes) Shuffle();
                else Application.Exit();
            }
        }


        private void OnPicBoxClick(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            int x = -1, y = -1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (picBoxes[i, j].Image == p.Image)
                    {
                        x = i;
                        y = j;
                        break;
                    }
                }
            }


            if (isValid(x + 1, y))
            {
                var b1 = Properties.Resources.solid_color_image;
                var b2 = (Bitmap)picBoxes[x + 1, y].Image;
                var res = ComparingImages.Compare(b1, b2);

                if (res == ComparingImages.CompareResult.ciCompareOk)
                {
                    SwapPicBoxImage(ref picBoxes[x, y], ref picBoxes[x + 1, y]);
                    GoodClickWorks(x, y);
                    return;
                }
            }
            
            if (isValid(x - 1, y))
            {
                var b1 = Properties.Resources.solid_color_image;
                var b2 = (Bitmap)picBoxes[x - 1, y].Image;
                var res = ComparingImages.Compare(b1, b2);

                if (res == ComparingImages.CompareResult.ciCompareOk)
                {
                    SwapPicBoxImage(ref picBoxes[x, y], ref picBoxes[x - 1, y]);
                    GoodClickWorks(x, y);
                    return;
                }
            }
            
            if (isValid(x, y + 1))
            {
                var b1 = Properties.Resources.solid_color_image;
                var b2 = (Bitmap)picBoxes[x, y + 1].Image;
                var res = ComparingImages.Compare(b1, b2);

                if (res == ComparingImages.CompareResult.ciCompareOk)
                {
                    SwapPicBoxImage(ref picBoxes[x, y], ref picBoxes[x, y + 1]);
                    GoodClickWorks(x, y);
                    return;
                }
            }
            
            if (isValid(x, y - 1))
            {
                var b1 = Properties.Resources.solid_color_image;
                var b2 = (Bitmap)picBoxes[x, y - 1].Image;
                var res = ComparingImages.Compare(b1, b2);

                if (res == ComparingImages.CompareResult.ciCompareOk)
                {
                    SwapPicBoxImage(ref picBoxes[x, y], ref picBoxes[x, y - 1]);
                    GoodClickWorks(x, y);
                    return;
                }
            }

            //if (!moveOver) MessageBox.Show("No space to move.");
        }

        private void OnPicBoxMouseEnter(object sender, EventArgs e)
        {
            var b1 = Properties.Resources.solid_color_image;
            var b2 = (Bitmap)((PictureBox)sender).Image;
            var res = ComparingImages.Compare(b1, b2);
            if (res != ComparingImages.CompareResult.ciCompareOk) ((PictureBox)sender).Size = new Size(110, 110);
        }

        private void OnPicBoxMouseLeave(object sender, EventArgs e)
        {
            ((PictureBox)sender).Size = new Size(105, 105);
        }

        private void btnReshuffle_Click(object sender, EventArgs e)
        {
            Shuffle();
        }
    }




    public class ComparingImages
    {
        public enum CompareResult
        {
            ciCompareOk,
            ciPixelMismatch,
            ciSizeMismatch
        };

        public static CompareResult Compare(Bitmap bmp1, Bitmap bmp2)
        {
            CompareResult cr = CompareResult.ciCompareOk;

            //Test to see if we have the same size of image
            if (bmp1.Size != bmp2.Size)
            {
                cr = CompareResult.ciSizeMismatch;
            }
            else
            {
                //Convert each image to a byte array
                System.Drawing.ImageConverter ic = new System.Drawing.ImageConverter();
                byte[] btImage1 = new byte[1];
                btImage1 = (byte[])ic.ConvertTo(bmp1, btImage1.GetType());
                byte[] btImage2 = new byte[1];
                btImage2 = (byte[])ic.ConvertTo(bmp2, btImage2.GetType());

                //Compute a hash for each image
                SHA256Managed shaM = new SHA256Managed();
                byte[] hash1 = shaM.ComputeHash(btImage1);
                byte[] hash2 = shaM.ComputeHash(btImage2);

                //Compare the hash values
                for (int i = 0; i < hash1.Length && i < hash2.Length && cr == CompareResult.ciCompareOk; i++)
                {
                    if (hash1[i] != hash2[i]) cr = CompareResult.ciPixelMismatch;
                }
            }
            return cr;
        }
    }

}
