using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Base
{
    public interface IEntity
    {
        int Id { get; set; }
        bool IsDelete { get; set; }
        DateTime CreateDate { get; set; }
        Nullable<DateTime> UpdateDate { get; set; }
        Nullable<DateTime> DeleteDate { get; set; }
    }
}
