using Ihatud.Services;

namespace Ihatud;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService _dbService;

    public MainPage(DatabaseService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    private async void OnUserLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginRegisterPage(_dbService));
    }

    private async void OnAdminLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminLoginPage(_dbService));
    }
}
