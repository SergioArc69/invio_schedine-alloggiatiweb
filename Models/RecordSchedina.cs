using System;
using System.Runtime.InteropServices;
using System.Text;

namespace InvioSchedineAlloggiatiWeb.Models
{
    public class RecordSchedina
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1, Size = 168)]
        unsafe public struct RecSA
        {
            /// <summary>
            /// TipoAlloggiato (obbligatorio): Codice tabella TipoAlloggiati
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            private char[] _TipoAlloggiato;
            public string TipoAlloggiato
            {
                get { return new string(_TipoAlloggiato); }
                set { _TipoAlloggiato = value.ToCharArray(); }
            }

            /// <summary>
            /// DataArrivo (obbligatorio): gg/mm/aaaa
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            private char[] _DataArrivo;
            public string DataArrivo
            {
                get { return new string(_DataArrivo); }
                set { _DataArrivo = value.ToCharArray(); }
            }

            /// <summary>
            /// GiorniPermanenza (obbligatorio): min=1 - max=30
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            private char[] _GiorniPermanenza;
            public string GiorniPermanenza
            {
                get { return new string(_GiorniPermanenza); }
                set { _GiorniPermanenza = value.ToCharArray(); }
            }

            /// <summary>
            /// Cognome (obbligatorio): Uppercase &amp; blank-padded
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
            private char[] _Cognome;
            public string Cognome
            {
                get { return new string(_Cognome); }
                set { _Cognome = value.ToCharArray(); }
            }

            /// <summary>
            /// Nome (obbligatorio): Uppercase &amp; blank-padded
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            private char[] _Nome;
            public string Nome
            {
                get { return new string(_Nome); }
                set { _Nome = value.ToCharArray(); }
            }

            /// <summary>
            /// Sesso (obbligatorio): 1=M - 2=F
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            private char[] _Sesso;
            public string Sesso
            {
                get { return new string(_Sesso); }
                set { _Sesso = value.ToCharArray(); }
            }

            /// <summary>
            /// DataNascita (obbligatorio): gg/mm/aaaa
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            private char[] _DataNascita;
            public string DataNascita
            {
                get { return new string(_DataNascita); }
                set { _DataNascita = value.ToCharArray(); }
            }

            /// <summary>
            /// ComuneNascita (obbligatorio se StatoNascita==Italia): Codice tabella Comuni
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            private char[] _ComuneNascita;
            public string ComuneNascita
            {
                get { return new string(_ComuneNascita); }
                set { _ComuneNascita = value.ToCharArray(); }
            }

            /// <summary>
            /// ProvinciaNascita (obbligatorio se StatoNascita==Italia): Sigla Provincia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            private char[] _ProvinciaNascita;
            public string ProvinciaNascita
            {
                get { return new string(_ProvinciaNascita); }
                set { _ProvinciaNascita = value.ToCharArray(); }
            }

            /// <summary>
            /// StatoNascita (obbligatorio): Codice tabella Stati
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            private char[] _StatoNascita;
            public string StatoNascita
            {
                get { return new string(_StatoNascita); }
                set { _StatoNascita = value.ToCharArray(); }
            }

            /// <summary>
            /// Cittadinanza (obbligatorio): Codice tabella Stati
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            private char[] _Cittadinanza;
            public string Cittadinanza
            {
                get { return new string(_Cittadinanza); }
                set { _Cittadinanza = value.ToCharArray(); }
            }

            /// <summary>
            /// TipoDoc (obbligatorio se TipoAlloggiato in[16,17,18], riempire con blank): Codice tabella Documenti
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            private char[] _TipoDoc;
            public string TipoDoc
            {
                get { return new string(_TipoDoc); }
                set { _TipoDoc = value.ToCharArray(); }
            }

            /// <summary>
            /// NumeroDoc (obbligatorio se TipoAlloggiato in[16,17,18], riempire con blank): numero del documento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            private char[] _NumeroDoc;
            public string NumeroDoc
            {
                get { return new string(_NumeroDoc); }
                set { _NumeroDoc = value.ToCharArray(); }
            }

            /// <summary>
            /// LuogoRilascioDoc (obbligatorio se TipoAlloggiato in[16,17,18], riempire con blank): Codice tabella Comuni o Stati
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            private char[] _LuogoRilascioDoc;
            public string LuogoRilascioDoc
            {
                get { return new string(_LuogoRilascioDoc); }
                set { _LuogoRilascioDoc = value.ToCharArray(); }
            }

            public override string ToString()
            {
                return string.Format($"{TipoAlloggiato}{DataArrivo}{GiorniPermanenza}{Cognome}{Nome}{Sesso}{DataNascita}{ComuneNascita}{ProvinciaNascita}{StatoNascita}{Cittadinanza}{TipoDoc}{NumeroDoc}{LuogoRilascioDoc}");
            }
        }

        public unsafe RecSA line;

        // Padding fisso per ogni campo: garantisce la lunghezza corretta nel record a larghezza fissa
        private static string Pad(string value, int length) =>
            (value ?? string.Empty).PadRight(length).Substring(0, length);

        public string TipoAlloggiato
        {
            get { return line.TipoAlloggiato; }
            set { line.TipoAlloggiato = Pad(value, 2); }
        }
        public string DataArrivo
        {
            get { return line.DataArrivo; }
            set { line.DataArrivo = Pad(value, 10); }
        }
        public string GiorniPermanenza
        {
            get { return line.GiorniPermanenza; }
            set { line.GiorniPermanenza = Pad(value, 2); }
        }
        public string Cognome
        {
            get { return line.Cognome; }
            set { line.Cognome = Pad((value ?? "").ToUpper(), 50); }
        }
        public string Nome
        {
            get { return line.Nome; }
            set { line.Nome = Pad((value ?? "").ToUpper(), 30); }
        }
        public string Sesso
        {
            get { return line.Sesso; }
            set { line.Sesso = Pad(value, 1); }
        }
        public string DataNascita
        {
            get { return line.DataNascita; }
            set { line.DataNascita = Pad(value, 10); }
        }
        public string ComuneNascita
        {
            get { return line.ComuneNascita; }
            set { line.ComuneNascita = Pad(value, 9); }
        }
        public string ProvinciaNascita
        {
            get { return line.ProvinciaNascita; }
            set { line.ProvinciaNascita = Pad(value, 2); }
        }
        public string StatoNascita
        {
            get { return line.StatoNascita; }
            set { line.StatoNascita = Pad(value, 9); }
        }
        public string Cittadinanza
        {
            get { return line.Cittadinanza; }
            set { line.Cittadinanza = Pad(value, 9); }
        }
        public string TipoDoc
        {
            get { return line.TipoDoc; }
            set { line.TipoDoc = Pad(value, 5); }
        }
        public string NumeroDoc
        {
            get { return line.NumeroDoc; }
            set { line.NumeroDoc = Pad((value ?? "").ToUpper(), 20); }
        }
        public string LuogoRilascioDoc
        {
            get { return line.LuogoRilascioDoc; }
            set { line.LuogoRilascioDoc = Pad(value, 9); }
        }

        // Costruttore da record esistente (parsing da stringa a larghezza fissa)
        public RecordSchedina(string buffer)
        {
            IntPtr pBuf = Marshal.StringToBSTR(buffer);
            try
            {
                line = (RecSA)Marshal.PtrToStructure(pBuf, typeof(RecSA));
            }
            finally
            {
                Marshal.FreeBSTR(pBuf);
            }
        }

        // Costruttore per nuova schedina (campi inizializzati a blank)
        public RecordSchedina()
        {
            TipoAlloggiato   = "";
            DataArrivo       = DateTime.Today.ToString("dd/MM/yyyy");
            GiorniPermanenza = "1";
            Cognome          = "";
            Nome             = "";
            Sesso            = "1";
            DataNascita      = "";
            ComuneNascita    = "";
            ProvinciaNascita = "";
            StatoNascita     = "";
            Cittadinanza     = "";
            TipoDoc          = "";
            NumeroDoc        = "";
            LuogoRilascioDoc = "";
        }

        public override string ToString()
        {
            return line.ToString();
        }
    }
}
