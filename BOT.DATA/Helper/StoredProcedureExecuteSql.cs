using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace BOT.DATA.Helper
{
    public static class StoredProcedureExecuteSql
    {
        static SqlConnection conn = new SqlConnection();
        public static SqlCommand LoadStoredProcedureSql(this DbContext context, string storedProcName)
        {
            string contexto = context.Database.GetDbConnection().ConnectionString;
            SqlCommand cmd;

            conn = new SqlConnection(contexto);
            cmd = new SqlCommand(storedProcName, conn);
            
            cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        public static SqlCommand AddParamSql(this SqlCommand cmd, string paramName, object paramValue)
        {
            if (string.IsNullOrEmpty(cmd.CommandText))
                throw new InvalidOperationException("Call LoadStoredProcedureSql before using this method");
            if (string.IsNullOrEmpty(paramName))
                throw new ArgumentNullException(nameof(paramName));

            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            param.SqlDbType = SqlHelper.GetDbTypefromString(paramValue.GetType().ToString());
            cmd.Parameters.Add(param);
            return cmd;
        }

        public static SqlCommand SetTimeoutSql(this SqlCommand cmd, int timeout)
        {
            cmd.CommandTimeout = timeout;
            return cmd;
        }

        private static async Task<List<T>> MapToListSql<T>(this SqlDataReader dr)
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

        public static async Task ExecuteNonQueryAsyncSql(this SqlCommand cmd, bool isTransaction = false)
        {
            using (cmd)
            {
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                finally
                {
                    if (!isTransaction)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }

        public static async Task<List<T>> ExecuteReaderAsyncSql<T>(this SqlCommand cmd, bool isTransaction = false)
        {
            using (cmd)
            {
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                try
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        return await reader.MapToListSql<T>();
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
