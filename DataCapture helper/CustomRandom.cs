using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_capture.DataCapture_helper
{
    public class CustomRandom
    {
        private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        //Holding 36 because Microsoft identity creates 36 chars for user ID 
        private static char[] stringChars = new char[36];

#warning Random should not be used for anything security related. RNGCryptoServiceProvider class is recommended!
        private System.Random random = new System.Random();

        public string get36()
        {

            for (int i = 0; i<stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }
         
    }
}
