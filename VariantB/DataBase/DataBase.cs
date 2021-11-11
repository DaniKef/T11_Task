using System;
using System.IO;
using System.Text;

namespace VariantB.DataBase
{
    class DataBase
    {
        public static void CreateDataBaseFile(string fileName) // Создать файл БД.
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"DataBase"); // Создание отдельного каталога
                                                                    // для хранения БД.
            if (!dirInfo.Exists) // Если не существует каталога - создать.
                dirInfo.Create();

            StringBuilder dataBaseName = new StringBuilder(); // Путь к файлу БД.
            dataBaseName.Append(@"DataBase\");
            dataBaseName.Append(fileName);
            dataBaseName.Append(@".txt");

            if (File.Exists(dataBaseName.ToString())) // Если файл с таким именем существует.
                Console.WriteLine("Файл базы данных уже существует.");
            else
                File.Create(dataBaseName.ToString()).Close(); // Иначе создать такой файл.
        }
        public static void DeleteDataBaseFile(string fileName) // Удалить файл БД.
        {
            StringBuilder dataBaseName = new StringBuilder(); // Путь к файлу БД.
            dataBaseName.Append(@"DataBase\");
            dataBaseName.Append(fileName);
            dataBaseName.Append(@".txt");

            if (File.Exists(dataBaseName.ToString())) // Если существует - удалить.
                File.Delete(dataBaseName.ToString());
            else // Иначе.
                Console.WriteLine("Такого файла не существует.");
        }

    }
}
