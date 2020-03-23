﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TwinSovet.Data.DataBase.Config 
{
    /// <summary>
    /// Содержит константы описания схемы БД.
    /// </summary>
    public static class DbConst 
    {
        /// <summary>
        /// Название файла базы данных.
        /// </summary>
        public const string DbFileName = "twin-sovet.db";

        /// <summary>
        /// Дефолтное значение для строк (<see cref="string.Empty"/>, дабы не путать null с пустой строкой).
        /// </summary>
        public static readonly string DefaulStringValue = string.Empty;


        /// <summary>
        /// Содержит названия таблиц БД.
        /// </summary>
        public static class TableNames 
        {
            /// <summary>
            /// Название таблицы жителей дома.
            /// </summary>
            public const string AborigensTableName = "aborigens";
        }
    }
}