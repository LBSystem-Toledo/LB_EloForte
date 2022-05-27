using System;

namespace LB_EloForte.Services
{
    public static class Extensoes
    {
        public static string SoNumero(this object valor)
        {
            if (valor == null)
                return string.Empty;
            string ret = string.Empty;
            foreach (char c in valor.ToString().ToCharArray())
                if (char.IsNumber(c))
                    ret += c;
            return ret;
        }

        public static bool ValidaCNPJ(this string num)
        {
            num = num.SoNumero();
            if (num.SoNumero().Length != 14)
                return false;
            Int32[] n = new Int32[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int d1 = 0, d2 = 0, y = 0;
            string digitado = num.Substring(12, 2), calculado = "";

            try
            {
                for (int x = 0; x < 12; x++)
                    n[x] = Convert.ToInt32(num[x].ToString());

                y = 2;
                d1 = 0;
                d2 = 0;
                for (int x = 12; x >= 1; x--)
                {
                    d1 += n[x - 1] * y;
                    y++;
                    if (y > 9) y = 2;
                };
                d1 = 11 - (d1 % 11);
                if (d1 >= 10) d1 = 0;

                y = 3;
                d2 = d1 * 2;

                for (int x = 12; x >= 1; x--)
                {
                    d2 += n[x - 1] * y;
                    y++;
                    if (y > 9) y = 2;
                };


                d2 = 11 - (d2 % 11);
                if (d2 >= 10) d2 = 0;
                calculado = d1.ToString() + d2.ToString();


                if (calculado == digitado)
                    return true;
                else
                    return false;

            }
            catch
            {
                return false;
            };
        }
        public static string FormatarCNPJ(this string num)
        {
            if (num.SoNumero().Length.Equals(14))
            {
                string aux = num.SoNumero();
                return aux.Substring(0, 2) + "." +
                    aux.Substring(2, 3) + "." +
                    aux.Substring(5, 3) + "/" +
                    aux.Substring(8, 4) + "-" +
                    aux.Substring(12, 2);
            }
            else return num;
        }
        public static bool ValidaCPF(this string num)
        {
            num = num.SoNumero();
            if (num.SoNumero().Length != 11)
                return false;
            Int32[] n = new Int32[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int d1 = 0, d2 = 0, y = 0;
            string digitado = num.Substring(9, 2), calculado = "";

            try
            {
                for (int x = 0; x < 9; x++)
                    n[x] = Convert.ToInt32(num[x].ToString());

                y = 2;
                for (int x = 9; x >= 1; x--)
                {
                    d1 += n[x - 1] * y;
                    y++;
                };
                d1 = 11 - (d1 % 11);
                if (d1 >= 10) d1 = 0;

                y = 3;
                for (int x = 9; x >= 1; x--)
                {
                    d2 += n[x - 1] * y;
                    y++;
                };

                d2 = d1 * 2 + d2;
                d2 = 11 - (d2 % 11);
                if (d2 >= 10) d2 = 0;
                calculado = d1.ToString() + d2.ToString();
                if (calculado == digitado)
                    return true;
                else
                    return false;

            }
            catch
            {
                return false;
            }
        }
        public static string FormatarCPF(this string num)
        {
            if (num.SoNumero().Length.Equals(11))
            {
                string aux = num.SoNumero();
                return aux.Substring(0, 3) + "." +
                    aux.Substring(3, 3) + "." +
                    aux.Substring(6, 3) + "-" +
                    aux.Substring(9, 2);
            }
            else return num;
        }
        public static string FormatarFone(this string num)
        {
            if (num.SoNumero().Length.Equals(10))
            {
                string aux = num.SoNumero();
                return "|" + aux.Substring(0, 2) + "|" +
                    aux.Substring(2, 4) + "-" +
                    aux.Substring(6, 4);
            }
            else return num;
        }
        public static string FormatarCelular(this string num)
        {
            if (num.SoNumero().Length.Equals(11))
            {
                string aux = num.SoNumero();
                return "|" + aux.Substring(0, 2) + "|" +
                    aux.Substring(2, 5) + "-" +
                    aux.Substring(7, 4);
            }
            else return num;
        }
    }
}
