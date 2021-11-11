using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariantB.Storage;

namespace VariantC.TaskClasses
{
    record Order //запись заказа
    {
        private int _orderNumber; // Номер заказа.
        private List<ProductInOrder> _productsInOrder = new List<ProductInOrder>(); // Товары в заказе.
        private DateTime _receiptDay; // Дата поступления.
        public static int orderCount; // Считает все выполненные заказы
        public Order(int orderNumber, int day, List<ProductInOrder> newOrder) // Конструктор.
        {
            OrderNumber = orderNumber; // устанавливает номер
            ProductsInOrder.AddRange(newOrder.ToArray());// получает все заказы
            ReceiptDay = new DateTime(2020, 10, day); // Передается только день, все остальное остается.
            orderCount++;
        }
        public Order(int orderNumber, List<ProductInOrder> newOrder) // Конструктор.
        {
            OrderNumber = orderNumber; // устанавливает номер
            ProductsInOrder.AddRange(newOrder.ToArray());// получает все заказы
            ReceiptDay = DateTime.Today; // день ставится сегодняшний.
            orderCount++;
        }
        public Order(int orderNumber, DateTime time, List<ProductInOrder> newOrder) // Конструктор.
        {
            OrderNumber = orderNumber; // устанавливает номер
            ProductsInOrder.AddRange(newOrder.ToArray());// получает все заказы
            ReceiptDay = time; // день ставится сегодняшний.
            orderCount++;
        }
        public Order() // Конструктор.
        {
        }

        public int OrderNumber // Свойство номера заказа.
        {
            get { return _orderNumber; }
            init // Чтобы после инициализации было доступно только для чтения.
            {
                if (Double.IsNegative(value)  || value.ToString().Length > 8) // Проверка, чтоб было положительным, нормальным и число символов < 8
                    throw new ArgumentException($"{value}"); // Исключение.
                _orderNumber = value;
            }
        }
        public DateTime ReceiptDay // Свойство даты получения.
        {
            get { return _receiptDay; }
            init
            {
                _receiptDay = value;
            }
        }
        public List<ProductInOrder> ProductsInOrder // Свойство списка товаров в заказе.
        {
            get { return _productsInOrder; }
            init
            {
                _productsInOrder = value;
            }
        }
        public double CountSumOfProducts()// Считает сумму всех товарова в заказе
        {
            double sum = 0;// сумма
            for (int i = 0; i < ProductsInOrder.Count; i++) // перебирает все товары в заказе
            {
                sum += (ProductsInOrder[i].Amount * ProductsInOrder[i].ProductIn.ProductPrice);//количество на цену
            }
            return sum;
        }
        public bool ContainProduct(string productName) //  Проверяет, что в заказе есть товар с таким именем
        {
            for(int i = 0; i< _productsInOrder.Count; i++)
            {
                if (_productsInOrder[i].ProductIn.ProductName == productName)
                    return true;
            }
            return false;
        }
        public string GetInformationAboutOrder() //Вывести полную информацию о заказе
        {
            StringBuilder st = new StringBuilder();
            st.Append($"Номер заказа: {_orderNumber}. Дата поступления: {_receiptDay}\n");
            st.Append("Товары в заказе: \n");
            for (int i = 0; i < ProductsInOrder.Count; i++) // Перебирает все товары
            {
                st.Append($"{ProductsInOrder[i].ProductIn}. Количество товара: { ProductsInOrder[i].Amount}.\n");
            }                        // в структуре Product переопределение ToString
            Console.WriteLine();
            return st.ToString();
        }
        public override string ToString()
        {
            return GetInformationAboutOrder();
        }
    }
}
