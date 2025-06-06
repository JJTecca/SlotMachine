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
    public partial class Form3 : Form
    {
        public string Username_Name { get; set; }
        public static Form3 form3; //irelevant
        public static Form4 form4 = new Form4();
        public Form3()
        {
            InitializeComponent();
            form3 = this;
        }
        private void ClearItems(bool hideOrShow) {
            if (hideOrShow == true && Convert.ToString(Username.Text) != "") {
                foreach (Control anyItem in form3.Controls) { if (anyItem.TabIndex >= 4 && anyItem.TabIndex<=13) { anyItem.Show(); } }
            }
            else { foreach (Control anyItem in form3.Controls) { if (anyItem.TabIndex >= 4 && anyItem.TabIndex <= 13) { anyItem.Hide(); } } }
        }
        private void Form3_Load_1(object sender, EventArgs e) {
            Username.Text = Username_Name;
            form4.UsernameForm4 = Username_Name;
            Menu.Items.Add("View account details"); Menu.Items.Add("Deposit money"); Menu.Items.Add("Withdraw money"); Menu.Items.Add("Change Password"); Menu.Items.Add("Delete Account");
            ClearItems(false);
            //smart move , using tabindex

        } //nu merge pus dupa InitializeComponent() nush dc 
        private void Menu_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection Connection; SqlCommand QUERY; SqlDataReader ReadRows;
            using (Connection = new SqlConnection("Data Source=Lenovo-82XV\\SQLEXPRESS;Initial Catalog=players_dabase;Integrated Security=True"))
                    using(QUERY=new SqlCommand("SELECT USERNAME,PASSWORD,moneyInserted,moneyWithdraw FROM players_dabase WHERE USERNAME=@username",Connection)) {
                        Connection.Open(); QUERY.Parameters.AddWithValue("@username", Username_Name);
                        using (ReadRows = QUERY.ExecuteReader()) { 
                            while (ReadRows.Read()) {
                                try { textBox1.Text = ReadRows["USERNAME"].ToString(); textBox2.Text = ReadRows["PASSWORD"].ToString();textBox3.Text = ReadRows["moneyInserted"].ToString(); }
                                catch(ArgumentNullException wrong_info) { MessageBox.Show("NO DATA TO RETRIEVE"); }
                            }
                    }           
            }
            form4.MoneyPlayed = Convert.ToInt32(textBox3.Text); //convertesc odata in int32 si dupa in string
            switch (Convert.ToString(Menu.SelectedItem))
            {
                case "View account details": { ClearItems(true); label6.Hide(); textBox4.Hide(); break; }
                case "Deposit money": { ClearItems(false); label6.Show(); textBox4.Show(); insert.Show(); label6.Text = "Desposit now"; insert.Text = "Deposit now"; break; }
                case "Withdraw money": { ClearItems(false); label6.Show(); textBox4.Show(); insert.Show(); label6.Text = "Withdraw now"; insert.Text = "Withdraw now"; break; }
                case "Change Password": { ClearItems(false); label6.Show(); textBox4.Show(); insert.Show(); label6.Text = "Change Passowrd"; insert.Text = "Change Password"; break;  }
                case "Delete Account": { ClearItems(false); insert.Show(); label6.Text = "Delete Account"; insert.Text = "Delete Account"; break; }
            }
            
        }
        private void insert_Click(object sender, EventArgs e) //folosim si pt withdraw acelasi cod dar cu -
        {
            //if-uri egale 
            SqlConnection playersConnection = new SqlConnection("Data Source=Lenovo-82XV\\SQLEXPRESS;Initial Catalog=players_dabase;Integrated Security=True");
            playersConnection.Open(); 
            if (insert.Text == "Deposit now") {
                Int32 sumDeposited = Convert.ToInt32(textBox4.Text);
                string SQLquery = "UPDATE players_dabase SET moneyInserted=moneyInserted+@moneyInserted WHERE username=@username";
                SqlCommand moneyInsert_command = new SqlCommand(SQLquery, playersConnection); moneyInsert_command.Parameters.AddWithValue("@moneyInserted", sumDeposited);
                moneyInsert_command.Parameters.AddWithValue("@username", Username_Name); moneyInsert_command.ExecuteNonQuery();
                DialogResult insert_done = MessageBox.Show("Money Deposited", "", MessageBoxButtons.OK);
                if (insert_done == DialogResult.OK) { ClearItems(false); }
            }
            if(insert.Text=="Withdraw now"){
                Int32 sumDeposited = Convert.ToInt32(textBox4.Text);
                string SQLquery = "UPDATE players_dabase SET moneyInserted=moneyInserted-@X WHERE USERNAME=@username";
                SqlCommand moneyInsert_command = new SqlCommand(SQLquery, playersConnection); moneyInsert_command.Parameters.AddWithValue("@X", sumDeposited);
                moneyInsert_command.Parameters.AddWithValue("@USERNAME", Username_Name);
                moneyInsert_command.ExecuteNonQuery(); 
                DialogResult insert_done = MessageBox.Show("Money Withdrawed", "", MessageBoxButtons.OK);
                if (insert_done == DialogResult.OK) { ClearItems(false); }
            }
            if (insert.Text == "Change Password") {
                string SQLquery = "UPDATE players_dabase SET PASSWORD=@password WHERE USERNAME=@username";
                SqlCommand moneyInsert_command = new SqlCommand(SQLquery, playersConnection); moneyInsert_command.Parameters.AddWithValue("@password", Convert.ToString(textBox4.Text));
                moneyInsert_command.Parameters.AddWithValue("@username", Username_Name);
                moneyInsert_command.ExecuteScalar(); 
                DialogResult insert_done = MessageBox.Show("Password changed", "", MessageBoxButtons.OK);
                if (insert_done == DialogResult.OK) { ClearItems(false); }
            }
            if (insert.Text == "Delete Account") { 
                string SQLquery = "DELETE FROM players_dabase WHERE USERNAME=@username";
                SqlCommand moneyInsert_command = new SqlCommand(SQLquery, playersConnection); moneyInsert_command.Parameters.AddWithValue("@username", Convert.ToString(textBox4.Text));
                moneyInsert_command.ExecuteNonQuery();
                DialogResult insert_done = MessageBox.Show("Account deleted", "", MessageBoxButtons.OK);
                if (insert_done == DialogResult.OK) { ClearItems(false); }
                playersConnection.Close();
            }
            playersConnection.Close();
        }
        private void AnotherEvent_Click(object sender, EventArgs e) { ClearItems(false); }
        private void button1_Click(object sender, EventArgs e) { form4.Show(); }
    }
}

