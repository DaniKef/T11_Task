using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariantC.TaskClasses
{
    interface IProductInOrder
    {
        int Amount { get; init; }
        Product ProductIn { get; init; }
    }
    class ProductInOrder: IProductInOrder // класс товара в заказе
    {
        private Product _productIn; // Товар.
        private int _amount;// Количество.
        public ProductInOrder(Product product, int amount) // Конструктор.
        {
            _productIn = product;
            Amount = amount;
        }
        public ProductInOrder()
        {
        }
        public int Amount // Свойство цены продукта.
        {
            get { return _amount; }
            init
            {
                if (Double.IsNegative(value) || value.ToString().Length > 8) // проверка
                    throw new ArgumentException($"{value}"); // Исключение.
                _amount = value;
            }
        }
        public Product ProductIn // Свойство продукта
        {
            get { return _productIn; }
            init { _productIn = value; }
        }
        public override string ToString() //Переопределение метода
        {
            return $"{ProductIn}, Количество: {Amount}";
        }
    }
}
