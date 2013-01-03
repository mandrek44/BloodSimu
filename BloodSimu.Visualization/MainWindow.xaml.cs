using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using BloodSimu.Model;

namespace BloodSimu.Visualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private World _world;
        private DispatcherTimer _dispatcherTimer;
        private Dictionary<WorldElement, UIElement> _visualizationMap;
        private Random _randomGenerator;
        private int _particleSize;

        public MainWindow()
        {
            InitializeComponent();
            _particleSize = 16;


            BuildWorld();
            BuildWorldVisualization();
            BuildTimer();
        }

        private void BuildWorldVisualization()
        {
            _visualizationMap = new Dictionary<WorldElement, UIElement>();
            foreach (var border in _world.Borders)
            {
                var line = new Line();
                line.Stroke = Brushes.Blue;
                line.StrokeThickness = 2;
                line.X1 = border.Start.X;
                line.Y1 = border.Start.Y;
                line.X2 = border.End.X;
                line.Y2 = border.End.Y;

                canvas.Children.Add(line);
                _visualizationMap.Add(border, line);
            }

            foreach (var particle in _world.Particles)
            {
                var ellipse = CreateParticleVisualisation(particle);
                _visualizationMap.Add(particle, ellipse);
            }
        }

        private Ellipse CreateParticleVisualisation(Particle particle)
        {
            var ellipse = new Ellipse();
            ellipse.Fill = new SolidColorBrush(Colors.Brown) { Opacity = 0.4 };
            
            ellipse.Width = _particleSize;
            ellipse.Height = _particleSize;

            
            UpdatePosition(ellipse, particle);

            canvas.Children.Add(ellipse);
            return ellipse;
        }

        private void BuildTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(5);
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Start();
        }

        private void BuildWorld()
        {
            var worldBuilder = new WorldBuilder();
            worldBuilder
                .AddBorder(0, 0, 200, 200)
                .AddBorder(200, 0, 350, 150)

                .AddBorder(350, 150, 500, 0)
                .AddBorder(400, 210, 610, 0)

                .AddBorder(200, 200, 200, 800)
                .AddBorder(400, 210, 400, 800)

                .AddAccelerationArea(200, 200, 340, 140, 10, new Vector2D(-1000, -150))
                .AddAccelerationArea(350, 150, 400, 200, 10, new Vector2D(1000, -150))

                .AddStopArea(350, 150, 5)

                .AddParticle(438, 160, 100, 0);
                
            _world = worldBuilder.Build();
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop();
            _world.Move(TimeSpan.FromMilliseconds(5));

            var toRemove = new Collection<Model.Particle>();

            // find all particles that need to be removed
            foreach (var particle in _world.Particles)
            {
                if (particle.Position.X > canvas.ActualWidth || particle.Position.X < 0 || particle.Position.Y > canvas.ActualHeight || particle.Position.Y < 0)
                    toRemove.Add(particle);
            }

            foreach (var particle in toRemove)
            {
                _world.RemoveParticle(particle);
                canvas.Children.Remove(_visualizationMap[particle]);
                _visualizationMap.Remove(particle);
            }

            // generate new particles
            _randomGenerator = new Random(DateTime.Now.Millisecond);
            int count =  _randomGenerator.Next(2, 5);
            for (int i = 0; i < count; i++)
            {
                var sx = _randomGenerator.Next(200, 390);
                var sy = 300;
                var vx = 0;
                var vy = _randomGenerator.Next(-150, -75);

                var particle = new Particle(new Vector2D(sx, sy), new Vector2D(vx, vy));
                _world.AddParticle(particle);

                var ellipse = CreateParticleVisualisation(particle);
                _visualizationMap.Add(particle, ellipse);
            }

            // find all particles that needs to be added
            foreach (var particle in _world.Particles.Where(p => !_visualizationMap.ContainsKey(p)).ToArray())
            {
                var ellipse = CreateParticleVisualisation(particle);
                _visualizationMap.Add(particle, ellipse);
            }

            // update position of each particle visualisation
            foreach (var particle in _world.Particles)
            {
                UpdatePosition(_visualizationMap[particle], particle);
            }

            _dispatcherTimer.Start();
        }

        private void UpdatePosition(UIElement ellipse, Particle particle)
        {
            ellipse.SetValue(Canvas.LeftProperty, particle.Position.X - _particleSize /2);
            ellipse.SetValue(Canvas.TopProperty, particle.Position.Y - _particleSize / 2);
        }
    }

    public class WorldBuilder
    {
        private readonly Collection<Model.Border> _borders = new Collection<Model.Border>();
        private readonly Collection<Model.Particle> _particles = new Collection<Particle>();
        private readonly Collection<Model.AccelerationArea> _areas = new Collection<AccelerationArea>();
        private readonly Collection<Model.StopArea> _stopAreas = new Collection<StopArea>();

        public WorldBuilder AddBorder(int sx, int sy, int ex, int ey)
        {
            _borders.Add(new Model.Border(new Vector2D(sx, sy), new Vector2D(ex, ey)));

            return this;
        }

        public WorldBuilder AddParticle(int sx, int sy, int vx, int vy)
        {
            _particles.Add(new Model.Particle(new Vector2D(sx, sy), new Vector2D(vx, vy)));

            return this;
        }

        public WorldBuilder AddAccelerationArea(int sx, int sy, int ex, int ey, int range, Vector2D maxForce)
        {
            _areas.Add(new AccelerationArea(new Vector2D(sx, sy), new Vector2D(ex, ey), range, maxForce));
            return this;
        }

        public WorldBuilder AddStopArea(int sx, int sy, int radius)
        {
            _stopAreas.Add(new StopArea(new Vector2D(sx, sy), radius));
            return this;
        }

        public World Build()
        {
            return new World(_borders, _particles, _areas, _stopAreas);
        }
    }
}
