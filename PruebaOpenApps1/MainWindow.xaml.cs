using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace PruebaOpenApps1
{
	public class Url
	{
		public int Id { get; set; }
		public string url { get; set; }
	}

	public partial class MainWindow : Window
	{
		readonly List<string> listcollection = new();

		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += new RoutedEventHandler(Window1_Loaded);


		}
		void Window1_Loaded(object sender, RoutedEventArgs e)
		{
			GetData();
			string path = "config.txt";
			if (File.Exists(path))
			{
				string[] texts = File.ReadAllLines(path);
				foreach (string text in texts)
				{
					listBox1.Items.Add(text);
				}
			}
		}

		public void GetData()
		{
			myCombo.Items.Clear();
			SqlConnectionStringBuilder str = new SqlConnectionStringBuilder();
			// servidor
			str.DataSource = "ASPIREX";
			// Database
			str.InitialCatalog = "appOpener";
			str.UserID = "sa";
			str.Password = "su1234";
			SqlConnection myConnection = new SqlConnection(str.ConnectionString);
			myConnection.Open();
			SqlDataReader dr = new SqlCommand("SELECT * FROM dbo.app_opener_urls", myConnection).ExecuteReader();

			while (dr.Read())
			{
				myCombo.Items.Add(dr.GetString(1));
			}
			myConnection.Close();
		}

		private void ButtonAddName_Click(object sender, RoutedEventArgs e)
		{
			AddNewEntry();
		}
		// Añadir elementos a la lista
		private void AddNewEntry()
		{
			listBox1.Items.Add(myCombo.Text);
		}
		// Eliminar elemnto de la lista
		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (listBox1.Items.Count > 0)
			{
				if (listBox1.SelectedIndex != -1)
					listBox1.Items.RemoveAt(listBox1.Items.IndexOf(listBox1.SelectedItem));
				else
					MessageBox.Show("Select an item");
			}
		}

		private void OpenAppsBtn_Click(object sender, RoutedEventArgs e)
		{
			if (listBox1.Items.Count > 0)
				foreach (string item in listBox1.Items)
				{
					string url_string = item;
					try
					{
						Process.Start(url_string);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				}
			else
				MessageBox.Show("No item selected");
		}

		public void InsertData()
		{
			string _path = pathBox.Text;
			SqlConnectionStringBuilder str = new SqlConnectionStringBuilder();
			// servidor
			str.DataSource = "ASPIREX";
			// Database
			str.InitialCatalog = "appOpener";
			str.UserID = "sa";
			str.Password = "1234";
			SqlConnection myConnection = new SqlConnection(str.ConnectionString);
			myConnection.Open();
			try
			{
				SqlCommand dr = new SqlCommand("INSERT INTO dbo.app_opener_urls(url) VALUES(@Param1)", myConnection);
				dr.Parameters.AddWithValue("@Param1", _path);
				dr.ExecuteNonQuery();
				MessageBox.Show(_path + " path added correctly");
				GetData();
			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.Message);
			}
			myConnection.Close();
		}

		private void ButtonInsertUrl_Click(object sender, RoutedEventArgs e)
		{
			if (pathBox.Text != string.Empty)
			{
				InsertData();
				pathBox.Clear();
			}
			else
				MessageBox.Show("No data");
		}

		private void SaveConfigButton_Click(object sender, RoutedEventArgs e)
		{
			string path = "config.txt";
			if (listBox1.Items.IsEmpty == false)
			{
				// Borra los datos del archivo
				File.Delete(path);
				listcollection.Clear();
				// Por cada elemento que haya en el listbox, lo inserta a la lista
				foreach (string str in listBox1.Items)
				{
					listcollection.Add(str);
				}
				// por cada elemento en la lista lo inserta al archivo, con line break
				foreach (string a in listcollection)
				{
					File.AppendAllText(path, a + "\r\n");
				}
			}
			else
			{
				File.Delete(path);
				listcollection.Clear();
				MessageBox.Show("si");
			}

			// Manda mensaje con los items guardados
			ReadData();
		}
		public void ReadData()
		{
			string path = "config.txt";
			if (File.Exists(path))
			{
				string[] texts = File.ReadAllLines(path);
				MessageBox.Show(string.Join("\r\n", texts));
			}
		}

	}
}
