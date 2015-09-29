using ChongCore.Services.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChongCore.Model;

namespace ChongCore.Services.Database
{
    public class Store : IStore
    {
        private IDictionary<Guid, Record> cache;
        private bool isDirty = true;

        public Store()
        {

        }

        public void Add(IList<Record> records)
        {
            throw new NotImplementedException();
        }

        public void Delete(IList<Record> records)
        {
            throw new NotImplementedException();
        }

        public void Update(IList<Record> records)
        {
            throw new NotImplementedException();
        }
    }
}
