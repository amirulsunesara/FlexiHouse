using FlexiHouseFinal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace FlexiHouseFinal.Models
{
    public class OrderBusinessLayer { 


        String connectionString = ConfigurationManager.ConnectionStrings["UserDBContext"].ConnectionString;


        public void updateShelf(int id, string shelfItems)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("Update [Shelf] SET shelfItems=@items  Where id=@id", con);

                cmd.Parameters.AddWithValue("@items", shelfItems);
                cmd.Parameters.AddWithValue("@id", id);




                con.Open();
                cmd.ExecuteNonQuery();

            }

        }






        public void updateOrder(int id, string instruction, int maxid)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("Update [Order] SET instruction=@instruction, orderStatus='Dispatched',dispatchNo=@dispatch Where orderId=@id", con);

                cmd.Parameters.AddWithValue("@instruction", instruction);
                cmd.Parameters.AddWithValue("@dispatch", maxid);
                cmd.Parameters.AddWithValue("@id", id);




                con.Open();
                cmd.ExecuteNonQuery();

            }

        }


    }
}