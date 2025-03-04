using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoginRegister.Helpers;
using LoginRegister.Interface;
using LoginRegister.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginRegister.ViewModel
{
    public partial class InformacionViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<GatoDTO> items;

        private readonly IHttpJsonProvider<GatoDTO> _httpJsonProvider;
        private readonly DetallesViewModel _detallesViewModel;
        private readonly IStringUtils _stringUtils;

        [ObservableProperty]
        private ViewModelBase? _selectedViewModel;

        public InformacionViewModel(IHttpJsonProvider<GatoDTO> httpJsonProvider, DetallesViewModel detallesViewModel, IStringUtils stringUtils)
        {
            _httpJsonProvider = httpJsonProvider;
            _detallesViewModel = detallesViewModel;
            _stringUtils = stringUtils;
            items = new ObservableCollection<GatoDTO>();
        }

        public override async Task LoadAsync()
        {
            IEnumerable<GatoDTO> gato = await _httpJsonProvider.GetAsync((Constants.GATO_URL));
            foreach (var gatos in gato)
            {
                if (string.IsNullOrEmpty(gatos.Image))
                {
                    gatos.Image = "/Resources/Not_found.png";
                }
                items.Add(gatos);
            }
        }

        [RelayCommand]
        private async Task SelectViewModel(object? parameter)
        {
            _detallesViewModel.SetIdGato(_stringUtils.ConvertToInteger(parameter?.ToString() ?? string.Empty) ?? int.MinValue);
            _detallesViewModel.SetParentViewModel(this);
            SelectedViewModel = _detallesViewModel;
            await _detallesViewModel.LoadAsync();
        }
    }
}
