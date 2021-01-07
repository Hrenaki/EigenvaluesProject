using System;
using System.IO;
using System.Windows.Forms;

namespace EigenvaluesProject
{
    class Program
    {
        private static OpenFileDialog openDialog = new OpenFileDialog()
        {
            InitialDirectory = Environment.CurrentDirectory,
            Title = "Select file",
            DefaultExt = ".txt"
        };
        
        static void GenerateGilbert(int size)
        {
            int i, j;
            string path = Path.Combine(Environment.CurrentDirectory, $"h{size}_test.txt");
            string[] lines = new string[size];
            for(i = 0; i < size; i++)
            {
                for (j = 0; j < size; j++)
                {
                    if (j == size - 1)
                        lines[i] += (1.0 / (i + j + 1.0));
                    else lines[i] += (1.0 / (i + j + 1.0)) + " ";
                }
            }
            File.WriteAllLines(path, lines);
        }

        [STAThread]
        static void Main(string[] args)
        {
            string path = null;
            do
            {
                Console.Clear();
                if (path != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("File extention was wrong!");
                    Console.ResetColor();
                }
                Console.WriteLine("Choose .txt file with matrix");
                Console.WriteLine("Enter path or write 'ctrl + q' to open file dialog");
                path = Console.ReadLine();
                if (path[0] == 17)
                {
                    openDialog.ShowDialog();
                    path = openDialog.FileName;
                }

            } while (Path.GetExtension(path) != ".txt");

            string[] lines = File.ReadAllLines(path);
            double[,] mat = new double[lines.Length, lines.Length];
            for(int i = 0; i < lines.Length; i++)
            {
                string[] curRow = lines[i].Split(' ');
                if (curRow.Length != lines.Length)
                    throw new IndexOutOfRangeException();
                for (int j = 0; j < curRow.Length; j++)
                    mat[i, j] = double.Parse(curRow[j]);
            }

            EigenValues values = new EigenValues(mat, 1000, 1E-12);
            Console.WriteLine("max lambda = " + values.MaxValue.ToString("E6"));
            Console.WriteLine("min lambda = " + values.MinValue.ToString("E6"));
            Console.WriteLine("max lambda steps = " + values.MaxValueSteps);
            Console.WriteLine("min lambda steps = " + values.MinValueSteps);
            Console.WriteLine("v1 = " + ArrayToString(values.MaxEigenValueVector));
            Console.WriteLine("v2 = " + ArrayToString(values.MinEigenValueVector));

            Console.ReadLine();
        }
        static string ArrayToString(double[] t)
        {
            string str = "";
            for (int i = 0; i < t.Length; i++)
                str += t[i].ToString("E6") + " ";
            return str;
        }
    }
}
