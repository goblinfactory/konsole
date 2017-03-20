using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace Konsole.Internal
{
    // generates non  repeating useful testdata. Important for manual testing to be effective.
    public static class TestData
    {
        // some inspiration, source for names with non latin encodings.
        // https://en.wikipedia.org/wiki/List_of_the_most_common_surnames_in_Europe#Latvia


        /// <summary>
        /// generate random (unique) object names. Maximum of 4200 unique names.
        /// </summary>
        public static string[] MakeObjectNames(int howMany = 4200, string format = "{0}{1}")
        {
            if (howMany > 4200) howMany = 4200;

            // names that do not contain any special characters that windows console cannot display nicely! How wonderfully politically incorrect ;-O
            var namesets = new
            {
                Adjectives = new string[60]
                {
                    "Red", "Heavy", "Fast", "Blazing", "Deep", "Dark", "Solid", "Sharp", "Razor", "Grit",
                    "Angel", "Blue", "White","Black", "Ghost", "Blaze", "Burning", "Slippery", "Project", "Green",
                    "Orange","Slow", "Risky", "Stable","Bright", "Rough", "Smooth", "Fake", "Stub", "Mock",
                    "Gritty", "Angelic", "Whiter", "Iberian", "Russian", "Sand","Jungle", "Southern","Pantanal","Sunda",
                    "Flat", "BlackFooted", "SharpEyed","BlueEyed", "GreenEyed","African","Snow", "Desert", "Moon", "Sun",
                    "Golden", "Sea", "CalmSea", "DarkSea", "Mount", "Long", "A", "B", "X", "Y"
                },
                Modifier = new string[70] 
                {
                    "Lion", "Tiger","Mercury", "Gold", "Silver", "Adamantium","Snow", "Mouse", "Dart","Start",
                    "Cut", "Leather", ".io",".com", "ist", "y", "ly", "er", "s", "Diamond",
                    "Opal", "Quartz", "Spark", "Launch", "Apple", "Orange", "Pinapple","Pen", "Cobra", "Eagle",
                    "Fox", "Hound", "Horse", "Panther", "Lynx", "Bobcat", "Puma", "Cougar", "Leopard","Tigrina",
                    "Owl", "Falcon", "Osprey", "Hawk", "Merlin", "Kite", "Sabre", "Katana", "Rapier", "Hammer",
                    "Denali","Everest","Peaks","Rainier", "K2", "Massive","Qogir","Lhotse","Makula","Oyu",
                    "Parbat", "0","1","2","3","4","5","6","7","8",
                }
            };

            var names = from first in namesets.Adjectives
                        from last in namesets.Modifier
                        select string.Format(format, first, last);

            var shuffled = ShuffleStrings(names);
            return shuffled.Take(howMany).ToArray();
        }


        /// <summary>
        /// generate random (unique) file names. 
        /// </summary>
        public static string[] MakeFileNames(int howMany = 4200, params string[] extensions)
        {
            if (extensions.Length == 0) return new string[] {};
            int cnt = howMany/extensions.Length;
            var onames = MakeObjectNames(cnt);
            var filenames = from file in onames
                from ext in extensions
                select $"{file}.{ext}";

            var shuffled = ShuffleStrings(filenames);
            return shuffled.Take(howMany).ToArray();
        }



        /// <summary>
        /// make a set of names, formatstring {0}=firstname, {1}=lastname
        /// </summary>
        public static string[] MakeNames(int howMany = 450, string format = "{0} {1}", bool windowsFriendlyOnly = false)
        {
            if (howMany > 450) howMany = 450;

            // names that do not contain any special characters that windows console cannot display nicely! How wonderfully politically incorrect ;-O
            var windowsFriendlyNames = new
            {
                Adjectives = new[]
                {
                    "Chrudos", "Kazi", "Liana", "Sarka", "Jozefa","Gloria", "Alan", "Susan", "Cathy", "Mahakhinaoratalova",
                    "Benedetto", "Marco", "Sinzenza", "Ke", "Graham", "Chloe", "Diane", "Fred"
                },
                Modifier = new[]
                {
                    "Beridze", "Rebane", "Tsiklauri", "Angelopoulos","Jackson", "Papadopoulos", "Mac Giolla Mhuire", "O'Brien",
                    "O Ceanneidigh", "Berzins", "Vītols", "Dabrowski", "Kendriksen", "Popov", "Kuznetsov",
                    "Kaminski", "Walters", "Ferarri", "Ricci", "Baker", "Vafoozala", "Sinzenza", "Ke", "Frank",
                    "Watson"
                }
            };

            var whatTheRestOfTheWorldUses = new
            {
                Adjectives = new[]
                {
                    "Chrudoš", "Kazi", "Liana", "Šárka", "Józefa","Gloria", "Alan", "Susan", "Cathy", "Mahakhinaoratalova",
                    "Benedetto", "Marco", "Sinzenza", "Ke", "Graham", "Chloe", "Diane", "Fred"
                },
                Modifier = new[]
                {
                    "ბერიძე", "Rebane", "წიკლაური", "Αγγελόπουλος","Jackson", "Παπαδόπουλος", "Mac Giolla Mhuire", "O'Brien",
                    "Ó Ceannéidigh", "Bērziņš", "Vītols", "Dąbrowski", "Kendriksen", "Popov", "Kuznetsov",
                    "Kamiński", "Walters", "Ferarri", "Ricci", "Baker", "Vafoozala", "Sinzenza", "Ke", "Frank",
                    "Watson"
                }
            };

            var namesets = windowsFriendlyOnly ? windowsFriendlyNames : whatTheRestOfTheWorldUses;

            var names = from first in namesets.Adjectives
                from last in namesets.Modifier
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
