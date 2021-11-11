using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VariantB.Storage;
using VariantC.TaskClasses;

namespace VariantB.DataBase
{
    class CRUDOp : DataBase
    {
        public static void CreateRecord(OrderStorage order, string fileName) // Создать запись заказа в БД.
        {
            StringBuilder dataBaseName = new StringBuilder(); // Путь к файлу БД.
            dataBaseName.Append(@"DataBase\");
            dataBaseName.Append(fileName);
            dataBaseName.Append(@".txt");
            try
            {
                using (StreamWriter sw = new StreamWriter(dataBaseName.ToString(), false, Encoding.Default))
                {
                    for(int i = 0; i < order.Count; i++) // Записать все элементы коллекции.
                    {
                        sw.WriteLine(order[i].Item1);
                        sw.WriteLine(order[i].Item2.OrderNumber);
                        sw.WriteLine(order[i].Item2.ReceiptDay);
                        foreach(var at in order[i].Item2.ProductsInOrder) // Записать все продукты
                        {
                            sw.WriteLine("-"); // Разделяет продукты.
                            sw.WriteLine(at.Amount);
                            sw.WriteLine(at.ProductIn.ProductName);
                            sw.WriteLine(at.ProductIn.ProductDescription);
                            sw.WriteLine(at.ProductIn.ProductPrice);
                        }
                        sw.WriteLine("------------"); // Разделяет заказы.
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void AddRecord(string phone, Order order, string fileName) // Добавить Запись заказа.
        {
            StringBuilder dataBaseName = new StringBuilder(); // Путь к файлу БД.
            dataBaseName.Append(@"DataBase\");
            dataBaseName.Append(fileName);
            dataBaseName.Append(@".txt");
            try
            {
                using (StreamWriter sw = new StreamWriter(dataBaseName.ToString(), true, Encoding.Default))
                {
                    sw.WriteLine(phone); // Записать телефон
                    sw.WriteLine(order.OrderNumber); //Записать всю информацию о заказе. Пишется в конец.
                    sw.WriteLine(order.ReceiptDay);
                    foreach (var at in order.ProductsInOrder) // Записать все продукты
                    {
                        sw.WriteLine("-"); // Разделяет продукты.
                        sw.WriteLine(at.Amount);
                        sw.WriteLine(at.ProductIn.ProductName);
                        sw.WriteLine(at.ProductIn.ProductDescription);
                        sw.WriteLine(at.ProductIn.ProductPrice);
                    }
                    sw.WriteLine("------------"); // Разделяет заказы.
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void ReadRecords(string fileName) // Считать все заказы.
        {
            StringBuilder dataBaseName = new StringBuilder(); // Путь к файлу БД.
            dataBaseName.Append(@"DataBase\");
            dataBaseName.Append(fileName);
            dataBaseName.Append(@".txt");
            try
            {
                using(StreamReader sr = new StreamReader(dataBaseName.ToString(), Encoding.Default))
                {
                    string check; // Проверяет следующий символ.
                    do
                    {
                        string phoneNumber = sr.ReadLine(); // Получаю нужные переменные по очереди.
                        int orderNumber = Int32.Parse(sr.ReadLine());
                        DateTime receiptDay = DateTime.Parse(sr.ReadLine());
                        ProductStorage productInOrderStorage = new ProductStorage(); // Список продуктов
                        sr.ReadLine(); // Пропуск ненужного символа.
                        do
                        {
                            int amount = Int32.Parse(sr.ReadLine());
                            string productName = sr.ReadLine();
                            string productDescription = sr.ReadLine();
                            double productPrice = Double.Parse(sr.ReadLine());
                            productInOrderStorage.AddProduct(new ProductInOrder(new Product(productName, productDescription,
                                productPrice), amount)); // Добавляю продукт в список.
                            check = sr.ReadLine();
                        } while (check == "-");
                        (string, Order) storage = (phoneNumber, new Order(orderNumber, receiptDay, new List<ProductInOrder>(productInOrderStorage.GetAll()))); // Кортеж 
                        Console.WriteLine(storage); // Вывести кортеж
                    } while (check == "------------");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void UpdateRecord(string phone, Order order, string fileName) // Обновить запись заказа по телефону.
        {
            StringBuilder dataBaseName = new StringBuilder(); // Путь к файлу БД.
            dataBaseName.Append(@"DataBase\");
            dataBaseName.Append(fileName);
            dataBaseName.Append(@".txt");
            try
            {
                string[] s = File.ReadAllLines(dataBaseName.ToString(), Encoding.Default); // Считать все строки в файле.
                int count = 0; // Сколько строк удалено.
                for (int i = 0; i< s.Length; i++) 
                {
                    if(s[i] == phone) // Если нашелся телефон
                    {
                        int index = i;
                        while(s[index] != "------------") // Стирать до отметки
                        {
                            count++;
                            s[index] = "";
                            index++;
                        }
                        count++;
                        s[index] = "";
                        break;
                    }
                }
                string[] ss = new string[s.Length - count]; // Новый массив длиной с предыдущий - сколько удалено
                int index1 = 0;
                for (int i = 0; i < s.Length; i++) // Заполнение.
                {
                    if (!string.IsNullOrWhiteSpace(s[i])) // Не заполнять пустые.
                    {
                        ss[index1] = s[i];
                        index1++;
                    }
                }
                using (StreamWriter sw = new StreamWriter(dataBaseName.ToString(), false, Encoding.Default))
                {
                    for (int i = 0; i < ss.Length; i++) // Записать все что было в новом массиве.
                    {
                        sw.WriteLine(ss[i]);
                    }
                    sw.WriteLine(phone); // Записать телефон
                    sw.WriteLine(order.OrderNumber); //Записать всю информацию о заказе. Пишется в конец.
                    sw.WriteLine(order.ReceiptDay);
                    foreach (var at in order.ProductsInOrder) // Записать все продукты
                    {
                        sw.WriteLine("-"); // Разделяет продукты.
                        sw.WriteLine(at.Amount);
                        sw.WriteLine(at.ProductIn.ProductName);
                        sw.WriteLine(at.ProductIn.ProductDescription);
                        sw.WriteLine(at.ProductIn.ProductPrice);
                    }
                    sw.WriteLine("------------"); // Разделяет заказы.
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void DeleteRecordByPhone(string phone, string fileName) // Удалить запись по телефону
        {
            StringBuilder dataBaseName = new StringBuilder(); // Путь к файлу БД.
            dataBaseName.Append(@"DataBase\");
            dataBaseName.Append(fileName);
            dataBaseName.Append(@".txt");
            try
            {
                string[] s = File.ReadAllLines(dataBaseName.ToString(), Encoding.Default); // Считать все строки в файле.
                int count = 0; // Сколько строк удалено.
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == phone) // Если нашелся телефон
                    {
                        int index = i;
                        while (s[index] != "------------") // Стирать до отметки
                        {
                            count++;
                            s[index] = "";
                            index++;
                        }
                        count++;
                        s[index] = "";
                        break;
                    }
                }
                string[] ss = new string[s.Length - count]; // Новый массив длиной с предыдущий - сколько удалено
                int index1 = 0;
                for (int i = 0; i < s.Length; i++) // Заполнение.
                {
                    if (!string.IsNullOrWhiteSpace(s[i])) // Не заполнять пустые.
                    {
                        ss[index1] = s[i];
                        index1++;
                    }
                }
                using (StreamWriter sw = new StreamWriter(dataBaseName.ToString(), false, Encoding.Default))
                {
                    for (int i = 0; i < ss.Length; i++) // Записать все что было в новом массиве.
                    {
                        sw.WriteLine(ss[i]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void DeleteAllRecords(string fileName) // Сделать файл пустым.
        {
            StringBuilder dataBaseName = new StringBuilder(); // Путь к файлу БД.
            dataBaseName.Append(@"DataBase\");
            dataBaseName.Append(fileName);
            dataBaseName.Append(@".txt");
            try
            {
                File.WriteAllText(dataBaseName.ToString(), String.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
