using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace SlotMachine
{
    public partial class Form1 : Form
    {
        
        public static Form1 instance;
        public static Form3 form3=new Form3(); //transmitem data
        public Form1()
        {
            InitializeComponent();
            instance = this; //nu e necesar 
            button1.Location = new Point(766, 600); button2.Location = new Point(400, 600);
            PasswordTBox.PasswordChar = '*';
        }
        public int Unwanted_Characters(string username)
        {
            List<String> exceptions = new List<string>(); exceptions.Add(" OR 1=1 "); exceptions.Add(" OR "); exceptions.Add(" OR 1='1'" );
            exceptions.Add(" OR 1=1--' "); exceptions.Add("/*"); exceptions.Add("CONCAT");
            foreach (string sqlInjections in exceptions) { if (username.Contains(sqlInjections) == true) return 1; }
            return 0;
        }

        private void button1_Click(object sender, EventArgs e) //butonu de Login
        {
            SqlConnection playersConnection = new SqlConnection("Data Source=Lenovo-82XV\\SQLEXPRESS;Initial Catalog=players_dabase;Integrated Security=True");
            playersConnection.Open();
            string SQLquery = "SELECT COUNT(*) FROM players_dabase WHERE USERNAME=@username AND PASSWORD=@password";
            SqlCommand login_command = new SqlCommand(SQLquery, playersConnection);
            login_command.Parameters.AddWithValue("@username", UsernameTBox.Text); login_command.Parameters.AddWithValue("@password", PasswordTBox.Text);
            int c = (int)login_command.ExecuteScalar(); 
            if ((Unwanted_Characters(Convert.ToString(UsernameTBox.Text)) == 1 || Unwanted_Characters(Convert.ToString(PasswordTBox.Text)) == 1) && c > 0) {
                MessageBox.Show("Valid Credentials but SQL injection detected"); 
            }
            else { if ((Unwanted_Characters(Convert.ToString(UsernameTBox.Text)) == 1 || Unwanted_Characters(Convert.ToString(PasswordTBox.Text)) == 1) && c < 0)
                    MessageBox.Show("Invalid Credentials and SQL injection detected"); 
            }
            if (c > 0) {                
                DialogResult dialogLoginSucces = MessageBox.Show("Login Succesfull", "Cancel", MessageBoxButtons.OK); form3.Username_Name = Convert.ToString(UsernameTBox.Text);
                if (dialogLoginSucces == DialogResult.OK) { 
                    try { form3.Show(); throw new Exception(); } //dupa schimbarea parolei apare eroarea aceasta
                    catch(Exception) { MessageBox.Show("Error in the application after login"); }
                   }
            }
            else { MessageBox.Show("Wrong");   }
            playersConnection.Close();

        }
        private void button2_Click(object sender, EventArgs e) { Form2 form2 = new Form2(); form2.Show(); }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            //UsernameTBox.PasswordChar=checkBox1.Checked ? '\0' : '*';  VAR EFICIENT
            if(checkBox1.Checked==false) { PasswordTBox.PasswordChar = '*'; }
            else { PasswordTBox.PasswordChar = '\0'; }
        }
    }
}
