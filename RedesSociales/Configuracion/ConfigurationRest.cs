using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Configuration
{
    public class ConfigurationRest
    {
        #region Properties
        public readonly string NameSpaceRest;
        public Dictionary<string, string> ConfigurationVerbs { get; set; }
        #endregion Properties

        #region Initialize
        public ConfigurationRest() {
            NameSpaceRest = "RedesSociales.Servicios.Rest";
            InitializeConfigurationVerbs();
        }
        #endregion Initialize

        #region Methods
        private void InitializeConfigurationVerbs()
        {
            ConfigurationVerbs = new Dictionary<string, string>();
            ConfigurationVerbs.Add("GET", string.Concat(NameSpaceRest, ".RequestParameters`1"));
            ConfigurationVerbs.Add("DELETE", string.Concat(NameSpaceRest, ".RequestParameters`1"));
            ConfigurationVerbs.Add("POST", string.Concat(NameSpaceRest, ".RequestBody`1"));
            ConfigurationVerbs.Add("PUT", string.Concat(NameSpaceRest, ".RequestBody`1"));
        }
        #endregion Methods

    }
}
