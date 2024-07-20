using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


class Program
{
    static void Main(string[] args)
    {
        string file_input = string.Empty;
        List<string> loaded_files = new List<string>();
        List<string> loaded_file_hashes = new List<string>();
        List<string> loaded_hashes = new List<string>();

        string path = Path.Combine(Directory.GetCurrentDirectory(), "hashes.txt");

        if (args.Length == 0)
        {
            Console.WriteLine("Failed to find input file.");
        }
        else
        {
            foreach (string fe in args)
            {
                Console.WriteLine($"Loaded file: {fe}");
            }
        }

        if (!File.Exists(path))
        {
            Console.WriteLine("Failed to find hashes.txt");
            return;
        }

        foreach (string hash in File.ReadAllLines(path))
        {
            for (int i = 0; i < hash.Length; i++)
            {
                loaded_hashes.Add(hash);
            }
        }


        Func<string, string> calc_hash = (string file) => {

            var md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(File.ReadAllBytes(file));
            string buffer = string.Empty;

            foreach (byte b in bytes)
            {
                buffer += b.ToString("x2");
            }

            return buffer;

        };


        for (int i = 0; i < args.Length; i++)
        {
            loaded_files.Add(args[i]);
            string hash = calc_hash(args[i].Replace("\"", string.Empty));
            loaded_file_hashes.Add(hash);
            Console.WriteLine("File hash: " + hash);
        }

        Console.WriteLine();

        foreach (string file_h in loaded_file_hashes)
        {
            bool is_flagged = false;

            foreach (string hash in loaded_hashes)
            {
                if (hash.Contains(file_h))
                {
                    Console.WriteLine($"File with hash {file_h} is flagged.");
                    is_flagged = true;
                    break;
                }
            }

            if (!is_flagged)
            {
                Console.WriteLine($"Not matched any hash with {file_h}");
            }
        }


        Console.WriteLine();
        Console.WriteLine("Finished. Press enter to exit.");
        Console.ReadLine();
        return;
    }
} 

