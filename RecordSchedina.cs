using System;
using System.Runtime.InteropServices;
using System.Text;

namespace InvioSchedineAlloggiatiWeb
{
    public class RecordSchedina
    {


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 168)]
        unsafe public struct RecSA
        {
            /// <summary>
            /// TipoAlloggiato (obbligatorio): Codice tabella TipoAlloggiati
            /// </summary>
            //public fixed char TipoAlloggiato[2];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            private char[] _TipoAlloggiato;
            public string TipoAlloggiato 
            {
                get {return new string(_TipoAlloggiato); }
                set { this._TipoAlloggiato = value.ToCharArray(); }
            }

            /// <summary>
            /// DataArrivo (obbligatorio): gg/mm/aaaa
            /// </summary>
            //public fixed char DataArrivo[10];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] _DataArrivo;
            public string DataArrivo
            {
                get {return new string(_DataArrivo); }
                set { this._DataArrivo = value.ToCharArray(); }
            }

            /// <summary>
            /// GiorniPermanenza (obbligatorio): min=1 - max=30
            /// </summary>
            //public fixed char GiorniPermanenza[2];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            private char[] _GiorniPermanenza;
            public string GiorniPermanenza
            {
                get {return new string(_GiorniPermanenza); }
                set { this._GiorniPermanenza = value.ToCharArray(); }
            }

            /// <summary>
            /// Cognome (obbligatorio): Uppercase & blank-padded
            /// </summary>
            //public fixed char Cognome[50];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
            private char[] _Cognome;
            public string Cognome
            {
                get {return new string(_Cognome); }
                set { this._Cognome = value.ToCharArray(); }
            }

            /// <summary>
            /// Nome (obbligatorio): Uppercase & blank-padded
            /// </summary>
            //public fixed char Nome[30];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            private char[] _Nome;
            public string Nome
            {
                get {return new string(_Nome); }
                set { this._Nome = value.ToCharArray(); }
            }

            /// <summary>
            /// Sesso (obbligatorio): 1=M - 2=F
            /// </summary>
            //public fixed char Sesso[1];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            private char[] _Sesso;
            public string Sesso
            {
                get {return new string(_Sesso); }
                set { this._Sesso = value.ToCharArray(); }
            }

            /// <summary>
            /// DataNascita (obbligatorio): gg/mm/aaaa
            /// </summary>
            //public fixed char DataNascita[10];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            private char[] _DataNascita;
            public string DataNascita
            {
                get {return new string(_DataNascita); }
                set { this._DataNascita = value.ToCharArray(); }
            }

            /// <summary>
            /// ComuneNascita (obbligatorio se StatoNascita==Italia): Codice tabella Comuni
            /// </summary>
            //public fixed char ComuneNascita[9];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            private char[] _ComuneNascita;
            public string ComuneNascita
            {
                get {return new string(_ComuneNascita); }
                set { this._ComuneNascita = value.ToCharArray(); }
            }

            /// <summary>
            /// ProvinciaNascita (obbligatorio se StatoNascita==Italia): Sigla Provincia
            /// </summary>
            //public fixed char ProvinciaNascita[2];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            private char[] _ProvinciaNascita;
            public string ProvinciaNascita
            {
                get { return new string(_ProvinciaNascita); }
                set { this._ProvinciaNascita = value.ToCharArray(); }
            }

            /// <summary>
            /// StatoNascita (obbligatorio): Codice tabella Stati
            /// </summary>
            //public fixed char StatoNascita[9];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            private char[] _StatoNascita;
            public string StatoNascita
            {
                get { return new string(_StatoNascita); }
                set { this._StatoNascita = value.ToCharArray(); }
            }

            /// <summary>
            /// Cittadinanza (obbligatorio): Codice tabella Stati
            /// </summary>
            //public fixed char Cittadinanza[9];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            private char[] _Cittadinanza;
            public string Cittadinanza
            {
                get { return new string(_Cittadinanza); }
                set { this._Cittadinanza = value.ToCharArray(); }
            }

            /// <summary>
            /// TipoDoc (obbligatorio se TipoAlloggiato in[16,17,18], riempire con blank): Codice tabella Documenti
            /// </summary>
            //public fixed char TipoDoc[5];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            private char[] _TipoDoc;
            public string TipoDoc
            {
                get { return new string(_TipoDoc); }
                set { this._TipoDoc = value.ToCharArray(); }
            }

            /// <summary>
            /// NumeroDoc (obbligatorio se TipoAlloggiato in[16,17,18], riempire con blank):  numero del documento
            /// </summary>
            //public fixed char NumeroDoc[20];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            private char[] _NumeroDoc;
            public string NumeroDoc
            {
                get { return new string(_NumeroDoc); }
                set { this._NumeroDoc = value.ToCharArray(); }
            }

            /// <summary>
            /// LuogoRilascioDoc (obbligatorio se TipoAlloggiato in[16,17,18], riempire con blank): Codice tabella Comuni o Stati
            /// </summary>
            //public fixed char LuogoRilascioDoc[9];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            private char[] _LuogoRilascioDoc;
            public string LuogoRilascioDoc
            { 
                get { return new string(_LuogoRilascioDoc); }
                set { this._LuogoRilascioDoc = value.ToCharArray(); }
            }
        }

        public unsafe RecSA line;

        public string TipoAlloggiato { get { return line.TipoAlloggiato; } }
        public string DataArrivo { get { return line.DataArrivo; } }
        public string GiorniPermanenza { get { return line.GiorniPermanenza; } }
        public string Cognome { get { return line.Cognome; } }
        public string Nome { get { return line.Nome; } }
        public string Sesso { get { return line.Sesso; } }
        public string DataNascita { get { return line.DataNascita; } }
        public string ComuneNascita { get { return line.ComuneNascita; } }
        public string ProvinciaNascita { get { return line.ProvinciaNascita; } }
        public string StatoNascita { get { return line.StatoNascita; } }
        public string Cittadinanza { get { return line.Cittadinanza; } }
        public string TipoDoc { get { return line.TipoDoc; } }
        public string NumeroDoc { get { return line.NumeroDoc; } }
        public string LuogoRilascioDoc { get { return line.LuogoRilascioDoc; } }

        public RecordSchedina(string buffer)
        {
            IntPtr pBuf = Marshal.StringToBSTR(buffer);
            line = (RecSA)Marshal.PtrToStructure(pBuf, typeof(RecSA));
        }

        public override string ToString()
        {
            return line.ToString();            
        }

    }
}
