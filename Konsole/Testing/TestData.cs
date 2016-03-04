using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Testing
{
    public static class TestData
    {
        // some inspiration, source for names with non latin encodings.
        // https://en.wikipedia.org/wiki/List_of_the_most_common_surnames_in_Europe#Latvia


        /// <summary>
        /// make a set of names, formatstring {0}=firstname, {1}=lastname
        /// </summary>
        public static string[] MakeNames(string format = "{0} {1}")
        {
            var namesets = new
            {
                Firsts = new[]
                {
                    "Chrudoš", "Kazi", "Liana", "Šárka", "Józefa", "Alan", "Susan", "Cathy", "Mahakhinaoratalova",
                    "Benedetto", "Marco", "Sinzenza", "Ke"
                },
                Lasts = new[]
                {
                    "ბერიძე", "Rebane", "წიკლაური", "Αγγελόπουλος", "Παπαδόπουλος", "Mac Giolla Mhuire", "O'Brien",
                    "Ó Ceannéidigh", "Bērziņš", "Vītols", "Dąbrowski", "Kendriksen", "Popov", "Kuznetsov",
                    " Kamiński", "Walters", "Ferarri", "Ricci", "Baker", "Vafoozala", "Sinzenza", "Ke", "Frank",
                    "Watson"
                }
            };

            var names = from first in namesets.Firsts
                from last in namesets.Lasts
                select string.Format(format, first, last);

            return names.ToArray();
        }
    }
}
