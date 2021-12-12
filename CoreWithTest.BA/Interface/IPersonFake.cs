using CoreWithTest.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWithTest.BA.Interface
{
    public interface IPersonFake
    {
        public IEnumerable<Person> GetAllItems();
        Person Add(Person newItem);
        Person GetById(int id);
        void Remove(int id);
    }
}
