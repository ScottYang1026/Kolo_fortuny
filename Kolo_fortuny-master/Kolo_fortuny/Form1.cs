using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
/***
 * 20170109~20170111 群鵬屏東大路觀、高雄義大尾牙, 20170110義大抽獎
 * 總人數：35人
 * 總獎金：原179800, 加碼：3000->4000, 2000->4000, 新總獎金：194800
 * 獎項：30000 * 1, 20000 * 1, 10000 * 2, 8000 * 5, 5000 * 8, 3000 * 5, 2000 * 5, 600 * 8
 * 抽獎rule：每一個人按照區來決定金額大小，第一區的人數留4位變成最後面4位，且最大兩個獎項30000, 20000也只能這4為才能夠抽
 * 
 * */

namespace Kolo_fortuny
{
    public partial class Form1 : Form
    {
        Haslo haselko;
        Pole[] pole;
        Koło koloFortuny;
        bool wheelIsMoved;
        float startWheelTimes;
        float wheelTimes;
        
        //Timer gameTimer;
        Timer wheelTimer;
        Button[] przycisk;
        Button[] samogloska;
        Button[] spolgloska;
        //下列此行數值暫時拿掉
        String[] hasla = { "OCEANARIUM", "UNIWERSYTET", "TITANIC", "KRAJOBRAZ", "MEDYCYNA", "TELEWIZOR" };
        //String[] hasla = { };//"OCEANARIUM", "UNIWERSYTET", "TITANIC", "KRAJOBRAZ", "MEDYCYNA", "TELEWIZOR" };
        Random rand;
        int los;
        //int[,] array2D = new int[,] 
        int[,] dMember1 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 } };
        int[,] dMember2 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 } };
        int[,] dMember3 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 }, {10, 1} };
        int[,] dMember4 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 } };

        String[] Member1 = { "陳亭如","鄭雅綺","洪蓉玫","崔士堅","陳芃儒","王琨元","聶耀君","陳立恒","林嘉俊" };
        String[] Member2 = { "陳志豪", "游婷聿", "李奕緯", "郭展羽", "吳柏諺", "李璧伶", "陳孟宗", "楊國樺" };
        String[] Member3 = { "許閣珮", "方碩緯", "陳玟卉", "郭展次", "王皓", "楊淳迪", "許竣凱", "尹邦仲", "顏嘉宏", "粘錦芳" };
        String[] Member4 = { "林怡欣","葉家瑋","蔡宜君","李耀旗","許惟恩","康喻涵","殷純慧","柯言達" };



        int[,] bonus = new int[,] { { 30000, 1 }, { 20000, 1 }, { 10000, 2 }, { 8000, 5 }, { 5000, 8 }, { 3000, 5 }, { 2000, 5 }, { 600, 8 } };
        int iRotateTimer;
        int iMemberCounter;
        int iMember1Counter;
        int iMember2Counter;
        int iMember3Counter;
        int iMember4Counter;
        int iRealWheelTime;
        int iRealMember;
        int iRealBonus;
        int iRealPerson;
        bool bGetResult;
        FileInfo EngleLottery = new FileInfo(@"C:\Lottery\EngleLottery.txt");
        FileInfo LotteryNumber = new FileInfo(@"C:\Lottery\LotteryNumber.txt");
        FileInfo fMember1 = new FileInfo(@"C:\Lottery\Member1.txt");
        FileInfo fMember2 = new FileInfo(@"C:\Lottery\Member2.txt");
        FileInfo fMember3 = new FileInfo(@"C:\Lottery\Member3.txt");
        FileInfo fMember4 = new FileInfo(@"C:\Lottery\Member4.txt");
        FileInfo fBonus = new FileInfo(@"C:\Lottery\Bonus.txt");

        DirectoryInfo logDirection = new System.IO.DirectoryInfo(@"C:\Lottery\");

        //File.Create("C:\\EngleLottery.txt");

        Gra gra;
        Gracz gracz1;

        public Form1()
        {
            //rand = new Random();
            //los = rand.Next(0, hasla.Length);
            koloFortuny = new Koło();
            //haselko = new Haslo(hasla[los]);
            //pole = new Pole[haselko.size];
            //gracz1 = new Gracz("player");
            wheelIsMoved = false;
            wheelTimes = 200;
            InitializeComponent();
            rysujHaslo();
            wheelTimer = new Timer();
            wheelTimer.Interval = 10;
            wheelTimer.Tick += wheelTimer_Tick;
            iRotateTimer = 0;
            iMemberCounter = 0;
            iMember1Counter = 9;
            iMember2Counter = 8;
            iMember3Counter = 10;
            iMember4Counter = 8;
            iRealWheelTime = 0;
            iRealMember = 0;
            iRealBonus = 0;
            iRealPerson = 0;
            bGetResult = false;
            button1.Enabled = false;
            label1.Visible = false;
            label2.Visible = false;
            if (logDirection.Exists == false)
                logDirection.Create();
           
            //EngleLottery = new FileInfo(@"C:\Lottery\EngleLottery.txt");
            //EngleLottery.Create();
            //przycisk = new Button[35];
            //samogloska = new Button[9];
            //spolgloska = new Button[26];
            /*
            przycisk[0] = button1;
            przycisk[1] = button2;
            przycisk[2] = button3;
            przycisk[3] = button4;
            przycisk[4] = button5;
            przycisk[5] = button6;
            przycisk[6] = button7;
            przycisk[7] = button8;
            przycisk[8] = button9;
            przycisk[9] = button10;
            przycisk[10] = button11;
            przycisk[11] = button12;
            przycisk[12] = button13;
            przycisk[13] = button14;
            przycisk[14] = button15;
            przycisk[15] = button16;
            przycisk[16] = button17;
            przycisk[17] = button18;
            przycisk[18] = button19;
            przycisk[19] = button20;
            przycisk[20] = button21;
            przycisk[21] = button22;
            przycisk[22] = button23;
            przycisk[23] = button24;
            przycisk[24] = button25;
            przycisk[25] = button26;
            przycisk[26] = button27;
            przycisk[27] = button28;
            przycisk[28] = button29;
            przycisk[29] = button30;
            przycisk[30] = button31;
            przycisk[31] = button32;
            przycisk[32] = button33;
            przycisk[33] = button34;
            przycisk[34] = button35;

            samogloska[0] = button1;
            samogloska[1] = button2;
            samogloska[2] = button7;
            samogloska[3] = button8;
            samogloska[4] = button12;
            samogloska[5] = button20;
            samogloska[6] = button21;
            samogloska[7] = button28;
            samogloska[8] = button32;



            spolgloska[0] = button3;
            spolgloska[1] = button4;
            spolgloska[2] = button5;
            spolgloska[3] = button6;
            spolgloska[4] = button9;
            spolgloska[5] = button10;
            spolgloska[6] = button11;
            spolgloska[7] = button13;
            spolgloska[8] = button14;
            spolgloska[9] = button15;
            spolgloska[10] = button16;
            spolgloska[11] = button17;
            spolgloska[12] = button18;
            spolgloska[13] = button19;
            spolgloska[14] = button22;
            spolgloska[15] = button23;
            spolgloska[16] = button24;
            spolgloska[17] = button25;
            spolgloska[18] = button26;
            spolgloska[19] = button27;
            spolgloska[20] = button29;
            spolgloska[21] = button30;
            spolgloska[22] = button31;
            spolgloska[23] = button33;
            spolgloska[24] = button34;
            spolgloska[25] = button35;

            for (int i = 0; i < 34; i++ )
            {
                przycisk[i].IsAccessible = true;    //juz wybrana
                przycisk[i].Enabled = true;         //aktywna albo wygaszona
                przycisk[i].Visible = false;        // widoczna lub nie
            }

            gra = new Gra();

            gameTimer = new Timer();
            gameTimer.Interval = 100;
            gameTimer.Tick += gameTimer_Tick;
            label4.Text = gra.podpowiedz[0];

            gameTimer.Start();
            */
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
           

        }

        private void obsluzButton(object sender, EventArgs e)
        {
            Boolean ifExist = false;
            Button obiekt = (Button)sender;
            for(int i=0; i<haselko.size; i++ )
            {
                if ((haselko.pole[i].Text).Equals((obiekt.Text)))
                {
                    haselko.pole[i].UseSystemPasswordChar = false;
                    obiekt.IsAccessible = false;
                    
                    if (obiekt.Text.Equals("A") || obiekt.Text.Equals("Ą")
                        || obiekt.Text.Equals("E") || obiekt.Text.Equals("Ę")
                        || obiekt.Text.Equals("I") || obiekt.Text.Equals("O")
                        || obiekt.Text.Equals("U") || obiekt.Text.Equals("Ó")
                        || obiekt.Text.Equals("Y") )
                    {
                        gracz1.stanKonta -= 300;
                        label6.Text = Convert.ToString(gracz1.stanKonta);
                    }
                    else
                    {
                        gra.odgadnietaSpolgloska += 1;
                        gracz1.stanKonta += gra.stawka;
                        label6.Text = Convert.ToString(gracz1.stanKonta);
                        gra.etap = 3;
                    }

                    ifExist = true;
                    haselko.ileOdsloniete++;
                }
            }

            if( !ifExist )
            {
                label4.Text = gra.podpowiedz[4];
                gra.odgadnietaSpolgloska = 0;
                gra.etap = 1;
            }

        }
        
        public Bitmap rotateImage()
        {
            Bitmap rotatedImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform((pictureBox1.Width / 2), pictureBox1.Height / 2); //set the rotation point as the center into the matrix
                g.RotateTransform(koloFortuny.kat); //rotate
                g.TranslateTransform(-pictureBox1.Width / 2, -pictureBox1.Height / 2); //restore rotation point into the matrix
                g.DrawImage(koloFortuny.tempObrazek, new Point(0, 0)); //draw the image on the new bitmap
            }
            return rotatedImage;
        }

        public static Bitmap RotateImage(Image image, float angle)
        {
            return RotateImage(image, new PointF((float)image.Width / 2, (float)image.Height / 2), angle);
        }

        public static Bitmap RotateImage(Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);
            //angle += 18;
            
            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }

        private void RotateImage(PictureBox pb, Image img, float angle)
        {
            if (img == null || pb.Image == null)
                return;

            Image oldImage = pb.Image;
            pb.Image = RotateImage(img, angle);
            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }

        private void wheelTimer_Tick(object sender, EventArgs e)
        {
            label4.Visible = false;
          

            if (wheelIsMoved && wheelTimes > 0)
            {
                iRotateTimer += 1;

                ///*
                if(iRotateTimer >= 1 && iRotateTimer <= 18)
                {
                    koloFortuny.kat += 60F;
                    koloFortuny.kat = koloFortuny.kat % 360;
                    RotateImage(pictureBox1, koloFortuny.obrazek, koloFortuny.kat);
                }
                else if(iRotateTimer >= 19 && iRotateTimer <= 34)
                {
                    koloFortuny.kat += 45F;
                    koloFortuny.kat = koloFortuny.kat % 360;
                    RotateImage(pictureBox1, koloFortuny.obrazek, koloFortuny.kat);
                }
                else if (iRotateTimer >= 35 && iRotateTimer <= 52)
                {
                    koloFortuny.kat += 40F;
                    koloFortuny.kat = koloFortuny.kat % 360;
                    RotateImage(pictureBox1, koloFortuny.obrazek, koloFortuny.kat);
                }
                else if(iRotateTimer >= 53 && iRotateTimer <= 64)
                {
                    koloFortuny.kat += 30F;
                    koloFortuny.kat = koloFortuny.kat % 360;
                    RotateImage(pictureBox1, koloFortuny.obrazek, koloFortuny.kat);
                }
                else if (iRotateTimer >= 65 && iRotateTimer <= 78)
                {
                    koloFortuny.kat += 25F;
                    koloFortuny.kat = koloFortuny.kat % 360;
                    RotateImage(pictureBox1, koloFortuny.obrazek, koloFortuny.kat);
                }
                
                else if (iRotateTimer >= 79 && iRotateTimer <= 79)
                {
                    koloFortuny.kat += 10F;
                    koloFortuny.kat = koloFortuny.kat % 360;
                    RotateImage(pictureBox1, koloFortuny.obrazek, koloFortuny.kat);
                }
                  
                else
                {
                    koloFortuny.kat += wheelTimes/10;
                    //koloFortuny.kat += 10F;
                    koloFortuny.kat = koloFortuny.kat % 360;
                    RotateImage(pictureBox1, koloFortuny.obrazek, koloFortuny.kat);
                    wheelTimes--;
                    
                }
               // */
                /*   
                koloFortuny.kat += wheelTimes / 10;
                //koloFortuny.kat += 10F;
                koloFortuny.kat = koloFortuny.kat % 360;
                RotateImage(pictureBox1, koloFortuny.obrazek, koloFortuny.kat);
                wheelTimes--;
              */

            }

            koloFortuny.stan = Convert.ToInt32(Math.Ceiling(koloFortuny.kat / 45));
            

            if (koloFortuny.stan == 0)
            {
                koloFortuny.stan = 0;
            }
            else
            {
                koloFortuny.stan -= 1;
            }
            

            //label1.Text = Convert.ToString(koloFortuny.kat);
            //label2.Text = Convert.ToString(koloFortuny.stan);
            //label3.Text = Convert.ToString(koloFortuny.wartosciStanu[koloFortuny.stan]);

            //gra.stawka = koloFortuny.wartosciStanu[koloFortuny.stan];
            //gra.podpowiedz[2] = "Grasz o " + gra.stawka;

            if (wheelTimes == 0)
            {
                label3.Text = Convert.ToString(koloFortuny.wartosciStanu[koloFortuny.stan]);
                wheelIsMoved = false;
                iRotateTimer = 0;

                if (!LotteryNumber.Exists)
                    using (StreamWriter sw = LotteryNumber.CreateText())
                        sw.WriteLine("{0}", iMemberCounter);
                else
                {
                    LotteryNumber.Delete();
                    using (StreamWriter sw = LotteryNumber.CreateText())
                        sw.WriteLine("{0}", iMemberCounter);
                }
                /*
                for (int i = 0; i < przycisk.Length;i++ )
                {
                    if (przycisk[i].IsAccessible)
                    {
                        przycisk[i].Visible = true;
                    }
                }
                
                if(koloFortuny.wartosciStanu[koloFortuny.stan] == 0)
                {
                    gracz1.stanKonta = 0;
                    gra.etap = 1;
                }
                else
                {
                    gra.etap = 2;
                }
                */
                //label5.Text = Convert.ToString(iRealMember);
                //label6.Text = Convert.ToString(iRealPerson);
                //label1.Text = Convert.ToString(iRealBonus);
                wheelTimer.Stop();
                button36.Enabled = true;
                
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {

            /*
            switch (gra.etap)
            {
                case 1:
                    etap1();
                    break;
                case 2:
                    etap2();
                    break;
                case 3:
                    etap3();
                    break;
            }

            if(haselko.ileOdsloniete>0 && haselko.ileOdsloniete == haselko.size)
            {
                gracz1.zgadnieteHaslo = true;
                haselko.ileOdsloniete =0;
            }

            if(gracz1.zgadnieteHaslo)
            {
                gameTimer.Stop();

                if (DialogResult.OK == MessageBox.Show("Wygrałeś!. Wygrana kwota to " + gracz1.stanKonta + ". Rozpocząć kolejną grę? ", "Alert"
                              , MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    System.Diagnostics.Process.Start(Application.ExecutablePath); // to start new instance of application
                    this.Close();
                }
                else
                {
                    System.Windows.Forms.Application.Exit();
                }
                gracz1.zgadnieteHaslo = false;

            }
            */
        }

        public void etap1()
        {
            label4.Visible = false;
            
            //pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);

            //for (int i = 0; i < przycisk.Length; i++)
            //{
             //   przycisk[i].Visible = false;
            //}

        }

        public void etap2()
        {
            label4.Visible = false;
            label4.Text = gra.podpowiedz[2];
            //pictureBox1.Click -= new System.EventHandler(this.pictureBox1_Click);


                    for (int i = 0; i < samogloska.Length; i++)
                    {
                         if (samogloska[i].IsAccessible)
                         {
                             samogloska[i].Enabled = false;
                         }
                    
                    } 


            for (int i = 0; i < spolgloska.Length; i++)
            {
                if(spolgloska[i].IsAccessible)
                {
                   spolgloska[i].Enabled = true;
                   spolgloska[i].Visible = true;
                }
                else
                {
                   spolgloska[i].Enabled = false;
                   spolgloska[i].Visible = false;
                }
            }

           
        }

        public void etap3()
        {
            label4.Text = gra.podpowiedz[1];
            //pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);

            for (int i = 0; i < spolgloska.Length; i++) spolgloska[i].Enabled = false;


            if (gracz1.stanKonta >= 300)
            {
                for (int i = 0; i < samogloska.Length; i++) samogloska[i].Enabled = true;
            }
            else
            {
                for (int i = 0; i < samogloska.Length; i++) samogloska[i].Enabled = false;
            }

            for (int i = 0; i < spolgloska.Length; i++)
            {
                spolgloska[i].Enabled = false;
            }
        }
        /*
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            wheelIsMoved = true;
            //Random rand = new Random();
            //wheelTimes = rand.Next(150, 200);
            //wheelTimes = 104;
            //startWheelTimes = wheelTimes;
           
           
            //decisionPerson();
            //wheelTimes = iRealWheelTime;
            //startWheelTimes = iRealWheelTime;
           
           // for (int i = 0; i < przycisk.Length; i++)
           // {
           //     przycisk[i].Visible = false;
           // }
            wheelTimer.Start();
        }
    */
        /*
          * 總共抽獎35位
          * 1.抽哪一區
          * 2.決定金額，塞入座標位址
          * 3.決定人名
          * 
          * */
        public void decisionResult()
        {
            //int[,] dMember1 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 } };
            //int[,] dMember2 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 } };
            //int[,] dMember3 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 }, { 10, 1 } };
            //int[,] dMember4 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 } };
            //int[,] bouns = new int[,] { { 30000, 1 }, { 20000, 1 }, { 10000, 2 }, { 8000, 5 }, { 5000, 8 }, { 3000, 5 }, { 2000, 5 }, { 600, 8 } };
        
            iMemberCounter = 35;
            iMember1Counter = 9;
            iMember2Counter = 8;
            iMember3Counter = 10;
            iMember4Counter = 8;
          
              
        }
        public int decisionMember()
        {
            bool loopMember = true;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            int member = (rand.Next() % 4) + 1;
            do
            {
                if (member == 1)
                {
                    if (iMember1Counter <= 9 && iMember1Counter >= 5) //此區人數介於9~5人時，才可以抽。即要留4位到最後總人數4位才參加抽獎
                    {
                        iMember1Counter -= 1;
                        loopMember = false;
                    }
                    else if (iMember1Counter <= 4 && iMemberCounter > 31 && iMember1Counter != 0)//總人數最後四位時，才可以抽此區
                    {
                        iMember1Counter -= 1;
                        loopMember = false;
                    }
                    else
                    {
                        loopMember = true;
                        member = (rand.Next() % 4) + 1;
                    }
                    
                }
                //if ((member == 1 && iMember1Counter != 0) && (iMemberCounter >))
                //{
                //    iMember1Counter -= 1;
                //    loopMember = false;
                //}
                else if (member == 2 && iMember2Counter != 0)
                {
                    iMember2Counter -= 1;
                    loopMember = false;
                    
                }
                else if (member == 3 && iMember3Counter != 0)
                {
                    iMember3Counter -= 1;
                    loopMember = false;
                    
                }
                else if (member == 4 && iMember4Counter != 0)
                {
                    iMember4Counter -= 1;
                    loopMember = false;
                   
                }
                else //抽到哪一區的人員已經全部抽完，再跑一次抽區的部分
                {
                    loopMember = true;

                    member = (rand.Next() % 4) + 1;
                }
            } while (loopMember);
            
            return member;
        }
        public void decisionBouns()
        {
            //int[,] dMember1 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 } };
            //int[,] dMember2 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 } };
            //int[,] dMember3 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 }, { 10, 1 } };
            //int[,] dMember4 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 } };
            //int[,] bouns = new int[,] { { 30000, 1 }, { 20000, 1 }, { 10000, 2 }, { 8000, 5 }, { 5000, 8 }, { 3000, 5 }, { 2000, 5 }, { 600, 8 } };
        
            int iMember = decisionMember();
            iRealMember = iMember;
            Random moneyRand = new Random(unchecked((int)DateTime.Now.Ticks));
            int iBonus;
            bool loopBouns = true;
            do
            {
                if (iMember == 1)
                {
                    iBonus = (moneyRand.Next() % 4) + 1;//決定這一區的人金額
                    if (bonus[iBonus - 1, 1] != 0)
                    {
                        
                        if ((bonus[iBonus - 1, 0] == 30000 || bonus[iBonus - 1, 0] == 20000) && iMember1Counter > 4)//只有剩下4位時才可以抽20000 & 30000
                        {
                            iBonus = (moneyRand.Next() % 4) + 1;
                            loopBouns = true;
                        }
                        else if (bonus[iBonus - 1, 0] == 30000)
                        {
                            iRealWheelTime = (moneyRand.Next() % 3) + 120;
                            iRealBonus = 30000;
                            loopBouns = false;
                            
                        }
                        else if (bonus[iBonus - 1, 0] == 20000)
                        {
                            iRealWheelTime = (moneyRand.Next() % 3) + 124;
                            iRealBonus = 20000;
                            loopBouns = false;
                        }
                        else if (bonus[iBonus - 1, 0] == 10000)
                        {
                            iRealWheelTime = (moneyRand.Next() % 3) + 128;
                            iRealBonus = 10000;
                            loopBouns = false;
                        }
                        else if (bonus[iBonus - 1, 0] == 8000)
                        {
                            iRealWheelTime = (moneyRand.Next() % 4) + 100;
                            iRealBonus = 8000;
                            loopBouns = false;
                        }

                        if (!loopBouns)
                        {
                            bonus[iBonus - 1, 1] -= 1;
                        }
                    }
                    else
                    {
                        iBonus = (moneyRand.Next() % 4) + 1;
                        loopBouns = true;
                    }
                }
                else if (iMember == 2)
                {

                    iRealWheelTime = (moneyRand.Next(111111) % 4) + 104;
                    bonus[4, 1] -= 1;
                    iRealBonus = 5000;
                    loopBouns = false;
                }
                else if (iMember == 3)
                {
                    iBonus = (moneyRand.Next(1111111) % 2) + 6;
                    
                    if (bonus[iBonus - 1, 1] != 0)
                    {
                        bonus[iBonus - 1, 1] -= 1;
                        if (bonus[iBonus - 1, 0] == 3000)
                        {
                            iRealWheelTime = (moneyRand.Next() % 4) + 108;
                            iRealBonus = 3000;
                        }
                        else if (bonus[iBonus - 1, 0] == 2000)
                        {
                            iRealWheelTime = (moneyRand.Next() % 4) + 112;
                            iRealBonus = 2000;
                        }
                        
                        loopBouns = false;
                    }
                    else
                    {
                        iBonus = (moneyRand.Next() % 4) + 1;
                        loopBouns = true;
                    }
                    
                }
                else if (iMember == 4)
                {
                    iRealWheelTime = (moneyRand.Next() % 3) + 116;
                    bonus[7, 1] -= 1;
                    iRealBonus = 600;
                    loopBouns = false;
                }
               
            } while (loopBouns);
            
        }
        public void decisionPerson()
        {
            
            decisionBouns();

            Random personRand = new Random(unchecked((int)DateTime.Now.Ticks));
            
            //int[,] dMember1 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 } };
            //int[,] dMember2 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 } };
            //int[,] dMember3 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 }, { 10, 1 } };
            //int[,] dMember4 = new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 } };
            //int[,] bouns = new int[,] { { 30000, 1 }, { 20000, 1 }, { 10000, 2 }, { 8000, 5 }, { 5000, 8 }, { 3000, 5 }, { 2000, 5 }, { 600, 8 } };
            int iPerson = 0;
            bool loopPerson = true;
            do
            {
                if (iRealMember == 1)
                {
                    iPerson = (personRand.Next() % 9) + 1;
                    if (dMember1[iPerson - 1, 1] != 0)
                    {
                        iRealPerson = iPerson;
                        dMember1[iPerson - 1, 1] = 0;
                        loopPerson = false;
                    }
                    else
                    {
                        loopPerson = true;
                    }
                }
                else if (iRealMember == 2)
                {
                    iPerson = (personRand.Next() % 8) + 1;
                    if (dMember2[iPerson - 1, 1] != 0)
                    {
                        iRealPerson = iPerson;
                        dMember2[iPerson - 1, 1] = 0;
                        loopPerson = false;
                    }
                    else
                    {
                        loopPerson = true;
                    }
                }
                else if (iRealMember == 3)
                {
                    iPerson = (personRand.Next() % 10) + 1;
                    if (dMember3[iPerson - 1, 1] != 0)
                    {
                        iRealPerson = iPerson;
                        dMember3[iPerson - 1, 1] = 0;
                        loopPerson = false;
                    }
                    else
                    {
                        loopPerson = true;
                    }
                }
                else if (iRealMember == 4)
                {
                    iPerson = (personRand.Next() % 8) + 1;
                    if (dMember4[iPerson - 1, 1] != 0)
                    {
                        iRealPerson = iPerson;
                        dMember4[iPerson - 1, 1] = 0;
                        loopPerson = false;
                    }
                    else
                    {
                        loopPerson = true;
                    }
                }

            } while (loopPerson);
            
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button36_Click(object sender, EventArgs e)
        {
            iMemberCounter++;
            
                if (iMemberCounter <= 35)
                {
                    label8.Text = Convert.ToString(iMemberCounter);
                    decisionPerson();

                }
           
            label4.Text = Convert.ToString(iRealBonus);
            label6.Text = Convert.ToString(iRealWheelTime);
            //label3.Text = Convert.ToString(0);
            switch (iRealMember)
            {
                case 1:
                    label9.Text = Member1[iRealPerson - 1];
                    break;
                case 2:
                    label9.Text = Member2[iRealPerson - 1];
                    break;
                case 3:
                    label9.Text = Member3[iRealPerson - 1];
                    break;
                case 4:
                    label9.Text = Member4[iRealPerson - 1];
                    break;
            }
            /*
             * FileInfo EngleLottery = new FileInfo(@"C:\EngleLottery.txt");
        FileInfo LotteryNumber = new FileInfo(@"C:\LotteryNumber.txt");
        FileInfo fMember1 = new FileInfo(@"C:\Member1.txt");
        FileInfo fMember2 = new FileInfo(@"C:\Member2.txt");
        FileInfo fMember3 = new FileInfo(@"C:\Member3.txt");
        FileInfo fMember4 = new FileInfo(@"C:\Member4.txt");
        FileInfo fBonus = new FileInfo(@"C:\Bonus.txt");
             * */

            //Create a file to write to.
           // using (StreamWriter sw = EngleLottery.CreateText())
            //{
            //    sw.WriteLine("{0} [{1}{2}] {3}",label9.Text,iRealMember-1,iRealPerson-1, iRealBonus);
                //sw.WriteLine("And");
                //sw.WriteLine("Welcome");
//            }	
            if (!EngleLottery.Exists)
                using (StreamWriter sw = EngleLottery.CreateText())
                    sw.WriteLine("{0} [{1},{2}] {3} {4}", label9.Text, iRealMember - 1, iRealPerson - 1, iRealBonus, DateTime.Now.ToString());
            else
                using (StreamWriter sw = EngleLottery.AppendText())
                    sw.WriteLine("{0} [{1},{2}] {3} {4}", label9.Text, iRealMember - 1, iRealPerson - 1, iRealBonus, DateTime.Now.ToString());
            /*****************************************************/
            if (!fMember1.Exists)
            {
                using (StreamWriter sw = fMember1.CreateText())
                    for (int i = 0; i < 9; i++)
                        sw.WriteLine("{0}", dMember1[i, 1]);
            }
            else
            {
                fMember1.Delete();
                using (StreamWriter sw = fMember1.CreateText())
                    for (int i = 0; i < 9; i++)
                        sw.WriteLine("{0}", dMember1[i, 1]);
            }
            /*****************************************************/
            if (!fMember2.Exists)
            {
                using (StreamWriter sw = fMember2.CreateText())
                    for (int i = 0; i < 8; i++)
                        sw.WriteLine("{0}", dMember2[i, 1]);
            }
            else
            {
                fMember2.Delete();
                using (StreamWriter sw = fMember2.CreateText())
                    for (int i = 0; i < 8; i++)
                        sw.WriteLine("{0}", dMember2[i, 1]);
            }

            if (!fMember3.Exists)
            {
                using (StreamWriter sw = fMember3.CreateText())
                    for (int i = 0; i < 10; i++)
                        sw.WriteLine("{0}", dMember3[i, 1]);
            }
            else
            {
                fMember3.Delete();
                using (StreamWriter sw = fMember3.CreateText())
                    for (int i = 0; i < 10; i++)
                        sw.WriteLine("{0}", dMember3[i, 1]);
            }

            if (!fMember4.Exists)
            {
                using (StreamWriter sw = fMember4.CreateText())
                    for (int i = 0; i < 8; i++)
                        sw.WriteLine("{0}", dMember4[i, 1]);
            }
            else
            {
                fMember4.Delete();
                using (StreamWriter sw = fMember4.CreateText())
                    for (int i = 0; i < 8; i++)
                        sw.WriteLine("{0}", dMember4[i, 1]);
            }   
            RotateImage(pictureBox1, koloFortuny.obrazek, 0);
            koloFortuny.kat = 0;
            wheelTimes = iRealWheelTime;//角度、即決定會停在哪一個位置
            label3.Text = "";
            bGetResult = true;
            button36.Enabled = false;
            button1.Enabled = true;
            //this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
                
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            wheelIsMoved = true;
            //Random rand = new Random();
            //wheelTimes = rand.Next(150, 200);
            //wheelTimes = 104;
            //startWheelTimes = wheelTimes;

            /*
           * 總共抽獎35位
           * 1.抽哪一區
           * 2.決定金額，塞入座標位址
           * 3.決定人名
           * 
           * */
            //decisionPerson();
            //wheelTimes = iRealWheelTime;
            //startWheelTimes = iRealWheelTime;

            // for (int i = 0; i < przycisk.Length; i++)
            // {
            //     przycisk[i].Visible = false;
            // }
            wheelTimer.Start();
        }
        
        

   

    }
}
