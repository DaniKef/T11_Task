using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using VariantC.TaskClasses;
using VariantB.DelegateEventSort;

using System.Text.Json.Serialization;
namespace VariantB.Storage
{
    delegate Dictionary<string, Order> SortDelegate(Dictionary<string, Order> storage); // делегат сортировки.
    class OrderStorage : Storage // Коллекция всех заказов
    {
        private Dictionary<string, Order> _storage = new Dictionary<string, Order>(); // ключ- номер телефона заказчика, value - заказ
        public OrderStorage() // Конструктор без параметров
        {
            storageName = "B-52";
            storagePlace = "Харьков";
        }
        public void AddOrder(string phone, Order newOrder) // Добавить заказ, телефон-заказ
        {
            _storage.Add(phone, newOrder);
            var productEvent = new ProductEvent(); // Класс события.
            productEvent.AddProducts += ProductEvent.Message; // добавление метода к событию.
            productEvent.ProductsEvent(newOrder); // Событие.
        }
        public void RemoveOrder(string phone) // Выполнить заказ. удалить
        {
            var productEvent = new ProductEvent(); // Класс события.
            productEvent.AddProducts += ProductEvent.MessageDelete; // добавление метода к событию.
            productEvent.ProductsEvent(_storage[phone]); // Событие.
            _storage.Remove(phone);
        }
        public void RemoveAll() // Очистить список заказов
        {
            _storage.Clear();
        }
        public override int Count // Свойство количества элементов в хранилище заказов
        {
            get { return _storage.Count; }
        }
        public Dictionary<string, Order> GetStorage()
        {
            return _storage;
        }
        public (string, Order) this[int index] // Индексатор словаря
        {
            get
            {
                return (_storage.ElementAt(index).Key, _storage.ElementAt(index).Value);
            }
            set
            {
                if(index < 0 || index > _storage.Count - 1)
                {
                    throw new ArgumentException($"{value}"); // Исключение.
                }
                else
                {
                    string tKEy = _storage.ElementAt(index).Key;
                    _storage.Remove(tKEy);
                    _storage.Add(value.Item1, value.Item2);
                }
            }
        }
        public IEnumerator GetEnumerator() // Итератор словаря
        {
            for (int i = 0; i < _storage.Count; i++)
            {
                yield return _storage.ElementAt(i);
            }
        }
        public void Sort(SortDelegate sort) // Сортировка по разным алгоритмам. Делегат.
        {
            _storage = sort(_storage);
        }
        public static void WriteResultOfRequest(string orderResult, string requestName) // записывает результат обращения к коллекции в файл.
        {
            string fileName = "RequestResult.txt"; 
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, true, Encoding.Default))
                {
                    if(requestName != "")
                        sw.WriteLine("Запрос " + requestName + ". Время - " + DateTime.Now + " :");
                    if(orderResult != "")
                        sw.WriteLine(orderResult);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void ReadResultOfRequest() // Читает результаты забросов к коллекции с файла.
        {
            string fileName = "RequestResult.txt";
            try
            {
                using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
                {
                    Console.WriteLine(sr.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void Serialize() // Сериализация. JSON
        {
            try
            {
                string json = JsonSerializer.Serialize(_storage,
                    new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) // Чтоб читало кириллицу
                });
                string fileName = "OrderStorage.json";
                File.WriteAllText(fileName, json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void Deserialize() //Десериализация
        {
            try
            {
                string fileName = "OrderStorage.json";
                string jsonString = File.ReadAllText(fileName, Encoding.UTF8);
                _storage = JsonSerializer.Deserialize<Dictionary<string, Order>>(jsonString,
                    new JsonSerializerOptions()
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                        WriteIndented = true
                    }
                    );
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }

}
