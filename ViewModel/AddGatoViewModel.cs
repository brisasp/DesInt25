using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoginRegister.Interface;
using LoginRegister.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LoginRegister.ViewModel
{
    public partial class AddGatoViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private string _pais;

        private readonly IGatoServiceToApi _gatoServiceToApi;
       

        public AddGatoViewModel(IGatoServiceToApi gatoServiceToApi, LoginDTO loginDTO)
        {
            _gatoServiceToApi = gatoServiceToApi;          
        }

        [RelayCommand]
        public async Task Add()
        {
            if (string.IsNullOrEmpty(Name) ||
                string.IsNullOrEmpty(Description) ||
                string.IsNullOrEmpty(Pais))
            {
                MessageBox.Show("Por favor, rellene todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            GatoDTO gatoDTO = new()
            {
                Name = Name,
                Description = Description,
                Pais = Pais,
            };

            try
            {
                await _gatoServiceToApi.PostGato(gatoDTO);
                 
                 MessageBox.Show("Gato añadido con exito");
                 App.Current.Services.GetService<MainViewModel>().LoadAsync();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error durante el registro: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}


