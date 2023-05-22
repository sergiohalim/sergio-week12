using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace sergioweek12
{
    public partial class Form1 : Form
    {
        MySqlConnection MySqlConnection;
        MySqlCommand Command;
        MySqlDataAdapter DataAdapter;
        MySqlDataReader Reader;
        DataTable player = new DataTable();
        DataTable nationality=new DataTable();
        DataTable team= new DataTable();
        DataTable manager1= new DataTable();
        DataTable managermager1 = new DataTable();
        DataTable dtplayer = new DataTable();
        DataTable manager2 = new DataTable();
        DataTable idManager = new DataTable();
        DataTable playerdelete = new DataTable();
        string managername = "";
        string deleteplayer = "";


        public Form1()
        {
            try
            {
                string connection = "server=localhost;uid=root;pwd=;database=premier_league";
                MySqlConnection = new MySqlConnection(connection);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            InitializeComponent();
        }
        private void updateplayer()
        {
            player.Clear();
            try
            {
                string playerdata = "select*from player";
                Command = new MySqlCommand(playerdata, MySqlConnection);
                DataAdapter = new MySqlDataAdapter(Command);
                DataAdapter.Fill(player);
                dataGridView3.DataSource=(player);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string query = "select nationality_id,nation from nationality;";
            Command= new MySqlCommand(query,MySqlConnection);
            DataAdapter = new MySqlDataAdapter(Command);
            DataAdapter.Fill(nationality);
            comnationality.DataSource = nationality;
            comnationality.ValueMember = "nationality_id";
            comnationality.DisplayMember = "nation";

            string hello = "select team_id,team_name from team;";
            Command = new MySqlCommand(hello, MySqlConnection);
            DataAdapter = new MySqlDataAdapter(Command);
            DataAdapter.Fill(team);
            comteamname.DataSource = team;
            comteamname.ValueMember = "team_id";
            comteamname.DisplayMember = "team_name";

            string hello1 = "select team_id,team_name from team;";
            Command = new MySqlCommand(hello1, MySqlConnection);
            DataAdapter = new MySqlDataAdapter(Command);
            DataAdapter.Fill(manager2);
            comteam.DataSource = manager2;
            comteam.ValueMember = "team_id";
            comteam.DisplayMember = "team_name";

            string hello2 = "select team_id,team_name from team;";
            Command = new MySqlCommand(hello2, MySqlConnection);
            DataAdapter = new MySqlDataAdapter(Command);
            DataAdapter.Fill(playerdelete);
            comboBox2.DataSource = playerdelete;
            comboBox2.ValueMember = "team_id";
            comboBox2.DisplayMember = "team_name";

            string managermager = "select m.manager_name,m.birthdate,n.nation from manager m,nationality n where m.working=0 and m.nationality_id=n.nationality_id;";
            Command = new MySqlCommand(managermager, MySqlConnection);
            DataAdapter = new MySqlDataAdapter(Command);
            DataAdapter.Fill(managermager1);
            dataGridView2.DataSource = managermager1;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string player_id = textpalyerid.Text;
            string player_name = textname.Text;
            string team_number = textteamnumber.Text;
            string nationality_id = comnationality.SelectedValue.ToString();
            string playing_pos = textposition.Text;
            string height = textheight.Text;
            string weight = textweight.Text;
            string birthdate = dateTimebirthdate.Value.ToString("yyyy-MM-dd");
            string teamname = comteamname.SelectedValue.ToString();
            string player = $"insert into player values('{player_id}','{team_number}','{player_name}','{nationality_id}','{playing_pos}','{height}','{weight}','{birthdate}','{teamname}',1,0)";
            try
            {
                MySqlConnection.Open();
                Command = new MySqlCommand(player, MySqlConnection);
                Reader=Command.ExecuteReader();
            }
            catch(Exception f)
            {
                MessageBox.Show(f.Message);
            }
            finally
            {
                MySqlConnection.Close();
                updateplayer();
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string playerID = dataGridView3.CurrentRow.Cells[0].Value.ToString();
            label15.Text = playerID;
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dtplayer.Clear();
            try
            {
                string chooseteam = $"select p.player_name,n.nation,p.playing_pos,p.team_number,p.height,p.weight,p.birthdate from player p, team t,nationality n where p.team_id = t.team_id and p.status = 1 and t.team_id = '{comboBox2.SelectedValue.ToString()}' and n.nationality_id=p.nationality_id;";
                Command = new MySqlCommand(chooseteam, MySqlConnection);
                DataAdapter = new MySqlDataAdapter(Command);
                DataAdapter.Fill(dtplayer);
                dataGridView4.DataSource = dtplayer;
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            manager1.Clear();
            string manager = $" select m.manager_name,t.team_name,m.birthdate,n.nation from manager m,team t,nationality n where m.manager_id=t.manager_id and n.nationality_id=m.nationality_id and t.team_id='{comteam.SelectedValue.ToString()}';";
            Command = new MySqlCommand(manager, MySqlConnection);
            DataAdapter = new MySqlDataAdapter(Command);
            DataAdapter.Fill(manager1);
            dataGridView1.DataSource = manager1;
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            managername = dataGridView2.CurrentCell.Value.ToString();
            blabla.Text=managername;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            idManager.Clear();
            string updatemanager = manager1.Rows[0][0].ToString();
            string managerid = $"select manager_id from manager where manager_name='{managername}';";
            Command = new MySqlCommand(managerid,MySqlConnection);
            DataAdapter = new MySqlDataAdapter(Command);
            DataAdapter.Fill(idManager);
            string id = idManager.Rows[0][0].ToString();

            string update = $"UPDATE team t,manager mkanan,manager mkiri set t.manager_id='{id}',mkiri.working=1,mkanan.working=0 where mkanan.manager_name='{updatemanager}' AND mkiri.manager_id='{id}' AND t.team_id='{comteam.SelectedValue.ToString()}';";
            try
            {
                MySqlConnection.Open();
                Command = new MySqlCommand(update, MySqlConnection);
                Reader = Command.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                MySqlConnection.Close();
                manager1.Clear();
                managermager1.Clear();
                string manager = $" select m.manager_name,t.team_name,m.birthdate,n.nation from manager m,team t,nationality n where m.manager_id=t.manager_id and n.nationality_id=m.nationality_id and t.team_id='{comteam.SelectedValue.ToString()}';";
                Command = new MySqlCommand(manager, MySqlConnection);
                DataAdapter = new MySqlDataAdapter(Command);
                DataAdapter.Fill(manager1);
                dataGridView1.DataSource = manager1;
                string managermager = "select m.manager_name,m.birthdate,n.nation from manager m,nationality n where m.working=0 and m.nationality_id=n.nationality_id;";
                Command = new MySqlCommand(managermager, MySqlConnection);
                DataAdapter = new MySqlDataAdapter(Command);
                DataAdapter.Fill(managermager1);
                dataGridView2.DataSource = managermager1;
                managername = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int count = 0;
            for (int i = 0; i < dtplayer.Rows.Count; i++)
            {
                count++;
            }
            if(count<=11)
            {
                MessageBox.Show("error", "cannot remove player", MessageBoxButtons.OK);
            }
            else
            {
                string deletepemain = $"UPDATE player set status=0 WHERE player_name='{deleteplayer}';";
               
                try
                {
                    MySqlConnection.Open();
                    Command = new MySqlCommand(deletepemain, MySqlConnection);
                    Reader = Command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    dtplayer.Clear();
                    MySqlConnection.Close();
                    string chooseteam = $"select p.player_name,n.nation,p.playing_pos,p.team_number,p.height,p.weight,p.birthdate from player p, team t,nationality n where p.team_id = t.team_id and p.status = 1 and t.team_id = '{comboBox2.SelectedValue.ToString()}' and n.nationality_id=p.nationality_id;";
                    Command = new MySqlCommand(chooseteam, MySqlConnection);
                    DataAdapter = new MySqlDataAdapter(Command);
                    DataAdapter.Fill(dtplayer);
                    dataGridView4.DataSource = dtplayer;
                }
            }
            deleteplayer = "";
        }

        private void dataGridView4_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            deleteplayer = dataGridView4.CurrentCell.Value.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void addPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playerid.Visible = true;
            name.Visible = true;
            teamnumber.Visible = true;
            national.Visible = true;
            position.Visible = true;
            height.Visible = true;
            weight.Visible = true;
            birthdate.Visible = true;
            teamname.Visible = true;
            textpalyerid.Visible = true;
            textname.Visible = true;
            textteamnumber.Visible = true;
            comnationality.Visible = true;
            textposition.Visible = true;
            textweight.Visible = true;
            dateTimebirthdate.Visible = true;
            textname.Visible = true;
            dataGridView3.Visible = true;
            textheight.Visible = true;
            Addplayer.Visible = true;
            comteamname.Visible = true;
        }

        private void addManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blabla.Visible = true;
            manager.Visible = true;
            dataGridView1.Visible = true;
            dataGridView2.Visible = true;
            buttonaddmanager.Visible = true;
            comteam.Visible = true;
            teamname2.Visible = true;

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playerr.Visible = true;
            teamid.Visible = true;
            comboBox2.Visible = true;
            delateplayer.Visible = true;
            dataGridView4.Visible = true;
        }
    }
}
