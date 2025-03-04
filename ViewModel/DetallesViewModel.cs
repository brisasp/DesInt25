using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoginRegister.Helpers;
using LoginRegister.Interface;
using LoginRegister.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginRegister.ViewModel
{
    public partial class DetallesViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<GatoDTO> _items;

        private int _gatoId;
        private InformacionViewModel _informacionViewModel;
        private readonly IHttpJsonProvider<GatoDTO> _httpJsonProvider;
        private readonly IFileService<GatoDTO> _fileService;

        [ObservableProperty]
        private GatoDTO _gato;

        public DetallesViewModel(IHttpJsonProvider<GatoDTO> httpJsonProvider, IFileService<GatoDTO> fileService)
        {
            _httpJsonProvider = httpJsonProvider;
            _fileService = fileService;
            _items = new ObservableCollection<GatoDTO>();
        }

        public void SetIdGato(int id)
        {
            _gatoId = id;
        }

        public override async Task LoadAsync()
        {
            IEnumerable<GatoDTO> gatos = await _httpJsonProvider.GetAsync(Constants.GATO_URL);
            foreach (var gato in gatos)
            {
                if (string.IsNullOrEmpty(gato.Image))
                {
                    gato.Image = "/Resources/Not_found.png";
                }
                Items.Add(gato);
            }
            Gato = gatos.FirstOrDefault(x => x.Id == _gatoId) ?? new GatoDTO();
        }

        internal void SetParentViewModel(ViewModelBase galaxyOverviewViewModel)
        {
            if (galaxyOverviewViewModel is InformacionViewModel informacionview)
            {
                _informacionViewModel = informacionview;
            }
        }

        [RelayCommand]
        private async Task Close(object? parameter)
        {
            if (_informacionViewModel != null)
            {
                _informacionViewModel.SelectedViewModel = null;
            }
        }

        [RelayCommand]
        public void Save()
        {

            var saveFileDialog = new SaveFileDialog
            {
                Filter = Constants.JSON_FILTER
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _fileService.Save(saveFileDialog.FileName, (IEnumerable<GatoDTO>)Gato);
            }
        }
    }
}

