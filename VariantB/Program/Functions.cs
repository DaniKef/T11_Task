using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VariantC.TaskClasses;
using VariantB.Storage;
using System.Threading;

namespace VariantC.Program
{
    static class Functions // Класс реализует функции с задания
    {
        static readonly object _locker = new object();
        public static void SearchOrdersWithSumAndCOuntOfProducts(OrderStorage orderList, int sum, int countProducts) // Вывести номера заказов, сумма которых не превосходит заданную и количество различных товаров равно заданному.
        {
            lock(_locker)
            {
                Console.WriteLine("Поток:" + Thread.CurrentThread.ManagedThreadId);
                OrderStorage.WriteResultOfRequest("", "SearchOrdersWithSumAndCOuntOfProducts"); // Запись обращения
                var orderNumberQuery = orderList.GetStorage().Where(number => number.Value.CountSumOfProducts() <= sum && number.Value.ProductsInOrder.Count == countProducts).
                    Select(number => number.Value.OrderNumber);
                foreach (var item in orderNumberQuery)
                {
                    Console.WriteLine(item);
                    OrderStorage.WriteResultOfRequest(item.ToString(), ""); // Запись результата в файл.
                }
            }
        }
        public static void SearchThisProduction(OrderStorage orderList, string productName)// Вывести номера заказов, содержащих заданный товар.
        {
            lock (_locker)
            {
                Console.WriteLine("Поток:" + Thread.CurrentThread.ManagedThreadId);
                OrderStorage.WriteResultOfRequest("", "SearchThisProduction");
                // Сначала с помощью синтаксиса запросов, но они не используются
                //var orderNumberQuery = from order in orderList.GetStorage() 
                //                       from productIn in order.Value.ProductsInOrder
                //                       where productIn.ProductIn.ProductName == productName
                //                       select order.Value.OrderNumber;
                var orderNumberQuery = orderList.GetStorage().SelectMany(x => x.Value.ProductsInOrder, (u, l) => new { phone = u, order = l }).
                    Where(u => u.order.ProductIn.ProductName == productName).Select(u => u.phone.Value.OrderNumber);
                foreach (var item in orderNumberQuery)
                {
                    Console.WriteLine(item);
                    OrderStorage.WriteResultOfRequest(item.ToString(), ""); // Запись результата в файл.
                }
            }
        }
        public static void SearchNotContainsProductAndToday(OrderStorage orderList, string productName, int day) //Вывести номера заказов, не содержащих заданный товар и поступивших в течение текущего дня.
        {
            lock (_locker)
            {
                Console.WriteLine("Поток:" + Thread.CurrentThread.ManagedThreadId);
                OrderStorage.WriteResultOfRequest("", "SearchNotContainsProductAndToday");
                // Сначала с помощью синтаксиса запросов, но они не используются
                //var orderNumberQuery = from order in orderList.GetStorage()
                //                       where (!(order.Value.ContainProduct(productName)) && order.Value.ReceiptDay.Date.Day == day)
                //                       select order.Value.OrderNumber;
                var orderNumberQuery = orderList.GetStorage().SelectMany(x => x.Value.ProductsInOrder, (u, l) => new { phone = u, order = l }).
                    Where(u => !(u.phone.Value.ContainProduct(productName)) && u.phone.Value.ReceiptDay.Date.Day == day).Select(u => u.phone.Value.OrderNumber);

                foreach (var item in orderNumberQuery)
                {
                    Console.WriteLine(item);
                    OrderStorage.WriteResultOfRequest(item.ToString(), "");
                }
            }
        }
        public static Order CreateOrder(ref OrderStorage orderList, int day) //Сформировать новый заказ, состоящий из товаров, заказанных в текущий день.
        {
            List<ProductInOrder> productsOrderlist = new List<ProductInOrder>(); // для составления товаров список товаров
            lock (_locker)
            {
                Console.WriteLine("Поток:" + Thread.CurrentThread.ManagedThreadId);
                OrderStorage.WriteResultOfRequest("", "CreateOrder");
                // Сначала с помощью синтаксиса запросов, но они не используются
                //var productsOrder = from order in orderList.GetStorage()
                //                    where order.Value.ReceiptDay.Date.Day == day
                //                    select order.Value.ProductsInOrder;
                var productsOrder = orderList.GetStorage().SelectMany(x => x.Value.ProductsInOrder, (u, l) => new { phone = u, order = l }).
                    Where(u => u.phone.Value.ReceiptDay.Date.Day == day).Select(u => u.phone.Value.ProductsInOrder).Distinct();
                var rand = new Random();
                foreach (var item in productsOrder)
                {
                    for (int i = 0; i < item.Count; i++)
                        productsOrderlist.Add(item[i]);
                }
                var newOrder = new Order(rand.Next(3000, 4000), DateTime.Now, productsOrderlist);
                OrderStorage.WriteResultOfRequest(newOrder.ToString(), "");
                orderList.AddOrder("0556833325", newOrder); // Создать заказ из товаров заказанных в этот день
                return newOrder;// Возвращается
            }
        }
        public static void RemoveOrdersThisProductThisAmount(ref OrderStorage orderList, string productName, int amount)//Удалить все заказы, в которых присутствует заданное количество заданного товара.
        {
            lock (_locker)
            {
                Console.WriteLine("Поток:" + Thread.CurrentThread.ManagedThreadId);
                OrderStorage.WriteResultOfRequest("", "RemoveOrdersThisProductThisAmount");
                // Сначала с помощью синтаксиса запросов, но они не используются
                //var productsOrder = from order in orderList.GetStorage()
                //                    from productIn in order.Value.ProductsInOrder
                //                    where (productIn.ProductIn.ProductName == productName && productIn.Amount == amount)
                //                    select order.Key;
                var productsOrder = orderList.GetStorage().SelectMany(x => x.Value.ProductsInOrder, (u, l) => new { phone = u, order = l }).
                    Where(u => u.order.ProductIn.ProductName == productName && u.order.Amount == amount).Select(u => u.phone.Key);
                foreach (var item in productsOrder)
                {
                    orderList.RemoveOrder(item);
                }
                foreach (var at in orderList)
                    OrderStorage.WriteResultOfRequest(at.ToString(), "");
            }
        }
    }
}
