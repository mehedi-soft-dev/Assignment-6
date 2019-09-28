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
    public partial class CustomerInformationUI : Form
    {

        public CustomerInformationUI()
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

                if (String.IsNullOrEmpty(contactTextBox.Text))
                {
                    MessageBox.Show("Please Enter Valid Mobile Number..!");
                    return;
                }

                if (String.IsNullOrEmpty(addressTextBox.Text))
                {
                    MessageBox.Show("Please Enter Customer Address..!");
                    return;
                }

                DataTable result = SearchCustomerByName(customerNameTextBox.Text);

                if (result.Rows.Count == 0 && customerNameTextBox.TextLength<=30 && contactTextBox.TextLength==11 &&  addressTextBox.TextLength<=30)
                {
                    AddCustomer(customerNameTextBox.Text, contactTextBox.Text, addressTextBox.Text);
                    Reset();
                    MessageBox.Show("Customer Added Successfully...!");
                }
                else
                {
                    //Cheack Validation
                    if (customerNameTextBox.TextLength > 30)
                        MessageBox.Show("Customer name is required and MAXIMUM 30 Charecters...!");
                    else if (result.Rows.Count > 0)
                        MessageBox.Show("Customer Already Exist...!");
                    else if (contactTextBox.TextLength != 11)
                        MessageBox.Show("Please Enter a Valid Mobile Number...!");
                    else if (addressTextBox.TextLength > 30)
                        MessageBox.Show("Address is required and MAXIMUM 30 Charecters...!");

                    return;
                }
                
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
        }

        private void showAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable result = ShowAllCustomers();
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
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }  
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Validation
                if(String.IsNullOrEmpty(idTextBox.Text))
                {
                    MessageBox.Show("Please Enter a ID for Update...!");
                    return;
                }
                if (String.IsNullOrEmpty(customerNameTextBox.Text))
                {
                    MessageBox.Show("Please Enter Name for Update...!");
                    return;
                }
                if (String.IsNullOrEmpty(contactTextBox.Text))
                {
                    MessageBox.Show("Please Enter contact number for Update...!");
                    return;
                }
                if (String.IsNullOrEmpty(addressTextBox.Text))
                {
                    MessageBox.Show("Please Enter Address for Update...!");
                    return;
                }


                DataTable result = SearchCustomerById(Convert.ToInt32(idTextBox.Text));
                DataTable cheackName = SearchCustomerByName(customerNameTextBox.Text);

                if (cheackName.Rows.Count > 0 && (Convert.ToInt32(cheackName.Rows[0]["ID"]) != Convert.ToInt32(idTextBox.Text)))
                {
                    MessageBox.Show("Customer Name already Exist...!");
                    return;
                }

                //Update
                if (result.Rows.Count > 0)
                {
                    UpdateCustomer(Convert.ToInt32(idTextBox.Text), customerNameTextBox.Text, contactTextBox.Text, addressTextBox.Text);
                    Reset();
                    MessageBox.Show("Customer Information Updated Successfully...!");
                }
                else
                {
                    if(result.Rows.Count<=0)
                        MessageBox.Show("No Data Found with this ID...!");

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

                DataTable result = SearchCustomerByName(customerNameTextBox.Text);

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
            catch(Exception exception)
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

                DataTable result = SearchCustomerById(Convert.ToInt32(idTextBox.Text));

                if (result.Rows.Count > 0)
                {
                    DeleteCustomer(Convert.ToInt32(idTextBox.Text));
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

        private void AddCustomer(string name, string contact, string address)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"INSERT INTO Customers (Name, Contact, Address) VALUES('"+name+"','"+contact+"','"+address+"')";
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);

            //oen Connection
            sqlConnection.Open();

            //Execute Operation
            sqlCommand.ExecuteNonQuery();

            //Close Connection
            sqlConnection.Close();
            
        }

        private DataTable ShowAllCustomers()
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"SELECT * FROM Customers";
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

        private void UpdateCustomer(int id, string name, string contact, string address)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"UPDATE Customers SET Name = '"+name+"', Contact = '"+contact+"', address = '"+address+"' WHERE ID = "+id+"";
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);

            //Open Connectio
            sqlConnection.Open();

            //Execute Qeury
            sqlCommand.ExecuteNonQuery();

            //Close Connection
            sqlConnection.Close();
        }

        private DataTable SearchCustomerByName(string name)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"SELECT * FROM Customers WHERE name = '"+name+"'";
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

        private DataTable SearchCustomerById(int id)
        {
            //Connection
            string connectionString = @"Server=MH-PC\SQLEXPRESS; Database=CoffeeShop; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Command
            string sqlString = @"SELECT * FROM Customers WHERE ID = '" + id + "'";
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
            string commandString = @"DELETE FROM Customers WHERE ID = " + id + "";
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
            contactTextBox.Text = "";
            addressTextBox.Text = "";
        }

    }
}
