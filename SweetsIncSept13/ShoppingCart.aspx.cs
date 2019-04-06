using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetsIncSept13
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        private string strConn = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGridView();
                PrintMessage("Shopping Cart");
            }
        }

        private void LoadGridView()
        {
            HttpCookie custCookie = Request.Cookies["CustId"];

            if(custCookie == null)
            {
                return;
            }

            string custId = Request.Cookies["CustId"].Value;
            SqlDataReader dr = default(SqlDataReader);
            SqlCommand cmd = default(SqlCommand);

            try
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    cmd = new SqlCommand("GetCartByCustomerId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter cartIdParm = new SqlParameter();
                    cartIdParm.ParameterName = "@CustomerId";
                    cartIdParm.Value = custId;
                    cartIdParm.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(cartIdParm);
                    conn.Open();

                    dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    if (dr.HasRows)
                    {
                        cartGrid.DataSource = dr;
                        cartGrid.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                PrintMessage(ex.Message);
            }
            finally
            {
                dr.Close();

            }
        }

        private void PrintMessage(string msg)
        {
            Label lblMasterMessage = (Label)Master.FindControl("lblMasterMessage");

            lblMasterMessage.Text = msg;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            
            foreach(GridViewRow row in cartGrid.Rows)
            {
                string itemId = ((Label)row.Cells[0].FindControl("ItemId")).Text;
                string qty = ((TextBox)row.Cells[3].FindControl("Quantity")).Text;
                if (((CheckBox)row.Cells[6].FindControl("Remove")).Checked)
                {
                    SqlCommand cmd = new SqlCommand("deleteCartItem", new SqlConnection(strConn));
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter cartItemId = new SqlParameter();
                    cartItemId.ParameterName = "@ItemId";
                    cartItemId.Value = itemId;
                    cartItemId.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(cartItemId);

                    using (cmd.Connection)
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }
                else
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("updateCartItemQty", new SqlConnection(strConn));
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter itemIdParm = new SqlParameter();
                        itemIdParm.ParameterName = "@CartItemId";
                        itemIdParm.Value = itemId;
                        itemIdParm.SqlDbType = SqlDbType.Int;
                        SqlParameter qtyParm = new SqlParameter();
                        qtyParm.ParameterName = "@Qty";
                        qtyParm.Value = qty;
                        qtyParm.SqlDbType = SqlDbType.Int;
                        SqlParameter over = new SqlParameter();
                        over.ParameterName = "@Override";
                        over.Value = 1;
                        over.SqlDbType = SqlDbType.Bit;

                        cmd.Parameters.Add(over);
                        cmd.Parameters.Add(qtyParm);
                        cmd.Parameters.Add(itemIdParm);

                        using (cmd.Connection)
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            cmd.Connection.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        PrintMessage(ex.Message);
                    }
                }
            }

            LoadGridView();
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            Server.Transfer("~/Default.aspx");
        }
        
    }
}