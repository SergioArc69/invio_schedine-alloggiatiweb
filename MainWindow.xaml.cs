using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
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
using InvioSchedineAlloggiatiWeb.AlloggiatiWeb;

namespace InvioSchedineAlloggiatiWeb
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Properties.Settings settingsDef = Properties.Settings.Default;
        ServiceSoapClient soapClientAW = null;
        TokenInfo tiToken = null;

        ObservableCollection<RecordSchedina> schedine = null;
        DataTable dtLuoghi = null;

        public MainWindow()
        {
            InitializeComponent();

            tbUsername.Text = settingsDef.Username;
            tbPassword.Text = settingsDef.Password;
            pbPassword.Password = settingsDef.Password;
            tbWsKey.Text = settingsDef.WSKey;

            dpDataRicevuta.SelectedDate = dpDataRicevuta.DisplayDateEnd = DateTime.Today.AddDays(-1);
            dpDataRicevuta.DisplayDateStart = dpDataRicevuta.SelectedDate.Value.AddMonths(-1);

            soapClientAW = new AlloggiatiWeb.ServiceSoapClient();

            schedine = new ObservableCollection<RecordSchedina>();
        }

        private void cbAbilitaModificaCredenziali_Clicked(object sender, RoutedEventArgs e)
        {
            tbUsername.IsEnabled = (bool)cbAbilitaModificaCredenziali.IsChecked;
            pbPassword.IsEnabled = (bool)cbAbilitaModificaCredenziali.IsChecked;
            tbPassword.IsEnabled = (bool)cbAbilitaModificaCredenziali.IsChecked;
            tbWsKey.IsEnabled = (bool)cbAbilitaModificaCredenziali.IsChecked;
        }

        private void cbShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            pbPassword.Visibility = System.Windows.Visibility.Collapsed;
            tbPassword.Visibility = System.Windows.Visibility.Visible;
            tbPassword.IsEnabled = pbPassword.IsEnabled;
            if (tbPassword.IsEnabled)
            {
                tbPassword.Focus();
            }            
        }

        private void cbShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            tbPassword.Visibility = System.Windows.Visibility.Collapsed;
            pbPassword.Visibility = System.Windows.Visibility.Visible;
            if (pbPassword.IsEnabled)
            {
                pbPassword.Focus();
            }
        }

        private void tbUsername_Changed(object sender, TextChangedEventArgs e)
        {
            if (!settingsDef.Username.Equals(tbUsername.Text))
            {
                settingsDef.Username = tbUsername.Text;
                settingsDef.Save();
            }
        }

        private void pbPassword_Changed(object sender, RoutedEventArgs e)
        {
            if (!settingsDef.Password.Equals(pbPassword.Password))
            {
                settingsDef.Password = pbPassword.Password;
                settingsDef.Save();
            }
        }

        private void tbWsKey_Changed(object sender, TextChangedEventArgs e)
        {
            if (!settingsDef.WSKey.Equals(tbWsKey.Text))
            {
                settingsDef.WSKey = tbWsKey.Text;
                settingsDef.Save();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            EsitoOperazioneServizio eos = new EsitoOperazioneServizio();
            tiToken = soapClientAW.GenerateToken(tbUsername.Text, pbPassword.Password, tbWsKey.Text, ref eos);
            if (eos.esito)
            {
                sbItem.Content = "WS Token generated!";
                lblToken.Content = tiToken.token;
                lblIssue.Content = tiToken.issued.ToString("G");
                lblExpiration.Content = tiToken.expires.ToString("G");

                btnCheckToken.IsEnabled = true;
                btnCheckSchedine.IsEnabled = true;
                //btnSendSchedine.IsEnabled = true;
                btnTabella.IsEnabled = true;
                btnDownloadRicevuta.IsEnabled = true;
                dpDataRicevuta.IsEnabled = true;
            }
            else
            {
                sbItem.Content = string.Format("GenerateToken Error: {0}", eos.ErroreDes);

                lblToken.Content = "----";
                lblIssue.Content = lblExpiration.Content = "--/--/---- --:--:--";

                btnCheckToken.IsEnabled = false;
                btnCheckSchedine.IsEnabled = false;
                btnSendSchedine.IsEnabled = false;
                btnTabella.IsEnabled = false;
                btnDownloadRicevuta.IsEnabled = false;
                dpDataRicevuta.IsEnabled = false;
            }
        }

        private void btnLoadSchedine_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            
            dialog.DefaultExt = "txt"; // Default file extension
            dialog.Filter = "File di testo (.txt)|*.txt|Tutti i file (*.*)|*.*"; // Filter files by extension
            if (settingsDef.CartellaSchedine.Equals(".") || !System.IO.Directory.Exists(settingsDef.CartellaSchedine))
            {
                dialog.InitialDirectory = Environment.CurrentDirectory;
            }
            else
              dialog.InitialDirectory = settingsDef.CartellaSchedine;

            dialog.CheckPathExists = true;
            dialog.CheckFileExists = true;
            dialog.DereferenceLinks = true;

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                string sPath = System.IO.Path.GetDirectoryName(dialog.FileName);
                if (!sPath.Equals(settingsDef.CartellaSchedine))
                {
                    settingsDef.CartellaSchedine = sPath;
                    settingsDef.Save();
                }

                schedine.Clear();

                //tbSchedine.Text = System.IO.File.ReadAllText(dialog.FileName);
                string[] sLines = System.IO.File.ReadAllLines(dialog.FileName);
                foreach (string line in sLines)
                {
                    RecordSchedina rs = new RecordSchedina(line);
                    schedine.Add(rs);
                }

                dgSchedine.ItemsSource = schedine;

                dgSchedine.IsReadOnly = true;
            }
        }

        private void cbEnableEdit_Checked(object sender, RoutedEventArgs e)
        {
            dgSchedine.IsReadOnly = false;
        }

        private void cbEnableEdit_Unchecked(object sender, RoutedEventArgs e)
        {
            dgSchedine.IsReadOnly = true;
        }

        private void btnCheckSchedine_Click(object sender, RoutedEventArgs e)
        {
            ElencoSchedineEsito ese = new ElencoSchedineEsito();
            ArrayOfString ES = new ArrayOfString();

            int lineCount = dgSchedine.Items.Count;

            for (int i = 0; i < lineCount; i++)
            {
                string s = ""; // dgSchedine.dGetLineText(i).TrimEnd(new char[] { '\r', '\n' });
                ES.Add(s);
            }
            EsitoOperazioneServizio res = soapClientAW.Test(tbUsername.Text, tiToken.token, ES, ref ese);
            if (res.esito)
            {
                sbItem.Content = "Test: Eseguito";

                StringBuilder testo = new StringBuilder(lineCount + 2);                

                testo.AppendLine(string.Format("Schedine valide: {0}\r\n", ese.SchedineValide));

                btnSendSchedine.IsEnabled = ese.SchedineValide == lineCount;

                int i = 0;
                foreach (var item in ese.Dettaglio)
                {
                    testo.AppendLine(string.Format("Schedina {0}: {1}", i++, item.esito ? "Ok" : item.ErroreDettaglio));
                }

                tbEsitoVerifica.Text = testo.ToString();
            }
            else
            {
                sbItem.Content = string.Format("Test: Error: {0} - {1}", res.ErroreCod, res.ErroreDes);
                btnSendSchedine.IsEnabled = false;
            }
        }

        private void btnSendSchedine_Click(object sender, RoutedEventArgs e)
        {
            ElencoSchedineEsito ese = new ElencoSchedineEsito();
            ArrayOfString ES = new ArrayOfString();
            /*
            int lineCount = tbSchedine.LineCount;

            for (int i = 0; i < lineCount; i++)
            {
                string s = tbSchedine.GetLineText(i).TrimEnd(new char[] { '\r', '\n' });
                ES.Add(s);
            }
            */
            EsitoOperazioneServizio res = soapClientAW.Send(tbUsername.Text, tiToken.token, ES, ref ese);
            if (res.esito)
            {
                sbItem.Content = "Send: Eseguito";

                //StringBuilder testo = new StringBuilder(lineCount + 2);
                StringBuilder testo = new StringBuilder( 2);

                testo.AppendLine(string.Format("Schedine acquisite: {0}\r\n", ese.SchedineValide));

                int i = 0;
                foreach (var item in ese.Dettaglio)
                {
                    testo.AppendLine(string.Format("Schedina {0}: {1}", i++, item.esito ? "Ok" : item.ErroreDettaglio));
                }

                tbEsitoVerifica.Text = testo.ToString();
            }
            else
            {
                sbItem.Content = string.Format("Send: Error: {0} - {1}", res.ErroreCod, res.ErroreDes);
            }
            btnSendSchedine.IsEnabled = false;
        }

        private void btnDownloadRicevuta_Click(object sender, RoutedEventArgs e)
        {
            byte[] pdf = null;

            DateTime dtDate = dpDataRicevuta.SelectedDate.HasValue ? dpDataRicevuta.SelectedDate.Value : DateTime.Today;

            EsitoOperazioneServizio eos =  soapClientAW.Ricevuta(tbUsername.Text, tiToken.token, dtDate, ref pdf);
            if (eos.esito)
            {
                sbItem.Content = "Download Ricevuta: Ok";

                // Configure open file dialog box
                var dialog = new Microsoft.Win32.SaveFileDialog();

                dialog.Title = "Salva Ricevuta in PDF";

                dialog.FileName = string.Format("Ricevuta_{0}.pdf", dtDate.ToString("yyyyMMdd"));
                dialog.DefaultExt = "pdf"; // Default file extension
                dialog.Filter = "File PDF (.pdf)|*.pdf|Tutti i file (*.*)|*.*"; // Filter files by extension
                if (settingsDef.CartellaRicevute.Equals(".") || !System.IO.Directory.Exists(settingsDef.CartellaRicevute))
                {
                    dialog.InitialDirectory = Environment.CurrentDirectory;
                }
                else
                    dialog.InitialDirectory = settingsDef.CartellaRicevute;

                dialog.CheckPathExists = true;
                dialog.CheckFileExists = false;
                dialog.DereferenceLinks = true;

                // Show open file dialog box
                bool? result = dialog.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    string sPath = System.IO.Path.GetDirectoryName(dialog.FileName);
                    if (!sPath.Equals(settingsDef.CartellaRicevute))
                    {
                        settingsDef.CartellaRicevute = sPath;
                        settingsDef.Save();
                    }

                    try
                    {
                        System.IO.File.WriteAllBytes(dialog.FileName, pdf);
                        sbItem.Content = "Download Ricevuta: Ok  -  PDF salvato in " + dialog.FileName;
                    }
                    catch { }
                    
                }
            }
            else
            {
                sbItem.Content = string.Format("DownloadRicevuta Error: {0}", eos.ErroreDes);
            }
        }

        private void tbSchedine_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblNumSchedine.Content = string.Format("Tot. schedine: {0}", "xxx"); //dgSchedine.LineCount
        }

        private void btnCheckToken_Click(object sender, RoutedEventArgs e)
        {
            EsitoOperazioneServizio eos = soapClientAW.Authentication_Test(tbUsername.Text, tiToken.token);
            if (eos.esito)
            {
                sbItem.Content = "Check Token: Ok";                
            }
            else
            {
                sbItem.Content = string.Format("Check Token: Error {0}", eos.ErroreDes);
            }
        }

        public DataTable ConvertCSVtoDataTable(string csv)
        {
            DataTable dt = new DataTable();

            string[] csvLines = csv.Split('\n');

            string[] headers = csvLines[0].Split(';');
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }

            for  (int r = 1; r < csvLines.Count(); r++ )
            {
                string row = csvLines[r];
                if (!string.IsNullOrEmpty(row))
                {
                    string[] cells = row.Split(';');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = cells[i];
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        private void btnTabella_Click(object sender, RoutedEventArgs e)
        {
            string csv = "";
            EsitoOperazioneServizio eos = soapClientAW.Tabella(tbUsername.Text, tiToken.token, TipoTabella.Luoghi, ref csv);
            if (eos.esito)
            {
                tbEsitoVerifica.Text = csv;

                dtLuoghi = ConvertCSVtoDataTable(csv);
                sbItem.Content = String.Format("Tabella 'Luoghi': Ok (Lines={0}+Header)", tbEsitoVerifica.LineCount-1);
            }
            else
            {
                sbItem.Content = string.Format("Tabella 'Luoghi': Error {0}", eos.ErroreDes);
            }
        }
    }
}
