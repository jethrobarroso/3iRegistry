using _3iRegistry.DAL;
using Microsoft.Win32;
using QuickHash.Gen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _3iRegistry.WPF.Services
{
    public class FdTokenFinder : ITokenFinder
    {
        public string GetToken()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "Token file (.tkn)|*.tkn"
            };

            if (ofd.ShowDialog() == true)
                return ofd.FileName;
            return string.Empty;
        }
    }
}
