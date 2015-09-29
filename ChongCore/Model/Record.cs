using ChongCore.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChongCore.Model
{
    public class Record : Bindable
    {
        private Guid id;
        public Guid Id
        {
            get
            {
                return id;
            }
            set
            {
                SetProperty(ref id, value);
            }
        }
    }
}
