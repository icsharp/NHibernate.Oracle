﻿using System.Data;
using NHibernate.AdoNet;
using NHibernate.Engine.Query;
using NHibernate.SqlTypes;
using Oracle.ManagedDataAccess.Client;

namespace NHibernate.Driver.OracleManager
{
    /// <summary>
    /// A NHibernate Driver for using the Oracle.ManagedDataAccess DataProvider
    /// </summary>
    public class OracleManagedDataClientDriver : DriverBase, IEmbeddedBatcherFactoryProvider
    {
        private static readonly SqlType GuidSqlType = new SqlType(DbType.Binary, 16);

        /// <summary></summary>
        public override string NamedPrefix
        {
            get { return ":"; }
        }

        /// <summary></summary>
        public override bool UseNamedPrefixInParameter
        {
            get { return true; }
        }

        /// <summary></summary>
        public override bool UseNamedPrefixInSql
        {
            get { return true; }
        }

        System.Type IEmbeddedBatcherFactoryProvider.BatcherFactoryClass
        {
            get { return typeof (OracleDataClientBatchingBatcherFactory); }
        }

        /// <remarks>
        /// This adds logic to ensure that a DbType.Boolean parameter is not created since
        /// ODP.NET doesn't support it.
        /// </remarks>
        protected override void InitializeParameter(IDbDataParameter dbParam, string name, SqlType sqlType)
        {
            // if the parameter coming in contains a boolean then we need to convert it 
            // to another type since ODP.NET doesn't support DbType.Boolean
            switch (sqlType.DbType)
            {
                case DbType.Boolean:
                    base.InitializeParameter(dbParam, name, SqlTypeFactory.Int16);
                    break;
                case DbType.Guid:
                    base.InitializeParameter(dbParam, name, GuidSqlType);
                    break;
                default:
                    base.InitializeParameter(dbParam, name, sqlType);
                    break;
            }
        }

        protected override void OnBeforePrepare(IDbCommand command)
        {
            var oracleCommand = (OracleCommand) command;
            base.OnBeforePrepare(oracleCommand);

            // need to explicitly turn on named parameter binding
            // http://tgaw.wordpress.com/2006/03/03/ora-01722-with-odp-and-command-parameters/
            oracleCommand.BindByName = true;

            var detail = CallableParser.Parse(oracleCommand.CommandText);

            if (!detail.IsCallable)
                return;

            oracleCommand.CommandType = CommandType.StoredProcedure;
            oracleCommand.CommandText = detail.FunctionName;
            oracleCommand.BindByName = false;

            var cursor = oracleCommand.CreateParameter();
            cursor.OracleDbType = OracleDbType.RefCursor;
            cursor.Direction = detail.HasReturn ? ParameterDirection.ReturnValue : ParameterDirection.Output;
            oracleCommand.Parameters.Insert(0, cursor);
        }

        public override IDbConnection CreateConnection()
        {
            return new OracleConnection();
        }

        public override IDbCommand CreateCommand()
        {
            return new OracleCommand();
        }
    }
}
