using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;


namespace WpfApp1
{
    public partial class AdminPage : Page
    {

        LinqToDataClassesDataContext dataContext;
        string connectionString = ConfigurationManager.ConnectionStrings["WpfApp1.Properties.Settings.LibrarysqlConnectionString"].ConnectionString;
        public AdminPage()
        {
            InitializeComponent();            
            PopulateGrid();
            scrollview.VerticalScrollBarVisibility = 0;
        }

        public void PopulateGrid()
        {
            //Populates the grid with a full data context of what is in the SQL Database
            
            dataContext = new LinqToDataClassesDataContext(connectionString);
            MainDataGrid.ItemsSource = dataContext.Librarysqltables;
            MainDataGrid.FontSize = 30;
            MainDataGrid.SelectedValuePath = "Id";



        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //Creates an object and addes the properties from the Text boxes
            //adds that object to the database via Linq
            //Clears the data from the text boxes to make sure that the user knows it has been added. Throws an error if the add failed. 
            
            Librarysqltable newEntry = new Librarysqltable();
            string idnumstring = IDtxt.Text.ToString();
            int Id;
            bool properID = Int32.TryParse(IDtxt.Text, out Id);
            if (properID)
            {
                newEntry.Id = Id;

            }
            else
            {
                MessageBox.Show(idnumstring + " is not a proper Tag ID.");
            }

            string Date = Datetxt.Text;
            string Title = Titletxt.Text;
            string Location = Locationtxt.Text;
            newEntry.Date = Date;
            newEntry.Title = Title;
            newEntry.Location = Location;
            dataContext.Librarysqltables.InsertOnSubmit(newEntry);
            try
            {
                dataContext.SubmitChanges();
                PopulateGrid();
                IDtxt.Clear();
                Datetxt.Clear();
                Titletxt.Clear();
                Locationtxt.Clear();
            }
            catch
            {
                MessageBox.Show("Entry could not be added at this time, Please try again Later");
            }

            PopulateGrid();

        }

        private void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Selected the item and create an object of that selection so the program can work with it. 

            Librarysqltable oldEntry = new Librarysqltable();
            oldEntry = MainDataGrid.SelectedItem as Librarysqltable;
            int selectednum = MainDataGrid.SelectedIndex;
            int count = MainDataGrid.Items.Count;
            
            /* TroubleShoot Data Selection Logic
            MessageBox.Show(selectednum + " Select Int.");
            MessageBox.Show("Total Count " + count); */


            if (selectednum != -1 && selectednum < count - 1)
            {              
                IDtxt.Text = oldEntry.Id.ToString();              
                Datetxt.Text = oldEntry.Date.ToString();
                Titletxt.Text = oldEntry.Title.ToString();
                Locationtxt.Text = oldEntry.Location.ToString();
            }
            else
            {
                //Clear Text if nothing is selected or a Blank container is selected.
                IDtxt.Clear();
                Datetxt.Clear();
                Titletxt.Clear();
                Locationtxt.Clear();
            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            int Id;

            // tries to parse the ID so we can make sure that the program can work with the Selection
            // and it applies the changes using the ID as a reference.

            if (Int32.TryParse(IDtxt.Text, out Id))
            {
                               
                var query = from entry in dataContext.Librarysqltables
                            where entry.Id == Id
                            select entry;

                // Execute the query, and change the column values
                // you want to change.
                foreach (Librarysqltable entry in query)
                {
                    entry.Title = Titletxt.Text;
                    entry.Date = Datetxt.Text;
                    entry.Location = Locationtxt.Text;
                    // Insert any additional changes to column values.
                }

                // Submit the changes to the database.
                try
                {
                    dataContext.SubmitChanges();
                    MessageBox.Show("Update Sucessful.");
                }
                catch
                {
                    MessageBox.Show("Update Failed.");
                }
            }
            else
            {
                MessageBox.Show("ID could not be found, Error!");
            }

          

            PopulateGrid();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            //Removes selected Item and Submits the changes.

            Librarysqltable edititem = new Librarysqltable();

            edititem = MainDataGrid.SelectedItem as Librarysqltable;

            dataContext.Librarysqltables.DeleteOnSubmit(edititem);

            try
            {
                dataContext.SubmitChanges();
                PopulateGrid();
                IDtxt.Clear();
                Datetxt.Clear();
                Titletxt.Clear();
                Locationtxt.Clear();
                MessageBox.Show("Item Removed");

            }
            catch
            {
                MessageBox.Show("Changes could not be made at this time.");
            }
            PopulateGrid();
        }

        private void Searchbtn_Click(object sender, RoutedEventArgs e)
        {
            List<Librarysqltable> results = new List<Librarysqltable>();

            string searchtxt = searchbartextbox.Text;

            if (searchbartextbox.Text.Length > 0) 
            {
                if (searchtxt.All(char.IsDigit))
                {
                    // First and primary check aside from date, Tag should always be correct and searched 000 dont really matter yet.

                    Librarysqltable search = dataContext.Librarysqltables.First(entry => entry.Id.Equals(searchtxt));
                    results.Add(search);
                    MainDataGrid.ItemsSource = results;
                }
                else if (searchtxt.All(char.IsLetter))
                {
                    //Checks if searched time is all Letters, Wont work when you add time for the show unless its written out.

                    Librarysqltable search1 = dataContext.Librarysqltables.First(entry => entry.Title.Equals(searchtxt));
                    results.Add(search1);
                    MainDataGrid.ItemsSource = results;
                }
                else if (searchtxt.IndexOf("/") == 2)
                {
                    //Check Date by checking that the third charcter is a "/"
                    Librarysqltable search2 = dataContext.Librarysqltables.First(entry => entry.Date.Equals(searchtxt));
                    results.Add(search2);
                    MainDataGrid.ItemsSource = results;
                }
                else if (searchtxt.All(char.IsLetterOrDigit))
                {
                    // Check Location
                    Librarysqltable search3 = dataContext.Librarysqltables.First(entry => entry.Location.Equals(searchtxt));
                    results.Add(search3);
                    MainDataGrid.ItemsSource = results;
                }

                else
                {
                    //Clear Search Box if item not Found.
                    searchbartextbox.Clear();
                }
            }

            else
            {
                // if nothing is enterer clear textbox and reset Data Source.

                searchbartextbox.Clear();
                MainDataGrid.ItemsSource = dataContext.Librarysqltables;
            }
            
           

        }
    }
}
