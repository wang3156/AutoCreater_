using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommLibrary
{
    using System.Collections.Specialized;
    using System.Configuration;
    public class ConfigHelper
    {
        public static string GetConfing(string key, ConfigType type = ConfigType.Connection)
        {

            if (type == ConfigType.Connection)
            {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            else
            {
                return ConfigurationManager.AppSettings[key];
            }

        }

    }

    public enum ConfigType
    {
        AppConfig,
        Connection

    }
}
