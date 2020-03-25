using System;
using System.Data;
using System.Data.SQLite;

using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Config;
using TwinSovet.Data.Models;

using NLog;


namespace TwinSovet.Data.Helpers 
{
    public static class DbValidator 
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();


        public static bool VerifyDatabase() 
        {
            SQLiteConnection sqliteConn = null;
            try
            {
                sqliteConn = DbContextBase<AborigenModel>.CreateConnection();
                sqliteConn.Open();

                using (SQLiteCommand foreigKeysPragmaCmd = sqliteConn.CreateCommand())
                {
                    foreigKeysPragmaCmd.CommandText = "PRAGMA foreign_keys = ON";
                    int result = foreigKeysPragmaCmd.ExecuteNonQuery();
                }

                using (SQLiteCommand journalModePragmaCmd = sqliteConn.CreateCommand())
                {
                    journalModePragmaCmd.CommandText = "PRAGMA journal_mode = WAL";
                    int result = journalModePragmaCmd.ExecuteNonQuery();
                }

                using (SQLiteCommand cmd = sqliteConn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT * FROM { DbConst.TableNames.AborigensTableName }";

                    SQLiteDataReader testResult = cmd.ExecuteReader();
                }

                return true;
            }
            catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.NotADb)
            {
                // исключение воспринимаем как признак того, что база не является sqlite
                logger.Info(ex, "Failed to open test connection as SQLite");
                return false;
            }
            catch
            {
                logger.Info("Failed to test SELECT, but db is SQLite");
                return true;
            }
            finally
            {
                if (sqliteConn?.State == ConnectionState.Open)
                {
                    sqliteConn.Close();
                }
            }
        }
    }
}