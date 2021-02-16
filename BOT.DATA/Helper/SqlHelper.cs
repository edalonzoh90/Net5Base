using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BOT.DATA.Helper
{
    public static class SqlHelper
    {
        public static SqlDbType GetDbTypefromString( string TypeOfObject) {
            switch (TypeOfObject) {

                case "System.String":
                    return SqlDbType.NVarChar;
                case "System.Boolean":
                    return SqlDbType.Bit;
                case "System.Decimal":
                    return SqlDbType.Decimal;
                case "System.Double" :
                    return SqlDbType.Float;
                case "System.Sbyte":
                    return SqlDbType.SmallInt;
                case "System.Byte":
                    return SqlDbType.VarBinary;
                case "System.Short":
                    return SqlDbType.SmallInt;
                case "System.UShort":
                    return SqlDbType.SmallInt;
                case "System.Int32":
                    return SqlDbType.Int;
                case "System.UInt32":
                    return SqlDbType.Int;
                case "System.Int64"://long
                    return SqlDbType.BigInt;
                case "System.ULong64":
                    return SqlDbType.BigInt;
                case "System.Single"://float
                    return SqlDbType.Float;
                case "System.Char":
                    return SqlDbType.NChar;
                case "System.Data.DataTable":
                    return SqlDbType.Structured;
                case "System.DateTime":
                    return SqlDbType.DateTime2;
                default:
                    return SqlDbType.NVarChar;
            }        
        }
    }
}
