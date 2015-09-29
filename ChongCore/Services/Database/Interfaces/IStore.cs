using ChongCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChongCore.Services.Database.Interfaces
{
    public interface IStore
    {
        void Add(IList<Record> records);
        void Update(IList<Record> records);
        void Delete(IList<Record> records);
    }
}
