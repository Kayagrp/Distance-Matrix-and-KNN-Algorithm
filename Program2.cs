using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proje1._2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("k değerini giriniz: ");
            int k = Convert.ToInt32(Console.ReadLine());
            double gVaryans, gÇarpıklık, gBasıklık, gEntropi;
            Console.WriteLine("Varyans değerini giriniz: ");
            gVaryans = Convert.ToDouble(Console.ReadLine().Replace('.', ','));
            Console.WriteLine("Çarpıklık değerini giriniz: ");
            gÇarpıklık = Convert.ToDouble(Console.ReadLine().Replace('.', ','));
            Console.WriteLine("Basıklık değerini giriniz: ");
            gBasıklık = Convert.ToDouble(Console.ReadLine().Replace('.', ','));
            Console.WriteLine("Entropi değerini giriniz: ");
            gEntropi = Convert.ToDouble(Console.ReadLine().Replace('.', ','));
            GirilenPara girilenPara = new GirilenPara(gVaryans, gÇarpıklık, gBasıklık, gEntropi);
            KNNYazdır(k, girilenPara, ParaAtama());
            Console.WriteLine("******************* Başarı Ölçümü *******************");
            List<GirilenPara> ikiYüzlükListe = TestParacı().ToList();
            for (int i = 0; i < ikiYüzlükListe.Count; i++)
            {
                KNNYazdır(k, ikiYüzlükListe[i], ListDöndür());
            }
            Console.WriteLine("******************* Gerçek/Tahmini Sınıf Karşılaştırma*******************");
            TestParaKNN(k);

            Console.WriteLine("******************* Veriseti Listeleme *******************");
            List<Para> paraListesi = ParaAtama();
            Console.WriteLine("|  Varyans | Çarpıklık| Basıklık |  Entropi |    Tür   |");
            for (int i = 0; i < paraListesi.Count; i++)
            {
                Console.WriteLine(paraListesi[i].ToString());
            }
            Console.ReadKey();
        }
        public static List<Para> ParaAtama() // Verisetindeki değerleri, oluşturulan Para nesnelerine atayıp bunları bir liste içinde döndüren metod
        {
            string dosya_yolu = @"C:\Users\emrek\Desktop\veriler.txt";
            string metin = System.IO.File.ReadAllText(dosya_yolu); // Dosya okunur
            string[] paraOzellik = metin.Split(',', '\n'); // Metin, virgüllerden parçalanıp bir dizide tutulur
            List<Para> paralar = new List<Para>(); // Oluşturulacak nesneleri tutması için bir liste tanımlanır
            int j = 0;
            for (int i = 0; i < paraOzellik.Length / 5; i++)
            {
                double varyans = Convert.ToDouble(paraOzellik[j].Replace('.', ','));   // Dizideki değerler
                j++;
                double çarpıklık = Convert.ToDouble(paraOzellik[j].Replace('.', ',')); // sırasıyla bir Para nesnesine atanır
                j++;
                double basıklık = Convert.ToDouble(paraOzellik[j].Replace('.', ','));
                j++;
                double entropi = Convert.ToDouble(paraOzellik[j].Replace('.', ','));
                j++;
                double tür = Convert.ToDouble(paraOzellik[j].Replace('.', ','));
                j++;
                paralar.Add(new Para(varyans, çarpıklık, basıklık, entropi, tür)); // Nesneler listeye atanır
            }
            return paralar; // Oluşturulan Para nesnelerini tutan liste döndürülür
        }
        public static double Knn(int k, GirilenPara gpr, List<Para> pr) // KNN algoritmasını yürüten metod
        {

            double uzaklıkhesabı;
            List<Para> para_list = pr;
            double[,] uzaklıklist = new double[2, para_list.Count]; // Verisetindeki paraların, türleriyle beraber 
                                                                    // test banknotuna uzaklıklarını tutan dizi tanımlanır (Yatay bir dizi)
            for (int i = 0; i < pr.Count; i++)
            {
                uzaklıkhesabı = Math.Sqrt(Math.Pow(gpr.varyans - para_list[i].GetVaryans(), 2) +
                                          Math.Pow(gpr.çarpıklık - para_list[i].GetÇarpıklık(), 2) + // Uzaklıklar hesaplanır
                                          Math.Pow(gpr.basıklık - para_list[i].GetBasıklık(), 2) +
                                          Math.Pow(gpr.entropi - para_list[i].GetEntropi(), 2));
                uzaklıklist[0, i] = uzaklıkhesabı;  // Uzaklıklar dizinin ilk satırına
                uzaklıklist[1, i] = para_list[i].GetTür(); // o uzaklıktaki banknotun türü ise 2. satırına yerleştirilir
            }
            // Uzaklıklar dizisi bubble sort yöntemiyle sıralanır
            double temp;
            double temp2;
            Para temp3 = new Para();
            for (int i = 0; i < para_list.Count - 1; i++)
            {
                for (int j = 0; j < para_list.Count - 1; j++)
                {
                    if (uzaklıklist[0, j + 1] < uzaklıklist[0, j])
                    {
                        temp = uzaklıklist[0, j];
                        temp2 = uzaklıklist[1, j];
                        temp3 = para_list[j];
                        uzaklıklist[0, j] = uzaklıklist[0, j + 1];
                        uzaklıklist[1, j] = uzaklıklist[1, j + 1];
                        para_list[j] = para_list[j + 1];
                        uzaklıklist[0, j + 1] = temp;
                        uzaklıklist[1, j + 1] = temp2;
                        para_list[j + 1] = temp3;
                    }
                }
            }
            double[] komşuTür = new double[k];
            int sıfırsayısı = 0;
            int birsayısı = 0;
            double paratürü;
            for (int i = 0; i < k; i++) // En yakın k sayıda banknotun tür değerleri sayılır
            {
                komşuTür[i] = uzaklıklist[1, i];
                if (uzaklıklist[1, i] == 0)
                    sıfırsayısı++;
                else
                    birsayısı++;
            }
            // Maksimum sayıdaki tür değeri, test banknotunun türü olarak atanır
            if (sıfırsayısı > birsayısı)
                paratürü = 0;
            else if (birsayısı > sıfırsayısı)
                paratürü = 1;
            else
                paratürü = uzaklıklist[1, 0]; // Eğer tür sayıları eşitse en yakın banknotun türü test banknotunun türü olarak atanır

            //Para paratahmin = new Para(gpr.varyans, gpr.çarpıklık, gpr.basıklık, gpr.entropi, paratürü); // Türü tahminlenen test banknotunu tutması için bir nesne oluşturulur
            return paratürü; // Paranın türü döndürülür
        }

        public static double KNNYazdır(int k, GirilenPara gpr, List<Para> pr) // KNN algoritmasını yürüten metod
        {
            double uzaklıkhesabı;
            List<Para> para_list = pr;
            double[,] uzaklıklist = new double[2, para_list.Count]; // Verisetindeki paraların, türleriyle beraber 
                                                                    // test banknotuna uzaklıklarını tutan dizi tanımlanır (Yatay bir dizi)
            for (int i = 0; i < pr.Count; i++)
            {
                uzaklıkhesabı = Math.Sqrt(Math.Pow(gpr.varyans - para_list[i].GetVaryans(), 2) +
                                          Math.Pow(gpr.çarpıklık - para_list[i].GetÇarpıklık(), 2) + // Uzaklıklar hesaplanır
                                          Math.Pow(gpr.basıklık - para_list[i].GetBasıklık(), 2) +
                                          Math.Pow(gpr.entropi - para_list[i].GetEntropi(), 2));
                uzaklıklist[0, i] = uzaklıkhesabı;  // Uzaklıklar dizinin ilk satırına
                uzaklıklist[1, i] = para_list[i].GetTür(); // o uzaklıktaki banknotun türü ise 2. satırına yerleştirilir
            }
            // Uzaklıklar dizisi bubble sort yöntemiyle sıralanır
            double temp;
            double temp2;
            Para temp3 = new Para();
            for (int i = 0; i < para_list.Count - 1; i++)
            {
                for (int j = 0; j < para_list.Count - 1; j++)
                {
                    if (uzaklıklist[0, j + 1] < uzaklıklist[0, j])
                    {
                        temp = uzaklıklist[0, j];
                        temp2 = uzaklıklist[1, j];
                        temp3 = para_list[j];
                        uzaklıklist[0, j] = uzaklıklist[0, j + 1];
                        uzaklıklist[1, j] = uzaklıklist[1, j + 1];
                        para_list[j] = para_list[j + 1];
                        uzaklıklist[0, j + 1] = temp;
                        uzaklıklist[1, j + 1] = temp2;
                        para_list[j + 1] = temp3;
                    }
                }
            }
            double[] komşuTür = new double[k];
            int sıfırsayısı = 0;
            int birsayısı = 0;
            double paratürü;
            for (int i = 0; i < k; i++) // En yakın k sayıda banknotun tür değerleri sayılır
            {
                komşuTür[i] = uzaklıklist[1, i];
                if (uzaklıklist[1, i] == 0)
                    sıfırsayısı++;
                else
                    birsayısı++;
            }
            // Maksimum sayıdaki tür değeri, test banknotunun türü olarak atanır
            if (sıfırsayısı > birsayısı)
                paratürü = 0;
            else if (birsayısı > sıfırsayısı)
                paratürü = 1;
            else
                paratürü = uzaklıklist[1, 0]; // Eğer tür sayıları eşitse en yakın banknotun türü test banknotunun türü olarak atanır

            Para paratahmin = new Para(gpr.varyans, gpr.çarpıklık, gpr.basıklık, gpr.entropi, paratürü); // Türü tahminlenen test banknotunu tutması için bir nesne oluşturulur
            Console.WriteLine("|Örnek No |" + " Varyans |" + "Çarpıklık|" + "Basıklık |" + " Entropi |" + " Uzaklık |" + "   Tür   |");
            Console.WriteLine("_______________________________________________________________________");
            for (int i = 0; i < k; i++)
            {
                Console.WriteLine(String.Format("|{0,9:0}|{1,9:0.00}|{2,9:0.00}|{3,9:0.00}|{4,9:0.00}|{5,9:0.00}|{6,9:0}|", (i + 1), para_list[i].GetVaryans(), para_list[i].GetÇarpıklık(), para_list[i].GetBasıklık(), para_list[i].GetEntropi(), uzaklıklist[0, i], para_list[i].GetTür()));
            }
            Console.WriteLine("Tahminlenen paranın türü: " + paratahmin.GetTür()); // Tahminlenen paranın türü yazdırılır
            return paratürü; // Paranın türü döndürülür
        }
        public static Para[] SonİkiYüzBulucu() // Başarı tahmini için gereken 200 test banknotunu bir dizide döndüren metod
        {
            //Para[] sonYüzSıfır = new Para[100];
            //Para[] sonYüzBir = new Para[100];
            Para[] sonİkiYüz = new Para[200];
            List<Para> list = ParaAtama(); // Verisetinden gelen paralar bir listede tutulur
            list.Reverse(); // 1 türündeki son 100 banknotu kolay ayırabilmek için liste ters çevrilir
            for (int i = 0; i < 100; i++)
            {
                sonİkiYüz[100 + i] = list[0]; // İlk 100 tane 1 türündeki banknot diziye aktarılır
                list.RemoveAt(0);       // Aynı zamanda genel listeden çıkarılır
            }
            int syc = 0;
            while (true)
            {
                if (list[syc].GetTür() == 0)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        sonİkiYüz[j] = list[syc]; // İlk 100 tane 0 türündeki banknot diziye aktarılır
                        list.RemoveAt(syc);         // Aynı zamanda genel listeden çıkarılır
                    }
                    break; // Gereken sayıda banknot alındıktan sonra döngü sonlandırılır
                }
                syc++;
            }
            return sonİkiYüz; // 200 banknottan oluşan dizi döndürülür
        }
        public static GirilenPara[] TestParacı() // Test banknotlarının türlerinden ayrılmış halini bir dizi şeklinde döndüren metod
        {
            GirilenPara[] testPara = new GirilenPara[200]; // Dizi oluşturulur
            Para[] ikiyüzlük = SonİkiYüzBulucu(); // Türleri belirli banknotlar dizide tutulur
            for (int i = 0; i < 200; i++)
            {
                // Banknotlar türleri hariç değerleri alınarak yeni GirilenPara nesneleri oluşturulur
                testPara[i] = new GirilenPara(ikiyüzlük[i].GetVaryans(), ikiyüzlük[i].GetÇarpıklık(), ikiyüzlük[i].GetBasıklık(), ikiyüzlük[i].GetEntropi());
            }
            return testPara; // Oluşan dizi döndürülür
        }
        public static void TestParaKNN(int k) // Test banknotlarını KNN ile sınıflandıran metod
        {
            GirilenPara[] testparalist = TestParacı();
            Para[] ikiyüzlük2 = SonİkiYüzBulucu();
            double syc = 0;
            for (int i = 0; i < 200; i++)
            {
                Console.WriteLine((i + 1) + ". paranın  gerçek türü: " + ikiyüzlük2[i].GetTür() + "/ Tahminlenen türü: " + Knn(k, testparalist[i], ListDöndür()));
                if (Knn(k, testparalist[i], ListDöndür()) == ikiyüzlük2[i].GetTür())
                {
                    syc++; // Doğru tahminlenen tür sayısı elde edilir
                }
            }
            double oran = syc / 200; // Oran oluşur
            Console.WriteLine("Doğru tahmin oranı: %" + oran * 100);
        }
        public static List<Para> ListDöndür() // Genel listeden 200 test banknotunun çıkmış halini döndüren metod
        {
            Para[] sonYüzSıfır = new Para[100];
            Para[] sonYüzBir = new Para[100];
            List<Para> list1 = ParaAtama();
            list1.Reverse();
            for (int i = 0; i < 100; i++)
            {
                sonYüzBir[i] = list1[0];
                list1.RemoveAt(0);
            }
            int syc = 0;
            while (true)
            {
                if (list1[syc].GetTür() == 0)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        sonYüzSıfır[j] = list1[syc];
                        list1.RemoveAt(syc);
                    }
                    break;
                }
                syc++;
            }
            return list1;
        }
        public class Para
        {
            private double varyans, çarpıklık, basıklık, entropi, tür;
            public Para()
            {
                varyans = 0;
                çarpıklık = 0;
                basıklık = 0;
                entropi = 0;
                tür = 0;
            }
            public Para(double varyans, double çarpıklık, double basıklık, double entropi, double tür)
            {
                this.varyans = varyans;
                this.çarpıklık = çarpıklık;
                this.basıklık = basıklık;
                this.entropi = entropi;
                this.tür = tür;
            }
            public double GetVaryans()
            {
                return varyans;
            }
            public double GetÇarpıklık()
            {
                return çarpıklık;
            }
            public double GetBasıklık()
            {
                return basıklık;
            }
            public double GetEntropi()
            {
                return entropi;
            }
            public double GetTür()
            {
                return tür;
            }
            override
            public string ToString()
            {
                return String.Format("|{0,10}|{1,10}|{2,10}|{3,10}|{4,10}|",GetVaryans(),GetÇarpıklık(),GetBasıklık(),GetEntropi(),GetTür());
            }
        }
        public class GirilenPara
        {
            public double varyans, çarpıklık, basıklık, entropi;
            public GirilenPara()
            {
                varyans = 0;
                çarpıklık = 0;
                basıklık = 0;
                entropi = 0;
            }
            public GirilenPara(double varyans, double çarpıklık, double basıklık, double entropi)
            {
                this.varyans = varyans;
                this.çarpıklık = çarpıklık;
                this.basıklık = basıklık;
                this.entropi = entropi;
            }
        }
    }
}