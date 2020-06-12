using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RedesSociales.Servicios.Handler
{
    public class LoadDataHandler
    {
        public async Task<bool> LoadData(string key)
        {
            try
            {
                var secureValue = await SecureStorage.GetAsync(key);
                if (secureValue != null)
                {
                    Application.Current.Properties[key] = secureValue;
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task PersistenceDataAsync(string key, object value)
        {
            try
            {
                var stringValue = value.ToString();
                await SecureStorage.SetAsync(key, stringValue);
            }
            catch (Exception e)
            {
                throw e;
            }
            Application.Current.Properties[key] = value;
        }

        public void DeleteKey(string key)
        {
            Application.Current.Properties[key] = null;
            SecureStorage.Remove(key);
        }

        public void RemoveAll()
        {
            Application.Current.Properties.Clear();
            SecureStorage.RemoveAll();
        }
    }
}

