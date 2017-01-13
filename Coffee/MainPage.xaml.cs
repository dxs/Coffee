using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Coffee
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		public ObservableCollection<People> ListPeople { get; set; }
		public People Selected { get; set; }
		private string key = "Save";
		private bool locker = false;
		private string newName = "";

        public MainPage()
        {
			ListPeople = new ObservableCollection<People>();
			Selected = new People();
			this.InitializeComponent();
			GetDataAsync();
			this.Bindings.Update();
			if (combo.Items.Count > 0)
			{
				Selected = ListPeople[0];
				combo.SelectedIndex = 0;
			}
			this.Bindings.Update();
		}

		private async void GetDataAsync()
		{
			var helper = new LocalObjectStorageHelper();
			if (await helper.FileExistsAsync(key))
			{
				Debug.WriteLine("Loaded data");
				ListPeople = await helper.ReadFileAsync<ObservableCollection<People>>(key);
			}
			else
			{
				ListPeople.Add(new People("Morgane", 2, 0, 0));
				SaveData();
			}
		}

		private async void SaveData()
		{
			if (locker == true)
					return;
			locker = true;
			var helper = new LocalObjectStorageHelper();
			await helper.SaveFileAsync(key, ListPeople);
			Debug.WriteLine("Saved");
			locker = false;
		}

		private async void backgroundImage_Loaded(object sender, RoutedEventArgs e)
		{
			foreach (var child in MainStack.Children)
			{
				if (child.GetType() == typeof(StackPanel))
				{
					foreach (var subchild in (child as StackPanel).Children)
					{
						if (subchild.GetType() != typeof(StackPanel))
							subchild.Fade(0, 0, 0).Start();
						else
							subchild.Fade(0, 0, 0).Start();
						Debug.WriteLine("Done fade subchild");
					}
				}
				else
					child.Fade(0, 0, 0).Start();
			}
			await backgroundImage.Blur(8f, 1000, 300).StartAsync();
			int delay = 400;
			int total = 0;
			foreach (var child in MainStack.Children)
			{
				if (child.GetType() == typeof(StackPanel))
				{
					foreach (var subchild in (child as StackPanel).Children)
					{
						if (subchild.GetType() != typeof(StackPanel))
							subchild.Fade(1, 500, delay * total++).Start();
						else
							subchild.Fade(1, 500, delay * total++).Start();
						Debug.WriteLine("Done subchild");
					}
				}
					child.Fade(1, 500, delay * total++).Start();
			}
		}

		private void ButtonCoffee_Click(object sender, RoutedEventArgs e)
		{
			int n = combo.SelectedIndex;
			if (n < 0)
				return;
			ListPeople[n].AddCoffee(sucreBox.IsChecked, CremeBox.IsChecked);
			Selected = ListPeople[n];
			SaveData();
			this.Bindings.Update();
		}

		private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (combo.SelectedIndex < 0)
				return;
			Selected = ListPeople[combo.SelectedIndex];
			this.Bindings.Update();
		}

		private async void AppBarButton_ClickAsync(object sender, RoutedEventArgs e)
		{
			TextBox t = new TextBox();
			t.PlaceholderText = "Nom";
			t.TextChanged += (s, i) => { newName = t.Text; };
			ContentDialog contentDialog = new ContentDialog() { Content = t, Title = "Ajouter un buveur de feca" };
			contentDialog.MinWidth = this.ActualWidth;
			contentDialog.MaxWidth = this.ActualWidth;
			contentDialog.PrimaryButtonText = "Ajouter";
			contentDialog.SecondaryButtonText = "Annuler";
			contentDialog.SecondaryButtonClick += (obj, ex) => { newName = ""; };
			contentDialog.PrimaryButtonClick += (obj, ex) => { ListPeople.Add(new People(newName, 0, 0, 0)); SaveData(); };
			await contentDialog.ShowAsync();
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			string _name = (sender as Button).Tag.ToString();
			People current = null;
			foreach (People item in ListPeople)
				if (item.Name == _name)
					current = item as People;
			if (current == null)
				return;
			ContentDialog content = new ContentDialog() { Title = "Valider la suppression", Content = "Supprimer " + _name + "?" };
			content.MinWidth = this.ActualWidth;
			content.MaxWidth = this.ActualWidth;
			content.PrimaryButtonText = "Supprimer";
			content.SecondaryButtonText = "Non!";
			content.SecondaryButtonClick += (ob, ex) => { };
			content.PrimaryButtonClick += (ob, ex) => { ListPeople.Remove(current); SaveData(); };
			await content.ShowAsync();

		}
	}
}
