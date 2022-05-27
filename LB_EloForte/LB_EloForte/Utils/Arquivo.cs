using System;
using System.IO;

namespace LB_EloForte.Utils
{
    public static class Arquivo
    {
        static string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp.txt");
        public static string GetValues()
        {
            if (File.Exists(fileName))
            {
                string[] s = File.ReadAllLines(fileName);
                if (s.Length.Equals(2))
                    return s[0] + "|" + s[1];
                else return null;
            }
            else return null;
        }
        public static void SetValues(string login, string senha, bool Lembrar = true)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
            if (Lembrar)
                File.WriteAllLines(fileName, new string[] { login, senha });
        }
        public static void DeleteFile()
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}
