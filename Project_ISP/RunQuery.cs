using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Project_ISP
{
    public static class RunQuery
    {
        internal static SqlTransaction Trans;
        internal static SqlCommand Cmnd;
        internal static ISPContext db = new ISPContext();
        private static SqlConnection AppConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ISPConnectionString"].ConnectionString);

        internal static DataTable GetData(string query)
        {

            SqlCommand cmd = new SqlCommand(query);
            using (SqlConnection con = new SqlConnection(AppConn.ConnectionString))
            {
                DataTable dt = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    cmd.CommandTimeout = 0;
                    return dt;
                }
            }
        }

        internal static object ExecuteScalar(string strSQL)
        {
            OpenAppConnection();

            try
            {
                if (Trans == null)
                {
                    Cmnd = new SqlCommand(strSQL, AppConn);
                }
                else
                {
                    Cmnd.CommandText = strSQL;
                }

                Cmnd.CommandType = CommandType.Text;

                return Cmnd.ExecuteScalar();
            }
            catch (SqlException Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                    Trans = null;
                }
                throw Ex;

            }
            catch (Exception Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                    Trans = null;
                }
                throw Ex;

            }
            finally
            {
                if (Trans == null)
                {
                    CloseAppConnection();
                }

            }
        }

        internal static void OpenAppConnection()
        {
            //string ConnectionString = ConfigurationManager.ConnectionStrings["EasyLifeConnection"].ConnectionString;

            if (!AppConn.ConnectionString.Equals(""))
            {
                if (AppConn.State != ConnectionState.Open)
                {
                    AppConn.Open();
                }
            }
        }

        internal static void CloseAppConnection()
        {
            if (AppConn.State == ConnectionState.Open)
            {
                AppConn.Close();
            }
        }
    }
}