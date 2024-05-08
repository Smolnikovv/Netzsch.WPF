using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace NetzschWPF
{
    public partial class MainWindow : Window
    {
        HubConnection _connection;
        public MainWindow()
        {
            InitializeComponent();

            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5016/signal")
                .WithAutomaticReconnect()
                .Build();
        }

        private async void connectToServer_Click(object sender, RoutedEventArgs e)
        {
            _connection.On<string>("SendWebInput", (input) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    outputBox.Text = input;
                });
            });

            try
            {
                await _connection.StartAsync();
                statusInfo.Text = "Connected";
                connectToServer.IsEnabled = false;
            }
            catch
            {
                statusInfo.Text = "Connection Failed";
            }
        }

        private async void inputBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            await Task.Delay(2000);
            try
            {
                await _connection.InvokeAsync("SendWpfInput", inputBox.Text);
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }
    }
}