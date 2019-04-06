using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetsIncSept13
{
    public partial class Default : System.Web.UI.Page
    {
        private string strConn = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

        void Page_PreInit(object sender, EventArgs e)
        {
            MasterPageFile = "~/SiteTemplate.master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["CustId"].Value = "977";
            Response.Cookies["CustId"].Expires = DateTime.Now.AddMinutes(5);

            FillCategoryList();
            string pid = Request.QueryString["pid"];
            string catid = Request.QueryString["catid"];

            if (!string.IsNullOrEmpty(pid))
            {
                FillDetailedItem();
                Panel pnlDetails = (Panel)(rptProducts.Items[0].FindControl("prodDetails"));
                pnlDetails.Visible = true;
                prodAdd.Visible = true;
                PrintMessage("Full Product Details");
            }
            
            else if(!string.IsNullOrEmpty(catid))
            {
                FillProductListByCat();
                PrintMessage("Products in Selected Category");
            }
            else if (!IsPostBack)
            {
                FillProductListWithFeatured();
                PrintMessage("Featured Products");
            }

        }

        private void FillDetailedItem()
        {
            string pid = Request.QueryString["pid"];

            SqlDataReader dr = default(SqlDataReader);
            SqlCommand cmd = default(SqlCommand);

            try
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    cmd = new SqlCommand("getFullProductDetails", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@prodID", pid);
                    conn.Open();

                    dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    if (dr.HasRows)
                    {
                        rptProducts.DataSource = dr;
                        rptProducts.DataBind();
                        return;
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

        private void FillProductListWithFeatured()
        {
            SqlDataReader dr = default(SqlDataReader);
            SqlCommand cmd = default(SqlCommand);

            try
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    cmd = new SqlCommand("getFeaturedProducts", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    if (dr.HasRows)
                    {
                        rptProducts.DataSource = dr;
                        rptProducts.DataBind();
                        return;
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

        private void FillCategoryList()
        {
            SqlDataReader dr = default(SqlDataReader);
            SqlCommand cmd = default(SqlCommand);

            try
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    cmd = new SqlCommand("getAllCategories", conn);
                    conn.Open();

                    dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    if (dr.HasRows)
                    {
                        rptCat.DataSource = dr;
                        rptCat.DataBind();
                    }
                    else
                    {
                        
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

        private void FillProductListByCat()
        {
            string catId = Request.QueryString["catid"];

            if (string.IsNullOrEmpty(catId))
            {
                return;
            }

            SqlDataReader dr = default(SqlDataReader);
            SqlCommand cmd = default(SqlCommand);

            try
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    cmd = new SqlCommand("getProdByCat", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@category", catId);
                    conn.Open();

                    dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    if (dr.HasRows)
                    {
                        rptProducts.DataSource = dr;
                        rptProducts.DataBind();
                        return;
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

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("addProductToCart", new SqlConnection(strConn));
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter productIdParm = new SqlParameter();
                productIdParm.ParameterName = "@ProductId";
                productIdParm.Value = Request.QueryString["pid"];
                productIdParm.SqlDbType = SqlDbType.Int;
                SqlParameter customerIdParm = new SqlParameter();
                customerIdParm.ParameterName = "@CustomerId";
                customerIdParm.Value = Request.Cookies["CustId"].Value;
                customerIdParm.SqlDbType = SqlDbType.Int;
                cmd.Parameters.Add(customerIdParm);
                cmd.Parameters.Add(productIdParm);

                using (cmd.Connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

                Server.Transfer("~/ShoppingCart.aspx");
            }
            catch (Exception ex)
            {
                PrintMessage(ex.Message);
            }
            
        }
    }
}