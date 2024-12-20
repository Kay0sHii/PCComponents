using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PCComponents
{
    /// <summary>
    /// Логика взаимодействия для WindowActivity.xaml
    /// </summary>
    public partial class WindowActivity : Window
    {
        string connectionString = @"Data Source=DataBasePCC.db"; // Строка подключения
        public ObservableCollection<string> ComponentsList { get; set; } = new ObservableCollection<string>(); // ObservableCollection для автоматического обновления ListBox

        public string selectedComponent = ""; // Получение имя из RadioButton

        public WindowActivity()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer // Настройка таймера для обновления времени каждую секундучускенгшщз
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
            UpdateTime(); // Установка времени сразу при запуске

            DataContext = this;  // Привязываем данные к интерфейсу

            LabelLoaded(); // Загрузка Того что было в Lable на случай несанкционированного закрытия qweg
        }

        private void Timer_Tick(object sender, EventArgs e) // При каждом тике обновляються данные в TimeLabel
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            TimeZoneInfo TimeZone = TimeZoneInfo.FindSystemTimeZoneById("North Asia Standard Time"); // Получение текущего времени в часовом поясе Красноярска
            DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZone);

            TimeLabel.Content = "Time: " + currentTime.ToString("HH:mm"); // Обновление текста в Label
        }

        private void OpenAddWindow(object sender, RoutedEventArgs e) // Открытие окна Settings по нажатию на кнопку 
        {
            AddWindow addWindow = new AddWindow();
            addWindow.Show(); // Открываем окно
        }

        private void PCInfo(object sender, RoutedEventArgs e) // Открытие окна Info по нажатию на кнопку
        {
            InfoWindow infoWindow = new InfoWindow();
            infoWindow.Show();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e) // Проверяет нажатие на RadioButton и выводит данные из БД в ListBox )
        {
            selectedComponent = (sender as RadioButton)?.Name; // Получение имя из RadioButton

            if (selectedComponent != null)
            {
                ComponentsList.Clear();

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string query;

                    switch (selectedComponent)
                    {
                        case "CPU":
                            query = "SELECT name, soket FROM CPU";
                            CopList(query, "CPU");
                            break;
                        case "GPU":
                            query = "SELECT name FROM GPU";
                            CopList(query, "GPU");
                            break;
                        case "RAM":
                            query = "SELECT name, dimm FROM RAM";
                            CopList(query, "RAM");
                            break;
                        case "MOTHERBOARD":
                            query = "SELECT name, soket, dimm FROM MOTHERBOARD";
                            CopList(query, "MOTHERBOARD");
                            break;
                        case "STORAGE":
                            query = "SELECT name FROM STORAGE";
                            CopList(query, "STORAGE");
                            break;
                        case "POWERUNIT":
                            query = "SELECT name, power FROM POWERUNIT";
                            CopList(query, "POWERUNIT");
                            break;
                        default:
                            query = null;
                            break;
                    }
                    connection.Close();
                }
            }
        }

        private void CopList(string query, string name)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                if (query != null)
                {
                    using (var command = new SqliteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (name == "CPU") { ComponentsList.Add($"{reader.GetString(0)} (Soket: {reader.GetString(1)})"); } // Добавляем данные в список
                            else
                            {
                                if (name == "RAM") { ComponentsList.Add($"{reader.GetString(0)} (DIMM: {reader.GetString(1)})"); } // Добавляем данные в список
                                else
                                {
                                    if (name == "MOTHERBOARD") { ComponentsList.Add($"{reader.GetString(0)} (Soket: {reader.GetString(1)}, DIMM: {reader.GetString(2)})"); } // Добавляем данные в список
                                    else
                                    {
                                        if (name == "POWERUNIT")
                                        {
                                            ComponentsList.Add($"{reader.GetString(0)} (Power: {reader.GetString(1)})");
                                        }
                                        else { ComponentsList.Add(reader.GetString(0)); }
                                    }
                                }
                            }
                        }
                    }
                }
                connection.Close();
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // Выводит на экран то что выбранно в Grid и выводит выбранные комплектующие в label так же сохроняет их в , данных 
        {
            if (ListBox.SelectedItem != null) // Если не нажато то ничего не выводим
            {
                foreach (string component in ComponentsList) // Перебираем компоненты из листа ComponentsList
                {
                    if (component == ListBox.SelectedItem.ToString()) // Сравниваем компоненты с тем что находиться в ListBox на который мы нажали
                    {
                        if (selectedComponent != null) // selectedComponent - имя активного RadioButton
                        {
                            switch (selectedComponent) // Обращение к активного RadoiButton
                            {
                                case "CPU":
                                    SaveConfig(component, "CPU"); // Сохранение данных в БД Saves 
                                    LabelLoaded(); // Обновление данных в Label
                                    break;
                                case "GPU":
                                    SaveConfig(component, "GPU");
                                    LabelLoaded();
                                    break;
                                case "RAM":
                                    SaveConfig(component, "RAM");
                                    LabelLoaded();
                                    break;
                                case "MOTHERBOARD":
                                    SaveConfig(component, "MOTHERBOARD");
                                    LabelLoaded();
                                    break;
                                case "STORAGE":
                                    SaveConfig(component, "STORAGE");
                                    LabelLoaded();
                                    break;
                                case "POWERUNIT":
                                    SaveConfig(component, "POWERUNIT");
                                    LabelLoaded();
                                    break;
                                default:
                                    break;
                            }
                        }
                        MessageBox.Show($"Вы выбрали: {component}"); // Грамматика!!!!!!!!!!!!!!!!!!!!!!!!!
                    }
                }
            }
            //CheckSovmestimosti();
        }

        private void SaveConfig(string name, string Selected) // Сохранение данных в бд
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SaveToTable(connection, name, Selected); // Сохранение данных name - Название компонента(Razen 5 2600) !!!!!!!!!!!!!!!!! перед ним реализовать проврку совместимости !!!!!!!!

                connection.Close();
            }
        }
        private void SaveToTable(SqliteConnection connection, string name, string Selected) // Продолжение для сохранение 2 этап
        {
            string query = $"UPDATE Saves SET {Selected} = @{Selected}";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue($"@{Selected}", name);
                command.ExecuteNonQuery();
            }
        }

        private void ButtonReset(object sender, RoutedEventArgs e) // очищает данные из label и БД Saves заменяя на ...
        {
            SaveConfig("...", "CPU");
            SaveConfig("...", "GPU");
            SaveConfig("...", "RAM");
            SaveConfig("...", "MOTHERBOARD");
            SaveConfig("...", "STORAGE");
            SaveConfig("...", "POWERUNIT");
            LabelLoaded();
            MessageBox.Show("Данные были успешно очищены!");
        }

        private void LabelLoaded() // Загрузка данных из бд Saves ( Защита от пропадания данных)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // SQL-запрос для получения данных из таблицы Saves
                string query = "SELECT CPU, GPU, RAM, MOTHERBOARD, STORAGE, POWERUNIT FROM Saves LIMIT 1";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Если есть данные
                        {
                            // Привязываем данные к меткам
                            label1.Content = "CPU: " + reader["CPU"].ToString();
                            label2.Content = "GPU: " + reader["GPU"].ToString();
                            label3.Content = "RAM: " + reader["RAM"].ToString();
                            label4.Content = "MOTHERBOARD: " + reader["MOTHERBOARD"].ToString();
                            label5.Content = "STORAGE: " + reader["STORAGE"].ToString();
                            label6.Content = "POWER UNIT: " + reader["POWERUNIT"].ToString();
                        }
                    }
                }
            }
        }

        private void CheckSovmestimosti(object sender, RoutedEventArgs e) // Реализует проверку совместимости компонентов 
        {
            // Названия компонентов из таблицы Saves
            string CPU = "";
            string RAM = "";
            string MOTHERBOARD = "";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // SQL-запрос для получения данных из таблицы Saves
                string query = "SELECT CPU, RAM, MOTHERBOARD FROM Saves LIMIT 1";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Если есть данные то:
                        {
                            // Привязываем данные к меткам
                            CPU = reader["CPU"].ToString();
                            RAM = reader["RAM"].ToString();
                            MOTHERBOARD = reader["MOTHERBOARD"].ToString();
                        }
                    }
                }
            }
            infoSoketDimm(CPU, RAM, MOTHERBOARD);
        }
        private void infoSoketDimm(string CPU, string RAM, string MOTHERBOARD)
        {
            string soketCPU = "";
            string dimmRAM = "";
            string soketMOTHERBOARD = "";
            string dimmMOTHERBOARD = "";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // SQL-запрос для получения данных из таблицы Saves
                string queryCPU = $"SELECT soket FROM CPU WHERE name = '{Regex.Replace(CPU, @"\s*\([^)]*\)", "")}'"; // SELECT soket FROM CPU WHERE name = 'Intel core i9 12900k';
                string queryRAM = $"SELECT dimm FROM RAM WHERE name = '{Regex.Replace(RAM, @"\s*\([^)]*\)", "")}'";
                string queryMOT = $"SELECT soket, dimm FROM MOTHERBOARD WHERE name = '{Regex.Replace(MOTHERBOARD, @"\s*\([^)]*\)", "")}'";

                using (var command = new SqliteCommand(queryCPU, connection)) // CPU
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Если есть данные то:
                        {
                            // Привязываем данные к меткам
                            soketCPU = reader["soket"].ToString();
                        }
                    }
                }
                using (var command = new SqliteCommand(queryRAM, connection)) // RAM
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Если есть данные то:
                        {
                            // Привязываем данные к меткам
                            dimmRAM = reader["dimm"].ToString();
                        }
                    }
                }
                using (var command = new SqliteCommand(queryMOT, connection)) // MOTHERBOARD
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Если есть данные то:
                        {
                            // Привязываем данные к меткам
                            soketMOTHERBOARD = reader["soket"].ToString();
                            dimmMOTHERBOARD = reader["dimm"].ToString();
                        }
                    }
                }
            }
            // Сравнение soket and dimm
            if (dimmMOTHERBOARD != dimmRAM & soketMOTHERBOARD != soketCPU) { MessageBox.Show("Выбирете другую матринскую плату, процессор или ОЗУ. \nК сожалению они не совместимы"); }
            else
            {
                if (soketMOTHERBOARD != soketCPU)
                {
                    if (dimmMOTHERBOARD == dimmRAM)
                    {
                        MessageBox.Show("Выбирете другой процеессор или материнскую плату и ОЗУ. \nК сожалению они не совместимы");
                    }
                    else { MessageBox.Show("Выбирете другую матринскую плату или процеессор. \nК сожалению они не совместимы"); }
                }
                else
                {
                    if (dimmMOTHERBOARD != dimmRAM)
                    {
                        if (soketMOTHERBOARD == soketCPU)
                        {
                            MessageBox.Show("Выбирете другой ОЗУ или процессор с матринской платой. \nК сожалению они не совместимы");
                        }
                        else { MessageBox.Show("Выбирете другую матринскую плату или ОЗУ. \nК сожалению они не совместимы"); }
                    }
                    else { MessageBox.Show("Отлично, у васполучилась великолепная сборка :)"); }
                }
            }
        }
    }
}
