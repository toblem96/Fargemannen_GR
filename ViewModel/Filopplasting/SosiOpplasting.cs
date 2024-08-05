using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace Fargemannen_GR.ViewModel.Filopplasting
{
    public class SosiOpplasting : INotifyPropertyChanged
    {
        #region Instanse
        // Singleton implementasjon for å sikre at kun én instans eksisterer
        private static SosiOpplasting instance;

        public static SosiOpplasting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SosiOpplasting();
                }
                return instance;
            }
        }
        #endregion

        #region Egenskaper 

        private string _fullSosiFilePath;
        private string _fullSosiIDagenFilePath;

        public string SosiFilePath
        {
            get => _fullSosiFilePath;
            set
            {
                if (_fullSosiFilePath == value) return;
                _fullSosiFilePath = value;
                OnPropertyChanged(nameof(SosiFilePath));
                OnPropertyChanged(nameof(DisplaySosiFilePath));
            }
        }

        public string DisplaySosiFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_fullSosiFilePath))
                {
                    return "";
                }
                var parts = _fullSosiFilePath.Split('\\');
                return parts.Length > 1 ? string.Join("\\", parts.Skip(parts.Length - 2)) : _fullSosiFilePath;
            }
        }

        public string SosiIDagenFilePath
        {
            get => _fullSosiIDagenFilePath;
            set
            {
                if (_fullSosiIDagenFilePath == value) return;
                _fullSosiIDagenFilePath = value;
               OnPropertyChanged(nameof(_fullSosiIDagenFilePath));
                OnPropertyChanged(nameof(DisplaySosiIDagenFilePath));
            }
        }

        public string DisplaySosiIDagenFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_fullSosiIDagenFilePath))
                {
                    return ""; // Returnerer en tom streng hvis stien er null eller tom
                }

                var parts = _fullSosiIDagenFilePath.Split('\\');
                return parts.Length > 1 ? string.Join("\\", parts.Skip(parts.Length - 2)) : _fullSosiIDagenFilePath;
            }
        }
        #endregion


        #region Kommandoer
        public ICommand UploadSosiCommand {  get; private set; }
        public ICommand UploadSosIDagenCommand { get; private set; }
        #endregion


        #region Konstruktør 
        private SosiOpplasting()
        {
            // Initialiserer kommandoene
            UploadSosiCommand = new RelayCommand(UploadSosi);
            UploadSosIDagenCommand = new RelayCommand(UploadSosiIDagen);
        }
        #endregion


        #region Metoder
        private void UploadSosi()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "SOSI-filer|*.sos"
            };

            if (fileDialog.ShowDialog() == true)
            {
                SosiFilePath = fileDialog.FileName; // Oppdaterer filstien når en fil er valgt
            }

        }

     
        private void UploadSosiIDagen()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "SOSI-filer|*.sos"
            };

            if (fileDialog.ShowDialog() == true)
            {
                SosiIDagenFilePath = fileDialog.FileName; // Oppdaterer filstien når en fil er valgt
            }

        }
        #endregion



        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
    
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
