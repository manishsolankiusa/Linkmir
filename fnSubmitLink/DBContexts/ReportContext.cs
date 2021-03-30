using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

/*
 * Author   : Manish Solanki
 * Date     : March, 2021
 * All rights reserved.
 */

namespace Linkmir.AzFunctions.DBContexts
{
    class ReportContext
    {
        public static string connectionString;
        public SqlConnection sqlConnection;

        public string GetSummaryDataByDomain(string domain, string subdomain)
        {
            try
            {
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string summaryData = null;
                    using (SqlCommand sqlCommand = new SqlCommand("select count(*) as [TotalLinks], sum([AccessCount]) as [TotalAccessCount], sum([SubmitCount]) as [TotalSubmitCount] from [dbo].[Link] where [domain] like '" + domain + "' and [Subdomain] like '" + subdomain + "' for xml path('SummaryData');", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.Text;

                        var result = sqlCommand.ExecuteReader();
                        SqlDataReader sqlDataReader = (SqlDataReader) result;

                        sqlDataReader.Read();
                        summaryData = sqlDataReader.GetString(0);
                        sqlDataReader.Close();
                    }

                    sqlConnection.Close();
                    return summaryData;
                }
            }
            catch (Exception e)
            {
                if (sqlConnection.State != ConnectionState.Closed)
                    sqlConnection.Close();
                //throw e;
            }
            return null;
        }

        public string GetSummaryDataByLink(string linkShort)
        {
            try
            {
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string summaryData = null;
                    using (SqlCommand sqlCommand = new SqlCommand("select count(*) as [TotalLinks], sum([AccessCount]) as [TotalAccessCount], sum([SubmitCount]) as [TotalSubmitCount] from [dbo].[Link] where [LinkShort] = '" + linkShort + "' for xml path('SummaryData');", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.Text;

                        var result = sqlCommand.ExecuteReader();
                        SqlDataReader sqlDataReader = (SqlDataReader)result;

                        sqlDataReader.Read();
                        summaryData = sqlDataReader.GetString(0);
                        sqlDataReader.Close();
                    }

                    sqlConnection.Close();
                    return summaryData;
                }
            }
            catch (Exception e)
            {
                if (sqlConnection.State != ConnectionState.Closed)
                    sqlConnection.Close();
                //throw e;
            }
            return null;
        }
    }
}
