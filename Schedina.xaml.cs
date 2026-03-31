using InvioSchedineAlloggiatiWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
using System.Globalization;

namespace InvioSchedineAlloggiatiWeb
{
    /// <summary>
    /// Logica di interazione per Schedina.xaml
    /// </summary>
    public partial class Schedina : Window
    {
        public static bool BooleanTrue = true;
        public static bool BooleanFalse = false;

        private RecordSchedina recordSchedina = null;
        private bool _isDirty = false;

        public Schedina()
        {
            InitializeComponent();
        }

        internal void SetRecord(RecordSchedina rs)
        {
            recordSchedina = rs;

            // Tipo Alloggiato
            SetComboByCode(cbTipoAlloggiato, rs.TipoAlloggiato.Trim());

            // Data Arrivo
            if (DateTime.TryParseExact(rs.DataArrivo, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataArrivo))
            {
                dpDataArrivo.SelectedDate = dataArrivo;
                DateTime ieri = DateTime.Today.AddDays(-1);
                dpDataArrivo.DisplayDateStart = dataArrivo < ieri ? dataArrivo : ieri;
            }
            else
            {
                dpDataArrivo.SelectedDate = DateTime.Today;
            }

            // Giorni permanenza
            iudGiorniPerm.Value = int.TryParse(rs.GiorniPermanenza.Trim(), out int giorni) ? giorni : 1;

            // Anagrafica
            tbCognome.Text = rs.Cognome.Trim();
            tbNome.Text = rs.Nome.Trim();

            if (rs.Sesso.Trim() == "2")
                rbFemmina.IsChecked = true;
            else
                rbMaschio.IsChecked = true;

            // Data nascita
            if (dpDataNascita.DisplayDateEnd.HasValue)
                dpDataNascita.DisplayDateStart = dpDataNascita.DisplayDateEnd.Value.AddYears(-120);

            if (DateTime.TryParseExact(rs.DataNascita, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataNascita))
                dpDataNascita.SelectedDate = dataNascita;
            else
                dpDataNascita.SelectedDate = null;

            // Stato nascita: la SelectionChanged provvede ad abilitare/popolare cbComuneNascita
            // Default ITALIA per nuove schedine (codice blank)
            SetComboByCode(cbStatoNascita, rs.StatoNascita.Trim(), "ITALIA");

            // Cittadinanza — default ITALIA per nuove schedine
            SetComboByCode(cbStatoCittadinanza, rs.Cittadinanza.Trim(), "ITALIA");

            // Campi documento: abilitazione guidata dal TipoAlloggiato corrente nel combo
            AggiornaCampiDocumento();
            if (cbTipoDocumento.IsEnabled && !string.IsNullOrWhiteSpace(rs.TipoDoc))
                SetComboByCode(cbTipoDocumento, rs.TipoDoc.Trim());
            if (tbNumDoc.IsEnabled)
                tbNumDoc.Text = rs.NumeroDoc.Trim();
            if (cbLuogoDoc.IsEnabled && !string.IsNullOrWhiteSpace(rs.LuogoRilascioDoc))
                SetComboByCode(cbLuogoDoc, rs.LuogoRilascioDoc.Trim());

            // Sottoscrizione degli eventi di modifica DOPO l'inizializzazione:
            // in questo modo _isDirty resta false finché non interviene l'utente.
            TextChangedEventHandler                      onText      = (s, e) => _isDirty = true;
            SelectionChangedEventHandler                 onSelection = (s, e) => _isDirty = true;
            EventHandler<SelectionChangedEventArgs>      onDate      = (s, e) => _isDirty = true;
            RoutedEventHandler                           onChecked   = (s, e) => _isDirty = true;
            RoutedPropertyChangedEventHandler<object>    onValue     = (s, e) => _isDirty = true;

            tbCognome.TextChanged                += onText;
            tbNome.TextChanged                   += onText;
            tbNumDoc.TextChanged                 += onText;
            dpDataArrivo.SelectedDateChanged     += onDate;
            dpDataNascita.SelectedDateChanged    += onDate;
            iudGiorniPerm.ValueChanged           += onValue;
            rbMaschio.Checked                    += onChecked;
            rbFemmina.Checked                    += onChecked;
            cbTipoAlloggiato.SelectionChanged    += onSelection;
            cbStatoNascita.SelectionChanged      += onSelection;
            cbComuneNascita.SelectionChanged     += onSelection;
            cbStatoCittadinanza.SelectionChanged += onSelection;
            cbTipoDocumento.SelectionChanged     += onSelection;
            cbLuogoDoc.SelectionChanged          += onSelection;
        }

        // Seleziona un elemento del ComboBox per codice.
        // Se il codice è blank (record nuovo) e defaultDescrizione è specificata, cerca per descrizione.
        // In ultima istanza seleziona il primo elemento disponibile.
        private void SetComboByCode(ComboBox cb, string code, string defaultDescrizione = null)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                DataRowView row = cb.Items.OfType<DataRowView>()
                    .FirstOrDefault(r => ((string)r["Codice"]).Trim() == code);
                if (row != null)
                {
                    cb.SelectedItem = row;
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(defaultDescrizione))
            {
                DataRowView row = cb.Items.OfType<DataRowView>()
                    .FirstOrDefault(r => ((string)r["Descrizione"]).Trim().ToUpper() == defaultDescrizione.ToUpper());
                if (row != null)
                {
                    cb.SelectedItem = row;
                    return;
                }
            }
            if (cb.Items.Count > 0)
                cb.SelectedIndex = 0;
        }

        // Abilita/disabilita i campi documento in base al TipoAlloggiato selezionato (16, 17, 18)
        private void AggiornaCampiDocumento()
        {
            string tipo = cbTipoAlloggiato.SelectedItem is DataRowView r
                ? ((string)r["Codice"]).Trim()
                : "";
            bool haDoc = tipo == "16" || tipo == "17" || tipo == "18";

            cbTipoDocumento.IsEnabled = haDoc;
            tbNumDoc.IsEnabled        = haDoc;
            cbLuogoDoc.IsEnabled      = haDoc;

            if (!haDoc)
            {
                cbTipoDocumento.SelectedItem = null;
                tbNumDoc.Text = "";
                cbLuogoDoc.SelectedItem = null;
            }
        }

        private void cbTipoAlloggiato_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AggiornaCampiDocumento();
        }

        private void cbComuneNascita_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbComuneNascita.SelectedItem is DataRowView row)
                tbProvinciaNascita.Text = row.Row.Field<string>("Provincia").ToUpper();
            else
                tbProvinciaNascita.Text = "  ";
        }

        private void cbStatoNascita_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(cbStatoNascita.SelectedItem is DataRowView selRow)) return;

            if (selRow.Row.Field<string>("Descrizione").ToUpper() == "ITALIA")
            {
                cbComuneNascita.IsReadOnly = false;
                cbComuneNascita.IsEnabled  = true;

                // Se c'è già un comune nel record corrente, prova a pre-selezionarlo
                if (recordSchedina != null && !string.IsNullOrWhiteSpace(recordSchedina.ComuneNascita))
                {
                    try
                    {
                        DataRowView comuneRow = cbComuneNascita.Items.OfType<DataRowView>()
                            .Single(r => ((string)r["Codice"]).Trim() == recordSchedina.ComuneNascita.Trim());
                        cbComuneNascita.SelectedItem = comuneRow;
                        cbComuneNascita.Text = comuneRow.Row.Field<string>("Descrizione");
                    }
                    catch { }
                }
            }
            else
            {
                cbComuneNascita.IsReadOnly  = true;
                cbComuneNascita.IsEnabled   = false;
                cbComuneNascita.SelectedItem = null;
                cbComuneNascita.Text        = "         ";
                tbProvinciaNascita.Text     = "  ";
            }
        }

        private void cb_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            (cb.Template.FindName("PART_EditableTextBox", cb) as TextBox).CharacterCasing = CharacterCasing.Upper;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                TryCancel();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => TryCancel();

        private void TryCancel()
        {
            if (_isDirty)
            {
                var result = MessageBox.Show(
                    "Sono state apportate modifiche.\nAbbandonare senza salvare?",
                    "Conferma annullamento",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.No);

                if (result != MessageBoxResult.Yes) return;
            }
            DialogResult = false;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            // --- Validazione ---
            if (!(cbTipoAlloggiato.SelectedItem is DataRowView tipoRow))
            {
                MessageBox.Show("Selezionare il Tipo Alloggiato.", "Dati mancanti", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbTipoAlloggiato.Focus();
                return;
            }
            if (!dpDataArrivo.SelectedDate.HasValue)
            {
                MessageBox.Show("Inserire la Data di Arrivo.", "Dati mancanti", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpDataArrivo.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(tbCognome.Text))
            {
                MessageBox.Show("Inserire il Cognome.", "Dati mancanti", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbCognome.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(tbNome.Text))
            {
                MessageBox.Show("Inserire il Nome.", "Dati mancanti", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbNome.Focus();
                return;
            }
            if (!dpDataNascita.SelectedDate.HasValue)
            {
                MessageBox.Show("Inserire la Data di Nascita.", "Dati mancanti", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpDataNascita.Focus();
                return;
            }
            if (!(cbStatoNascita.SelectedItem is DataRowView statoNascitaRow))
            {
                MessageBox.Show("Selezionare lo Stato di Nascita.", "Dati mancanti", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbStatoNascita.Focus();
                return;
            }
            bool isItalia = statoNascitaRow.Row.Field<string>("Descrizione").ToUpper() == "ITALIA";
            if (isItalia && !(cbComuneNascita.SelectedItem is DataRowView))
            {
                MessageBox.Show("Selezionare il Comune di Nascita.", "Dati mancanti", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbComuneNascita.Focus();
                return;
            }
            if (!(cbStatoCittadinanza.SelectedItem is DataRowView))
            {
                MessageBox.Show("Selezionare lo Stato di Cittadinanza.", "Dati mancanti", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbStatoCittadinanza.Focus();
                return;
            }

            string tipoAlloggiato = ((string)tipoRow["Codice"]).Trim();
            bool haDoc = tipoAlloggiato == "16" || tipoAlloggiato == "17" || tipoAlloggiato == "18";

            if (haDoc)
            {
                if (!(cbTipoDocumento.SelectedItem is DataRowView))
                {
                    MessageBox.Show("Selezionare il Tipo Documento.", "Dati mancanti", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cbTipoDocumento.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(tbNumDoc.Text))
                {
                    MessageBox.Show("Inserire il Numero del Documento.", "Dati mancanti", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbNumDoc.Focus();
                    return;
                }
                if (!(cbLuogoDoc.SelectedItem is DataRowView))
                {
                    MessageBox.Show("Selezionare il Luogo di Rilascio del Documento.", "Dati mancanti", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cbLuogoDoc.Focus();
                    return;
                }
            }

            // --- Salvataggio nel record ---
            recordSchedina.TipoAlloggiato   = tipoAlloggiato;
            recordSchedina.DataArrivo       = dpDataArrivo.SelectedDate.Value.ToString("dd/MM/yyyy");
            recordSchedina.GiorniPermanenza = (iudGiorniPerm.Value ?? 1).ToString();
            recordSchedina.Cognome          = tbCognome.Text;
            recordSchedina.Nome             = tbNome.Text;
            recordSchedina.Sesso            = rbMaschio.IsChecked == true ? "1" : "2";
            recordSchedina.DataNascita      = dpDataNascita.SelectedDate.Value.ToString("dd/MM/yyyy");

            if (isItalia && cbComuneNascita.SelectedItem is DataRowView comuneRow)
            {
                recordSchedina.ComuneNascita    = ((string)comuneRow["Codice"]).Trim();
                recordSchedina.ProvinciaNascita = tbProvinciaNascita.Text.Trim();
            }
            else
            {
                recordSchedina.ComuneNascita    = "";
                recordSchedina.ProvinciaNascita = "";
            }

            recordSchedina.StatoNascita  = ((string)statoNascitaRow["Codice"]).Trim();
            recordSchedina.Cittadinanza  = ((string)((DataRowView)cbStatoCittadinanza.SelectedItem)["Codice"]).Trim();

            if (haDoc)
            {
                recordSchedina.TipoDoc          = ((string)((DataRowView)cbTipoDocumento.SelectedItem)["Codice"]).Trim();
                recordSchedina.NumeroDoc        = tbNumDoc.Text;
                recordSchedina.LuogoRilascioDoc = ((string)((DataRowView)cbLuogoDoc.SelectedItem)["Codice"]).Trim();
            }
            else
            {
                recordSchedina.TipoDoc          = "";
                recordSchedina.NumeroDoc        = "";
                recordSchedina.LuogoRilascioDoc = "";
            }

            DialogResult = true;
        }
    }

    public class RadioButtonCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}
