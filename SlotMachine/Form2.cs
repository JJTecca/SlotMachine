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
    
    public partial class Form2 : Form
    {
        player NewComer;
        int ID;
        public static Form2 form2;
        public class player
        {
            public Int32 AccountBalance,MoneyInserted,MoneyWithdraw;
            public DateTime history_transaction;
            public string username, password;
            public player(Int32 AccountBalanace,Int32 MoneyInserted,Int32 MoneyWithdraw,
                          DateTime history_transaction,string username,string password) 
            {
                this.AccountBalance = AccountBalance; this.history_transaction = history_transaction;
                this.username = username; this.password = password;
                this.MoneyInserted = MoneyInserted; this.MoneyWithdraw = MoneyWithdraw;
            }
        }
        public Form2()
        {
            InitializeComponent();
            form2 = this;
        }
        public int verificare_bani_introdusi() { if (Convert.ToInt32(newAccountMoneyIn.Text) != 0) { return 1; } return 0; }
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime registrare = DateTime.Now;
            if (verificare_bani_introdusi() == 1 && Convert.ToInt32(newAccountMoneyIn.Text)>=20) { //no username created
                NewComer = new player(0, Convert.ToInt32(newAccountMoneyIn.Text), 0, registrare, Convert.ToString(newAccountUName.Text), Convert.ToString(newAccountPName.Text));
            }
            else { MessageBox.Show("TREBUIE SA INTRODUCI O SUMA DE BANI MAI MARE DE 19 DE LEI"); }
            SqlConnection newPlayerConnection = new SqlConnection("Data Source=Lenovo-82XV\\SQLEXPRESS;Initial Catalog=players_dabase;Integrated Security=True");
            newPlayerConnection.Open(); string sqlquery = "INSERT INTO players_dabase (USERNAME,PASSWORD,moneyInserted,moneyWithdraw,tranzactions,ID) " +
                                                        "VALUES (@username,@password,@moneyIn,@moneyOut,@tranzactions,@ID)";
            SqlCommand register_command = new SqlCommand(sqlquery, newPlayerConnection);
            NewComer.MoneyWithdraw = 0;
            register_command.Parameters.AddWithValue("@username", NewComer.username); register_command.Parameters.AddWithValue("@password", NewComer.password);
            register_command.Parameters.AddWithValue("@moneyIn", NewComer.MoneyInserted); register_command.Parameters.AddWithValue("@moneyOut", NewComer.MoneyWithdraw);
            register_command.Parameters.AddWithValue("@tranzactions",registrare ); register_command.Parameters.AddWithValue("@ID", ++ID);
            string sqlquery1 = "SELECT COUNT(*) FROM players_dabase WHERE username=@username"; SqlCommand insert_command = new SqlCommand(sqlquery1, newPlayerConnection);
            insert_command.Parameters.AddWithValue("@username", NewComer.username);
            if((int)insert_command.ExecuteScalar() == 0) {
                register_command.ExecuteNonQuery();
                DialogResult dialog = MessageBox.Show("Account created", "Cancel", MessageBoxButtons.OK);
                if(dialog==DialogResult.OK) { form2.Hide(); }
            }
            else {
                DialogResult dialog = MessageBox.Show("Username already exists", "", MessageBoxButtons.OK);
                if(dialog==DialogResult.OK) { form2.Hide(); }
            }
            newPlayerConnection.Close();
        }
    }
}
