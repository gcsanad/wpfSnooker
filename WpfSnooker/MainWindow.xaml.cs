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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WpfSnooker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Versenyzo> versenyzok = File.ReadAllLines("snooker.txt").Skip(1).Select(x => new Versenyzo(x)).ToList();
        public MainWindow()
        {
            InitializeComponent();
            cbOrszagok.ItemsSource = versenyzok.Select(x => x.Orszag).Distinct();
            dgTablazat.ItemsSource = versenyzok;
            btnF3.Click += (x, y) =>
            {
                MessageBox.Show($"3.feladat: A világranglistán {versenyzok.Count} versenyzző szerepel");
            };
            btnF4.Click += (x, y) =>
            {
                MessageBox.Show($"4.feladat: A versenyzők átlagosan {versenyzok.Average(x => x.Nyeremeny):f2} fontot kerestek");
            };
            btnF5.Click += (x, y) => 
            {
                var legtobbetKeresoVersenyzo = versenyzok.Where(x => x.Orszag == cbOrszagok.SelectedItem.ToString()).MaxBy(x => x.Nyeremeny);
                lblHelyezes.Content = legtobbetKeresoVersenyzo.Helyezes;
                lblOrszag.Content = legtobbetKeresoVersenyzo.Orszag;
                lblNyeremeny.Content = $"{legtobbetKeresoVersenyzo.Nyeremeny*int.Parse(txtArfolyam.Text):n0} Ft";
                lblNev.Content = legtobbetKeresoVersenyzo.Nev;
            };
            btnF6.Click += (x, y) =>
            {
                MessageBox.Show(versenyzok.Exists(x =>x.Orszag == txtVanIlyenOrszag.Text) ? "A versenyzők között van ilyen versenyző!" : "Nincsen ilyen versenyző!");
            };
            btnF7.Click += (x, y) =>
            {
                lbStatisztika.Items.Clear();
                lbStatisztika.Items.Add($"7.feladat: Statisztika \nMinimum {lblminFo.Content} fő ");
                versenyzok.GroupBy(x => x.Orszag).Where(x => x.Count() >= int.Parse(lblminFo.Content.ToString())).ToList().ForEach(x => lbStatisztika.Items.Add($"{x.Key} - {x.Count()} fő"));

            };
            lbStatisztika.MouseDoubleClick += (sender, events) =>
            {
                dgTablazat.ItemsSource = versenyzok.Where(x => x.Orszag.Contains(lbStatisztika.SelectedItem.ToString().Split()[0]));
            };
        }

       
    }
}
