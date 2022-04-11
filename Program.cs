using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjeDeneme
{

    class Program
    {
        public static double[,] Noktaci(double yukseklik,double genislik, int n)//Yükseklik ve genişlik aralığında rastgele n tane nokta üretir
        {
            double x=yukseklik;
            double y=genislik;
            Random r = new Random();
            double[,] matris = new double[n, 2];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    x = r.NextDouble() * yukseklik;
                    y = r.NextDouble() * genislik;

                    if (j == 0)
                    {
                        matris[i, j] = x;//noktaların x değişkenini tutar
                    }
                    else
                    {
                        matris[i, j] = y;//noktaların y değişkenini tutar
                    }
                }
            }
            return matris;//üretilen noktaları matris içinde döndürür
        }
        public static void Noktayazdir(double[,] matris,int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (j == 0)
                    {
                        if (i<9)
                            Console.Write(" x" + (i + 1) + "= " + String.Format("{0,6:0.00}", matris[i, j]));
                        else
                            Console.Write("x" + (i + 1) + "= " + String.Format("{0,6:0.00}", matris[i, j]));
                    }
                    else
                    {
                        Console.WriteLine("| y" + (i + 1) + "= " + String.Format("{0,6:0.00}", matris[i, j]));
                    }
                }
            }
        }
        public static void Matrisyaz(double[,] matris,int noktasayı)//Üretilen uzaklıkları ekrana yazdırır
        {
            double[,] a = Uzaklikbul(matris, noktasayı);
            Console.WriteLine(String.Format("{0,52}", "-----Distance Matrix-----"));
            Console.Write("  ");
            for (int i = 0; i < noktasayı; i++)
            {
                Console.Write(String.Format("|{0,6}|", i));
            }
            for (int i = 0; i < noktasayı; i++)
            {
                Console.WriteLine();
                Console.Write(String.Format("{0,2}",i));
                for (int j = 0; j < noktasayı; j++)
                {
                    Console.Write(String.Format("|{0,6:0.00}|", (a[i, j])));
                }
            }
        }
        public static double[,] Uzaklikbul(double [,]matris,int n)//x ve y koordinatlarını alıp noktalar arası uzaklığı hesaplar
        {
            double[] xler=new double[n];
            double[] yler=new double[n];
            double[,] matris2 = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    if (j == 0)
                    {
                        xler[i] = matris[i, j];//xler dizisine x kordinatları atılır

                    }
                    else
                    {
                        yler[i] = matris[i, j];//yler dizisine y kordinatları atılır
                    }
                }
            }
            for (int x1 = 0; x1< n; x1++)//noktalar arası uzaklıklar hesaplanır
            {

                for (int x2 = 0; x2 < n; x2++)
                {
                    double degerx1 ;
                    double degerx2 ;
                    double degery1 ;
                    double degery2 ;
                    degerx1 = xler[x1];
                    degerx2 = xler[x2];
                    degery1 = yler[x1];
                    degery2 = yler[x2];
                    double uzaklık = Math.Sqrt(Math.Pow((degerx1 - degerx2),2)+Math.Pow((degery1-degery2),2));
                    matris2[x1, x2] = uzaklık;//uzaklıklar matris2 içinde tutulur
                }
            }
            return matris2; 
        }
        static void Main(string[] args)
        {
            Console.Write("Yükseklik değerini giriniz: ");
            double yukseklik=Convert.ToDouble(Console.ReadLine().Replace(".",","));
            Console.Write("\nGenişlik değerini giriniz: ");
            double genislik = Convert.ToDouble(Console.ReadLine().Replace(".", ","));
            Console.Write("\nNokta sayısı giriniz: ");
            int noktasayı = Convert.ToInt32(Console.ReadLine());
            double[,] matris = Noktaci(yukseklik, genislik, noktasayı);
            Noktayazdir(matris, noktasayı);
            Matrisyaz(matris, noktasayı);      
            Console.ReadKey();
        }
    }
}
           

