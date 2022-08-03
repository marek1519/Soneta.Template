using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Soneta.Template
{
    /// <summary>
    /// Szyfrowanie tekstu
    /// </summary>
    public static class Code
    {
        /// <summary>
        /// Odszyfrowanie wcześniej zahashowanego tekstu
        /// </summary>
        /// <param name="TekstZaszyfrowany">tekst do odszyfrowania</param>
        /// <returns></returns>
        public static string Odszyfruj(this string TekstZaszyfrowany)
        {
            TekstZaszyfrowany = String.IsNullOrEmpty(TekstZaszyfrowany) ? "" : TekstZaszyfrowany;
            try
            {
                string Password = "(rbaiuef3JBI&giU#riGOR*#OGRFIUFVWI&#VFQI#E&QFIQV#I&@RIL";
                byte[] cipherBytes = Convert.FromBase64String(TekstZaszyfrowany);

                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65,
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});


                byte[] decryptedData = Decrypt(cipherBytes,
                    pdb.GetBytes(32), pdb.GetBytes(16));

                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            catch { throw new Exception(); }
        }

        /// <summary>
        /// Zaszyfrowanie tekstu
        /// </summary>
        /// <param name="TekstZaszyfrowany">tekst do zaszyfrowania</param>
        /// <returns></returns>
        public static string Zaszyfruj(this string TekstDoZaszyfr)
        {
            TekstDoZaszyfr = String.IsNullOrEmpty(TekstDoZaszyfr) ? "" : TekstDoZaszyfr;
            try
            {
                string Password = "(rbaiuef3JBI&giU#riGOR*#OGRFIUFVWI&#VFQI#E&QFIQV#I&@RIL";
                byte[] clearBytes =
                  System.Text.Encoding.Unicode.GetBytes(TekstDoZaszyfr);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

                byte[] encryptedData = Encrypt(clearBytes,
                         pdb.GetBytes(32), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            catch { throw new Exception(); }

        }

        /// <summary>
        /// Odszyfrowanie wcześniej zahashowanego tekstu
        /// </summary>
        /// <param name="TekstZaszyfrowany">tekst do odszyfrowania</param>
        /// /// <param name="Haslo">hasło uzyte podczas szyfrowania</param>
        /// <returns></returns>
        public static string Odszyfruj(this string TekstZaszyfrowany, string Haslo)
        {
            try
            {
                string Password = Haslo;
                byte[] cipherBytes = Convert.FromBase64String(TekstZaszyfrowany);

                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65,
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});


                byte[] decryptedData = Decrypt(cipherBytes,
                    pdb.GetBytes(32), pdb.GetBytes(16));

                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            catch { return TekstZaszyfrowany; }
        }

        /// <summary>
        /// Zaszyfrowanie tekstu
        /// </summary>
        /// <param name="TekstDoZaszyfr">tekst do zaszyfrowania</param>
        /// <param name="Haslo">Dodatkowe hasło do zaszyfrowania</param>
        /// <returns></returns>
        public static string Zaszyfruj(this string TekstDoZaszyfr, string Haslo)
        {
            try
            {
                string Password = Haslo;
                byte[] clearBytes =
                  System.Text.Encoding.Unicode.GetBytes(TekstDoZaszyfr);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

                byte[] encryptedData = Encrypt(clearBytes,
                         pdb.GetBytes(32), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            catch { throw new Exception(); }
        }

        /// <summary>
        /// Szyfrowanie tekstu algorytmem MD5
        /// </summary>
        /// <param name="TekstDoZaszyfr"> Tekst do zaszyfrowania</param>
        /// <returns></returns>
        public static string ZaszyfrujAlgMD5(this string TekstDoZaszyfr)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(TekstDoZaszyfr));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();

        }

        //FUNKCJE POMOCNICZE
        private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                Rijndael alg = Rijndael.Create();
                alg.Key = Key;
                alg.IV = IV;
                CryptoStream cs = new CryptoStream(ms,
                   alg.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(clearData, 0, clearData.Length);
                cs.Close();
                byte[] encryptedData = ms.ToArray();

                return encryptedData;
            }
            catch { throw new Exception(); }
        }

        private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                Rijndael alg = Rijndael.Create();
                alg.Key = Key;
                alg.IV = IV;
                CryptoStream cs = new CryptoStream(ms,
                    alg.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(cipherData, 0, cipherData.Length);
                cs.Close();
                byte[] decryptedData = ms.ToArray();

                return decryptedData;
            }
            catch { throw new Exception(); }
        }

        /// <summary>
        /// Zamienia polskie znaki na zwykłe
        /// <para>ą -> a</para>
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ReplacePL(this string param)
        {
            string tmp = param;

            tmp = tmp.Replace("Ą", "A");
            tmp = tmp.Replace("Ć", "C");
            tmp = tmp.Replace("Ę", "E");
            tmp = tmp.Replace("Ł", "L");
            tmp = tmp.Replace("Ń", "N");
            tmp = tmp.Replace("Ó", "O");
            tmp = tmp.Replace("Ś", "S");
            tmp = tmp.Replace("Ź", "Z");
            tmp = tmp.Replace("Ż", "Z");

            tmp = tmp.Replace("ą", "a");
            tmp = tmp.Replace("ć", "c");
            tmp = tmp.Replace("ę", "e");
            tmp = tmp.Replace("ł", "l");
            tmp = tmp.Replace("ń", "n");
            tmp = tmp.Replace("ó", "o");
            tmp = tmp.Replace("ś", "s");
            tmp = tmp.Replace("ź", "z");
            tmp = tmp.Replace("ż", "z");

            return tmp;
        }

        /// <summary>
        /// Usunięcie polskich znaków z tekstu
        /// </summary>
        /// <param name="tekst"></param>
        /// <returns></returns>
        public static string RemovePL(this string tekst)
        {
            StringBuilder sb = new StringBuilder(tekst);
            sb.Replace('ą', ""[0])
              .Replace('ć', ""[0])
              .Replace('ę', ""[0])
              .Replace('ł', ""[0])
              .Replace('ń', ""[0])
              .Replace('ó', ""[0])
              .Replace('ś', ""[0])
              .Replace('ż', ""[0])
              .Replace('ź', ""[0])
              .Replace('Ą', ""[0])
              .Replace('Ć', ""[0])
              .Replace('Ę', ""[0])
              .Replace('Ł', ""[0])
              .Replace('Ń', ""[0])
              .Replace('Ó', ""[0])
              .Replace('Ś', ""[0])
              .Replace('Ż', ""[0])
              .Replace('Ź', ""[0]);

            return sb.ToString();
        }



        #region C# CODE PARSER

        /// <summary>
        /// Generowanie nazwy zmiennej na podstawie ciagu znaków
        /// <para>nowe testowe => noweTestowe</para>
        /// </summary>
        /// <param name="Nazwa"></param>
        /// <param name="checkIsDigit"></param>
        /// <returns></returns>
        public static string NazwaZmiennej(this string Nazwa, bool checkIsDigit = true)
        {
            if (String.IsNullOrEmpty(Nazwa))
                return Nazwa;

            if (Char.IsDigit(Nazwa[0]))
                throw new Exception("Nazwa powinna zaczynać sie od litery: a-z");

            string[] temps = Nazwa.Replace(".", "_").Split(' ');
            string temp = "";
            bool first = true;
            foreach (string s in temps)
            {

                if (first)
                {
                    first = false;
                    temp += s;
                }
                else
                {
                    temp += FirstToUpper(s);
                }
            }

            return FirstToLower(temp);
        }

        /// <summary>
        /// Generowanie nazwy accesora na podstawie ciagu znakow
        /// <para>nowe testowe => NoweTestowe</para>
        /// </summary>
        /// <param name="Nazwa"></param>
        /// <returns></returns>
        public static string NazwaAkcesora(this string Nazwa)
        {
            return FirstToUpper(NazwaZmiennej(Nazwa));
        }

        /// <summary>
        /// Zamiana pierwszej litery na mała
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public static string FirstToLower(this string parametr)
        {
            if (String.IsNullOrEmpty(parametr))
                return parametr;
            return parametr[0].ToString().ToLower() + parametr.Substring(1, parametr.Length - 1);

        }

        /// <summary>
        /// Zamiana pierwszej litery na wielką
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public static string FirstToUpper(this string parametr)
        {
            if (String.IsNullOrEmpty(parametr))
                return parametr;
            return parametr[0].ToString().ToUpper() + parametr.Substring(1, parametr.Length - 1);

        }

        /// <summary>
        /// Podzielenie nazwy zmiennej
        /// <para>NowaNazwa => Nowa nazwa</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PodzielNazweZmiennej(this string value)
        {
            string tmp = "";
            bool isFirst = true;
            foreach (char cr in value)
            {
                if (cr >= 'A' & cr <= 'Z')
                {
                    if (isFirst)
                    {
                        tmp += cr;
                    }
                    else
                        tmp += " " + cr.ToString().ToLower();
                }
                else
                {
                    tmp += cr;
                }

                isFirst = false;
            }
            tmp = tmp.Replace(" g u i d", "GUID");
            return tmp;
        }

        #endregion
    }
}
