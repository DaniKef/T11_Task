using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariantC.TaskClasses
{
    struct Product
    {
        private string _productName; // Название.
        private string _productDescription; // Описание.
        private double _productPrice; // Цена.
        public Product(string prodName, string prodDes, double prodPrice) // Конструктор
        {
            _productName = prodName;
            _productDescription = prodDes;
            _productPrice = prodPrice;
        }

        public string ProductName // Свойсво имени продукта.
        {
            get { return _productName; }
            set
            {
                if(string.IsNullOrWhiteSpace(value)) // Проверка. 
                    throw new ArgumentException("Название товара введено некорректно.", value); // Исключение.
                _productName = value;
            }
        }
        public string ProductDescription // Свойство описания продукта.
        {
            get { return _productDescription; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Описание товара введено некорректно.", value); // Исключение.
                _productDescription = value;
            }
        }
        public double ProductPrice // Свойство цены продукта.
        {
            get { return _productPrice; }
            set
            {
                if (Double.IsNegative(value) || !Double.IsNormal(value))
                    throw new ArgumentException($"{value}"); // Исключение.
                _productPrice = value;
            }
        }
        public void CreateProduct(string prodName, string prodDes, double prodPrice)// Создать Товар
        {
            ProductName = prodName;
            ProductDescription = prodDes;
            ProductPrice = prodPrice;
        }
        public override string ToString() //Переопределение метода
        {
            return $"Продукт: {ProductName}. Описание: {_productDescription}. Цена: {_productPrice}";
        }
    }
}
