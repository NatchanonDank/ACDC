using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace FinalProject.Pages
{
    public class ReadEmail : PageModel

    {
        public EmailInfo Emails { get; set; }
        public void OnGet()
        {
            string id = Request.Query["emailid"];
            try
            {
                String connectionString = "Server=tcp:acdcproject.database.windows.net,1433;Initial Catalog=ACDC;Persist Security Info=False;User ID=acdc;Password=@Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "SELECT * FROM emails WHERE emailid=@emailid";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@emailid", id);
                        using (SqlDataReader reader = command.ExecuteReader())
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
