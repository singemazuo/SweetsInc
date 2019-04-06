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
    public partial class index : System.Web.UI.Page
    {
        private string strConn = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillCategoryList();
                FillProductListWithFeatured();
            }

            lblMasterMessage.Text = "Products by Category";

            FillProductList();
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
                PrintErr(ex.Message);
            }
            finally
            {
                dr.Close();

            }
        }

        private void PrintErr(string err)
        {
            Label lblMasterMessage = (Label)Master.FindControl("lblMasterMessage");

            lblMasterMessage.Text = err;
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
                        //
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            finally
            {
                dr.Close();
            }
        }

        private void FillProductList()
        {
            string catId = Request.QueryString["id"];

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
                
            }
            finally
            {
                dr.Close();

            }
        }
    }
}