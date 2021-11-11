using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariantC.TaskClasses;

namespace VariantB.Storage
{
    class ProductStorage : Storage // Коллекция всех продуктов в заказах
    {
        List<ProductInOrder> _storage = new List<ProductInOrder>();
        public ProductStorage() // Конструктор без праметров
        {
            storageName = "СкладПродуктов";
            storagePlace = "Харьков";
        }
        public override int Count // Свойство количества элементов в хранилище заказов
        {
            get { return _storage.Count; }
        }
        public void AddProduct(ProductInOrder newProduct)// Добавить продукт
        {
            _storage.Add(newProduct);
        }
        public void RemoveAll() // Удалить все продукты
        {
            _storage.Clear();
        }
        public List<ProductInOrder> GetProdictsInOrder() // Вернуть список всех продуктов
        {
            return _storage;
        }
        public ProductInOrder this[int index] // Индексатор 
        {
            get
            {
                return _storage[index];
            }
            set
            {
                _storage[index] = value;
            }
        }
        public IEnumerator GetEnumerator() // Итератор
        {
            for (int i = 0; i < _storage.Count; i++)
            {
                yield return _storage[i];
            }
        }
        public List<ProductInOrder> GetLastValues(int count) // Используется в меню, получает последние count заказов.
        {
            var tl = new List<ProductInOrder>();
            for (int i = 1; i <= count; i++)
            {
                tl.Add(_storage[_storage.Count - i]);
            }
            return tl;
        }
        public List<ProductInOrder> GetAll()
        {
            return _storage;
        }
    }
}
