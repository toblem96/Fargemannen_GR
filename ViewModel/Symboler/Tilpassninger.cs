using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.Runtime.CompilerServices;
using System.Windows;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System.Linq;
using System.Windows.Media;
using System.Windows.Forms;

using System;
using System.Linq.Expressions;
using Autodesk.AutoCAD.GraphicsSystem;

namespace Fargemannen_GR.ViewModel.Symboler
{
    internal class Tilpassninger : INotifyPropertyChanged
    {
        #region Instace

        private static Tilpassninger _instance;
        public static Tilpassninger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Tilpassninger();
                }
                return _instance;
            }
        }

        #endregion

        #region Egenskaper
        private double _rotation = 0;
        private double _scale = 1;

        public double Rotation
        {
            get => _rotation;
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    OnPropertyChanged(nameof(Rotation));
                    UpdateSymbolRotation();  // Oppdater rotasjon når brukeren justerer slideren
                }
            }
        }

        public double Scale
        {
            get => _scale;
            set
            {
                if (_scale != value)
                {
                    _scale = value;
                    OnPropertyChanged(nameof(Scale));
                    UpdateSymbolScale();  // Oppdater skala når brukeren justerer slideren
                }
            }
        }

        #endregion

        #region Konstruktør

        private Tilpassninger()
        {

        }


        #endregion

        #region Metoder
        private void UpdateSymbolRotation()
        {
            SymbolHandlers.RoterBlokker(Model.SymbolModel.GenerteSymbol, Rotation);
        }

        private void UpdateSymbolScale()
        {
            SymbolHandlers.EndreSkala(Model.SymbolModel.GenerteSymbol, Scale);
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
}
