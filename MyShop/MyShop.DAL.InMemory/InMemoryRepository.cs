using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAL.InMemory
{
    public class InMemoryRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> elements;
        string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name;
            elements = cache[className] as List<T>;
            if(elements == null)
            {
                elements = new List<T>();
            }
        }

        public void Commit()
        {
            cache[className] = elements;
        }

        public void Insert(T tElement)
        {
            elements.Add(tElement);
        }

        public void Update(T tElement, string Id)
        {
            T tElementToUpdate = elements.Find(p => p.Id == Id);
            if(tElementToUpdate == null)
            {
                throw new Exception(className + " not found");
            }
            else
            {
                tElementToUpdate = tElement;
            }
        }

        public T Find(string Id)
        {
            T tElementToSearch = elements.Find(p => p.Id == Id);
            if (tElementToSearch == null)
            {
                throw new Exception(className + " not found");
            }
            else
            {
                return tElementToSearch;
            }
        }

        public IQueryable<T> Collection()
        {
            return elements.AsQueryable();
        }

        public void Delete(string Id)
        {
            T tElementToDelete = elements.Find(p => p.Id == Id);
            if (tElementToDelete == null)
            {
                throw new Exception(className + " not found");
            }
            else
            {
                elements.Remove(tElementToDelete);
            }
        }
    }
}
