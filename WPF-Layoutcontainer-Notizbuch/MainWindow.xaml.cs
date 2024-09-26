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
using WPF_Layoutcontainer_Notizbuch.Classes;

namespace WPF_Layoutcontainer_Notizbuch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Notiz AktuelleNotiz { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            //Initialise Notices
            new Notiz(Kategorie.Geburtstage, "Mutter: 18.03.1945");
            new Notiz(Kategorie.Geburtstage, "Vater: 21.08.1940");
            new Notiz(Kategorie.Internet, "www.ibb.com\r\nViele interessante Kurse");
            new Notiz(Kategorie.Urlaub, "Mallorca\r\nwar nicht gut!");
            new Notiz(Kategorie.Wichtig, "Steuererklärung\r\nmuss noch gemacht werden!!!");

            // Get all enum values and bind them to the ComboBox
            cbxKategorie.ItemsSource = Enum.GetValues(typeof(Kategorie)).Cast<Kategorie>();

            //Set default selection to Alle(all categories)

            cbxKategorie.SelectedItem = Kategorie.Alle;

            // To bind the listbox on window load
            listeAktualisieren();

        }

        public void listeAktualisieren()
        {
            var vorherigeAuswahl = AktuelleNotiz;
            lbxNotizen.Items.Clear();
            // Get selected item of kategorie combobox
            var selectedKategorie = (Kategorie)cbxKategorie.SelectedItem;

            foreach (var notiz in Notiz.Notizen.Values)
            {
                if (selectedKategorie == Kategorie.Alle || notiz.Kategorie == selectedKategorie)
                {
                    lbxNotizen.Items.Add(notiz);
                }
            }

            // Try to restore the original selection
            if (vorherigeAuswahl != null && lbxNotizen.Items.Contains(vorherigeAuswahl))
            {
                lbxNotizen.SelectedItem = vorherigeAuswahl;
            }
            else
            {
                AktuelleNotiz = null;
                tbxNotiz.IsEnabled = false;
                btnLöschen.IsEnabled = false;
            }
        }

        private void cbxKategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listeAktualisieren();
            btnNeu.IsEnabled = (Kategorie)cbxKategorie.SelectedItem != Kategorie.Alle;

            if (cbxKategorie.SelectedIndex == 0) // "All" category
            {
                Resources["rscgrayFarbe"] = Brushes.Red;
            }
            else
            {
                Resources["rscgrayFarbe"] = Brushes.DarkGray;
            }
        }

        private void lbxNotizen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxNotizen.SelectedItem != null)
            {
                AktuelleNotiz = (Notiz)lbxNotizen.SelectedItem;
                tbxNotiz.Text = AktuelleNotiz.Inhalt;
                tbxNotiz.IsEnabled = true;
                btnLöschen.IsEnabled = true;
            }
            else
            {
                tbxNotiz.Text = string.Empty;
                tbxNotiz.IsEnabled = false;
                btnLöschen.IsEnabled = false;
            }
            btnSpeichern.IsEnabled = false;
        }

        private void tbxNotiz_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AktuelleNotiz != null && !string.IsNullOrWhiteSpace(tbxNotiz.Text))
            {
                btnSpeichern.IsEnabled = true;
            }
            else
            {
                btnSpeichern.IsEnabled = false;
            }
        }
        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            if (AktuelleNotiz != null)
            {
                // Aktualisiere den Inhalt der aktuellen Notiz
                AktuelleNotiz.Inhalt = tbxNotiz.Text;

                // Aktualisiere die ListBox, um die Änderungen anzuzeigen
                listeAktualisieren();

                // Deaktiviere den Speichern-Button wieder
                btnSpeichern.IsEnabled = false;
            }
        }

        private void btnLöschen_Click(object sender, RoutedEventArgs e)
        {
            if (AktuelleNotiz != null)
            {
                // Bestätigungsdialog anzeigen
                var result = MessageBox.Show("Möchten Sie diese Notiz wirklich löschen?", "Bestätigung", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Entferne die Notiz und setze "AktuelleNotiz" auf null
                    Notiz.Entfernen(AktuelleNotiz);
                    AktuelleNotiz = null;

                    // Aktualisiere die ListBox und TextBox
                    listeAktualisieren();
                    tbxNotiz.Text = string.Empty;
                    tbxNotiz.IsEnabled = false;
                    btnLöschen.IsEnabled = false;
                }
            }
        }

        private void btnNeu_Click(object sender, RoutedEventArgs e)
        {
            var selectedKategorie = (Kategorie)cbxKategorie.SelectedItem;
            if (selectedKategorie != Kategorie.Alle)
            {
                // Erstelle eine neue Notiz und setze sie als "AktuelleNotiz"
                AktuelleNotiz = new Notiz(selectedKategorie, "Neue Notiz");

                // Aktualisiere die ListBox und TextBox
                listeAktualisieren();
                tbxNotiz.Text = AktuelleNotiz.Inhalt;
                tbxNotiz.IsEnabled = true;
                btnSpeichern.IsEnabled = true;
                btnLöschen.IsEnabled = true;
                tbxNotiz.Focus();
                tbxNotiz.SelectAll();

            }
        }

        private void btnBeenden_Click(object sender, RoutedEventArgs e)
        {
            // Beende die Anwendung
            Application.Current.Shutdown();
        }
    }
}