using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariantC.TaskClasses;

namespace VariantB.Storage
{
    abstract class Storage
    {
        public abstract int Count { get; }// Свойство количества элементов в хранилище
        public string storageName { get; init; } // Название скалада
        public string storagePlace { get; init; } // Место склада

    }
}
