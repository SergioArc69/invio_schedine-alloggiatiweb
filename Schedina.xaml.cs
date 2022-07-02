﻿using InvioSchedineAlloggiatiWeb.Models;
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
        public Schedina()
        {
            InitializeComponent();
        }

        internal void SetRecord(RecordSchedina rs)
        {
            DataRowView selRow = cbTipoAlloggiato.Items.OfType<DataRowView>().Single(e => (string)e["Codice"] == rs.TipoAlloggiato);
            cbTipoAlloggiato.SelectedItem = selRow;
            dpDataArrivo.SelectedDate = DateTime.ParseExact(rs.DataArrivo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime ieri = DateTime.Today.AddDays(-1);
            dpDataArrivo.DisplayDateStart = dpDataArrivo.SelectedDate < ieri ? dpDataArrivo.SelectedDate : ieri;
            iudGiorniPerm.Value = int.Parse(rs.GiorniPermanenza);
        }
    }
}