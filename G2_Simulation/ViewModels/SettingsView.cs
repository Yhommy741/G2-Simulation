using G2_Simulation.Models;
using G2_Simulation.Core;
using G2_Simulation.Services;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Diagnostics;

namespace G2_Simulation.ViewModels
{
    public class SettingsView : ObservableObject
    {
        private readonly Parameters _parameters;
        private readonly NavigationService _navService;
        private Map _selectedMap;

        public ICommand GoToMainPage { get; }
        public ICommand SetDefault { get; }

        public SettingsView(Parameters parameters, NavigationService navService)
        {
            _parameters = parameters;
            _navService = navService;

            GoToMainPage = new RelayCommand(_ => _navService.NavigateTo("MainPage"));
            SetDefault = new RelayCommand(_ => SetDefaultParameters());

            if (_parameters.SelectedMap == null)
                _parameters.SelectedMap = _parameters.Maps.FirstOrDefault();
        }

        public List<Map> Maps => _parameters.Maps;

        public Map SelectedMap
        {
            get => _parameters.SelectedMap;
            set
            {
                if (_parameters.SelectedMap != value)
                {
                    _parameters.SelectedMap = value;
                    OnPropertyChanged(nameof(SelectedMap));
                    OnPropertyChanged(nameof(HorizontalMapImage));
                    OnPropertyChanged(nameof(VerticalMapImage));
                }
            }
        }

        public string HorizontalMapImage => SelectedMap?.HorizontalImage;
        public string VerticalMapImage => SelectedMap?.VerticalImage;

        public double HeightDrop
        {
            get => _parameters.HeightDrop;
            set { _parameters.HeightDrop = value; OnPropertyChanged(nameof(HeightDrop)); }
        }

        public double HeightCollision
        {
            get => _parameters.HeightCollision;
            set { _parameters.HeightCollision = value; OnPropertyChanged(nameof(HeightCollision)); }
        }

        public double BallMass
        {
            get => _parameters.BallMass;
            set { _parameters.BallMass = value; OnPropertyChanged(nameof(BallMass)); }
        }

        public double RacketMass
        {
            get => _parameters.RacketMass;
            set { _parameters.RacketMass = value; OnPropertyChanged(nameof(RacketMass)); }
        }

        public double MomentOfInertia
        {
            get => _parameters.MomentOfInertia;
            set { _parameters.MomentOfInertia = value; OnPropertyChanged(nameof(MomentOfInertia)); }
        }

        public double CoefficientOfRestitution
        {
            get => _parameters.CoefficientOfRestitution;
            set { _parameters.CoefficientOfRestitution = value; OnPropertyChanged(nameof(CoefficientOfRestitution)); }
        }

        public double CollisionRadius
        {
            get => _parameters.CollisionRadius;
            set { _parameters.CollisionRadius = value; OnPropertyChanged(nameof(CollisionRadius)); }
        }

        public double KSpring
        {
            get => _parameters.KSpring;
            set { _parameters.KSpring = value; OnPropertyChanged(nameof(KSpring)); }
        }

        public double NSpring
        {
            get => _parameters.NSpring;
            set { _parameters.NSpring = value; OnPropertyChanged(nameof(NSpring)); }
        }

        public double MachineEff
        {
            get => _parameters.MachineEff;
            set { _parameters.MachineEff = value; OnPropertyChanged(nameof(MachineEff)); }
        }

        public double R
        {
            get => _parameters.R;
            set { _parameters.R = value; OnPropertyChanged(nameof(R)); }
        }

        public double ThetaDeg
        {
            get => _parameters.ThetaDeg;
            set { _parameters.ThetaDeg = value; OnPropertyChanged(nameof(ThetaDeg)); }
        }

        public void SetDefaultParameters()
        {
            _parameters.HeightDrop = 1.5;
            _parameters.HeightCollision = 0.4;
            _parameters.BallMass = 0.024;
            _parameters.RacketMass = 0.298;
            _parameters.MomentOfInertia = 0.28;
            _parameters.CoefficientOfRestitution = 0.37;
            _parameters.CollisionRadius = 0.285;
            _parameters.KSpring = 1500;
            _parameters.NSpring = 2;
            _parameters.MachineEff = 100;

            OnPropertyChanged(nameof(HeightDrop));
            OnPropertyChanged(nameof(HeightCollision));
            OnPropertyChanged(nameof(BallMass));
            OnPropertyChanged(nameof(RacketMass));
            OnPropertyChanged(nameof(MomentOfInertia));
            OnPropertyChanged(nameof(CoefficientOfRestitution));
            OnPropertyChanged(nameof(CollisionRadius));
            OnPropertyChanged(nameof(KSpring));
            OnPropertyChanged(nameof(NSpring));
            OnPropertyChanged(nameof(MachineEff));
            OnPropertyChanged(nameof(R));
            OnPropertyChanged(nameof(ThetaDeg));

        }
    }
}