using CoreWithTest.DAL.Interface;
using CoreWithTest.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWithTest.BA.Service
{
    public class PersonService
    {
        private readonly IRepository<Person> _person;

        public PersonService(IRepository<Person> perosn)
        {
            _person = perosn;
        }
        //Get Person Details By Person Id  
        public IEnumerable<Person> GetPersonByUserId(int UserId)
        {
            return _person.GetAll().Where(x => x.Id == UserId).ToList();
        }
        //GET All Perso Details   
        public IEnumerable<Person> GetAllPersons()
        {
            try
            {
                return _person.GetAll().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Get Person by Person Name  
        public Person GetPersonByUserName(string UserName)
        {
            return _person.GetAll().FirstOrDefault(x => x.UserEmail == UserName);
        }
        //Add Person  
        public async Task<Person> AddPerson(Person Person)
        {
            return await _person.Create(Person);
        }
        //Delete Person   
        public bool DeletePerson(int UserEmail)
        {

            try
            {
                var DataList = _person.GetAll().Where(x => x.Id == UserEmail).ToList();
                foreach (var item in DataList)
                {
                    _person.Delete(item);
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }

        }
        //Update Person Details  
        public bool UpdatePerson(Person person)
        {
            try
            {
                var DataList = _person.GetAll().Where(x => !x.IsDeleted).ToList();
                foreach (var item in DataList)
                {
                    _person.Update(item);
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
