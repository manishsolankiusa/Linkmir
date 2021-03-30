using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Linkmir.AzFunctions.Models;

/*
 * Author   : Manish Solanki
 * Date     : March, 2021
 * All rights reserved 
 */

namespace Linkmir.AzFunctions.DBContexts
{
    public class LinkContext
    {
        public static string connectionString;
        public SqlConnection sqlConnection;
        private ArrayList insertLinks = new ArrayList();
        private ArrayList getLinksByShortName = new ArrayList();

        public LinkContext()
        {
        }

        public void GetLinkByShortName(Link link)
        {
            getLinksByShortName.Add(link);
        }

        public void Insert(Link link)
        {
            insertLinks.Add(link);
        }

        public void RetrieveLinks()
        {
            try
            {
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    foreach (Link getLinkByShortName in getLinksByShortName)
                    {

                        if (getLinkByShortName.Status != "Valid")
                            continue;

                        try
                        {
                            using (SqlCommand sqlCommand = new SqlCommand("[spGetLinkByShortName]", sqlConnection))
                            {
                                sqlCommand.CommandType = CommandType.StoredProcedure;

                                sqlCommand.Parameters.Add("@LinkShort", SqlDbType.NVarChar).Value = getLinkByShortName.LinkShort;

                                var result = sqlCommand.ExecuteReader();
                                SqlDataReader sqlDataReader = (SqlDataReader)result;
                                sqlDataReader.Read();
                                getLinkByShortName.Status = "Retrieved";
                                getLinkByShortName.ID = long.Parse(sqlDataReader["ID"].ToString());
                                getLinkByShortName.LinkValidated = sqlDataReader["LinkOriginal"].ToString();
                                sqlDataReader.Close();
                            }
                        }
                        catch (Exception e)
                        {
                            getLinkByShortName.Status = "Error";
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                if (sqlConnection.State != ConnectionState.Closed)
                    sqlConnection.Close();
                //throw e;
            }
        }

        public void SaveChanges()
        {
            try
            {
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    foreach (Link insertLink in insertLinks)
                    {

                        if (insertLink.Status != "Valid")
                            continue;

                        SqlTransaction sqlTransaction;
                        sqlTransaction = sqlConnection.BeginTransaction();
                        try
                        {
                            using (SqlCommand sqlCommand = new SqlCommand("spSubmitLink", sqlConnection))
                            {
                                sqlCommand.CommandType = CommandType.StoredProcedure;
                                sqlCommand.Transaction = sqlTransaction;

                                sqlCommand.Parameters.Add("@LinkOriginal", SqlDbType.NVarChar).Value = insertLink.LinkValidated;
                                sqlCommand.Parameters.Add("@Domain", SqlDbType.NVarChar).Value = insertLink.LinkDomain;
                                sqlCommand.Parameters.Add("@Subdomain", SqlDbType.NVarChar).Value = insertLink.LinkSubdomain;
                                sqlCommand.Parameters.Add("@SubmitIP", SqlDbType.VarChar).Value = insertLink.SubmitIP;

                                var result = sqlCommand.ExecuteReader();
                                SqlDataReader sqlDataReader = (SqlDataReader)result;
                                sqlDataReader.Read();
                                insertLink.Status = sqlDataReader["LinkStatus"].ToString();
                                insertLink.ID = long.Parse(sqlDataReader["ID"].ToString());
                                insertLink.HashValue = sqlDataReader["HashValue"].ToString();
                                sqlDataReader.Close();
                            }

                            insertLink.GenerateShortLink();

                            using (SqlCommand sqlCommand = new SqlCommand("[spUpdateLink]", sqlConnection))
                            {
                                sqlCommand.CommandType = CommandType.StoredProcedure;
                                sqlCommand.Transaction = sqlTransaction;

                                sqlCommand.Parameters.Add("@ID", SqlDbType.BigInt).Value = insertLink.ID;
                                sqlCommand.Parameters.Add("@LinkShort", SqlDbType.VarChar).Value = insertLink.LinkShort;
                                sqlCommand.ExecuteNonQuery();
                            }
                            sqlTransaction.Commit();
                        }
                        catch (Exception e)
                        {
                            insertLink.Status = "Error";
                            sqlTransaction.Rollback();
                            //throw e;
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch(Exception e)
            {
                if (sqlConnection.State != ConnectionState.Closed)
                    sqlConnection.Close();
                //throw e;
            }
        }
    }
}
