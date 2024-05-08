# Wymagania
Do uruchomienia aplikacji wymagany jest .net8. Do poprawnego działania aplikacji może być potrzebna zmiana projektu startowego. W tym celu należy:
* Nacisnąć prawym przyciskiem myszki na jeden z projektów
* Wybranie opcji "Configure startup project"
* Zaznaczenie opcji "Multiple startup project"
* W obu projektach w kolumnie "Action" wybranie opcji "Start"
# Najważniejsze fragmenty kodu
## Netzsch.SignalR
### SignalHub.cs
```cs
public async Task SendWpfInput(string input)
{
    await Clients.All.SendAsync("SendWpfInput", input);
}

public async Task SendWebInput(string input)
{
    await Clients.All.SendAsync("SendWebInput", input);
}
```
Najważniejsze metody w aplikacji. Wywołanie oraz zasubskrybowanie ich pozwala na dwukierunkową komunikację
### Program.cs
```cs
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SignalHub>("/signal");
});
```
Dodanie obsługi Huba oraz ustawienie ścieżki do obsługi komunikacji
## Netzsch.WPF
### Połączenie z Hubem
```cs
 _connection = new HubConnectionBuilder()
     .WithUrl("http://localhost:5016/signal")
     .WithAutomaticReconnect()
     .Build();

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
```
Powyższy kod odpowiada za połączenie z Hubem. Możliwe, że do poprawnej komunikacji wymagana będzie zmiana ścieżki lub portu (Netzsch.SignalR/Properties/launchSettings.json). Program nie nawiązuje połączenia samoistnie, potrzebne jest do tego wciśnięcie przycisku "Connect To Server"
### Otrzymywanie danych z weba
```cs
_connection.On<string>("SendWebInput", (input) =>
{
    this.Dispatcher.Invoke(() =>
    {
        outputBox.Text = input;
    });
});
```
Powyższy kod subskrybuje metodę "SendWebInput", przy jej wywołaniu wartość zostaje zmieniona
### Wysłanie danych z aplikacji WPF
```cs
await Task.Delay(2000);
try
{
    await _connection.InvokeAsync("SendWpfInput", inputBox.Text);
}
catch
{
    MessageBox.Show("Error");
}
```
Metoda odpowiadająca za wysłanie danych do weba, dane zostają wysłane z 2 sekundowym opóźnieniem
