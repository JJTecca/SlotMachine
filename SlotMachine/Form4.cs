using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
/*maximum prize este 80%
seven=10x , pepene=5x, BAR=6x, pruna=4x , cirese=3x , lamaie=3x , struguri=2x
matrice de valori cu indicele elementului 
bet intre 1-2 => x=2
bet intre 3-3 => x=3
bet intre 5-20 => x=4
bet intre 50-100 => x=5
*/
namespace SlotMachine
{
    public partial class Form4 : Form
    {
        public class PictureBoxes{
            public PictureBox abstract_Picturebox = new PictureBox();
            public int tabindex; 
            public string FileStream;
            public PictureBoxes() { }
            public PictureBoxes(PictureBox PIC,int tabindex,string FileStream) {
                this.FileStream = FileStream;
                abstract_Picturebox = PIC; abstract_Picturebox.Image = Image.FromFile(this.FileStream);
                this.tabindex = tabindex;
            }         
        }
        
        PictureBoxes[] masiv_etrogen_picturebox = new PictureBoxes[16];
        List<int> Symbol_Position = new List<int>();
        bool first_spin = true;
        public string UsernameForm4 { get; set; }
        public double MoneyPlayed { get; set; }
        string FilePath = "C:\\Users\\Cristi\\Desktop\\SlotMachine\\SlotMachine\\Resources\\seven.jpg";
        string FilePath1 = "C:\\Users\\Cristi\\Desktop\\SlotMachine\\SlotMachine\\Resources\\lebenita.jpg";
        string FilePath2 = "C:\\Users\\Cristi\\Desktop\\SlotMachine\\SlotMachine\\Resources\\orange.jpg";
        string FilePath3 = "C:\\Users\\Cristi\\Desktop\\SlotMachine\\SlotMachine\\Resources\\lamaie.jpg";
        string FilePath4 = "C:\\Users\\Cristi\\Desktop\\SlotMachine\\SlotMachine\\Resources\\pruna.jpg";
        string FilePath5 = "C:\\Users\\Cristi\\Desktop\\SlotMachine\\SlotMachine\\Resources\\BAR.jpg";
        string FilePath6 = "C:\\Users\\Cristi\\Desktop\\SlotMachine\\SlotMachine\\Resources\\strugure.jpg";
        string FilePath7 = "C:\\Users\\Cristi\\Desktop\\SlotMachine\\SlotMachine\\Resources\\cherry.jpeg";
        public Form4()
        {
            InitializeComponent();
           
        }
        private void CenterImages(List<PictureBox> X) { foreach (PictureBox x in X) { x.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom; } }
        private void ClearEverything(List<int> X, PictureBoxes[] Y) {
            for (int column = 0; column < 15; column++) { Y[column].abstract_Picturebox.Hide(); System.Threading.Thread.Sleep(50); }            
            for (int column = 0; column < 5; column++) { 
                Y[column].abstract_Picturebox.Show(); Y[column + 5].abstract_Picturebox.Show(); Y[column + 10].abstract_Picturebox.Show();
                System.Threading.Thread.Sleep(50);
            }
            for (int clearPosition = 0; clearPosition < 15; clearPosition++) {
                X[clearPosition] = 0; Y[clearPosition].abstract_Picturebox.Image = Image.FromFile("C:\\Users\\Cristi\\Desktop\\SlotMachine\\SlotMachine\\Resources\\white.jpg");
            }         
        }
        private void AddChances(PictureBoxes[] Y) {
            double MAX;
            for (int reset = 0; reset <= 2; reset++) {
                MAX = 0;
                double[] frecventa_tabindex = new double[10];
                for (int rows = 0; rows <= 4; rows++) {
                    frecventa_tabindex[Y[5 * reset + rows].tabindex]++; //poate sa fie intre 0-7
                }
                for (int iterator = 0; iterator < 8; iterator++) { 
                    if (frecventa_tabindex[iterator] != 0 && frecventa_tabindex[iterator] > MAX) { MAX = frecventa_tabindex[iterator]; }
                }
                //MessageBox.Show(Convert.ToString(MAX));
                Random rand_position = new Random(); int R = rand_position.Next(0, 5); R = rand_position.Next(0, 5);
                int X = 0;
                while (X<5) {
                    if (masiv_etrogen_picturebox[X].FileStream != masiv_etrogen_picturebox[(int)MAX].FileStream) {
                        masiv_etrogen_picturebox[X] = masiv_etrogen_picturebox[(int)MAX]; break; 
                    }
                    X++;
                }   
                frecventa_tabindex = null;    
            }
        }
        private void Connections(PictureBoxes[] Y,int bet_amount)
        {
            List<int> ValuesOfSymbols = new List<int> { 10, 5, 2, 3, 4, 10, 2, 5 }; //se ia tot dupa filepath-uri
            for(int rows = 0; rows <= 2; rows++) {
                int rowsIndex = 0,caz_particular=0; //asta il folosesc si ca sa adaug in suma cate legaturi o fost
                if((Y[5*rows+rowsIndex].FileStream==FilePath && Y[5 * rows + rowsIndex + 1].FileStream == FilePath && Y[5*rows+rowsIndex+2].FileStream!=FilePath)){
                    caz_particular = 1; MoneyPlayed = MoneyPlayed + bet_amount*ValuesOfSymbols[Y[rows].tabindex]*2;
                    richTextBox1.AppendText("Last win :  " + bet_amount*ValuesOfSymbols[Y[rows].tabindex]*2+"\n");
                }
                if ((Y[5 * rows + rowsIndex].FileStream == FilePath5 && Y[5 * rows + rowsIndex + 1].FileStream == FilePath5 && Y[5 * rows + rowsIndex+2].FileStream != FilePath5)) { 
                    caz_particular = 1; MoneyPlayed = MoneyPlayed + bet_amount*ValuesOfSymbols[Y[rows].tabindex]*2;
                    richTextBox1.AppendText("Last win :  " + bet_amount*ValuesOfSymbols[Y[rows].tabindex]*2+"\n");
                }
                while (Y[5*rows+rowsIndex].tabindex == Y[5*rows+rowsIndex+1].tabindex && Y[5*rows+rowsIndex+1].tabindex==Y[5*rows+rowsIndex+2].tabindex && caz_particular==0) { rowsIndex+=2; }
                //primu index ar fi 5*0+0 dupa urmatoru 5*0+0+1,5*0+0+1,5*0+0+2, al doilea este la fel
                if (rowsIndex != 0) { 
                    MoneyPlayed = MoneyPlayed + bet_amount * ValuesOfSymbols[Y[rows].tabindex] * rowsIndex; 
                    richTextBox1.AppendText("Last win :  " + bet_amount * ValuesOfSymbols[Y[rows].tabindex] * rowsIndex + "\n");
                }
                //MessageBox.Show(Convert.ToString(rowsIndex)); MERGE , altfel trb facut nu cu switch
            }
            if(Y[0].tabindex==Y[6].tabindex && Y[6].tabindex==Y[12].tabindex) { 
                MoneyPlayed = MoneyPlayed + bet_amount*ValuesOfSymbols[Y[0].tabindex]*2; 
                richTextBox1.AppendText("Last win :  " + bet_amount*ValuesOfSymbols[Y[0].tabindex]*2+"\n");
            }
            if(Y[10].tabindex==Y[6].tabindex && Y[6].tabindex == Y[2].tabindex) { 
                MoneyPlayed = MoneyPlayed + bet_amount*ValuesOfSymbols[Y[0].tabindex]*2;
                richTextBox1.AppendText("Last win :  " + bet_amount*ValuesOfSymbols[Y[10].tabindex]*2 + "\n");
            }
            //urmeaza hashingu si mail bs
        }
        private void GamblersRuin(Int32 BET_AMOUNT) {
            for (int zeros = 0; zeros < 15; zeros++) { Symbol_Position.Add(0); }
            List<PictureBox> PictureBox_Position = new List<PictureBox> { pictureBox1,pictureBox2,pictureBox3,pictureBox4,pictureBox5,pictureBox6,pictureBox7,
                                                                        pictureBox8,pictureBox9,pictureBox10,pictureBox11,pictureBox12,pictureBox13,pictureBox14,pictureBox15 };
            List<string> FilePaths = new List<string> { FilePath, FilePath1, FilePath2, FilePath3, FilePath4, FilePath5, FilePath6, FilePath7 };
            CenterImages(PictureBox_Position);
            while (Symbol_Position.Sum() != 15) {
                // ecuatia diferentiala care are forma generala P(x)= p * P(x + 1) + (1 - p) * P(x - 1) cu solutia A* r^x + B = P(x)
                double SumOfPfunction = 0; //5 cel mai bun 1-cel mai rau
                Random probability_DIV10 = new Random();
                double ratio_of_probability = probability_DIV10.Next(1, 10) ; //number between 0.1 0.9
                ratio_of_probability = probability_DIV10.Next(1, 10);
                ratio_of_probability = (1 - ratio_of_probability) / ratio_of_probability;
                double left_boundarie = -1 / (Math.Pow(ratio_of_probability, BET_AMOUNT) - 1), right_boundarie = 1/(Math.Pow(ratio_of_probability,BET_AMOUNT)-1);
                //coef lui r^x este A si termen liber este B (se face P(0) si P(N))
                if (ratio_of_probability == 5) { 
                    for (int increase = 1; increase < 5 && BET_AMOUNT <= 5; increase++) { 
                        double PFunction = increase / BET_AMOUNT; PFunction = (int)Math.Abs(PFunction); SumOfPfunction += PFunction;
                    }
                    if (SumOfPfunction > 7 && SumOfPfunction < 10) { SumOfPfunction = Math.Abs(Math.Sqrt(SumOfPfunction)); }
                    for (int _insertPosition = 0; _insertPosition < SumOfPfunction && Symbol_Position.Sum() != 15;) { 
                        Random position = new Random(); int random_position = position.Next(0, 15);
                        if (Symbol_Position[random_position] == 0) { 
                            Symbol_Position[random_position] = 1; _insertPosition++;
                            Random random_filepath = new Random(); int rand_filepath_position = random_filepath.Next(0, 8);
                            rand_filepath_position = random_filepath.Next(0, 8); //double randomizer
                            masiv_etrogen_picturebox[random_position] = new PictureBoxes(PictureBox_Position[random_position], rand_filepath_position, FilePaths[rand_filepath_position]);
                            // in obiect avem nevoie de : patrat random unde se pune poza de la filepathu random
                        }
                    }
                }
                else { 
                    for (int increase = 1; increase < 5 && BET_AMOUNT <= 5; increase++) { 
                        double PFunction = right_boundarie + Math.Pow(ratio_of_probability, increase) * left_boundarie;
                        PFunction = (int)Math.Abs(PFunction); SumOfPfunction += PFunction;
                    }
                    if (SumOfPfunction >= 7 && SumOfPfunction <= 10) { SumOfPfunction = Math.Abs(Math.Sqrt(SumOfPfunction)); }
                    for (int _insertPosition = 0; _insertPosition < SumOfPfunction && Symbol_Position.Sum() != 15;) { 
                        Random position = new Random(); int random_position = position.Next(0, 15);
                        if (Symbol_Position[random_position] == 0) { 
                            Symbol_Position[random_position] = 1; _insertPosition++;
                            Random random_filepath = new Random(); int rand_filepath_position = random_filepath.Next(0, 8);
                            rand_filepath_position = random_filepath.Next(0, 8); //double randomizer
                            masiv_etrogen_picturebox[random_position] = new PictureBoxes(PictureBox_Position[random_position], rand_filepath_position, FilePaths[rand_filepath_position]);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 3 3 3 4 4 if(x[row]==x[row+1] && x[row+1]==x[row+2]) 
        /// 1 2 6 0 0 (2 6 10) if(x[2]==x[6] && x[6]==x[10])
        /// 2 2 3 1 5 (0 6 12) if(x[0]==x[6] && x[6]==x[12])
        /// </summary>
        private void unLeu_Click_1(object sender, EventArgs e) { 
            if(unLeu.Text=="1 leu" && first_spin) {
                unLeu.Text = "1 Leu"; first_spin = false;  GamblersRuin(2); Connections(masiv_etrogen_picturebox,1);
                MoneyPlayed -= 1; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
            else { 
                ClearEverything(Symbol_Position, masiv_etrogen_picturebox);AddChances(masiv_etrogen_picturebox); 
                GamblersRuin(2); Connections(masiv_etrogen_picturebox,1);
                //HERE COMES THE CONNECTIONS
                MoneyPlayed -= 1; textBox2.Text = Convert.ToString(MoneyPlayed);
                
            }           
        }
        private void doiLei_Click(object sender, EventArgs e) {
            if (doiLei.Text == "2 lei" && first_spin) { 
                 doiLei.Text = "2 Lei"; first_spin = false;GamblersRuin(2); Connections(masiv_etrogen_picturebox,2);
                MoneyPlayed -= 2; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
            else { 
                ClearEverything(Symbol_Position, masiv_etrogen_picturebox);AddChances(masiv_etrogen_picturebox); 
                GamblersRuin(2); Connections(masiv_etrogen_picturebox,2);
                MoneyPlayed -= 2; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
        }
        private void treiLei_Click(object sender, EventArgs e) {
            if (treiLei.Text == "3 lei" && first_spin) { 
                treiLei.Text = "3 Lei"; first_spin = false;GamblersRuin(2); Connections(masiv_etrogen_picturebox,3);
                MoneyPlayed -= 3; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
            else { 
                ClearEverything(Symbol_Position, masiv_etrogen_picturebox);AddChances(masiv_etrogen_picturebox); 
                GamblersRuin(2); Connections(masiv_etrogen_picturebox,3);
                MoneyPlayed -= 3; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
        }
        private void cinciLei_Click(object sender, EventArgs e) {
            if (cinciLei.Text == "5 lei" && first_spin) { 
                 cinciLei.Text = "5 Lei"; first_spin = false; GamblersRuin(2); Connections(masiv_etrogen_picturebox,5);
                MoneyPlayed -= 5; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
            else { 
                ClearEverything(Symbol_Position, masiv_etrogen_picturebox); AddChances(masiv_etrogen_picturebox);
                GamblersRuin(3); Connections(masiv_etrogen_picturebox,5);
                MoneyPlayed -= 5; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
        }
        private void sapteLei_Click(object sender, EventArgs e) {
            if (sapteLei.Text == "7 lei" && first_spin) { 
                 sapteLei.Text = "7 Lei"; first_spin = false; GamblersRuin(2); Connections(masiv_etrogen_picturebox,7);
                MoneyPlayed -= 7; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
            else { 
                ClearEverything(Symbol_Position, masiv_etrogen_picturebox);AddChances(masiv_etrogen_picturebox); 
                GamblersRuin(3); Connections(masiv_etrogen_picturebox,7);
                MoneyPlayed -= 7; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
        }
        private void zeceLei_Click(object sender, EventArgs e) {
            if (zeceLei.Text == "10 lei" && first_spin) { 
                 zeceLei.Text = "10 Lei"; first_spin = false; GamblersRuin(2); Connections(masiv_etrogen_picturebox,10);
                MoneyPlayed -= 10; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
            else { 
                ClearEverything(Symbol_Position, masiv_etrogen_picturebox); AddChances(masiv_etrogen_picturebox); 
                GamblersRuin(3); Connections(masiv_etrogen_picturebox,10);
                MoneyPlayed -= 10; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
        }
        private void douazeciLei_Click(object sender, EventArgs e) {
            if (douazeciLei.Text == "20 lei" && first_spin) { 
                 douazeciLei.Text = "20 Lei"; first_spin = false; GamblersRuin(2); Connections(masiv_etrogen_picturebox,20);
                MoneyPlayed -= 20; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
            else { 
                ClearEverything(Symbol_Position, masiv_etrogen_picturebox); AddChances(masiv_etrogen_picturebox); 
                GamblersRuin(4); Connections(masiv_etrogen_picturebox,20);
                MoneyPlayed -= 20; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
        }
        private void sutaLei_Click(object sender, EventArgs e) {
            if (sutaLei.Text == "100 lei" && first_spin) {
                 sutaLei.Text = "100 Lei"; first_spin = false; GamblersRuin(2); Connections(masiv_etrogen_picturebox,100);
                MoneyPlayed -= 100; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
            else { 
                ClearEverything(Symbol_Position, masiv_etrogen_picturebox); AddChances(masiv_etrogen_picturebox); 
                GamblersRuin(4); Connections(masiv_etrogen_picturebox,100);
                MoneyPlayed -= 100; textBox2.Text = Convert.ToString(MoneyPlayed);
            }
        }

        private void Form4_Load_1(object sender, EventArgs e) { textBox1.Text = UsernameForm4; textBox2.Text = Convert.ToString(MoneyPlayed); }
    }
}
