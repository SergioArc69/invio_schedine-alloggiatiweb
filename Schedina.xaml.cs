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

        RecordSchedina recordSchedina = null;

        public Schedina()
        {
            InitializeComponent();
        }

        internal void SetRecord(RecordSchedina rs)
        {
            recordSchedina = rs;

            DataRowView selRow = cbTipoAlloggiato.Items.OfType<DataRowView>().Single(r => (string)r["Codice"] == rs.TipoAlloggiato);
            cbTipoAlloggiato.SelectedItem = selRow;

            dpDataArrivo.SelectedDate = DateTime.ParseExact(rs.DataArrivo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime ieri = DateTime.Today.AddDays(-1);
            dpDataArrivo.DisplayDateStart = dpDataArrivo.SelectedDate < ieri ? dpDataArrivo.SelectedDate : ieri;
            
            iudGiorniPerm.Value = int.Parse(rs.GiorniPermanenza);

            tbCognome.Text = rs.Cognome.Trim();

            tbNome.Text = rs.Nome.Trim();

            if (rs.Sesso == "1")
            {
                rbMaschio.IsChecked = true;
            }
            else
            {
                rbFemmina.IsChecked = true;
            }
            if (dpDataNascita.DisplayDateEnd.HasValue)
            {
                dpDataNascita.DisplayDateStart = dpDataNascita.DisplayDateEnd.Value.AddYears(-120);
            }
            dpDataNascita.SelectedDate = DateTime.ParseExact(rs.DataNascita, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            selRow = cbStatoNascita.Items.OfType<DataRowView>().Single(r => (string)r["Codice"] == rs.StatoNascita);
            cbStatoNascita.SelectedItem = selRow;

            /*
            if (selRow.Row.Field<string>("Descrizione").ToUpper().Equals("ITALIA"))
            {
                selRow = cbComuneNascita.Items.OfType<DataRowView>().Single(r => (string)r["Codice"] == rs.ComuneNascita);
                cbComuneNascita.SelectedItem = selRow;
                cbComuneNascita.IsReadOnly = false;
                
            }
            else
            {
                cbComuneNascita.Text = "         ";
                cbComuneNascita.IsReadOnly = true;
                tbProvinciaNascita.Text = "  ";
            }
            */

            selRow = cbStatoCittadinanza.Items.OfType<DataRowView>().Single(r => (string)r["Codice"] == rs.Cittadinanza);
            cbStatoCittadinanza.SelectedItem = selRow;
        }

        private void cbComuneNascita_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbComuneNascita.SelectedItem != null)
            {
                tbProvinciaNascita.Text = ((DataRowView)cbComuneNascita.SelectedItem).Row.Field<string>("Provincia").ToUpper();
            }
            else
            {
                tbProvinciaNascita.Text = "  ";
            }
        }

        private void cbStatoNascita_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataRowView)cbStatoNascita.SelectedItem).Row.Field<string>("Descrizione").ToUpper().Equals("ITALIA"))
            {
                DataRowView selRow = null;
                try
                {
                    selRow = cbComuneNascita.Items.OfType<DataRowView>().Single(r => (string)r["Codice"] == recordSchedina.ComuneNascita);
                    cbComuneNascita.SelectedItem = selRow;
                    cbComuneNascita.Text = selRow.Row.Field<string>("Descrizione");
                }
                catch {}                
                
                cbComuneNascita.IsReadOnly = false;
                cbComuneNascita.IsEnabled = true;
            }
            else
            {
                cbComuneNascita.IsReadOnly = true;
                cbComuneNascita.IsEnabled = false;
                cbComuneNascita.Text = "         ";
                tbProvinciaNascita.Text = "  ";
            }
        }

        private void cb_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            (cb.Template.FindName("PART_EditableTextBox", cb) as TextBox).CharacterCasing = CharacterCasing.Upper;
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
