using System.IO;
using System.Reflection;
using Congestion.Tax.Business.Configs;
using Newtonsoft.Json;

namespace Congestion.Tax.Business.Utils
{
    public static class TaxConfigurationUtility
    {
        private static TaxRulesConfiguration _taxRulesConfiguration;
        public static TaxRulesConfiguration GetConfigs()
        {
            if (_taxRulesConfiguration == null)
            {
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "tax-rules-configurations.json");
                using StreamReader reader = new StreamReader(path);

                var json = reader.ReadToEnd();
                _taxRulesConfiguration = JsonConvert.DeserializeObject<TaxRulesConfiguration>(json);
            }

            return _taxRulesConfiguration;
        }
    }
}
