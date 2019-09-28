using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class ItemInformationUI : Form
    {
        public ItemInformationUI()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(itemNameTextBox.Text))
                {
                    MessageBox.Show("Please Enter Item Name...!");
                    return;
                }
                if (String.IsNullOrEmpty(priceTextBox.Text))
                {
                    MessageBox.Show("Please Enter Item Price...!");
                    return;
                }

                DataTable result = SearchItemByName(itemNameTextBox.Text);

                //Add Item
                if (result.Rows.Count == 0)
                {
                    AddItem(itemNameTextBox.Text, Convert.ToInt32(priceTextBox.Text));
                    Reset();
                    MessageBox.Show("Item Added Successfully...!");
                }
                else
                {
                    if (result.Rows.Count > 0)
                        MessageBox.Show("Item Already Exist...!");
                    
                    return;
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void showAllButton_Click(object sender, EventArgs e)
        {

            try
            {

                DataTable result = ShowAllItems();
                if (result.Rows.Count > 0)
                {
                    showDataGridView.DataSource = result;
                }
                else
                {
                    MessageBox.Show("No Data Found");
                    return;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(idTextBox.Text))
                {
                    MessageBox.Show("Please Enter a ID for Update...!");
                    return;
                }
                if (String.IsNullOrEmpty(itemNameTextBox.Text))
                {
                    MessageBox.Show("Please Enter Item Name for Update...!");
                    return;
                }
                if (String.IsNullOrEmpty(priceTextBox.Text))
                {
                    MessageBox.Show("Please Enter Price for Update...!");
                    return;
                }


                DataTable result = SearchItemById(Convert.ToInt32(idTextBox.Text));
                DataTable cheackName = SearchItemByName(itemNameTextBox.Text);

                if (cheackName.Rows.Count > 0 && (Convert.ToInt32(cheackName.Rows[0]["ID"]) != Convert.ToInt32(idTextBox.Text)))
                {
                    MessageBox.Show("Item Name already Exist...!");
                    return;
                }
                    
                //Update
                if (result.Rows.Count > 0)
                {
                    UpdateCustomer(Convert.ToInt32(idTextBox.Text), itemNameTextBox.Text, Convert.ToInt32(priceTextBox.Text));
                    Reset();
                    MessageBox.Show("Item Information Updated Successfully...!");
                }
                else
                {
                    if (result.Rows.Count <= 0)
                        MessageBox.Show("No Data Found with this ID...!");
                   
                    return;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(itemNameTextBox.Text))
                {
                    MessageBox.Show("Please Enter a Name for Search...!");
                    return;
                }

                DataTable result = SearchItemByName(itemNameTextBox.Text);

                if (result.Rows.Count > 0)
                {
                    showDataGridView.DataSource = result;
                }
                else
                {
                    MessageBox.Show("No Data Found With This Name...!");
                    return;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(idTextBox.Text))
                {
                    MessageBox.Show("Please Enter a ID for Delete...!");
                    return;
                }

                DataTable result = SearchItemById(Convert.ToInt32(idTextBox.Text));

                if (result.Rows.Count > 0)
                {
                    DeleteCustomer(Convert.ToInt32(idTextBox.Text));
                    MessageBox.Show("Item Deleted Successfully...!");
                }
                else
                {
                    MessageBox.Show("No Data Found With This ID...!");
                    return;
                }
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void AddItem(string name, int price)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"INSERT INTO Items (Name, Price) VALUES('" + name + "'," + price + ")";
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);

            //oen Connection
            sqlConnection.Open();

            //Execute Operation
            sqlCommand.ExecuteNonQuery();

            //Close Connection
            sqlConnection.Close();

        }

        private DataTable ShowAllItems()
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"SELECT * FROM Items";
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);

            //Connection Open
            sqlConnection.Open();

            //Execute Operation
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close Connection
            sqlConnection.Close();

            return dataTable;
        }

        private DataTable SearchItemByName(string name)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"SELECT * FROM Items WHERE name = '" + name + "'";
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);

            //Connection Open
            sqlConnection.Open();

            //Execute Crud Operation
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Connection Close
            sqlConnection.Close();

            return dataTable;
        }

        private void UpdateCustomer(int id, string name, int price)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"UPDATE Items SET Name = '" + name + "', Price = " + price + " WHERE ID = " + id + "";
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);

            //Open Connectio
            sqlConnection.Open();

            //Execute Qeury
            sqlCommand.ExecuteNonQuery();

            //Close Connection
            sqlConnection.Close();
        }

        private DataTable SearchItemById(int id)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"SELECT * FROM Items WHERE ID = '" + id + "'";
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);

            //Open Connection
            sqlConnection.Open();

            //Execute CRUD Operation
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close Connection
            sqlConnection.Close();

            return dataTable;
        }

        private void DeleteCustomer(int id)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command 
            string commandString = @"DELETE FROM Items WHERE ID = " + id + "";
            SqlCommand sqlCommand = new SqlCommand(commandString, sqlConnection);

            //Open
            sqlConnection.Open();

            //Delete
            sqlCommand.ExecuteNonQuery();

            //Close
            sqlConnection.Close();
        }

        private void Reset()
        {
            idTextBox.Text = "";
            itemNameTextBox.Text = "";
            priceTextBox.Text = "";
            
        }
    }
}
