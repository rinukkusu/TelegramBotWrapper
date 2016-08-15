using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;

namespace TextToSpeechPlugin
{
    public class TextToSpeech
    {
        private static string API_URL = "http://www.ispeech.org/p/generic/getaudio?text=%TEXT%&voice=eurgermanmale&speed=0&action=convert";

        private static string GetFinalUrl(string text)
        {
            return API_URL.Replace("%TEXT%", HttpUtility.UrlEncode(text));
        }

        public static byte[] GetSpeechFromText(string text)
        {
            WebClient wc = new WebClient();
            try
            {
                byte[] b = wc.DownloadData(GetFinalUrl(text));
                return b;
            }
            catch
            {
                return null;
            }
        }
    }
}
