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
            string id = Request.Query["emailid"];//��� id ���纤�� id �ͧ email ��ҹ Request.Query �ҡ Index
            try
            {
                String connectionString = "Server=tcp:acdcproject.database.windows.net,1433;Initial Catalog=ACDC;Persist Security Info=False;User ID=acdc;Password=@Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))//��С��method 㹡�� connect �Ѻ database
                {
                    connection.Open();//���¡�� Method connection 㹡�����͵�� database

                    String sql = "SELECT * FROM emails WHERE emailid=@emailid";//��С�ȵ�� �����sql ������¡�����Ũҡdatabase ����Ң����Ţͧ emailid ����
                    using (SqlCommand command = new SqlCommand(sql, connection))//��С�� method SqlCommand ���ͷ����ѹ Command�ͧSql
                    {
                        command.Parameters.AddWithValue("@emailid", id);//�觤�� id �����᷹ @emailid 㹵���� sql ���ͷ����������Ҩе�ͧ�֧�����Ţͧ ����˹
                        using (SqlDataReader reader = command.ExecuteReader())//��С�����¡��Method Read �����ŨҡDatabase��������ҹ��
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
                                };//�ç����繡�ùӢ����Ũҡ�Database�������㹵���õ�ҧ��ҹMethod .Get ��ҧ 
                                    String upconnectionString = "Server=tcp:acdcproject.database.windows.net,1433;Initial Catalog=ACDC;Persist Security Info=False;User ID=acdc;Password=@Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                                    using (SqlConnection upconnection = new SqlConnection(upconnectionString))//��С��method 㹡�� connect �Ѻ database
                                    {
                                        upconnection.Open();//���¡�� Method connection 㹡�����͵�� database

                                        String upsql = "UPDATE emails SET emailisread=@read WHERE emailid=@emailid";//��С�ȵ�� �����sql ����Ѿവ��������¹�ŧ����Colum emailisread �¼�ҹemailsid�ͧ email���� �Ѿവ
                                        using (SqlCommand upcommand = new SqlCommand(upsql, upconnection))//��С�� method SqlCommand ���ͷ����ѹ Command�ͧSql
                                        {
                                            upcommand.Parameters.AddWithValue("@emailid", id);//��� id ����᷹ @emailid 㹤���觢ͧSql
                                            upcommand.Parameters.AddWithValue("@read", 1);//��Ҥ�� 1 ����᷹ @read 㹤����駢ͧ Sql
                                            upcommand.ExecuteNonQuery();//�繡���ѹCommand �ͧ Sql ������ ���ͷ����Ѿവ

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
        public void OnPost()//��С���繿ѧ���� ���ͷ����������¡��� Onget()
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
