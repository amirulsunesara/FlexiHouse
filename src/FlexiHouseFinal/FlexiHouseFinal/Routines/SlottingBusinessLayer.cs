using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FlexiHouseFinal.Routines
{
    public class SlottingBusinessLayer
    {
        String connectionString = ConfigurationManager.ConnectionStrings["UserDBContext"].ConnectionString;

      

        public void updateShelfSlotting(int id, string jsonstring)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("Update shelf SET shelfItems=@jsonstring Where(id=@id)", con);
                cmd.Parameters.AddWithValue("@jsonstring", jsonstring);
                cmd.Parameters.AddWithValue("@id", id);
                



                con.Open();
                cmd.ExecuteNonQuery();

            }

        }
        public void UpdateConsignmentInstruction(int id,string instruction)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("Update Consignment SET instruction=@instruction Where(id=@id)", con);
                cmd.Parameters.AddWithValue("@instruction",instruction );
                cmd.Parameters.AddWithValue("@id", id);




                con.Open();
                cmd.ExecuteNonQuery();

            }

        }


    }
}