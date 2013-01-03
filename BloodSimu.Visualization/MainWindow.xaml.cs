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

        public MainWindow()
        {
            InitializeComponent();

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
                var ellipse = new Ellipse();
                ellipse.Fill = Brushes.Brown;
                ellipse.Width = 4;
                ellipse.Height = 4;
                ellipse.SetValue(Canvas.LeftProperty, particle.Position.X + 2);
                ellipse.SetValue(Canvas.TopProperty, particle.Position.Y + 2);

                canvas.Children.Add(ellipse);
                _visualizationMap.Add(particle, ellipse);
            }
        }

        private void BuildTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(50);
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Start();
        }

        private void BuildWorld()
        {
            var worldBuilder = new WorldBuilder();
            _world = worldBuilder
                .AddBorder(0, 0, 100, 100)
                .AddParticle(10, 0, 0, 10)
                .Build();
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _world.Move(TimeSpan.FromMilliseconds(50));

            // find all particles that needs to be added 
            foreach (var particle in _world.Particles)
            {
                var element = _visualizationMap[particle];
                element.SetValue(Canvas.LeftProperty, particle.Position.X + 2);
                element.SetValue(Canvas.TopProperty, particle.Position.Y + 2);
            }
        }
    }

    public class WorldBuilder
    {
        private readonly Collection<Model.Border> _borders = new Collection<Model.Border>();
        private readonly Collection<Model.Particle> _particles = new Collection<Particle>();

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

        public World Build()
        {
            return new World(_borders, _particles);
        }
    }
}
