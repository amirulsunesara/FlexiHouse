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
    public class ConsignmentBusinessLayer
    {
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






        public void updateOrder(int id,string instruction,int maxid)
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
        public List<Warehouse> getNameAddress(string name)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("select * from [Warehouse] where( country = @country) ", con);
                cmd.Parameters.AddWithValue("@country", name);
             

                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(ds);


            }
            List<Warehouse> list = ds.Tables[0].AsEnumerable().Select(r => new Warehouse()
            {
                id=(int)r["id"],
                warehouseName = (string)r["warehouseName"],
                warehouseAddress = (string)r["warehouseAddress"]
            }
    ).ToList();
            return list;

        }


        public DataSet getUsers(string username,string email)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("select * from [UserAccounts] where( UserName = @Username OR Email=@Email) ", con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(ds);


            }
            return ds;

        }
        public Item getItem(int id)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("select * from [Item] where( id = @id) ", con);
                cmd.Parameters.AddWithValue("@id", id);
              
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(ds);


            }
            Item it = new Item();
            DataRow dr = ds.Tables[0].Rows[0];

            it.id = Convert.ToInt32(dr["id"]);
            it.itemName = dr["itemName"].ToString();
            it.itemCode = dr["itemCode"].ToString();
            it.Manufacturer = dr["Manufacturer"].ToString();

            return it;


        }
        public Item getItem(int id,int quantity)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("select * from [Item] where( id = @id ) ", con);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(ds);


            }
            Item it = new Item();
            DataRow dr = ds.Tables[0].Rows[0];

            it.id = Convert.ToInt32(dr["id"]);
            it.itemName = dr["itemName"].ToString();
            it.itemCode = dr["itemCode"].ToString();
            it.Manufacturer = dr["Manufacturer"].ToString();
            it.Country = quantity.ToString();
            return it;


        }
        public void addItems(Item it)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("spAddItems", con);
                cmd.CommandType = CommandType.StoredProcedure;
                    
                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@id";
                paramName.Value = it.id;
                cmd.Parameters.Add(paramName);

                SqlParameter paramName2 = new SqlParameter();
                paramName2.ParameterName = "@itemName";
                paramName2.Value = it.itemName;
                cmd.Parameters.Add(paramName2);

                SqlParameter paramName3 = new SqlParameter();
                paramName3.ParameterName = "@Manufacturer";
                paramName3.Value = it.Manufacturer;
                cmd.Parameters.Add(paramName3);

                SqlParameter paramName4 = new SqlParameter();
                paramName4.ParameterName = "@Country";
                paramName4.Value = it.Country;
                cmd.Parameters.Add(paramName4);

                SqlParameter paramName5 = new SqlParameter();
                paramName5.ParameterName = "@itemCode";
                paramName5.Value = it.itemCode;
                cmd.Parameters.Add(paramName5);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void addAccount(UserAccount it)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("spAddAccount", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@name";
                paramName.Value = it.Name;
                cmd.Parameters.Add(paramName);

                SqlParameter paramName2 = new SqlParameter();
                paramName2.ParameterName = "@role";
                paramName2.Value = it.Role;
                cmd.Parameters.Add(paramName2);

                SqlParameter paramName3 = new SqlParameter();
                paramName3.ParameterName = "@Email";
                paramName3.Value = it.Email;
                cmd.Parameters.Add(paramName3);

                SqlParameter paramName4 = new SqlParameter();
                paramName4.ParameterName = "@contact";
                paramName4.Value = it.Contact;
                cmd.Parameters.Add(paramName4);

                SqlParameter paramName5 = new SqlParameter();
                paramName5.ParameterName = "@registered";
                paramName5.Value = it.Registered;
                cmd.Parameters.Add(paramName5);

                SqlParameter paramName6 = new SqlParameter();
                paramName6.ParameterName = "@username";
                paramName6.Value = it.UserName;
                cmd.Parameters.Add(paramName6);

                SqlParameter paramName7 = new SqlParameter();
                paramName7.ParameterName = "@password";
                paramName7.Value = it.Password;
                cmd.Parameters.Add(paramName7);

                if (it.workerId != null)
                {
                    SqlParameter paramName766 = new SqlParameter();
                    paramName766.ParameterName = "@workerId";
                    paramName766.Value = it.workerId;
                    cmd.Parameters.Add(paramName766);
                }
                else
                {
                    SqlParameter paramName766 = new SqlParameter();
                    paramName766.ParameterName = "@workerId";
                    paramName766.Value = 1;
                    cmd.Parameters.Add(paramName766);

                }

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void addConsignment(Consignment con)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("spAddConsignment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@id";
                paramName.Value = con.id;
                cmd.Parameters.Add(paramName);

                SqlParameter paramName2 = new SqlParameter();
                paramName2.ParameterName = "@warehouseId";
                paramName2.Value = con.warehouseId;
                cmd.Parameters.Add(paramName2);

                SqlParameter paramName3 = new SqlParameter();
                paramName3.ParameterName = "@supplier";
                paramName3.Value = con.supplier;
                cmd.Parameters.Add(paramName3);

                SqlParameter paramName4 = new SqlParameter();
                paramName4.ParameterName = "@totalItems";
                paramName4.Value = con.totalItems;
                cmd.Parameters.Add(paramName4);

                SqlParameter paramName5 = new SqlParameter();
                paramName5.ParameterName = "@arrivalDate";
                paramName5.Value = con.arrivalDate;
                cmd.Parameters.Add(paramName5);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }
        public void addItem_Consignment(Item_Consignment icon)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("spAddItem_Consignment", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@itemId";
                paramName.Value = icon.itemId;
                cmd.Parameters.Add(paramName);

                SqlParameter paramName2 = new SqlParameter();
                paramName2.ParameterName = "@consignmentId";
                paramName2.Value = icon.consignmentId;
                cmd.Parameters.Add(paramName2);

                SqlParameter paramName3 = new SqlParameter();
                paramName3.ParameterName = "@quantity";
                paramName3.Value = icon.quantity;
                cmd.Parameters.Add(paramName3);

                if (icon.expiry == null)
                {
                    
                   SqlParameter paramName4 = new SqlParameter();
                    paramName4.ParameterName = "@expiry";
                    paramName4.Value = DBNull.Value;
                    cmd.Parameters.Add(paramName4);
                }
                else
                {
                    SqlParameter paramName4 = new SqlParameter();
                    paramName4.ParameterName = "@expiry";
                    paramName4.Value = icon.expiry;
                    cmd.Parameters.Add(paramName4);

                }
               
                con.Open();
                cmd.ExecuteNonQuery();
            }

        }
        public void UpdateItem_Consignment(Item_Consignment icon)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("spUpdateItem_Consignment", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@itemId";
                paramName.Value = icon.itemId;
                cmd.Parameters.Add(paramName);

                SqlParameter paramName2 = new SqlParameter();
                paramName2.ParameterName = "@consignmentId";
                paramName2.Value = icon.consignmentId;
                cmd.Parameters.Add(paramName2);

                SqlParameter paramName3 = new SqlParameter();
                paramName3.ParameterName = "@quantity";
                paramName3.Value = icon.quantity;
                cmd.Parameters.Add(paramName3);

                SqlParameter paramName4 = new SqlParameter();
                paramName4.ParameterName = "@expiry";
                paramName4.Value = icon.expiry;
                cmd.Parameters.Add(paramName4);


                con.Open();
                cmd.ExecuteNonQuery();
            }

        }
        public void UpdateConsignment(Consignment con)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("spUpdateConsignment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@id";
                paramName.Value = con.id;
                cmd.Parameters.Add(paramName);

                SqlParameter paramName2 = new SqlParameter();
                paramName2.ParameterName = "@warehouseId";
                paramName2.Value = con.warehouseId;
                cmd.Parameters.Add(paramName2);

                SqlParameter paramName3 = new SqlParameter();
                paramName3.ParameterName = "@supplier";
                paramName3.Value = con.supplier;
                cmd.Parameters.Add(paramName3);

                SqlParameter paramName4 = new SqlParameter();
                paramName4.ParameterName = "@totalItems";
                paramName4.Value = con.totalItems;
                cmd.Parameters.Add(paramName4);

                SqlParameter paramName5 = new SqlParameter();
                paramName5.ParameterName = "@arrivalDate";
                paramName5.Value = con.arrivalDate;
                cmd.Parameters.Add(paramName5);

                conn.Open();
                cmd.ExecuteNonQuery();
            }



        }

        public void updateItemsCount(int count,int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("Update Consignment SET totalItems=@totalItems Where id=@id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@totalItems", count);



                con.Open();
                cmd.ExecuteNonQuery();

            }
        }

        public void updateItemWarehouse(Item_Warehouse iw)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("Update Item_Warehouse SET quantity=@quantity Where(itemId=@itemId AND warehouseId=@warehouseId)", con);
                cmd.Parameters.AddWithValue("@quantity", iw.quantity);
                cmd.Parameters.AddWithValue("@itemId", iw.itemId);
                cmd.Parameters.AddWithValue("@warehouseId", iw.warehouseId);



                con.Open();
                cmd.ExecuteNonQuery();

            }

        }

        public void addItemWarehouse(Item_Warehouse iw)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                SqlCommand cmd = new SqlCommand("Insert into Item_Warehouse values(@itemId,@warehouseId,@quantity,@orders)", con);

                cmd.Parameters.AddWithValue("@itemId", iw.itemId);
                cmd.Parameters.AddWithValue("@warehouseId", iw.warehouseId);
                cmd.Parameters.AddWithValue("@quantity", iw.quantity);
                cmd.Parameters.AddWithValue("@orders", iw.orders);

                con.Open();
                cmd.ExecuteNonQuery();

            }
        }
    }
}