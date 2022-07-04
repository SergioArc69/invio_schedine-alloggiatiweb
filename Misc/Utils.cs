﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvioSchedineAlloggiatiWeb.Misc
{
    public class Utils
    {

        /// <summary>
        /// LoadCSVintoDataTable
        /// </summary>
        /// <param name="csv">the whole csv to load</param>
        /// <returns>a DataTable instance</returns>
        /// <remarks>
        /// Lines terminator is: '\n',
        ///   Values separator is: ';',
        ///   First line is managed as Header
        /// </remarks>
        public static DataTable LoadCSVintoDataTable(string csv)
        {
            DataTable dt = new DataTable();

            string[] csvLines = csv.Split('\n');

            string[] headers = csvLines[0].Split(';');
            foreach (string header in headers)
            {
                DataColumn dc = dt.Columns.Add(header);

                if (header.Contains("Data"))
                {
                    dc.DataType = Type.GetType("System.DateTime");
                }
            }

            for (int r = 1; r < csvLines.Count(); r++)
            {
                string row = csvLines[r];
                if (!string.IsNullOrEmpty(row))
                {
                    string[] cells = row.Split(';');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        if (headers[i].Contains("Data"))
                        {
                            if (string.IsNullOrEmpty(cells[i]))
                            {
                                dr[i] = DBNull.Value;
                            }
                            else
                                dr[i] = DateTime.Parse(cells[i]);
                        }
                        else
                            dr[i] = cells[i];
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
    }
}
