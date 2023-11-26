using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FinalProject.Pages.ComposeMail
{
    public class ComposeEmailModel : PageModel
    {
        public String errormessage = "";
        public String successmessage = "";

        public void OnGet()
        {
        }
        public void OnPost() {
            String sender = Request.Query["Sender"];
            EmailInfo email = new EmailInfo();
            email.EmailReceiver = Request.Form["receiver"];
            email.EmailSubject = Request.Form["subject"];
            email.EmailMessage = Request.Form["message"];
                try
            {
                String connectionString = "Server=tcp:acdcproject.database.windows.net,1433;Initial Catalog=ACDC;Persist Security Info=False;User ID=acdc;Password=@Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))//ประกาศmethod ในการ connect กับ database
                {
                    connection.Open();//เรียกใช้ Method connection ในการเชื่อต่อ database

                    String sql = "INSERT INTO emails" +
                        "(emailsubject,emailmessage,emailisread,emailsender,emailreceiver)VALUES" +
                        "(@subject,@message,@read,@sender,@receiver);"; ;//ประกาศตัว คำสั่งsql ให้เรียกข้อมูลจากdatabase โดยเอาข้อมูลของ emailid นั้นๆ
                    using (SqlCommand command = new SqlCommand(sql, connection))//ประกาศ method SqlCommand เพื่อที่จะรัน CommandของSql
                    {
                        command.Parameters.AddWithValue("@subject", email.EmailSubject);
                        command.Parameters.AddWithValue("@message", email.EmailMessage);
                        command.Parameters.AddWithValue("@read", 0);
                        command.Parameters.AddWithValue("@sender", sender);
                        command.Parameters.AddWithValue("@receiver",email.EmailReceiver);
                        command.ExecuteNonQuery();


                    }
                };
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return;
            }
            email.EmailReceiver = "";
            email.EmailSubject = "";
            email.EmailMessage = "";
        }
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
