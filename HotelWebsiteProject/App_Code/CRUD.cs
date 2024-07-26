using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;


/// <summary>
/// Summary description for CRUD
/// </summary>
/// 
namespace HotelWebsiteProject.App_Code
{
   public class CRUD
    {
        SqlCommand cmd;
        DataTable dt;
        SqlDataAdapter adp;
        DataSet ds;
        DataView dv;
        // istesd of pasting connection string in each page this refrence the connection on the webconfig  
        // I create connection string once, then use it in creating a new connection in each method and use the static string "conStr"
        public static string conStr = WebConfigurationManager.ConnectionStrings["HotelWebsiteProjectConStr"].ConnectionString;
        // I create a con object once and then use it in methods
        public SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["HotelWebsiteProjectConStr"].ConnectionString);
        public CRUD()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        // to view data whenever its needed
        public SqlDataReader getDrPassSql(string mySql) //1
        {
            /// <summary>
            /// Optimized for con Pooling
            /// </summary>
            // in case of dr, I use using in the calling method to close the dr object
            //https://stackoverflow.com/questions/744051/is-it-necessary-to-manually-close-and-dispose-of-sqldatareader
            // SqlDataReader dr;
            using (SqlCommand cmd = new SqlCommand(mySql, con))
            {
                con.Open();
                 // dr = cmd.ExecuteReader();.
                SqlDataReader dr = cmd.ExecuteReader();
                return dr;
            }
          //  return dr;
        }
        public SqlDataReader getDrPassSql(string mySql, Dictionary<string, object> myPara) //2
        {
            /// <summary>
            /// Optimized for con Pooling
            /// </summary>
            //SqlDataReader dr;
            using (SqlCommand cmd = new SqlCommand(mySql, con))
            {
                foreach (KeyValuePair<string, object> p in myPara)
                {
                    // can put validation here to see if the value is empty or not 
                    cmd.Parameters.AddWithValue(p.Key, p.Value);
                }
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                return dr;
            }
        }
        public SqlDataReader getDrViaSpWithPara(string mySPName, Dictionary<string, object> myPara) //3
        {
            /// <summary>
            /// Optimized for con Pooling. Remember, do not close con here, close it after get the dr to the caller
            /// </summary>
            using (SqlCommand cmd = new SqlCommand(mySPName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, object> p in myPara)
                {
                    // can put validation here to see if the value is empty or not 
                    cmd.Parameters.AddWithValue(p.Key, p.Value);
                }
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                return dr;
            }
        }
        // try to get dr to fill List <T> and pass  the list 
        public DataTable getDT(string mySql) //4
        {
            /// <summary>
            /// Optimized for con Pooling
            /// </summary>
            using (con)
            {
                using (SqlCommand cmd = new SqlCommand(mySql, con))
                {
                    //foreach (KeyValuePair<string, object> p in myPara)
                    //{
                    //    // can put validation here to see if the value is empty or not 
                    //    cmd.Parameters.AddWithValue(p.Key, p.Value);
                    //}
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        con.Open();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public DataTable getDTPassSqlDic(string mySql, Dictionary<string, object> myPara) //5
        {
            /// <summary>
            /// Optimized for con Pooling
            /// </summary>
            using (con)
            {
                using (SqlCommand cmd = new SqlCommand(mySql, con))
                {
                    foreach (KeyValuePair<string, object> p in myPara)
                    {
                        // can put validation here to see if the value is empty or not 
                        cmd.Parameters.AddWithValue(p.Key, p.Value);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        con.Open(); // new
                        da.Fill(dt);
                        return dt;
                        //con.Open();
                    }
                }
            }
        }
        public DataTable getDTViaSpWithPara(string storedProcedure, Dictionary<string, object> spInputPara) //6
        {
            try
            {
                cmd = new SqlCommand();
                dt = new DataTable();
                adp = new SqlDataAdapter();
                con = new SqlConnection(conStr);  // ConnectionString changed to Global.DB_CONN_STR
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (cmd.Connection.State == ConnectionState.Closed)
                    using (con)
                    {
                        cmd.Connection.Open();
                        foreach (KeyValuePair<string, object> spData in spInputPara)
                        {
                            cmd.Parameters.AddWithValue(spData.Key, spData.Value);
                        }
                        adp.SelectCommand = cmd;
                        adp.Fill(dt);
                        return dt;
                    }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dt.Dispose();
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return dt;
        }
        // efficitive coding for future maintinance, all commands in one method called each time needed
        public int InsertUpdateDelete(string mySql) //7
        {
            /// <summary>
            /// Optimized for con Pooling
            /// </summary>
            int rtn = 0;
            using (con)  //.............1
            {
                using (SqlCommand cmd = new SqlCommand(mySql, con))  //..............2
                {
                    cmd.CommandType = CommandType.Text;
                    //foreach (KeyValuePair<string, object> p in myPara)
                    //{
                    //    cmd.Parameters.AddWithValue(p.Key, p.Value);
                    //}
                    con.Open();
                    rtn = cmd.ExecuteNonQuery();
                    return rtn;
                    // con.Close();  new
                }
            }
        }
        public int InsertUpdateDelete(string mySql, Dictionary<string, object> myPara) //8
        {
            int rtn = 0;
            using (SqlCommand cmd = new SqlCommand(mySql, con))
            {
                cmd.CommandType = CommandType.Text;
                foreach (KeyValuePair<string, object> p in myPara)
                {
                    cmd.Parameters.AddWithValue(p.Key, p.Value);
                }
                using (con)
                {
                    con.Open();
                    rtn = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return rtn;
        }

        public int InsertUpdateDeleteViaSqlDicRtnIdentity(string mySql, Dictionary<string, object> myPara) //9
        {
            Int32 newIdentityId = 000;
            try
            {
                using (SqlCommand cmd = new SqlCommand(mySql, con))
                {
                    cmd.CommandType = CommandType.Text;
                    foreach (KeyValuePair<string, object> p in myPara)
                    {
                        cmd.Parameters.AddWithValue(p.Key, p.Value);
                    }
                    using (con)
                    {
                        con.Open();
                        newIdentityId = (Int32.Parse(cmd.ExecuteScalar().ToString()));
                    }
                }
            }

            //catch (Exception ex)
            //{
            //    ex.Message.ToString();
            //}
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            //   return new List<Dictionary<string, object>>(); ////return new List<DataTable>();
            return newIdentityId;
        }
        public string getPk(string mySql, string ddlSelectedCategory, int ddlSelectedCategoryValue) //10
        {
            SqlCommand cmd = new SqlCommand(@mySql, con);
            cmd.Parameters.AddWithValue(ddlSelectedCategory, ddlSelectedCategoryValue);//@counterCategoryId    
            using (con)
            {
                con.Open();
                return cmd.ExecuteScalar().ToString();
            }
        }
        #region Calling Stored Procedure
        public DataSet select(string storedProcedure) // rtn to work on it for optimization   //11
        {
            try
            {
                cmd = new SqlCommand();
                ds = new DataSet();
                adp = new SqlDataAdapter();
                con = new SqlConnection(conStr);  // ConnectionString changed to Global.DB_CONN_STR
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (cmd.Connection.State == ConnectionState.Closed)
                    using (con)
                    {
                        cmd.Connection.Open();
                        //foreach (KeyValuePair<string, object> spData in param)
                        //{
                        //    cmd.Parameters.AddWithValue(spData.Key, spData.Value);
                        //}
                        //adp.SelectCommand = cmd;
                        adp.Fill(ds);
                        return ds;
                    }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ds.Dispose();
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return ds;
        }
        public DataSet select(string storedProcedure, Dictionary<string, object> spInputPara)  //12
        {
            try
            {
                cmd = new SqlCommand();
                ds = new DataSet();
                adp = new SqlDataAdapter();
                con = new SqlConnection(conStr);  // ConnectionString changed to Global.DB_CONN_STR
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (cmd.Connection.State == ConnectionState.Closed)
                    using (con)
                    {
                        cmd.Connection.Open();
                        foreach (KeyValuePair<string, object> spData in spInputPara)
                        {
                            cmd.Parameters.AddWithValue(spData.Key, spData.Value);
                        }
                        adp.SelectCommand = cmd;
                        adp.Fill(ds);
                        return ds;
                    }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ds.Dispose();
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return ds;
        }
        public string checkUserExist(string storedProcedure, Dictionary<string, object> spInputPara)  //13
        {
            string strUserName = "";
            try
            {
                cmd = new SqlCommand();
                adp = new SqlDataAdapter();
                con = new SqlConnection(conStr);  // ConnectionString changed to Global.DB_CONN_STR
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (cmd.Connection.State == ConnectionState.Closed)
                    using (con)
                    {
                        cmd.Connection.Open();
                        foreach (KeyValuePair<string, object> spData in spInputPara)
                        {
                            cmd.Parameters.AddWithValue(spData.Key, spData.Value);
                        }
                        strUserName = cmd.ExecuteScalar().ToString();
                    }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return strUserName;
        }
        public string checkUserExist(string storedProcedure, string myUser, string myAppName)  //14
        {
            string strUserName = "";
            try
            {
                cmd = new SqlCommand();
                adp = new SqlDataAdapter();
                con = new SqlConnection(conStr);  // ConnectionString changed to Global.DB_CONN_STR
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (cmd.Connection.State == ConnectionState.Closed)
                    using (con)
                    {
                        cmd.Connection.Open();
                        cmd.Parameters.AddWithValue("@userName", myUser);
                        cmd.Parameters.AddWithValue("@appName", myAppName);
                        strUserName = cmd.ExecuteScalar().ToString();
                    }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return strUserName;
        }
        private SqlDataReader testSp()  //15
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand("p_SearchDoc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@key", SqlDbType.VarChar).Value = "erp";
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    return dr;
                }
            }
        }
        public SqlDataReader getDrViaCmd(string storedProcedure, Dictionary<string, object> spInputPara)  //16
        {
            SqlDataReader dr = null;
            try
            {
                cmd = new SqlCommand();
                adp = new SqlDataAdapter();
                con = new SqlConnection(conStr);  // ConnectionString changed to Global.DB_CONN_STR
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (cmd.Connection.State == ConnectionState.Closed)
                    using (con)
                    {
                        cmd.Connection.Open();
                        foreach (KeyValuePair<string, object> spData in spInputPara)
                        {
                            cmd.Parameters.AddWithValue(spData.Key, spData.Value);
                        }
                        dr = cmd.ExecuteReader();
                    }
                return dr;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return dr;
        }
        #endregion
        public int checkUserExist2(string storedProcedure, Dictionary<string, object> spInputPara) //17
        {
            int docId = 0;
            try
            {
                cmd = new SqlCommand();
                adp = new SqlDataAdapter();
                con = new SqlConnection(conStr);  // ConnectionString changed to Global.DB_CONN_STR
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (cmd.Connection.State == ConnectionState.Closed)
                    using (con)
                    {
                        cmd.Connection.Open();
                        foreach (KeyValuePair<string, object> spData in spInputPara)
                        {
                            cmd.Parameters.AddWithValue(spData.Key, spData.Value);
                        }
                        docId = int.Parse(cmd.ExecuteScalar().ToString());
                   }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return docId;
        }
        public bool authenticateUser(string mySql, Dictionary<string, object> formValues)  //18
        {
            bool blnAuthenticate = false;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand(mySql, con);
            foreach (KeyValuePair<string, object> p in formValues)
            {
                // can put validation here to see if the value is empty or not 
                cmd.Parameters.AddWithValue(p.Key, p.Value);
            }
            using (con)
            {
                con.Open();
                dr = cmd.ExecuteReader();
                blnAuthenticate = dr.Read();// if has row, true, else false
                dr.Close();
                con.Close();
                return blnAuthenticate;
            }
        }
        // under test 
        public void populatComboViaDr(string mySql, DropDownList comboName, string myValue, string myText)  //19
        {
            //ddlDegree.Items.Clear();
            //ddlDegree.Items.Add("Select Degree");
            CRUD myCrud = new CRUD();
            using (SqlDataReader dr = myCrud.getDrPassSql(mySql))
            {
                comboName.DataTextField = myText.ToString();
                comboName.DataValueField = myValue.ToString();
                comboName.DataSource = dr;
                comboName.DataBind();
            }
        }

        #region contorl automation
        public void populateCombo(DropDownList myDDL, string mySql, string myDataValueField, string myDataTextField)
        {
            using (SqlDataReader dr = this.getDrPassSql(mySql))
            {
                myDDL.DataValueField = myDataValueField;
                myDDL.DataTextField = myDataTextField;
                myDDL.DataSource = dr;
                myDDL.DataBind();
            }
        }
        public void populateCombo(DropDownList myDDL, string mySql, string myDataValueField, string myDataTextField, Dictionary<string, object> myPara)
        {
            using (SqlDataReader dr = this.getDrPassSql(mySql, myPara))
            {
                myDDL.DataValueField = myDataValueField;
                myDDL.DataTextField = myDataTextField;
                myDDL.DataSource = dr;
                myDDL.DataBind();
            }
        }
        public void populateGv(GridView myGv, string mySql)
        {
            using (SqlDataReader dr = this.getDrPassSql(mySql))
            {
                myGv.DataSource = dr;
                myGv.DataBind();
            }
        }
        public void populateGvDr(GridView myGv, string mySql)
        {
            using (SqlDataReader dr = this.getDrPassSql(mySql))
            {
                myGv.DataSource = dr;
                myGv.DataBind();
            }
        }
        public void populateGvDT(GridView myGv, string mySql)
        {
            DataTable dt = this.getDT(mySql);
            myGv.DataSource = dt;
            myGv.DataBind();
        }
        public void populateGv(GridView myGv, string mySql, string myDataTextField, Dictionary<string, object> myPara)
        {
            using (SqlDataReader dr = this.getDrPassSql(mySql, myPara))
            {
                myGv.DataSource = dr;
                myGv.DataBind();
            }
        }
        public void populateRadio(RadioButton myDDL, string mySql, string myDataValueField, string myDataTextField)
        {
            using (SqlDataReader dr = this.getDrPassSql(mySql))
            {
                //myDDL.DataValueField = myDataValueField;
                //myDDL.DataTextField = myDataTextField;
                //myDDL.DataSource = dr;
                //myDDL.DataBind();
            }
        }
        #endregion

        // to solve max connection pool exceeded
        public static void clearAllPools()
        {
            SqlConnection.ClearAllPools();
        }


    }// Cls

}/// NS


