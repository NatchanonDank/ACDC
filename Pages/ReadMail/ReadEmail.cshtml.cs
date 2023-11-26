using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using static Humanizer.In;

namespace FinalProject.Pages
{
    public class ReadEmail : PageModel

    {
        public EmailInfo Emails { get; set; }
        public void OnGet()
        {
            string id = Request.Query["emailid"];//เอา id มาเก็บค่า id ของ email ผ่าน Request.Query จาก Index
            try
            {
                String connectionString = "Server=tcp:acdcproject.database.windows.net,1433;Initial Catalog=ACDC;Persist Security Info=False;User ID=acdc;Password=@Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))//ประกาศmethod ในการ connect กับ database
                {
                    connection.Open();//เรียกใช้ Method connection ในการเชื่อต่อ database

                    String sql = "SELECT * FROM emails WHERE emailid=@emailid";//ประกาศตัว คำสั่งsql ให้เรียกข้อมูลจากdatabase โดยเอาข้อมูลของ emailid นั้นๆ
                    using (SqlCommand command = new SqlCommand(sql, connection))//ประกาศ method SqlCommand เพื่อที่จะรัน CommandของSql
                    {
                        command.Parameters.AddWithValue("@emailid", id);//ส่งค่า id เข้้าไปแทน @emailid ในตัวแปร sql เพื่อที่จะส่งไปเช็คว่าจะต้องดึงข้อมูลของ เมลไหน
                        using (SqlDataReader reader = command.ExecuteReader())//ประกาศเรียกใช้Method Read ข้อมูลจากDatabaseที่เราอ่านได้
                        {
                            if(reader.Read()) {
                                Emails = new EmailInfo
                                {
                                    EmailID = reader.GetInt32(0).ToString(),
                                    EmailSubject = reader.GetString(1),
                                    EmailMessage = reader.GetString(2),
                                    EmailDate = reader.GetDateTime(3).ToString(),
                                    EmailIsRead = reader.GetString(4),
                                    EmailSender = reader.GetString(5),
                                    EmailReceiver = reader.GetString(6)
                                };//ตรงนี้เป็นการนำข้อมูลจากในDatabaseมาเก็บไว้ในตัวแปรต่างๆผ่านMethod .Get ต่าง 
                                    String upconnectionString = "Server=tcp:acdcproject.database.windows.net,1433;Initial Catalog=ACDC;Persist Security Info=False;User ID=acdc;Password=@Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                                    using (SqlConnection upconnection = new SqlConnection(upconnectionString))//ประกาศmethod ในการ connect กับ database
                                    {
                                        upconnection.Open();//เรียกใช้ Method connection ในการเชื่อต่อ database

                                        String upsql = "UPDATE emails SET emailisread=@read WHERE emailid=@emailid";//ประกาศตัว คำสั่งsql ให้อัพเดตหรือเปลี่ยนแปลงข้อในColum emailisread โดยผ่านemailsidของ emailที่จะ อัพเดต
                                        using (SqlCommand upcommand = new SqlCommand(upsql, upconnection))//ประกาศ method SqlCommand เพื่อที่จะรัน CommandของSql
                                        {
                                            upcommand.Parameters.AddWithValue("@emailid", id);//เอา id เข้าไปแทน @emailid ในคำสั่งของSql
                                            upcommand.Parameters.AddWithValue("@read", 1);//เอาค่า 1 เข้าไปแทน @read ในคำสั่้งของ Sql
                                            upcommand.ExecuteNonQuery();//เป็นการรันCommand ของ Sql ทั้งหมด เพื่อที่จะอัพเดต

                                        }
                                    };
                            }
                         

                        }
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void OnPost()//ประกาศเป็นฟังก์ชั่น เพื่อที่จะเอาไปเรียกใช้ใน Onget()
        {
        }
        public class EmailInfo
        {
            public String EmailID;
            public String EmailSubject;
            public String EmailMessage;
            public String EmailDate;
            public String EmailIsRead;
            public String EmailSender;
            public String EmailReceiver;
        }
    }

}
