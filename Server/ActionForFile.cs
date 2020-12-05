using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class ActionForFile
    {
        public static bool IsPalindrom(string text)
        {
            var length = text.Length;

            for (int i = 0; i < length / 2; i++)
                if (text[i] != text[length - i - 1])
                    return false;

            return true;
        }
    }
}
