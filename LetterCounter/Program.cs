using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            long[] ms = new long[3];
            for (int i = 0; i < 3; i++)
            {
                var file = File.OpenText(@"songz.txt");
                Stopwatch sw = new Stopwatch();
                sw.Start();
                for (int k = 0; k < 1000; k++)
                {
                    file.BaseStream.Position = 0;
                    switch (i)
                    {
                        case 0:
                            OldMethod(file);
                            break;
                        case 1:
                            NewMethod(file);
                            break;
                        case 2:
                            NewMethod2(file);
                            break;
                        default:
                            break;
                    }
                }
                sw.Stop();
                file.Close();
                ms[i] = sw.ElapsedMilliseconds;

            }
            foreach (var item in ms)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }

        private static void NewMethod2(StreamReader file)
        {
            var input = file.ReadToEnd();
            var freq = new int[26];
            for (var i = 0; i < 26; i++) freq[i] = 0;
            foreach (var x in input.ToLower())
            {
                if (x >= 'a' && x <= 'z') freq[x - 'a']++;
            }
            var max = new int[10];
            for (int i = 0; i < freq.Length; i++)
            {
                int count = freq[i];
                SetMax(i, count, 0, 4, max);
            }
            for (int i = 0; i < 5; i++)
            {
                Console.Write((char)(max[i] + 'a'));
            }

        }

        private static void SetMax(int ch, int count, int left, int right, int[] max)
        {
            int mchar;
            int mcount;

            if (left > 4 || right > 4)
            {
                return;
            }

            if (max[left + 5] == 0)
            {
                max[left] = ch;
                max[left + 5] = count;
                return;
            }
            if (max[right + 5] >= count)
            {
                return;
            }
            while (left < right)
            {
                int mid = left + ((right - left) >> 1);
                mcount = max[mid + 5];
                if (count > mcount)
                {
                    right = mid;
                }
                else
                {
                    left = mid + 1;
                }
            }

            mcount = max[left + 5];
            if (mcount != 0 && left != 4)
            {
                int i;
                for (i = left + 1; i < 4; i++)
                {
                    if (max[i + 5] == 0)
                    {
                        break;
                    }
                }
                for (int to = i, to_j = to + 5; to > left; )
                {
                    max[to] = max[--to];
                    max[to_j] = max[--to_j];
                }


            }
            max[left] = ch;
            max[left + 5] = count;

        }

        private static void NewMethod(StreamReader file)
        {
            var input = file.ReadToEnd();
            var chars = input.ToLower().ToCharArray();
            var filtered = chars.Where(x => x >= 'a' && x <= 'z');
            var result = filtered.GroupBy(x => x);


            foreach (var pair in result.OrderByDescending(gr => gr.Count()).Take(5))
            {
                Console.Write(pair.Key);
            }
        }

        private static void OldMethod(StreamReader file)
        {
            var input = file.ReadToEnd();
            var freq = new int[26];
            for (var i = 0; i < 26; i++) freq[i] = 0;
            foreach (var x in input.ToLower())
            {
                if (x >= 'a' && x <= 'z') freq[x - 'a']++;
            }
            for (int k = 0; k < 5; k++)
            {
                int n = 0;
                for (int i = 1; i < 26; i++)
                {
                    if (freq[i] > freq[n]) n = i;
                }
                Console.Write((char)('a' + n));
                freq[n] = 0;
            }
        }
    }
}
