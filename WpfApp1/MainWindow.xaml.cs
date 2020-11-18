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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string AdminUser = "admin";
        string AdminPass = "admin";

        string UserName = "user";
        string UserPass = "upassword";
        
        


        public MainWindow()
        {
            InitializeComponent();
           
        }

       

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            UserAuth();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                UserAuth();
            }


        }

        private void UserAuth()
        {
            
            string EnteredUsername = Username.Text;
            string EnteredPassword = Password.Text;
            
            if (EnteredUsername == AdminUser)
            {
                if (EnteredPassword == AdminPass)
                {
                    loadAdminPage();
                }
                else
                {
                    MessageBox.Show("Admin Password Incorrect!");
                }

            }
            else if (EnteredUsername == UserName)
            {
                if(EnteredPassword == UserPass)
                {
                    
                    MessageBox.Show($"Welcome {UserName}!");

                }
                else
                {
                    MessageBox.Show("You entered the Incorrect Password.");
                }

            }
            else
            {
                MessageBox.Show($"The Username {EnteredUsername}, Does not match.");
            }

        }
        private void loadAdminPage()
        {
            AdminPage adminPage = new AdminPage();
            this.Content = adminPage;
        }
    }
}


