using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using G2_Simulation.Core;
using G2_Simulation.Models;
using G2_Simulation.Services;

namespace G2_Simulation.ViewModels
{
    public class MainView : ObservableObject
    {
        private readonly Parameters _parameters;
        private readonly SimulationEngine _simulation;
        private readonly NavigationService _navService;

        private bool _isPolarMode;
        private double _x;
        private double _y;
        private double _r;
        private double _thetaRad;
        private double _thetaDeg;
        private double _angleOfRacket;
        private double _delayTime;
        private double _machinePosition;
        private PointCollection _trajectoryPoints = new PointCollection();

        // Constructor
        public MainView(Parameters parameters, NavigationService navService)
        {
            _parameters = parameters;
            _navService = navService;
            _simulation = new SimulationEngine();

            GoToMainPage = new RelayCommand(_ => _navService.NavigateTo("MainPage"));
            GoToSettingsPage = new RelayCommand(_ => _navService.NavigateTo("SettingsPage"));
            CalculateAll = new RelayCommand(_ => Calculate());
            ResetCommand = new RelayCommand(_ => ResetCalculation());


            IsPolarMode = false;
            if (_parameters.SelectedMap == null)
                _parameters.SelectedMap = _parameters.Maps.FirstOrDefault();
            _parameters.SetDefault();

            HeightDrop = _parameters.HeightDrop;
            HeightCollision = _parameters.HeightCollision;
            BallMass = _parameters.BallMass;
            RacketMass = _parameters.RacketMass;
            MomentOfInertia = _parameters.MomentOfInertia;
            CoefficientOfRestitution = _parameters.CoefficientOfRestitution;
            CollisionRadius = _parameters.CollisionRadius;
            KSpring = _parameters.KSpring;
            NSpring = _parameters.NSpring;
            MachineEff = _parameters.MachineEff;
            R = _parameters.R;
            ThetaDeg = _parameters.ThetaDeg;

            ResetCalculation();
        }

        // Commands
        public ICommand GoToMainPage { get; }
        public ICommand GoToSettingsPage { get; }
        public ICommand CalculateAll { get; }
        public ICommand ResetCommand { get; }


        public bool IsPolarMode
        {
            get => _isPolarMode;
            set
            {
                if (_isPolarMode != value)
                {
                    _isPolarMode = value;
                    OnPropertyChanged(nameof(IsPolarMode));
                    OnPropertyChanged(nameof(DisplayTextPos1));
                    OnPropertyChanged(nameof(DisplayTextPos2));
                    OnPropertyChanged(nameof(DisplayTextUnit));

                    if (_isPolarMode)
                        UpdateCartesianCoordinates();
                    else
                        UpdatePolarCoordinates();
                }
            }
        }

        public string DisplayTextPos1 => IsPolarMode ? "R = " : "X = ";
        public string DisplayTextPos2 => IsPolarMode ? "θ = " : "Y = ";
        public string DisplayTextUnit => IsPolarMode ? "Deg" : "cm";

        public double X
        {
            get => _x;
            set
            {
                _x = Math.Round(value, 2);
                OnPropertyChanged(nameof(X));
                if (!IsPolarMode)
                    UpdatePolarCoordinates();
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                _y = Math.Round(value, 2);
                OnPropertyChanged(nameof(Y));
                if (!IsPolarMode)
                    UpdatePolarCoordinates();
            }
        }

        public double R
        {
            get => _r;
            set
            {
                _r = Math.Round(value, 2);
                _parameters.R = _r;
                OnPropertyChanged(nameof(R));
                if (IsPolarMode)
                    UpdateCartesianCoordinates();
            }
        }

        public double ThetaDeg
        {
            get => _thetaDeg;
            set
            {
                _thetaDeg = Math.Round(value, 2);
                _parameters.ThetaDeg = _thetaDeg;
                _thetaRad = _thetaDeg * Math.PI / 180;
                OnPropertyChanged(nameof(ThetaDeg));
                if (IsPolarMode)
                    UpdateCartesianCoordinates();
            }
        }

        public double ThetaRad
        {
            get => _thetaRad;
            set
            {
                _thetaRad = value;
                _thetaDeg = _thetaRad * 180 / Math.PI;
                _parameters.ThetaDeg = _thetaDeg;
                OnPropertyChanged(nameof(ThetaRad));
                OnPropertyChanged(nameof(ThetaDeg));
                if (IsPolarMode)
                    UpdateCartesianCoordinates();
            }
        }

        public double HeightDrop
        {
            get => _parameters.HeightDrop;
            set
            {
                _parameters.HeightDrop = value;
                OnPropertyChanged(nameof(HeightDrop));
            }
        }

        public double HeightCollision
        {
            get => _parameters.HeightCollision;
            set
            {
                _parameters.HeightCollision = value;
                OnPropertyChanged(nameof(HeightCollision));
            }
        }

        public double BallMass
        {
            get => _parameters.BallMass;
            set
            {
                _parameters.BallMass = value;
                OnPropertyChanged(nameof(BallMass));
            }
        }

        public double RacketMass
        {
            get => _parameters.RacketMass;
            set
            {
                _parameters.RacketMass = value;
                OnPropertyChanged(nameof(RacketMass));
            }
        }

        public double MomentOfInertia
        {
            get => _parameters.MomentOfInertia;
            set
            {
                _parameters.MomentOfInertia = value;
                OnPropertyChanged(nameof(MomentOfInertia));
            }
        }

        public double CoefficientOfRestitution
        {
            get => _parameters.CoefficientOfRestitution;
            set
            {
                _parameters.CoefficientOfRestitution = value;
                OnPropertyChanged(nameof(CoefficientOfRestitution));
            }
        }

        public double CollisionRadius
        {
            get => _parameters.CollisionRadius;
            set
            {
                _parameters.CollisionRadius = value;
                OnPropertyChanged(nameof(CollisionRadius));
            }
        }

        public double KSpring
        {
            get => _parameters.KSpring;
            set
            {
                _parameters.KSpring = value;
                OnPropertyChanged(nameof(KSpring));
            }
        }

        public double NSpring
        {
            get => _parameters.NSpring;
            set
            {
                _parameters.NSpring = value;
                OnPropertyChanged(nameof(NSpring));
            }
        }

        public double MachineEff
        {
            get => _parameters.MachineEff;
            set
            {
                _parameters.MachineEff = value;
                OnPropertyChanged(nameof(MachineEff));
            }
        }

        public double AngleOfRacket
        {
            get => _angleOfRacket;
            set
            {
                _angleOfRacket = value;
                OnPropertyChanged(nameof(AngleOfRacket));
            }
        }

        public double DelayTime
        {
            get => _delayTime;
            set
            {
                _delayTime = value;
                OnPropertyChanged(nameof(DelayTime));
            }
        }

        public double MachinePosition
        {
            get => _machinePosition;
            set
            {
                _machinePosition = value;
                OnPropertyChanged(nameof(MachinePosition));
            }
        }

        public PointCollection TrajectoryPoints
        {
            get => _trajectoryPoints;
            set
            {
                _trajectoryPoints = value;
                OnPropertyChanged(nameof(TrajectoryPoints));
            }
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
                    OnPropertyChanged(nameof(ScaledDotX));
                    OnPropertyChanged(nameof(ScaledDotY));
                    OnPropertyChanged(nameof(IsMap1Selected));
                    OnPropertyChanged(nameof(IsMap2Selected));
                }
            }
        }

        public string HorizontalMapImage => SelectedMap?.HorizontalImage;
        public string VerticalMapImage => SelectedMap?.VerticalImage;
        public double ScaledDotX => SelectedMap != null ? -X * SelectedMap.ScaleFactor : X;
        public double ScaledDotY => SelectedMap != null ? -Y * SelectedMap.ScaleFactor : Y;

        public bool IsMap1Selected => SelectedMap?.Name == "Map1";
        public bool IsMap2Selected => SelectedMap?.Name == "Map2";



        private void UpdatePolarCoordinates()
        {
            var (r, thetaDeg, thetaRad) = Coordinate.ToPolar(X, Y);
            R = r;
            ThetaDeg = thetaDeg;
            ThetaRad = thetaRad;
            UpdateDotPosition();
        }

        private void UpdateCartesianCoordinates()
        {
            var (x, y) = Coordinate.ToCartesian(R, ThetaRad);
            X = x;
            Y = y;
            UpdateDotPosition();
        }

        private void UpdateDotPosition()
        {
            OnPropertyChanged(nameof(ScaledDotX));
            OnPropertyChanged(nameof(ScaledDotY));
        }

        private void Calculate()
        {
            AngleOfRacket = _simulation.CalculateAngle(_parameters);
            DelayTime = _simulation.CalculateDelayTime(_parameters);
            MachinePosition = _simulation.CalculateYaw(_parameters);
            TrajectoryPoints = _simulation.CalculateTrajectory(_parameters, 90 - AngleOfRacket);
        }

        private void ResetCalculation()
        {
            X = 0;
            Y = 0;
            R = 0;
            ThetaDeg = 0;
            ThetaRad = 0;
            AngleOfRacket = 0;
            DelayTime = 0;
            MachinePosition = 0;
            TrajectoryPoints = new PointCollection();
        }
    }
}
