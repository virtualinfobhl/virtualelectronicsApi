using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ClsConnection
/// </summary>
public class ClsConnection
{
    SqlBulkCopy Bcp;
    public SqlConnection Con;
    public SqlCommand Cmd;
    public SqlDataReader Dr;
    public DataSet Ds;
    SqlDataAdapter Add;
    SqlTransaction Tran;
    //public event  SqlRowsCopied(  Object  Sender,  System.Data.SqlClient.SqlRowsCopiedEventArgs e);
    public ClsConnection()
    {
        Con = new SqlConnection();
        Con.ConnectionString = ConfigurationManager.ConnectionStrings["VirtualInfoSystems"].ConnectionString;       
        Con.Open();
    }    
    public void cancel()
    {
        Cmd.Cancel();
    }
    public void class1(string SqlCon)
    {
        Con = new SqlConnection();
        Con.ConnectionString = SqlCon;
        Con.Open();
    }
    public bool Open()
    {
        if (Con.State != ConnectionState.Open)
        {
            try
            {
                Con.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        else
            return true;

    }
    public void Bulkcopy(string Destination, DataTable DataSource, int NotifyAfter)
    {
        try
        {
            Bcp = new SqlBulkCopy(Con);
            Bcp.BulkCopyTimeout = 300;
            Bcp.DestinationTableName = Destination;
            Bcp.NotifyAfter = NotifyAfter;
            Bcp.WriteToServer(DataSource);
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }   
    public int MaxValue(string Table, string Column, string DefaultValue)
    {
        return (MaxValue(Table, Column, DefaultValue, ""));
    }
    public int MaxValue(string Table, string Column, string DefaultValue, string Condition)
    {
        return (Convert.ToInt32(ExecuteScalar("Select isnull(max(" + Column + ")," + DefaultValue + ") from " + Table + (Condition == "" ? "" : " Where ") + Condition)));


    }
    public DataRow FillRow(string Sqlstr)
    {
        CommandText(Sqlstr);
        SqlDataAdapter Ad = new SqlDataAdapter();
        Ad.SelectCommand = Cmd;
        DataSet LDs = new DataSet();
        Ad.Fill(LDs, "Table");
        if (LDs.Tables["Table"].Rows.Count > 0)
            return ((LDs.Tables["Table"].Rows[0]));
        else
            return (null);

    }
    public void FillDataSet(DataSet LDataSet, string Sqlstr, string TableName)
    {
        CommandText(Sqlstr);
        SqlDataAdapter Ad = new SqlDataAdapter();

        Ad.SelectCommand = Cmd;
        Ad.Fill(LDataSet, TableName);
    }
    public void ClearDataSet(DataSet LDataSet)
    {
        LDataSet.Clear();
    }
    public void BeginTrans()
    {
        Tran = Con.BeginTransaction();
    }
    public void CommitTrans()
    {
        Tran.Commit();
    }
    public void RollBackTrans()
    {
        Tran.Rollback();
    }
    public void CommandText(string SqlStr)
    {
        Cmd = new SqlCommand();
        Cmd.CommandText = SqlStr;
        Cmd.Connection = Con;
        Cmd.Transaction = Tran;
    }
    
    public int ExecuteNonQuery(string SqlStr)
    {
        CommandText(SqlStr);
        return (Cmd.ExecuteNonQuery());
    }
    public object ExecuteScalar(string SqlStr)
    {
        CommandText(SqlStr);
        return (Cmd.ExecuteScalar());
    }
    public int ExecuteLineReader(string SqlStr)
    {
        CommandText(SqlStr);
        Dr = Cmd.ExecuteReader();
        if (Dr.Read())
            return (1);
        else
            return (0);
    }
    public DataTable FillTable(string Sqlstr, string TableName)
    {
        CommandText(Sqlstr);
        SqlDataAdapter Ad = new SqlDataAdapter();
        Ad.SelectCommand = Cmd;
        Ds = new DataSet();
        Ad.Fill(Ds, TableName);
        return (Ds.Tables[TableName]);
    }
    public DataTable ExecuteProc(string ProcName, params object[] Parameter)
    {
        CommandText(ProcName);
        Cmd.CommandType = CommandType.StoredProcedure;
        int i = 0;
        while (i < Parameter.Length)
        {
            Cmd.Parameters.Add(Cmd.CreateParameter());
            Cmd.Parameters[i].ParameterName = Convert.ToString(Parameter[i]);
            Cmd.Parameters[i].Value = Parameter[i * 2 + 1];
            i = i + 1;
        }
        SqlDataAdapter Ad = new SqlDataAdapter();
        Ad.SelectCommand = Cmd;
        Ds = new DataSet();
        Ad.Fill(Ds, ProcName);
        return (Ds.Tables[ProcName]);
    }
    public void CloseReader()
    {
        Dr.Close();
    }
    public void Close()
    {
        if (Con.State != ConnectionState.Closed)
            Con.Close();
    }
    
}
