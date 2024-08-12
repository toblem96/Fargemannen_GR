using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;

namespace Fargemannen_GR.ViewModel.Symboler
{
    public class SymbolsViewModel : INotifyPropertyChanged
    {
        #region Instanse
    

        private static SymbolsViewModel _instance;
        public static SymbolsViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SymbolsViewModel();
                }
                return _instance;
            }
        }
        #endregion


        #region Egenskaper 

        private int _minDrillingDepth = 2;
        private ObservableCollection<SonderingType> _sonderingTypes;

        public ObservableCollection<SonderingType> SonderingTypes
        {
            get => _sonderingTypes;
            set
            {
                _sonderingTypes = value;
                OnPropertyChanged(nameof(SonderingTypes)); //ENDRET HER: Sørger for at endringer i listen oppdaterer UI
            }
        }

        public int MinDrillingDepth
        {
            get => _minDrillingDepth;
            set
            {
                if (_minDrillingDepth != value)
                {
                    _minDrillingDepth = value;
                    OnPropertyChanged(nameof(MinDrillingDepth));
                }
            }
        }
        #endregion

        #region Kommandoer

        public ICommand GenerateSymbolsCommand { get; private set; }
        #endregion


        #region Konstruktør 

        private SymbolsViewModel()
        {
          

            SonderingTypes = new ObservableCollection<SonderingType>
        {
            new SonderingType { Name = "Totalsondering", IsChecked = true },
            new SonderingType { Name = "Dreietrykksondering", IsChecked = false },
            new SonderingType { Name = "Trykksondering", IsChecked = false },
            new SonderingType { Name = "Prøveserie", IsChecked = false },
            new SonderingType { Name = "Poretrykksmåler", IsChecked = false },
            new SonderingType { Name = "Vingeboring", IsChecked = false },
            new SonderingType { Name = "Fjellkontrollboring", IsChecked = false },
            new SonderingType { Name = "Dreiesondering", IsChecked = false },
            new SonderingType { Name = "Prøvegrop", IsChecked = false },
            new SonderingType { Name = "Ramsondering", IsChecked = false },
            new SonderingType { Name = "Enkel", IsChecked = false },
            new SonderingType { Name = "Fjellidagen", IsChecked = false }
        };

            GenerateSymbolsCommand = new RelayCommand(ExecuteGenerateSymbols);
        }
        #endregion

        #region Metoder
        private void ExecuteGenerateSymbols()
        {
           

            try
            {
                // Spor bruk av knappen
                

                var selectedTypes = SonderingTypes.Where(x => x.IsChecked).Select(x => x.Name).ToList();

                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;

                List<PunktInfo> pointsToSymbol = new List<PunktInfo>();
                List<Point3d> punkterMesh = new List<Point3d>();

                ProsseseringAvFiler.HentPunkter(pointsToSymbol, punkterMesh, MinYear, selectedTypes, NummerType, ProjectType);

                Fargemannen.ApplicationInsights.AppInsights.TrackMetric("Number of Symbols Generated", pointsToSymbol.Count);
                SymbolModel.FiltrerOgPrintUtenGBUMetode(pointsToSymbol);
                SymbolModel.PrintValgtBoring(pointsToSymbol, selectedTypes);
                SymbolModel.test(pointsToSymbol, selectedTypes, MinDrillingDepth, NormalSymbolColor, minDrillingSymbolColor);

            }
            catch (Exception ex)
            {
               
            }
        }

        #endregion


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class SonderingType : INotifyPropertyChanged
    {
        private bool _isChecked;

        public string Name { get; set; }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



}