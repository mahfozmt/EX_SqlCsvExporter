using EX_SqlCsvExporter;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace servicewithtopself
{
    public class CsvExporter : ICsvExporter
    {
        private bool IsRunning { get; set; }
        private readonly IConfiguration _config;
        string connectionString = "";
        string SqlSelectQuery = "";
        string CSV_StorePath = "";

        public CsvExporter(IConfiguration configuration)
        {
            connectionString = configuration["AppSettings:ConnectionString"];
            SqlSelectQuery = configuration["AppSettings:SqlSelectQuery"];
            CSV_StorePath = configuration["AppSettings:CSV_StorePath"];
            _config = configuration;
        }
        public void Stop()
        {
            IsRunning = false;
        }

        public void Start()
        {
            try
            {
                //IsRunning = true;

                //while (IsRunning)
                //{                                        
                //    Utils.LogToFile(1, "[INFO]", "Service running");

                //    ProcessExecuter();

                //    Thread.Sleep(1000);
                //}

                Utils.LogToFile(1, "[INFO]", "Service running");

                ProcessExecuter();

            }
            catch (Exception ex)
            {
                Utils.LogToFile(1, "[EXCEPTION]", "Calling Start Failed. " + ex.Message + " Stack Trace: " + ex.StackTrace.ToString());
            }
        }

        private void ProcessExecuter()
        {
            try
            {
                Utils.LogToFile(3, "[INFO]", "Calling ProcessExecuter");
                //List<Country> countries = this.GetDataFromDB();
                //Utils.LogToFile(1, "[INFO]", "No. of Records: " + countries.Count);

                DataTable dt = this.GetDataTableFromDb(SqlSelectQuery);
                Utils.LogToFile(1, "[INFO]", "No. of Records: " + dt.Rows.Count);

                string csvFileName = DateTime.Now.ToString("ddMMyyyy", CultureInfo.CreateSpecificCulture("en-US"))+"_backup.csv";
                string fullPathName = Path.Combine(CSV_StorePath, csvFileName);
                try
                {
                    Utils.LogToFile(1, "[INFO]", "Attempt to Writing CSV File");
                    using (StreamWriter writer = new StreamWriter(fullPathName))
                    {
                        DataTable_to_CSV_Writer.WriteDataTable(dt, writer, true);
                        Utils.LogToFile(1, "[INFO]", "Attempt to Writing CSV File");
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            catch (Exception ex)
            {
                Utils.LogToFile(1, "[EXCEPTION]", "Calling ProcessExecuter Failed. " + ex.Message + " Stack Trace: " + ex.StackTrace.ToString());
            }
        }

        

        public DataTable GetDataTableFromDb(string sqlQuery)
        {
            Utils.LogToFile(3, "[INFO]", "Calling GetDataTableFromDb");
            DataTable dtResult;
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter myDataAdapter = new SqlDataAdapter(sqlQuery, myConnection))
                {
                    dtResult = new DataTable();
                    myDataAdapter.Fill(dtResult);

                }
            }

            return dtResult;
        }

        public class Country
        {
            public int ID { get; set; }
            public string CountryName { get; set; }
        }

    }
}
