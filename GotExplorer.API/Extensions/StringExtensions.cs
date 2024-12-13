using GotExplorer.BLL.Services.Results;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GotExplorer.API.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveNonAsciiChars(this string str)
        {
            return Regex.Replace(str, @"[^\u001F-\u007F]+", string.Empty);
        }
    }
}
