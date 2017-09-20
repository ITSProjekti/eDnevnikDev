using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDnevnikDev.Tests.helpers
{
    internal static class Helper
    {

        public static string checkJsonJArray(string jsonString)
        {

            string jsonModified = jsonString;
            if (jsonString.Substring(0, 1) == "{" && jsonString.Substring(jsonString.Length - 1, 1) == "}")
            {
                jsonModified = jsonString.Insert(0, "[");
                jsonModified = jsonModified.Insert(jsonModified.Length, "]");
            }
            return jsonModified;
        }
    }
}
