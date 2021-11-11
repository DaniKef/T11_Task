using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariantC.TaskClasses;

namespace VariantB.DelegateEventSort
{
    class ProductsEventArgs : EventArgs // Класс, который хранит заказ.
    {
        public Order order { get; set; } // Заказ.
    }
    delegate void ProductsHandler(object source, ProductsEventArgs arg); // Делегат для события.
    class ProductEvent
    {
        public event ProductsHandler AddProducts; // Событие.

        public void ProductsEvent(Order order) // Событие добавления продуктов в заказ. В классеOrderStorage - AddOrder, RemoveOrder.
        {
            ProductsEventArgs productEventArgs = new ProductsEventArgs();
            productEventArgs.order = order;
            AddProducts?.Invoke(order, productEventArgs);
        }
     
        public static void Message(object source, ProductsEventArgs arg) // Сообщение для события.
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Заказ {arg.order.OrderNumber} с продуктами:");
            foreach (var item in arg.order.ProductsInOrder)
                Console.WriteLine(item);
            Console.WriteLine("Был оформлен.");
            Console.WriteLine("--------------------------------");
        }
        public static void MessageDelete(object source, ProductsEventArgs arg)// Сообщение для события.
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Был удален заказ {arg.order.OrderNumber}");
            Console.WriteLine("--------------------------------");
        }
    }
}
