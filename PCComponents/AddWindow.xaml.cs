using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace PCComponents
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private readonly string connectionString = @"Data Source=DataBasePCC.db";
        public AddWindow()
        {
            InitializeComponent();
            ClearFields();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SaveToCpuTable(connection, CpuName.Text, CpuSoket.Text, CpuRating.Text); // Сохранение данных CPU

                SaveToGpuTable(connection, GpuName.Text, GpuRating.Text); // Сохранение данных GPU

                SaveToRamTable(connection, RamName.Text, RamDimm.Text, RamMhz.Text); // Сохранение данных RAM

                SaveToMotherboardTable(connection, MotherboardName.Text, MotherboardSoket.Text, MotherboardDimm.Text); // Сохранение данных Motherboard

                SaveToStorageTable(connection, StorageName.Text); // Сохранение данных Storage

                SaveToPowerUnitTable(connection, PowerUnitName.Text, PowerUnitPower.Text); // Сохранение данных Power Unit

                connection.Close();
            }
            MessageBox.Show("Данные успешно сохранены!");
            ClearFields();
        }

        private void SaveToCpuTable(SqliteConnection connection, string name, string soket, string rating) // CPU
        {
            int nextId = 1; // Значение по умолчанию, если таблица пустая
            string getIdQuery = "SELECT MAX(Id) FROM CPU"; // Находим максимальный Id
            using (var getIdCommand = new SqliteCommand(getIdQuery, connection))
            {
                var result = getIdCommand.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    nextId = Convert.ToInt32(result) + 1; // Увеличиваем Id на 1
                }
            }

            string query = "INSERT INTO CPU (id, name, soket, rating) VALUES (@id, @name, @soket, @rating)";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", nextId);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@soket", soket);
                command.Parameters.AddWithValue("@rating", rating);
                command.ExecuteNonQuery();
            }
        }

        private void SaveToGpuTable(SqliteConnection connection, string name, string rating) // GPU
        {
            int nextId = 1; // Значение по умолчанию, если таблица пустая
            string getIdQuery = "SELECT MAX(Id) FROM GPU"; // Находим максимальный Id
            using (var getIdCommand = new SqliteCommand(getIdQuery, connection))
            {
                var result = getIdCommand.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    nextId = Convert.ToInt32(result) + 1; // Увеличиваем Id на 1
                }
            }

            string query = "INSERT INTO GPU (id, name, rating) VALUES (@id, @name, @rating)";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", nextId);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@rating", rating);
                command.ExecuteNonQuery();
            }
        }

        private void SaveToRamTable(SqliteConnection connection, string name, string dimm, string mhz) // RAM
        {
            int nextId = 1; // Значение по умолчанию, если таблица пустая
            string getIdQuery = "SELECT MAX(Id) FROM RAM"; // Находим максимальный Id
            using (var getIdCommand = new SqliteCommand(getIdQuery, connection))
            {
                var result = getIdCommand.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    nextId = Convert.ToInt32(result) + 1; // Увеличиваем Id на 1
                }
            }

            string query = "INSERT INTO RAM (id, name, dimm, mhz) VALUES (@id, @name, @dimm, @mhz)";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", nextId);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@dimm", dimm);
                command.Parameters.AddWithValue("@mhz", mhz);
                command.ExecuteNonQuery();
            }
        }

        private void SaveToMotherboardTable(SqliteConnection connection, string name, string soket, string dimm) //MOTHERBOARD
        {
            int nextId = 1; // Значение по умолчанию, если таблица пустая
            string getIdQuery = "SELECT MAX(Id) FROM MOTHERBOARD"; // Находим максимальный Id
            using (var getIdCommand = new SqliteCommand(getIdQuery, connection))
            {
                var result = getIdCommand.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    nextId = Convert.ToInt32(result) + 1; // Увеличиваем Id на 1
                }
            }

            string query = "INSERT INTO MOTHERBOARD (id, name, soket, dimm) VALUES (@id, @name, @soket, @dimm)";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", nextId);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@soket", soket);
                command.Parameters.AddWithValue("@dimm", dimm);
                command.ExecuteNonQuery();
            }
        }

        private void SaveToStorageTable(SqliteConnection connection, string name) //STORAGE
        {
            int nextId = 1; // Значение по умолчанию, если таблица пустая
            string getIdQuery = "SELECT MAX(Id) FROM STORAGE"; // Находим максимальный Id
            using (var getIdCommand = new SqliteCommand(getIdQuery, connection))
            {
                var result = getIdCommand.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    nextId = Convert.ToInt32(result) + 1; // Увеличиваем Id на 1
                }
            }

            string query = "INSERT INTO STORAGE (id, name) VALUES (@id, @name)";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", nextId);
                command.Parameters.AddWithValue("@name", name);
                command.ExecuteNonQuery();
            }
        }

        private void SaveToPowerUnitTable(SqliteConnection connection, string name, string power) //POWERUNIT
        {
            int nextId = 1; // Значение по умолчанию, если таблица пустая
            string getIdQuery = "SELECT MAX(Id) FROM POWERUNIT"; // Находим максимальный Id
            using (var getIdCommand = new SqliteCommand(getIdQuery, connection))
            {
                var result = getIdCommand.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    nextId = Convert.ToInt32(result) + 1; // Увеличиваем Id на 1
                }
            }

            string query = "INSERT INTO POWERUNIT (id, name, power) VALUES (@id, @name, @power)";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", nextId);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@power", power);
                command.ExecuteNonQuery();
            }
        }

        private void ClearFields() // Отчистка всех полей ввода 
        {
            CpuName.Clear();
            CpuSoket.Clear();
            CpuRating.Clear();
            GpuName.Clear();
            GpuRating.Clear();
            RamName.Clear();
            RamDimm.Clear();
            RamMhz.Clear();
            MotherboardName.Clear();
            MotherboardSoket.Clear();
            MotherboardDimm.Clear();
            StorageName.Clear();
            PowerUnitName.Clear();
            PowerUnitPower.Clear();
        }
    }
}
