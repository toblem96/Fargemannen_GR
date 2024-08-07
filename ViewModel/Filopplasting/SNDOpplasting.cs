using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autodesk.AutoCAD.Customization;
using Microsoft.Win32;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System.Runtime.CompilerServices;

using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using System.IO;
using System.Collections.ObjectModel;

namespace Fargemannen_GR.ViewModel.Filopplasting
{
    public class SNDOpplasting : INotifyPropertyChanged
    {
        #region Singleton
        private static SNDOpplasting instance;

        public static SNDOpplasting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SNDOpplasting();
                }
                return instance;
            }
        }
        #endregion

        #region Properties
        private Dictionary<int, FolderData> _folderData;
        private ObservableCollection<SndFolderMapping> _sndFolderMappings;

        public Dictionary<int, FolderData> FolderData
        {
            get => _folderData;
            private set
            {
                _folderData = value;
                OnPropertyChanged(nameof(FolderData));
            }
        }

        public ObservableCollection<SndFolderMapping> SndFolderMappings
        {
            get => _sndFolderMappings;
            private set
            {
                _sndFolderMappings = value;
                OnPropertyChanged(nameof(SndFolderMappings));
            }
        }
        #endregion

        #region Commands
        public ICommand UploadSndFolderCommand { get; private set; }
        #endregion

        #region Constructor
        public SNDOpplasting()
        {
            UploadSndFolderCommand = new RelayCommand<SndFolderMapping>(UploadSndFolder);
            SndFolderMappings = new ObservableCollection<SndFolderMapping>
            {
                new SndFolderMapping() // Initial tom mapping for å starte
            };
            _folderData = new Dictionary<int, FolderData>();
        }
        #endregion

        #region Methods
        private void UploadSndFolder(SndFolderMapping mapping)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mapping.Path = folderDialog.SelectedPath;

                int folderNumber = _folderData.Count + 1;
                var newFolderData = new FolderData
                {
                    Prefix = mapping.Prefix,
                    FilePaths = new List<string> { mapping.Path }
                };
                _folderData[folderNumber] = newFolderData;

                // Sett FolderData-referansen i SndFolderMapping
                mapping.FolderData = newFolderData;

                // Diagnostisk utskrift for å verifisere registrering av prefiksen
                System.Diagnostics.Debug.WriteLine($"FolderNumber: {folderNumber}, Prefix: {mapping.Prefix}, Path: {mapping.Path}");

                // Legg til en ny tom mapping for å muliggjøre flere opplastinger
                SndFolderMappings.Add(new SndFolderMapping());
            }
        }
        #endregion

        #region PropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }

    public class SndFolderMapping : INotifyPropertyChanged
    {
        private string _prefix;
        private string _path;
        private FolderData _folderData;

        public string Prefix
        {
            get => _prefix;
            set
            {
                if (_prefix != value)
                {
                    _prefix = value;
                    OnPropertyChanged(nameof(Prefix));
                    _folderData?.SetPrefix(_prefix);
                }
            }
        }

        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged(nameof(Path));
                }
            }
        }

        public FolderData FolderData
        {
            get => _folderData;
            set
            {
                if (_folderData != value)
                {
                    _folderData = value;
                    OnPropertyChanged(nameof(FolderData));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class FolderData : INotifyPropertyChanged
    {
        private string _prefix;
        private List<string> _filePaths;

        public string Prefix
        {
            get => _prefix;
            set
            {
                _prefix = value;
                OnPropertyChanged(nameof(Prefix));
            }
        }

        public List<string> FilePaths
        {
            get => _filePaths;
            set
            {
                _filePaths = value;
                OnPropertyChanged(nameof(FilePaths));
            }
        }

        public FolderData()
        {
            _filePaths = new List<string>();
        }

        public void SetPrefix(string prefix)
        {
            Prefix = prefix;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
