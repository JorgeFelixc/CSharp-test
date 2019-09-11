using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

// This code is all made by Jorge Felix Cazarez (Black Hole in StackOverflow).
// Is only a test.


namespace ringba
{
    class Program
    {
        static void Main(string[] args)
        {
            
            const string URL_BASE = "https://ringba-test-html.s3-us-west-1.amazonaws.com/TestQuestions/output.txt";
            const int DIV_NUMBER = 100;
            const string abc = "abcdefghijklmnopqrstvuwxy";

            var contentParts = new List<string>();
            var prefixDictionary = new Dictionary<string, int>();
            // Tell me if have a list with prefixes, well with this will work.
            #region initialize prefixes dictionary
            prefixDictionary.Add("An", 0);
            prefixDictionary.Add("Ab", 0);
            prefixDictionary.Add("As", 0);
            prefixDictionary.Add("Anti", 0);
            prefixDictionary.Add("Auto", 0);
            prefixDictionary.Add("Ben", 0);
            prefixDictionary.Add("Bi", 0);
            prefixDictionary.Add("Com", 0);
            prefixDictionary.Add("Con", 0);
            prefixDictionary.Add("Contra", 0);
            prefixDictionary.Add("Counter", 0);
            prefixDictionary.Add("De", 0);
            prefixDictionary.Add("Dis", 0);
            prefixDictionary.Add("Di", 0);
            prefixDictionary.Add("Eu", 0);
            prefixDictionary.Add("Fore", 0);
            prefixDictionary.Add("Extra", 0);
            prefixDictionary.Add("Extro", 0);
            prefixDictionary.Add("Exo", 0);
            prefixDictionary.Add("Hyper", 0);
            prefixDictionary.Add("Hypo", 0);
            prefixDictionary.Add("Inter", 0);
            prefixDictionary.Add("Intra", 0);
            prefixDictionary.Add("Mal", 0);
            prefixDictionary.Add("Macro", 0);
            prefixDictionary.Add("Micro", 0);
            prefixDictionary.Add("Multi", 0);
            prefixDictionary.Add("Mono", 0);
            prefixDictionary.Add("Non", 0);
            prefixDictionary.Add("Poly", 0);
            prefixDictionary.Add("Omni", 0);
            prefixDictionary.Add("Op", 0);
            prefixDictionary.Add("Ob", 0);
            prefixDictionary.Add("Post", 0);
            prefixDictionary.Add("Quad", 0);
            prefixDictionary.Add("Semi", 0);
            prefixDictionary.Add("Re", 0);
            prefixDictionary.Add("Sus", 0);
            prefixDictionary.Add("Sup", 0);
            prefixDictionary.Add("Sub", 0);
            prefixDictionary.Add("Super", 0);
            prefixDictionary.Add("Supra", 0);
            prefixDictionary.Add("Sym", 0);
            prefixDictionary.Add("Trans", 0);
            prefixDictionary.Add("Tri", 0);
            prefixDictionary.Add("Ultra", 0);
            prefixDictionary.Add("Un", 0);
            prefixDictionary.Add("Uni", 0);

            #endregion

            int upperWords = 0;
            var maxPrefixLength = prefixDictionary.Keys.Select(i => i.Length).Max();

            var abcDictionary = new Dictionary<string, int>();
            for (int i = 0; i < abc.Length; i++)
            {
                abcDictionary.Add(abc[i].ToString(), 0);
            }

            var client = new WebClient();
            var urlData = client.DownloadData(URL_BASE); //download the .txt from the url in BYTES[]

            int positionBuffer = 0;
            int iterationNumber = 0;
            for (int i = 0; i < DIV_NUMBER; i++)
            {
                //Sometimes mul dont fit in the last iteration because the inpairs and pairs numbers.
                //so with this if i get the last iteration without losses. 
                if (i == DIV_NUMBER - 1)
                {
                    iterationNumber = urlData.Length - ((urlData.Length / DIV_NUMBER) * (DIV_NUMBER - 1));
                }
                else
                {
                    iterationNumber = urlData.Length / DIV_NUMBER ;
                }

                string content = Encoding.UTF8.GetString(urlData, positionBuffer, iterationNumber);
                contentParts.Add(content);
                positionBuffer = iterationNumber * (i+1) ;
            }

            contentParts.ForEach((i) =>
            {
                var splitedContent = Regex.Split(i, @"(?<!^)(?=[A-Z])"); //Split the capitalized words
            
                //Counting letters
                for (int j = 0; j < abc.Length; j++)
                {
                    string currentLetter = abc[j].ToString();
                    abcDictionary[currentLetter] = CountLetters(i, currentLetter);
                }

                //Counting the Prefixed words
                splitedContent.ToList().ForEach((word) =>
                {
                    for (var k = maxPrefixLength; k > -1; --k)
                    {
                        if(word.Length > k){ 
                            string prefixWord = word.Substring(0, k);
                            if (prefixDictionary.Keys.Contains(prefixWord))
                            {
                                prefixDictionary[prefixWord]++;
                            }
                        }
                    }
                });
                upperWords += splitedContent.ToList().Count;
            });

            //Ordening the data to show in console.
            foreach (KeyValuePair<string, int> item in abcDictionary)
            {
                Console.WriteLine("The letter '{0}' is repeated: {1}", item.Key, item.Value);
            }

            foreach (KeyValuePair<string, int> item in prefixDictionary.OrderBy(i => i.Value))
            {
                Console.WriteLine("The prefix '{0}' is repeated: {1}", item.Key, item.Value);
                
            }

            var sortedPrefixs = prefixDictionary.OrderBy(i => i.Value);
            var firstPrefix = sortedPrefixs.ElementAt(sortedPrefixs.Count()-1);
            var secondPrefix = sortedPrefixs.ElementAt(sortedPrefixs.Count()-2);

            var abcMaxNumber = abcDictionary.Values.Max();
            var abcMaxKey = abcDictionary.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            Console.WriteLine("------------------- Results --------------------");
            Console.WriteLine("The most repeated prefix is '{0}' with {1}", firstPrefix.Key, firstPrefix.Value);
            Console.WriteLine("The second repeated prefix is '{0}' with {1}", secondPrefix.Key, secondPrefix.Value);

            Console.WriteLine("The number of capitalized words are {0}", upperWords);
            Console.WriteLine("The max letter repeated is '{0}' with {1}", abcMaxKey, abcMaxNumber);
        }

        //Gets the number of letters in a string.
        static int CountLetters(string word, string letter)
        {
            return word.ToCharArray().Where(i => i.ToString() == letter).Count();
        }

    }
}
