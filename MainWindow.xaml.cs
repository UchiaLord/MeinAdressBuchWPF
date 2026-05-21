using MeinAdressBuchBE;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MeinAdressBuchWPF
{
    public partial class MainWindow : Window
    {
        private Personen personen = new Personen();
        private readonly AdressbuchDateiService dateiService = new AdressbuchDateiService();

        private const string DateiPfad = "MeinAdressbuch.txt";

        public MainWindow()
        {
            InitializeComponent();
            AktualisiereDataGrid();
        }

        private void HinzufuegenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Person neuePerson = ErstellePersonAusEingabe();

                personen.AddPerson(neuePerson);

                RefreshDataGrid();
                LeereEingabeFelder();

                StatusTextBlock.Text = "Person erfolgreich hinzugefügt.";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Fehler: {ex.Message}";
            }
        }


        private void PersonenDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PersonenDataGrid.SelectedItem is Person ausgewaehltePerson)
            {
                VornameTextBox.Text = ausgewaehltePerson.Vorname;
                NachnameTextBox.Text = ausgewaehltePerson.Nachname;
                GeburtsdatumTextBox.Text = ausgewaehltePerson.Geburtsdatum;
                AdresseTextBox.Text = ausgewaehltePerson.Adresse;
                PLZTextBox.Text = ausgewaehltePerson.Postleitzahl;
                TelefonnummerTextBox.Text = ausgewaehltePerson.Telefonnummer;

                StatusTextBlock.Text = "Person ausgewählt.";
            }
        }

        private void BearbeitenButton_Click(object sender, RoutedEventArgs e)
        {
            if (PersonenDataGrid.SelectedItem is not Person ausgewaehltePerson)
            {
                StatusTextBlock.Text = "Bitte wählen Sie eine Person zum Bearbeiten aus.";
                return;
            }

            try
            {
                Person bearbeitetePerson = ErstellePersonAusEingabe();

                personen.ValidatePerson(bearbeitetePerson);

                ausgewaehltePerson.Vorname = bearbeitetePerson.Vorname;
                ausgewaehltePerson.Nachname = bearbeitetePerson.Nachname;
                ausgewaehltePerson.Geburtsdatum = bearbeitetePerson.Geburtsdatum;
                ausgewaehltePerson.Adresse = bearbeitetePerson.Adresse;
                ausgewaehltePerson.Postleitzahl = bearbeitetePerson.Postleitzahl;
                ausgewaehltePerson.Telefonnummer = bearbeitetePerson.Telefonnummer;

                PersonenDataGrid.Items.Refresh();
                LeereEingabeFelder();

                StatusTextBlock.Text = "Person erfolgreich bearbeitet.";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Fehler: {ex.Message}";
            }
        }

        private void LoeschenButton_Click(object sender, RoutedEventArgs e)
        {
            if (PersonenDataGrid.SelectedItem is not Person ausgewaehltePerson)
            {
                StatusTextBlock.Text = "Bitte wählen Sie eine Person zum Löschen aus.";
                return;
            }

            personen.RemovePerson(ausgewaehltePerson);

            AktualisiereDataGrid();
            LeereEingabeFelder();

            StatusTextBlock.Text = "Person erfolgreich gelöscht.";
        }

        private void SpeichernButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dateiService.SaveToFile(DateiPfad, personen.GetAllPersons());

                StatusTextBlock.Text = "Adressbuch erfolgreich gespeichert.";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Fehler beim Speichern: {ex.Message}";
            }
        }

        private void LadenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Person> geladenePersonen = dateiService.LoadFromFile(DateiPfad);

                personen = new Personen();

                foreach (Person person in geladenePersonen)
                {
                    personen.AddPerson(person);
                }

                AktualisiereDataGrid();
                LeereEingabeFelder();

                StatusTextBlock.Text = "Adressbuch erfolgreich geladen.";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Fehler beim Laden: {ex.Message}";
            }
        }

        private Person ErstellePersonAusEingabe()
        {
            return new Person
            {
                Vorname = VornameTextBox.Text.Trim(),
                Nachname = NachnameTextBox.Text.Trim(),
                Geburtsdatum = GeburtsdatumTextBox.Text.Trim(),
                Adresse = AdresseTextBox.Text.Trim(),
                Postleitzahl = PLZTextBox.Text.Trim(),
                Telefonnummer = TelefonnummerTextBox.Text.Trim()
            };
        }

        private void AktualisiereDataGrid()
        {
            PersonenDataGrid.ItemsSource = personen.GetAllPersons();
        }

        private void RefreshDataGrid()
        {
            PersonenDataGrid.Items.Refresh();
        }

        private void LeereEingabeFelder()
        {
            VornameTextBox.Clear();
            NachnameTextBox.Clear();
            GeburtsdatumTextBox.SelectedDate = null;
            AdresseTextBox.Clear();
            PLZTextBox.Clear();
            TelefonnummerTextBox.Clear();
        }
    }
}