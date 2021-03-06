using System.Collections.Generic;
using System.Data;

namespace AutoLotDal.BulkImport
{
    public interface IMyDataReader<T>:IDataReader
    {
        List<T> Records { get; set; }
    }
}