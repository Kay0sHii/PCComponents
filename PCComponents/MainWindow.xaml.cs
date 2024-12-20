using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PCComponents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 2);
            timer.Start();
        }
        private void timerTick(object sender, EventArgs e)
        {
            ProgressBar1.Value++;

            if (ProgressBar1.Value == ProgressBar1.Maximum)
            {
                timer.Stop();
                WindowActivity windowActivity = new WindowActivity();
                windowActivity.Show();
                this.Close();
            }
        }
    }
}