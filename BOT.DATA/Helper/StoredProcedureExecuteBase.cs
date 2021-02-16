using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using System.Linq;
using System.Data.SqlClient;

namespace BOT.DATA.Helper
{
    public static class StoredProcedureExecuteBase
    {
        public static DbCommand LoadStoredProcedureBase(this DbContext context, string storedProcName)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            if (context.Database.CurrentTransaction != null)
            {
                cmd.Transaction = context.Database.CurrentTransaction.GetDbTransaction();
            }
            cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        public static DbCommand AddParamBase(this DbCommand cmd, string paramName, object paramValue)
        {
            if (string.IsNullOrEmpty(cmd.CommandText)) 
                throw new InvalidOperationException("Call LoadStoredProcedureBase before using this method");
            if (string.IsNullOrEmpty(paramName))
                throw new ArgumentNullException(nameof(paramName));

            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            cmd.Parameters.Add(param);
            return cmd;
        }

        public static DbCommand SetTimeoutBase(this DbCommand cmd, int timeout)
        {
            cmd.CommandTimeout = timeout;
            return cmd;
        }

        private static async Task<List<T>> MapToListBase<T>(this DbDataReader dr)
        {
            var objList = new List<T>();
            var props = typeof(T).GetRuntimeProperties();

            var colMapping = dr.GetColumnSchema()
                .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
                .ToDictionary(key => key.ColumnName.ToLower());

            if (dr.HasRows)
            {
                while (await dr.ReadAsync())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var prop in props)
                    {
                        var val = dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
                        prop.SetValue(obj, val == DBNull.Value ? null : val);
                    }
                    objList.Add(obj);
                }
            }
            return objList;
        }

        public static async Task ExecuteNonQueryAsyncBase(this DbCommand cmd, bool isTransaction = false)
        {
            using (cmd)
            {
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                finally
                {
                    if (!isTransaction)
                        cmd.Connection.Close();
                }
            }
        }

        public static async Task<List<T>> ExecuteReaderAsyncBase<T>(this DbCommand cmd, bool isTransaction = false)
        {
            using (cmd)
            {
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    
                }
                try
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        return await reader.MapToListBase<T>();
                    }
                }
                finally
                {
                    if (!isTransaction)
                        cmd.Connection.Close();
                }
            }
        }
    }
}
