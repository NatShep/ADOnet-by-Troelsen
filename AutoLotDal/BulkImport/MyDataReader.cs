using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace AutoLotDal.BulkImport
{
    public class MyDataReader<T>: IMyDataReader<T>
    {
        private readonly PropertyInfo[] _propertyInfos;
        private readonly Dictionary<string, int> _nameDictionary;

        public MyDataReader(List<T> records) :
            this()
        {
            Records = records;
        }
        public MyDataReader()
        {
            _propertyInfos = typeof(T).GetProperties();
            _nameDictionary = _propertyInfos
                .Select((x, index) => new {x.Name, index})
                .ToDictionary(pair => pair.Name, pair => pair.index);

        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string GetName(int i) => (i >= 0 && i < FieldCount) ? _propertyInfos[i].Name : string.Empty;
        
        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int i) => _propertyInfos[i].GetValue(Records[_currentIndex]);

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name) => _nameDictionary.ContainsKey(name) ? _nameDictionary[name] : -1;

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public int FieldCount => _propertyInfos.Length;

        public object this[int i]
        {
            get { throw new NotImplementedException(); }
        }

        public object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }


        private int _currentIndex = -1;
        public bool Read()
        {
            if ((_currentIndex + 1) >= Records.Count) return false;
            _currentIndex++;
            return true;
        }

        public int Depth { get; }
        public bool IsClosed { get; }
        public int RecordsAffected { get; }
        public List<T> Records { get; set; }
    }
}