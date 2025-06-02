using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G2_Simulation.Core;

namespace G2_Simulation.Models
{
    public class Parameters : ObservableObject
    {
        public double HeightDrop { get; set; }
        public double HeightCollision { get; set; }
        public double BallMass { get; set; }
        public double RacketMass { get; set; }
        public double MomentOfInertia { get; set; }
        public double CoefficientOfRestitution { get; set; }
        public double CollisionRadius { get; set; }
        public double KSpring { get; set; }
        public double NSpring { get; set; }
        public double MachineEff { get; set; }
        public double R { get; set; }
        public double ThetaDeg { get; set; }

        public void SetDefault()
        {
            HeightDrop = 1.5;
            HeightCollision = 0.4;
            BallMass = 0.024;
            RacketMass = 0.298;
            MomentOfInertia = 0.28;
            CoefficientOfRestitution = 0.37;
            CollisionRadius = 0.285;
            KSpring = 1500;
            NSpring = 2;
            MachineEff = 100;
        }

        public List<Map> Maps { get; } = new List<Map>
        {
            new Map
            {
                Name = "Map1",
                VerticalImage = "/Assets/Map/VerticalMap1.png",
                HorizontalImage = "/Assets/Map/HorizontalMap1.png",
                ScaleFactor = 2.0
            },
            new Map
            {
                Name = "Map2",
                VerticalImage = "/Assets/Map/VerticalMap2.png",
                HorizontalImage = "/Assets/Map/HorizontalMap2.png",
                ScaleFactor = 1.365
            }
        };

        private Map _selectedMap;
        public Map SelectedMap
        {
            get => _selectedMap;
            set
            {
                if (_selectedMap != value)
                {
                    _selectedMap = value;
                    OnPropertyChanged(nameof(SelectedMap));
                }
            }
        }
    }
}
