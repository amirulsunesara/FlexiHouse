using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;

using System.Configuration;


namespace BusinessLayer
{
   public class WarehouseBusinessLayer
    {
        String connectionString = ConfigurationManager.ConnectionStrings["UserDBContext"].ConnectionString;
        public void UpdateWareHouse(WarehouseBL warehouse)
        {
           
          
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("spUpdateWareHouse", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@warehouseHTML";
                byte[] theBytes = Encoding.UTF8.GetBytes(warehouse.warehouseHtml);
                //  string ss = Encoding.UTF8.GetString(theBytes);

                paramName.Value = theBytes;
                cmd.Parameters.Add(paramName);


                SqlParameter paramName7 = new SqlParameter();
                paramName7.ParameterName = "@managerId";
                paramName7.Value = warehouse.managerId;
                cmd.Parameters.Add(paramName7);




                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateShelf(Shelves shelves)
        {


            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("spUpdateShelf", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@zone";
                paramName.Value = shelves.zone;
                cmd.Parameters.Add(paramName);


                SqlParameter paramName7 = new SqlParameter();
                paramName7.ParameterName = "@ShelfName";
                paramName7.Value = shelves.shelveID;
                cmd.Parameters.Add(paramName7);




                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    
        public void AddShelf(Shelves shelves)
        {


            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("spAddShelf", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@zone";
                paramName.Value = shelves.zone;
                cmd.Parameters.Add(paramName);

                SqlParameter paramName11 = new SqlParameter();
                paramName11.ParameterName = "@warehouse_id";
                paramName11.Value = shelves.warehouse_id;
                cmd.Parameters.Add(paramName11);



                SqlParameter paramName7 = new SqlParameter();
                paramName7.ParameterName = "@ShelfName";
                paramName7.Value = shelves.shelveID;
                cmd.Parameters.Add(paramName7);

                SqlParameter paramName44 = new SqlParameter();
                paramName44.ParameterName = "@shelfItems";
                paramName44.Value = shelves.shelfItems;
                cmd.Parameters.Add(paramName44);


                SqlParameter paramNamekk = new SqlParameter();
                paramNamekk.ParameterName = "@slotsRemaining";
                paramNamekk.Value = shelves.slotsRemaining;
                cmd.Parameters.Add(paramNamekk);



                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public int getWarehouseId(int uid)
        {

            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("Select id From Warehouse Where managerId=@id", con);
                cmd.Parameters.AddWithValue("@id",uid);



                con.Open();
               int id = Convert.ToInt32(cmd.ExecuteScalar());
                return id;
            }

        }
        public int getRecentlyId()
        {

            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("Select Max(userID) From UserAccounts", con);
               


             con.Open();
             int id= Convert.ToInt32(cmd.ExecuteScalar());
                return id;
            }

        }
        public void createWarehouse(int managerId,string warehousename,string warehouseaddress,string warehouselogo,string countries)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("Insert into Warehouse(managerId,warehouseName,warehouseAddress,warehouseLogo,country) values(@id,@warehousename,@warehouseaddress,@warehouselogo,@countries)", con);
                cmd.Parameters.AddWithValue("@id", managerId);
                cmd.Parameters.AddWithValue("@warehousename", warehousename);
                cmd.Parameters.AddWithValue("@warehouseaddress", warehouseaddress);
                cmd.Parameters.AddWithValue("@warehouselogo", warehouselogo);
                cmd.Parameters.AddWithValue("@countries", countries);
                con.Open();
                cmd.ExecuteNonQuery();
            
               
            }

        }

       

        public DataSet getWarehouseDetails(int managerId)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("select * from Warehouse where managerId=@id", con);
                cmd.Parameters.AddWithValue("@id", managerId);


                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(ds);
                

            }
            return ds;

        }
        public void UpdateStartWareHouse(WarehouseBL wareHouse)
        {

     

            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("spUpdateStartWareHouse", con);
                cmd.CommandType = CommandType.StoredProcedure;

         //       SqlParameter paramName99 = new SqlParameter();
         //   //    paramName99.ParameterName = "@warehouseHTML";
         //       byte[] theBytes = Encoding.UTF8.GetBytes("");
         //       paramName99.Value = theBytes;
         //       cmd.Parameters.Add(paramName99);

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@warehouseWidth";
                paramName.Value = wareHouse.warehouseWidth ;
                cmd.Parameters.Add(paramName);

                SqlParameter paramName2 = new SqlParameter();
                paramName2.ParameterName = "@warehouseLength";
                paramName2.Value = wareHouse.warehouseLength;
                cmd.Parameters.Add(paramName2);

                SqlParameter paramName3 = new SqlParameter();
                paramName3.ParameterName = "@shelveLength";
                paramName3.Value = wareHouse.shelveLength;
                cmd.Parameters.Add(paramName3);

                SqlParameter paramName4 = new SqlParameter();
                paramName4.ParameterName = "@shelveWidth";
                paramName4.Value = wareHouse.shelveWidth;
                cmd.Parameters.Add(paramName4);

                SqlParameter paramName5 = new SqlParameter();
                paramName5.ParameterName = "@shelveHeight";
                paramName5.Value = wareHouse.shelveHeight ;
                cmd.Parameters.Add(paramName5);

                SqlParameter paramName6 = new SqlParameter();
                paramName6.ParameterName = "@shelveRows";
                paramName6.Value = wareHouse.shelveRows;
                cmd.Parameters.Add(paramName6);


                SqlParameter paramName11 = new SqlParameter();
                paramName11.ParameterName = "@scaledWarehouseLength";
                paramName11.Value = wareHouse.scaledWarehouseLength;
                cmd.Parameters.Add(paramName11);


                SqlParameter paramName12 = new SqlParameter();
                paramName12.ParameterName = "@scaledWarehouseWidth";
                paramName12.Value = wareHouse.scaledWarehouseWidth;
                cmd.Parameters.Add(paramName12);

                SqlParameter paramName13 = new SqlParameter();
                paramName13.ParameterName = "@scaledShelfLength";
                paramName13.Value = wareHouse.scaledShelfLength;
                cmd.Parameters.Add(paramName13);

                SqlParameter paramName14 = new SqlParameter();
                paramName14.ParameterName = "@scaledShelfWidth";
                paramName14.Value = wareHouse.scaledShelfWidth;
                cmd.Parameters.Add(paramName14);

                SqlParameter paramName1444 = new SqlParameter();
                paramName1444.ParameterName = "@shelfSlots";
                paramName1444.Value = wareHouse.shelfSlots;
                cmd.Parameters.Add(paramName1444);

                SqlParameter paramName555 = new SqlParameter();
                paramName555.ParameterName = "@sections";
                paramName555.Value = wareHouse.sections;
                cmd.Parameters.Add(paramName555);

                SqlParameter paramName7 = new SqlParameter();
                paramName7.ParameterName = "@managerId";
                paramName7.Value = wareHouse.managerId;
                cmd.Parameters.Add(paramName7);
   



                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool getWarehouseAttr(int v)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("select * from Warehouse where managerId=@id", con);
                cmd.Parameters.AddWithValue("@id", v);

                DataSet ds = new DataSet();
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(ds);
                bool dr = ds.Tables[0].Rows[0].IsNull(2);
                if (dr)
                {
                    return false;
                }
                else
                {
                    return true;

                }

            }
        }

        public byte[] getWarehouse(int id)
        {

          
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spSelectWarehouse", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@managerId";
                paramName.Value = id;
                cmd.Parameters.Add(paramName);



                con.Open();
                return cmd.ExecuteScalar() as Byte[];
            }
        }

    }
}
