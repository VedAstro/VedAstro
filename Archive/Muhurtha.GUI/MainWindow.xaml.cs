using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Genso.DDNS.ClientGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainGrid MainGrid;

        public MainWindow()
        {
            InitializeComponent();

            //get the view modal of the main grid, which holds all other viewmodals
            //get it here to pass events to it 
            MainGrid = TryFindResource("MainGrid") as MainGrid;
        }



        /** EVENT ROUTING **/
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) => MainGrid.LoginPanel.ButtonBase_OnClick(sender, e);
        private void SignUpButton_OnClick(object sender, RoutedEventArgs e) => MainGrid.SignUpPanel.SignUpButton_OnClick(sender, e);
        private void GotoSignUpButton_OnClick(object sender, RoutedEventArgs e) => MainGrid.LoginPanel.GotoSignUpButton_OnClick(sender, e);
        private void CancelButton_OnClick(object sender, RoutedEventArgs e) => MainGrid.SignUpPanel.CancelButton_OnClick(sender, e);
        private void loginPanel_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) => MainGrid.LoginPanel.loginPanel_KeyDown(sender, e);
        private void logoutAccountMenu_Click(object sender, RoutedEventArgs e) => MainGrid.HomePanel.logoutAccountMenu_Click(sender, e);
        private void deleteAccountMenu_Click(object sender, RoutedEventArgs e) => MainGrid.HomePanel.deleteAccountMenu_Click(sender, e);
        private void Window_Initialized(object sender, EventArgs e) => MainGrid.Window_Initialized(sender, e);
        private void signUpPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => MainGrid.SignUpPanel.SignUpPanel_IsVisibleChanged(sender, e);
        private void loginPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => MainGrid.LoginPanel.LoginPanel_IsVisibleChanged(sender, e);
        private void signUpPanel_GotFocus(object sender, RoutedEventArgs e) => MainGrid.SignUpPanel.SignUpPanel_GotFocus(sender, e);
        private void signUpPanel_KeyDown(object sender, KeyEventArgs e) => MainGrid.SignUpPanel.SignUpPanel_KeyDown(sender, e);
        private void addDomainButton_Click(object sender, RoutedEventArgs e) => MainGrid.HomePanel.addDomainButton_Click(sender, e);
        private void newDomainOkButton_Click(object sender, RoutedEventArgs e) => MainGrid.AddDomainPanel.OkButton_OnClick(sender, e);
        private void newDomainCancelButton_Click(object sender, RoutedEventArgs e) => MainGrid.AddDomainPanel.CancelButton_OnClick(sender, e);
        private void homePanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => MainGrid.HomePanel.isVisible_Changed(sender, e);
        private void messagePanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => MainGrid.MessagePanel.isVisible_Changed(sender, e);
        private void deleteDomainButton_Click(object sender, MouseButtonEventArgs e) => MainGrid.HomePanel.deleteDomainButton_Click(sender, e);
        private void selectedDomainCheckBox_Checked(object sender, RoutedEventArgs e) => MainGrid.HomePanel.selectedDomainCheckBox_Checked(sender, e);
        private void selectedDomainCheckBox_Unchecked(object sender, RoutedEventArgs e) => MainGrid.HomePanel.selectedDomainCheckBox_Unchecked(sender, e);
        private void mainWindow_Closed(object sender, EventArgs e) => MainGrid.mainWindow_Closed(sender, e);


        /** PUBLIC METHODS **/

    }
}
