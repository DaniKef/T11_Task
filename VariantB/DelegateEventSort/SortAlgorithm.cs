using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariantC.TaskClasses;
using VariantB.Storage;

namespace VariantB.DelegateEventSort
{
    class SortAlgorithm
    {
        public Dictionary<string, Order> SortOrderStorageByTime(Dictionary<string, Order> storage) // Сортирует словарь по дате получения заказа
        {
            return storage.OrderBy(val => val.Value.ReceiptDay).ToDictionary(val =>val.Key, val => val.Value);
        }
        public Dictionary<string, Order> SortOrderStorageByAmountOfProducts(Dictionary<string, Order> storage) // По количеству товаров в заказе
        {
            return storage.OrderBy(val => val.Value.ProductsInOrder.Count).ToDictionary(val => val.Key, val => val.Value);
        }
        public Dictionary<string, Order> SortOrderStorageBySumOfProducts(Dictionary<string, Order> storage) // По общей цене
        {
            return storage.OrderBy(val => val.Value.CountSumOfProducts()).ToDictionary(val => val.Key, val => val.Value);
        }
    }
}
