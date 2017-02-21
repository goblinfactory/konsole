using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace Konsole.Internal
{
    public static class TestData
    {
        // some inspiration, source for names with non latin encodings.
        // https://en.wikipedia.org/wiki/List_of_the_most_common_surnames_in_Europe#Latvia

        /// <summary>
        /// make a set of names, formatstring {0}=firstname, {1}=lastname
        /// </summary>
        public static string[] MakeNames(int howMany = 450, string format = "{0} {1}")
        {
            if (howMany > 450) howMany = 450;
            var namesets = new
            {
                Firsts = new[]
                {
                    "Chrudoš", "Kazi", "Liana", "Šárka", "Józefa","Gloria", "Alan", "Susan", "Cathy", "Mahakhinaoratalova",
                    "Benedetto", "Marco", "Sinzenza", "Ke", "Graham", "Chloe", "Diane", "Fred"
                },
                Lasts = new[]
                {
                    "ბერიძე", "Rebane", "წიკლაური", "Αγγελόπουλος","Jackson", "Παπαδόπουλος", "Mac Giolla Mhuire", "O'Brien",
                    "Ó Ceannéidigh", "Bērziņš", "Vītols", "Dąbrowski", "Kendriksen", "Popov", "Kuznetsov",
                    " Kamiński", "Walters", "Ferarri", "Ricci", "Baker", "Vafoozala", "Sinzenza", "Ke", "Frank",
                    "Watson"
                }
            };

            var names = from first in namesets.Firsts
                from last in namesets.Lasts
                select string.Format(format, first, last);

            var shuffled = ShuffleStrings(names);
            return shuffled.Take(howMany).ToArray();
        }

        public static string[] ShuffleStrings(IEnumerable<string> src)
        {
            var from = src.ToList();
            int cnt = from.Count;
            var r = new Random();
            int left = cnt;
            var shuffled = new List<string>();
            for (int i = 0; i < cnt; i++)
            {
                int x = r.Next(left--);
                shuffled.Add(from[x]);
                from.RemoveAt(x);
            }
            return shuffled.ToArray();
        }
    }
}
