using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FinalProject.Pages.ComposeMail
{
    public class ComposeEmailModel : PageModel
    {
        public string ErrorMessage = "";
        public string SuccessMessage = "";
        public List<Name> ListName = new List<Name>();
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

                    String sql = "SELECT UserName FROM AspNetUsers ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Name name = new Name
                                {
                                    name = reader.GetString(0)
                                };
                                ListName.Add(name);
                            }
                        }
                    }
                    if (ListName.Exists(n => n.name == email.EmailReceiver))//cheack receiver name that is in database ?
                    {
                        string insertSql = "INSERT INTO emails (emailsubject, emailmessage, emailisread, emailsender, emailreceiver) " +
                                           "VALUES (@subject, @message, @read, @sender, @receiver)";
                        using (SqlCommand insertCommand = new SqlCommand(insertSql, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@subject", email.EmailSubject);
                            insertCommand.Parameters.AddWithValue("@message", email.EmailMessage);
                            insertCommand.Parameters.AddWithValue("@read", 0);
                            insertCommand.Parameters.AddWithValue("@sender", sender);
                            insertCommand.Parameters.AddWithValue("@receiver", email.EmailReceiver);
                            insertCommand.ExecuteNonQuery();
                        }

                        SuccessMessage = "Email sent successfully!";
                    }
                    else
                    {
                        ErrorMessage = "UserName Not Found.";
                    }

                };
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
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
    public class Name
    {
        public String name;
    }
}
