using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoginRegister.Helpers;
using LoginRegister.Interface;
using LoginRegister.Models;
using LoginRegister.View;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace LoginRegister.ViewModel;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly IHttpJsonProvider<GatoDTO> _httpJsonProvider;
    private readonly AddGatoViewModel _addViewModel;
    private readonly InformacionViewModel _informacionViewModel;

    public DashboardViewModel(IHttpJsonProvider<GatoDTO> httpJsonProvider, AddGatoViewModel addViewModel, InformacionViewModel informacionViewModel)
    {
        _httpJsonProvider = httpJsonProvider;
        _addViewModel = addViewModel;
        _informacionViewModel = informacionViewModel;

        Gatos = new List<GatoDTO>();
        PagedGatos = new ObservableCollection<GatoDTO>();

        ItemsPerPage = 5;
        CurrentPage = 0;
    }

    private List<GatoDTO> Gatos;

    [ObservableProperty]
    private ObservableCollection<GatoDTO> pagedGatos;

    [ObservableProperty]
    private int currentPage;

    [ObservableProperty]
    private int itemsPerPage;

    public int TotalPages => (int)Math.Ceiling((double)Gatos.Count / ItemsPerPage);


    public override async Task LoadAsync()
    {
        try
        {

            Gatos.Clear();
            PagedGatos.Clear();


            IEnumerable<GatoDTO> listaGatos = await _httpJsonProvider.GetAsync((Constants.GATO_URL));
            Gatos.AddRange(listaGatos.OrderBy(d => d.Id));


            CurrentPage = 0;
            UpdatePagedGatos();
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error al cargar datos: {ex.Message}");
        }
    }


    private void UpdatePagedGatos()
    {

        PagedGatos.Clear();

        var pagedItems = Gatos.Skip(CurrentPage * ItemsPerPage).Take(ItemsPerPage).ToList();

        foreach (var item in pagedItems)
        {
            PagedGatos.Add(item);
        }
    }

    [RelayCommand]
    public async Task AddGato()
    {
        var addGatoWindow = new AddGatoView();

        var addGatoViewModel = App.Current.Services.GetService<AddGatoViewModel>();
        addGatoWindow.DataContext = addGatoViewModel;
        addGatoWindow.ShowDialog();
        await LoadAsync();
    }


    [RelayCommand]
    public async Task Logout()
    {
        App.Current.Services.GetService<LoginDTO>().Token = "";
        App.Current.Services.GetService<MainViewModel>().SelectedViewModel = App.Current.Services.GetService<MainViewModel>().LoginViewModel;
    }

    [RelayCommand]
    public void PreviousPage()
    {
        if (CurrentPage > 0)
        {
            CurrentPage--;
            UpdatePagedGatos();
        }
    }

    [RelayCommand]
    public void NextPage()
    {
        if (CurrentPage < TotalPages - 1)
        {
            CurrentPage++;
            UpdatePagedGatos();
        }
    }

   [RelayCommand]
   public void Informacion()
  {
        App.Current.Services.GetService<MainViewModel>().SelectedViewModel = App.Current.Services.GetService<MainViewModel>().InformacionViewModel;
        App.Current.Services.GetService<MainViewModel>().LoadAsync();
    }

    public async void MyDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
        if (e.Row.Item is GatoDTO gatoDTO)
        {

            await _httpJsonProvider.PutAsync(Constants.BASE_URL + Constants.GATO_URL + "/" + gatoDTO.Id, gatoDTO);
        }
       
    }

    private bool CanGoToPreviousPage() => CurrentPage > 0;

    private bool CanGoToNextPage() => CurrentPage < TotalPages - 1;
}

