using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warpiris_CSharpQUIZ
{
    class Program
    {
        private const int tankCapacity = 9;
        private const int stockTankCount = 30; 
        public static int[] stockTanks = new int[stockTankCount]; 

        static void Main(string[] args)
        {
            #region dummy data
            string[] gelenVagonlar = new string[2];
            gelenVagonlar[0] = "10808313931813319430761116496";
            gelenVagonlar[1] = "93876532983858416774152932536";

            string depoDurum = "9#54134427902231984111412732221";
            #endregion

            string result = AntrepoYerlestir(depoDurum, gelenVagonlar);

            Console.WriteLine(result);
            Console.ReadKey();
        }



        //main function
        public static string AntrepoYerlestir(string depoDurum, string[] gelenVagonlar)
        {
            //Başlangıç paketleri ayarlanıyor
            int[] depoDurumArray = intParser(depoDurum);
            depoDurumSetstockTanks(depoDurumArray);

            for (int i = 0; i < gelenVagonlar.Length; i++)
            {
                int[] packetArray = intParser(gelenVagonlar[i]);

                // p. Vagon packetCount adet pakete sahip
                // p. Vagondaki paketler öncelikle p. stockTanks'a yüklenecek
                // p. stockTank müsait değilse sonraki ilk müsait tanka yükleme yapılır
                for (int p = 0; p < packetArray.Length; p++)
                {
                    int packetCount = packetArray[p];

                    for (int k = 0; k < packetCount; k++)
                    {
                        //Öncelik(karşı) tank müsaitlik durumu;
                        if (stockTanks[p] < tankCapacity)
                        {
                            stockTanks[p] += 1;
                        }
                        //Karşı tank müsait değilse;
                        else if (stockTanks[p] == tankCapacity)
                        {

                            //Sonraki ilk müsait tank bulunur, yükleme yapılır
                            int availableIndex = findAvailableTankIndex(p);
                            stockTanks[availableIndex] += 1;

                            //Karşısındaki tank boşaltılır
                            stockTanks[p] = 0;

                        }

                    }

                }
            }

            string result = stockTanksStatusWriter();
            return result;
        }


        //operation functions
        public static int[] intParser(string text)
        {
            text = text.Replace("#", "");
            int textLength = text.Length;

            int[] dizi = new int[textLength];

            for (int i = 0; i < textLength; i++)
            {
                dizi[((textLength - 1) - i)] = text[i] - '0';
            }

            return dizi;
        }

        public static void depoDurumSetstockTanks(int[] depoDurumArray)
        {
            Array.Copy(depoDurumArray, stockTanks, Math.Min(depoDurumArray.Length, stockTanks.Length));
        }

        public static int findAvailableTankIndex(int minIndis)
        {
            for (int m = minIndis + 1; m < stockTanks.Length; m++)
            {
                if (stockTanks[m] < tankCapacity)
                {
                    return m;
                }
            }
            return stockTanks.Length - 1;
        }

        public static string stockTanksStatusWriter()
        {

            Array.Reverse(stockTanks);
            string stockText = String.Join("-", stockTanks);

            int firstSpaceIndex = stockText.IndexOf("-");

            StringBuilder st = new StringBuilder(stockText);
            st.Insert(firstSpaceIndex, "#");
            string result = st.ToString().Replace("-", "");

            return result;

        }
    }
}
