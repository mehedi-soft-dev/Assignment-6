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
    public partial class OrderInformationUI : Form
    {
        public OrderInformationUI()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(customerNameTextBox.Text))
                {
                    MessageBox.Show("Please Enter Customer Name..!");
                    return;
                }
                    
                DataTable customerSearchResult = SearchCustomerByName(customerNameTextBox.Text);

                if (String.IsNullOrEmpty(itemNameTextBox.Text))
                {
                    MessageBox.Show("Please Enter an Item");
                    return;
                }

                DataTable itemSearchResult = SearchItemByName(itemNameTextBox.Text);
                
                if (customerSearchResult.Rows.Count>0 && itemSearchResult.Rows.Count>0 && !String.IsNullOrEmpty(quantityTextBox.Text))
                {
                    string name = customerNameTextBox.Text;
                    string contact = customerSearchResult.Rows[0]["Contact"].ToString();
                    string address = customerSearchResult.Rows[0]["Address"].ToString();
                    string item = itemSearchResult.Rows[0]["Name"].ToString();
                    int price = Convert.ToInt32(itemSearchResult.Rows[0]["Price"]);
                    int quantity = Convert.ToInt32(quantityTextBox.Text);
                    int totalBill = price * quantity;

                    AddOrder(name, contact, address, item, price, quantity, totalBill);
                    Reset();
                    MessageBox.Show("Order Added Successfully...!");
                }
                else
                {
                    if(customerSearchResult.Rows.Count == 0)
                        MessageBox.Show("No Customer Found by This Name!");
                    else if(itemSearchResult.Rows.Count == 0)
                        MessageBox.Show("No Item Found!");
                    else if(String.IsNullOrEmpty(quantityTextBox.Text))
                        MessageBox.Show("Please Enter Quantity!");

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
                DataTable result = ShowAllOrders();
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
                    MessageBox.Show("Please Enter ID to Update Order Information!");
                    return;
                }

                DataTable orderSearchResult = SearchOrderById(Convert.ToInt32(idTextBox.Text));

                if (orderSearchResult.Rows.Count == 0)
                {
                    MessageBox.Show("No Order Information is found with this ID..!");
                    return;
                }

                if (String.IsNullOrEmpty(customerNameTextBox.Text))
                {
                    MessageBox.Show("Please Enter Customer Name to Update Order Information!");
                    return;
                }
                if (String.IsNullOrEmpty(itemNameTextBox.Text))
                {
                    MessageBox.Show("Please Enter Item Name to Update Order Information!");
                    return;
                }

                DataTable customerSearchResult = SearchCustomerByName(customerNameTextBox.Text);
                DataTable itemSearchResult = SearchItemByName(itemNameTextBox.Text);

                if (customerSearchResult.Rows.Count > 0 && itemSearchResult.Rows.Count > 0)
                {
                    string name = customerNameTextBox.Text;
                    string contact = customerSearchResult.Rows[0]["Contact"].ToString();
                    string address = customerSearchResult.Rows[0]["Address"].ToString();
                    string item = itemSearchResult.Rows[0]["Name"].ToString();
                    int price = Convert.ToInt32(itemSearchResult.Rows[0]["Price"]);
                    int quantity = Convert.ToInt32(quantityTextBox.Text);
                    int totalBill = price * quantity;

                    UpdateOrder(Convert.ToInt32(idTextBox.Text), name, contact, address, item, price, quantity, totalBill);
                    MessageBox.Show("Update Order Information Successfully..!");
                }
                else
                {
                    if (customerSearchResult.Rows.Count == 0)
                        MessageBox.Show("No Customer Found with this Name..!");
                    else if (itemSearchResult.Rows.Count == 0)
                        MessageBox.Show("No Item Found With this Name..!");

                    return;
                }
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(customerNameTextBox.Text))
                {
                    MessageBox.Show("Please Enter a Name for Search...!");
                    return;
                }

                DataTable result = SearchOrderByCustomerName(customerNameTextBox.Text);

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

                DataTable result = SearchOrderById(Convert.ToInt32(idTextBox.Text));

                if (result.Rows.Count > 0)
                {
                    DeleteOrder(Convert.ToInt32(idTextBox.Text));
                    MessageBox.Show("Customer Information Deleted Successfully...!");
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

        private void AddOrder(string name, string contact, string address, string item, int price, int quantity, int totalBill)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"INSERT INTO Orders (Name, Contact, Address, Item, Price, Quantity, TotalBill) VALUES('" + name + "','" + contact + "','" + address + "', '"+item+"', "+price+", "+quantity+", "+totalBill+")";
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);

            //oen Connection
            sqlConnection.Open();

            //Execute Operation
            sqlCommand.ExecuteNonQuery();

            //Close Connection
            sqlConnection.Close();

        }

        private DataTable ShowAllOrders()
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"SELECT * FROM Orders";
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

        private void UpdateOrder(int id, string name, string contact, string address, string item, int price, int quantity, int totalBill)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"UPDATE Orders SET Name = '" + name + "', Contact = '" + contact + "', Address = '" + address + "', Item = '"+item+"', Price = "+price+", Quantity = "+quantity+", TotalBill = "+totalBill+" WHERE ID = " + id + "";
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);

            //Open Connectio
            sqlConnection.Open();

            //Execute Qeury
            sqlCommand.ExecuteNonQuery();

            //Close Connection
            sqlConnection.Close();
        }

        private DataTable SearchOrderByCustomerName(string name)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"SELECT * FROM Orders WHERE Name = '" + name + "'";
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

        private DataTable SearchOrderById(int id)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"SELECT * FROM Orders WHERE ID = '" + id + "'";
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


        private DataTable SearchCustomerByName(string name)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"SELECT * FROM Customers WHERE name = '" + name + "'";
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

        private void DeleteOrder(int id)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command 
            string commandString = @"DELETE FROM Orders WHERE ID = " + id + "";
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
            customerNameTextBox.Text = "";
            itemNameTextBox.Text = "";
            quantityTextBox.Text = "";
        }
    }
}
