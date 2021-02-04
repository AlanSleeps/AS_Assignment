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
    public partial class Register : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConString"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string dob = tb_dob.Text;
            if (isPwdStrong(checkPwdStrength(tb_password.Text)) && (tb_password.Text == tb_cfmPwd.Text))
            {
                if (!Regex.IsMatch(tb_cCardNum.Text, "[0-9]{15,16}$"))
                {
                    lbl_cCardNumCheck.Text = "Invalid credit card";
                }
                else if (!Regex.IsMatch(tb_dob.Text, "^([0-9]{4}|[0-9]{2})[./-]([0]?[0-9]|[12][0-9]|[3][01])[./-]([0]?[1-9]|[1][0-2])$"))
                {
                    lbl_dobCheck.Text = "Invalid date";
                }
                else
                {
                    if (true)
                    {
                        string pwd = tb_password.Text.ToString().Trim();

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

                        RijndaelManaged cipher = new RijndaelManaged();
                        cipher.GenerateKey();
                        Key = cipher.Key;
                        IV = cipher.IV;

                        createAccount();
                        Response.Redirect("/Login");
                    }
                }
            }
        }

        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@email,@passwordHash,@passwordSalt,@creditCardNo,@firstName,@lastName,@dateOfBirth,@IV,@Key,CURRENT_TIMESTAMP)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@passwordHash", finalHash);
                            cmd.Parameters.AddWithValue("@passwordSalt", salt);
                            cmd.Parameters.AddWithValue("@creditCardNo", Convert.ToBase64String(encryptData(tb_cCardNum.Text.Trim())));
                            cmd.Parameters.AddWithValue("@firstName", HttpUtility.HtmlEncode(tb_fName.Text.Trim()));
                            cmd.Parameters.AddWithValue("@lastName", HttpUtility.HtmlEncode(tb_lName.Text.Trim()));
                            cmd.Parameters.AddWithValue("@dateOfBirth", tb_dob.Text);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Connection = con;

                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());
                                // Response.Redirect("/CustomError/GenericError.html");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_forEmailCheck.Text = "Email has already been taken";
                lbl_forEmailCheck.ForeColor = Color.Red;
                // throw new Exception(ex.ToString());
            }
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
        }

        private bool isPwdStrong(int score)
        {
            switch (score)
            {
                case 1:
                    weakPwd.Text = "Weak";
                    mediumPwd.Text = "";
                    strongPwd.Text = "";
                    weakPwd.BackColor = Color.Red;
                    mediumPwd.BackColor = Color.White;
                    strongPwd.BackColor = Color.White;
                    break;
                case 2:
                    weakPwd.Text = "";
                    mediumPwd.Text = "Medium";
                    strongPwd.Text = "";
                    weakPwd.BackColor = Color.Blue;
                    mediumPwd.BackColor = Color.Blue;
                    strongPwd.BackColor = Color.White;
                    break;
                case 3:
                    weakPwd.Text = "";
                    mediumPwd.Text = "";
                    strongPwd.Text = "Strong";
                    weakPwd.BackColor = Color.Green;
                    mediumPwd.BackColor = Color.Green;
                    strongPwd.BackColor = Color.Green;
                    break;
                default:
                    break;
            }
            return (score >= 2) ? true : false;
        }

        private int checkPwdStrength(string password)
        {
            if (password.Length < 8) 
            {
                lbl_forPwdStrength.Text = "Password length MUST be at least 8";
                return 1;
            } 
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                lbl_forPwdStrength.Text = "Password MUST have at least 1 character";
                return 1;
            } 
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                lbl_forPwdStrength.Text = "Password MUST have at least 1 uppercase character";
                return 1;
            } 
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                lbl_forPwdStrength.Text = "Password MUST have at least 1 number";
                return 1;
            } 
            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                lbl_forPwdStrength.Text = "Password SHOULD contain at least 1 special character";
                return 2;
            }
            lbl_forPwdStrength.Text = "";
            return 3;
        }

        public bool validateCaptcha()
        {
            bool result = true;

            string captchaRes = Request.Form["g-recaptcha-response"];

            // get secret key stored in environment variable
            string recaptchaSecretKey = Environment.GetEnvironmentVariable("reCaptchaV3secret").ToString();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                ($"https://www.google.com/recaptcha/api/siteverify?secret={recaptchaSecretKey} &response" + captchaRes);

            try
            {
                using (WebResponse wRes = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wRes.GetResponseStream()))
                    {
                        string jsonRes = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        MyObject jsonObj = js.Deserialize<MyObject>(jsonRes);

                        result = Convert.ToBoolean(jsonObj.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
    }
}