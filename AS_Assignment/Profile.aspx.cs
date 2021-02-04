using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;


namespace AS_Assignment
{
    public partial class Profile : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConString"].ConnectionString;
        static string finalHash;
        static string salt;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lbl_profileName.Text = "Welcome " + Session["profile_name"].ToString();

                if (timeDifference() > 15)
                {
                    lbl_statusInfo.Text = "You are required to change your password";
                }
            }
            catch
            {
                Response.Redirect("Login");
            }
        }

        protected void btn_changePwd_Click(object sender, EventArgs e)
        {
            if (isPasswordMatch(tb_oldPwd.Text) == true)
            {
                if (tb_oldPwd.Text != tb_newPwd.Text)
                {
                    if (tb_newPwd.Text == tb_cfmNewPwd.Text)
                    {
                        if (checkPwdStrength(tb_newPwd.Text) >= 2)
                        {
                            try
                            {
                                string pwd = tb_newPwd.Text.ToString().Trim();

                                //Generate random "salt" 
                                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                                byte[] saltByte = new byte[8];

                                //Fills array of bytes with a cryptographically strong sequence of random values.
                                rng.GetBytes(saltByte);
                                salt = Convert.ToBase64String(saltByte);

                                SHA512Managed hashing = new SHA512Managed();

                                string pwdWithSalt = pwd + salt;
                                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                                finalHash = Convert.ToBase64String(hashWithSalt);

                                if (timeDifference() > 5)
                                {
                                    updatePassword();
                                } 
                                else
                                {
                                    lbl_statusInfo.Text = "You can only change password after 5 mins";
                                }

                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());
                            }
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    lbl_statusInfo.Text = "New password cannot be same as old!";
                }
                
            }
            else
            {
                lbl_statusInfo.Text = "Incorrect old password";
            }
        }

        protected void updatePassword()
        {
            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"UPDATE Account SET " +
                    $"passwordHash = @passwordHash, passwordSalt = @passwordSalt, lastChangeAt = CURRENT_TIMESTAMP WHERE email = @userid"))
                {

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@passwordHash", finalHash);
                        cmd.Parameters.AddWithValue("@passwordSalt", salt);
                        cmd.Parameters.AddWithValue("@userid", Session["user"].ToString());

                        cmd.Connection = con;

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Response.Redirect("/CustomError/GenericError.html");
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }

        protected double timeDifference()
        {
            // get timestamp
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT * FROM Account WHERE email=@userId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userId", Session["user"].ToString());

            double timeDiff = 0;
            try
            {
                DateTime lastChange;
                DateTime today = DateTime.Now;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["lastChangeAt"] != DBNull.Value)
                        {
                            lastChange = DateTime.Parse(reader["lastChangeAt"].ToString());
                            timeDiff = (today - lastChange).TotalMinutes;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

            return timeDiff;
        }

        protected bool isPasswordMatch(string pwd)
        {
            string userid = Session["user"].ToString();

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash.Equals(dbHash))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    lbl_statusInfo.Text = "Incorrect old password";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return false;
        }

        protected string getDBSalt(string userid)
        {
            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select passwordSalt FROM ACCOUNT WHERE email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", Session["user"].ToString());

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["passwordSalt"] != null)
                        {
                            if (reader["passwordSalt"] != DBNull.Value)
                            {
                                s = reader["passwordSalt"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string getDBHash(string userid)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select passwordHash FROM Account WHERE email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", Session["user"].ToString());

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["passwordHash"] != null)
                        {
                            if (reader["passwordHash"] != DBNull.Value)
                            {
                                h = reader["passwordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }

        private int checkPwdStrength(string password)
        {
            if (password.Length < 8)
            {
                lbl_statusInfo.Text = "Password length MUST be at least 8";
                return 1;
            }
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                lbl_statusInfo.Text = "Password MUST have at least 1 character";
                return 1;
            }
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                lbl_statusInfo.Text = "Password MUST have at least 1 uppercase character";
                return 1;
            }
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                lbl_statusInfo.Text = "Password MUST have at least 1 number";
                return 1;
            }
            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                lbl_statusInfo.Text = "Password SHOULD contain at least 1 special character";
                return 2;
            }
            lbl_statusInfo.Text = "";
            return 3;
        }
    }
}