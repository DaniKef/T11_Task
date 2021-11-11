//Гуренко Даниил

//Реализовать конкурентное исполнение кода в форме механизма многопоточности (System.Threading) для выполнения «длительных» операций (файловый ввод-вывод, обработка коллекций).
//Обеспечить синхронизацию задач (потоков) и доступа к разделяемым данным (объектам).


using System;
using System.Collections.Generic;
using VariantC.TaskClasses;
using VariantC.Program;
using VariantB.Storage;
using VariantB.DelegateEventSort;
using VariantB.DataBase;
using System.Threading;


namespace VariantC
{
    class MainClass
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Главный Поток:" + Thread.CurrentThread.ManagedThreadId);
            var AppleProduct = new Product();
            var TableProduct = new Product();
            var MouseProduct = new Product();
            var TShirtProduct = new Product("TShirt", "Синяя футболка, xl", 450);//создание продуктов-конструктор
            AppleProduct.CreateProduct("Apple", "Свежие яблоки \"Малинка\"", 15.60); //создание продуктов-методы
            TableProduct.CreateProduct("Table", "Лучший в мире стол", 2100);
            MouseProduct.CreateProduct("Mouse", "Logitech. Хорошее качество.", 800);

            ProductStorage productInOrderStorage = new ProductStorage(); // коллекция продуктов
            OrderStorage storageOrder = new OrderStorage(); // коллекция заказов
            SerialiseCheck(storageOrder);
            CRUDOp.CreateDataBaseFile("DataBase"); // Создание файла БД.

            productInOrderStorage.AddProduct(new ProductInOrder(TableProduct, 3)); // Продукты в коллекцию
            productInOrderStorage.AddProduct(new ProductInOrder(AppleProduct, 55));
            productInOrderStorage.AddProduct(new ProductInOrder(TShirtProduct, 5));
            productInOrderStorage.AddProduct(new ProductInOrder(MouseProduct, 7));
            productInOrderStorage.AddProduct(new ProductInOrder(TShirtProduct, 10));
            productInOrderStorage.AddProduct(new ProductInOrder(TableProduct, 1));
            productInOrderStorage.AddProduct(new ProductInOrder(AppleProduct, 20));
            productInOrderStorage.AddProduct(new ProductInOrder(TShirtProduct, 2));//

            storageOrder.AddOrder("0996154567", new Order(83444, 14, new List<ProductInOrder>() {
            productInOrderStorage[0], productInOrderStorage[1]}));// создать добавить заказ // 1 ЗАКАЗ//

            storageOrder.AddOrder("0994433565", new Order(80111, 15, new List<ProductInOrder>() {
            productInOrderStorage[2]}));// добавить заказ // 2 ЗАКАЗ//

            storageOrder.AddOrder("0568462346", new Order(90999, 16, new List<ProductInOrder>() {
            productInOrderStorage[3], productInOrderStorage[4], productInOrderStorage[5]}));// добавить заказ // 3 ЗАКАЗ//

            storageOrder.AddOrder("0564750381", new Order(10000, 15, new List<ProductInOrder>() {
            productInOrderStorage[6], productInOrderStorage[7]}));// добавить заказ // 4 ЗАКАЗ//

            SetOrder(ref productInOrderStorage, ref storageOrder); // Меню добавления заказов. Метод в этом классе ниже.
            CRUDOperations(storageOrder); // CRUD Операции, в этом классе ниже

            storageOrder.Serialize(); // Сериализация коллекции.

            Thread myThread;
            myThread = new Thread(delegate () { Functions.SearchOrdersWithSumAndCOuntOfProducts(storageOrder, 10000, 2); });//Вывести номера заказов, сумма которых не превосходит заданную и количество различных товаров равно заданному. 
            myThread.Start();
            myThread = new Thread(delegate () { Functions.SearchThisProduction(storageOrder, "TShirt"); });// Вывести номера заказов, содержащих заданный товар.
            myThread.Start(); 
            myThread = new Thread(delegate () { Functions.SearchNotContainsProductAndToday(storageOrder, "Apple", 15); });//Вывести номера заказов, не содержащих заданный товар и поступивших в течение текущего дня.
            myThread.Start(); 
            myThread = new Thread(delegate () { Functions.CreateOrder(ref storageOrder, 15); });
            myThread.Start(); 

            Console.WriteLine("---------------------------------------------");
            foreach (var item in storageOrder)
                Console.WriteLine(item);
            myThread = new Thread(delegate () { Functions.RemoveOrdersThisProductThisAmount(ref storageOrder, "TShirt", 2); }); //Удалить все заказы, в которых присутствует заданное количество заданного товара.
            myThread.Start(); 
            Thread.Sleep(2000);
            Console.WriteLine("---------------------------------------------");
            foreach (var item in storageOrder)
                Console.WriteLine(item);
            Console.ReadKey();
        }


        public static void SetOrder(ref ProductStorage productInOrderStorage, ref OrderStorage storageOrder) // Метод меню.
        {
            int choice = 0;
            string productName = "";
            string productDescription = "";
            double price = 0;
            int amount = 0;
            int orderNumber = 0;
            string phoneNumber = "";
            Console.WriteLine("1.Сделать заказ.\n2.Выйти.");
            do
            {
                choice = Int32.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    int count = 0;
                    int select = 0;
                    do
                    {
                        count++;
                        Console.WriteLine("Имя продукта: ");
                        productName = Console.ReadLine();
                        Console.WriteLine("Описание: ");
                        productDescription = Console.ReadLine();
                        Console.WriteLine("Цена: ");
                        price = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Количество: ");
                        amount = Int32.Parse(Console.ReadLine());

                        var someProduct = new Product(productName, productDescription, price);
                        productInOrderStorage.AddProduct(new ProductInOrder(someProduct, amount));

                        Console.WriteLine("Еще товар - 1. Закончить - 2.");
                        select = Int32.Parse(Console.ReadLine());
                    } while (select != 2);

                    Console.WriteLine("Номер заказа: ");
                    orderNumber = Int32.Parse(Console.ReadLine());
                    Console.WriteLine("Ваш номер телефона: ");
                    phoneNumber = Console.ReadLine();

                    storageOrder.AddOrder(phoneNumber, new Order(orderNumber, new List<ProductInOrder>(productInOrderStorage.GetLastValues(count))));

                }
            } while (choice != 2);
        }

        public static void IsDeserialize(bool check, OrderStorage storageOrder) // Спрашивает, нужна ли десериализация
        {
            if(check)
            {
                storageOrder.Deserialize();
            }
        }

        public static void SerialiseCheck(OrderStorage storageOrder)
        {
            Console.WriteLine("Програма завершилась с ошибкой? 1. Да 2. Нет");
            int checkSerialize = Int32.Parse(Console.ReadLine());
            switch (checkSerialize)
            {
                case 1:
                    IsDeserialize(true, storageOrder);// Нужна ли десериализация.  Метод в этом классе ниже.
                    break;
                case 2:
                    IsDeserialize(false, storageOrder);// Нужна ли десериализация.  Метод в этом классе ниже.
                    break;
                default:
                    break;
            }
        }
        public static void CRUDOperations(OrderStorage storageOrder) // 
        {
            int check = 0;
            do
            {
                Console.WriteLine("Операции над БД:\n" +
    "1.Создать запись заказов в БД.\n" +
    "2.Добавить один заказ.\n" +
    "3.Считать все заказы.\n" +
    "4.Обновить запись заказа по телефону.\n" +
    "5.Удалить запись по телефону.\n" +
    "6.Удалить все записи.\n" +
    "7.Выйти.");
                check = Int32.Parse(Console.ReadLine());
                switch (check)
                {
                    case 1:
                        ThreadPool.QueueUserWorkItem(state => CRUDOp.CreateRecord(storageOrder, "DataBase"));
                        break;
                    case 2:
                        ThreadPool.QueueUserWorkItem(state => CRUDOp.AddRecord("0999999999", storageOrder[1].Item2, "DataBase"));
                        break;
                    case 3:
                        ThreadPool.QueueUserWorkItem(state => CRUDOp.ReadRecords("DataBase"));
                        break;
                    case 4:
                        ThreadPool.QueueUserWorkItem(state => CRUDOp.UpdateRecord("0564750381", storageOrder[1].Item2, "DataBase"));
                        break;
                    case 5:
                        ThreadPool.QueueUserWorkItem(state => CRUDOp.DeleteRecordByPhone("0994433565", "DataBase"));
                        break;
                    case 6:
                        ThreadPool.QueueUserWorkItem(state => CRUDOp.DeleteAllRecords("DataBase"));
                        break;
                    default:
                        break;
                }
            } while (check != 7);
        }
    }
}
