using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee
{
	public class People
	{
		public string Name { get; set; }
		public int NbCoffee { get; set; }
		public int NbAddons { get; set; }
		public double Money { get; set; }
		private double PriceCoffee = 1.60;
		private double Addon = 0.1;

		public People()
		{

		}
		public People(string _name, int _nCoffee, int _nAddons, double _money)
		{
			Name = _name; NbCoffee = _nCoffee; NbAddons = _nAddons; Money = _money;
			if (Money == 0)
				ComputeMoney();
		}

		private void ComputeMoney()
		{
			Money = NbCoffee * PriceCoffee + NbAddons * Addon;
		}

		public void AddCoffee(bool? Sugar, bool? Cream)
		{
			NbCoffee++;
			Money += PriceCoffee;
			if(Cream == true)
			{
				NbAddons++; Money += Addon;
			}
			if (Sugar == true)
			{
				NbAddons++; Money += Addon;
			}
		}

	}
}
