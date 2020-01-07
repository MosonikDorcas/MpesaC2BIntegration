using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace KVB.Controllers
{
    public class ValidationController : ApiController
    {

        // GET api/validation
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/validation/5
        public string Get(int id)
        {
            return "value";
        }
        // POST api/validation
        //[HttpPost]
        //public async Task<HttpResponseMessage> PostRawBufferManual()
        //public async Task<string> PostRawBufferManual()
        public HttpResponseMessage Post(Models.ValidationContract ValidationContract)
        {
            try
            {
                string CallBackString = ValidationContract.TransID + "," +
                    ValidationContract.TransactionType + "," +
                    ValidationContract.TransTime + "," +
                    ValidationContract.TransAmount + "," +
                    ValidationContract.BusinessShortCode + "," +
                    ValidationContract.BillRefNumber + "," +
                    ValidationContract.InvoiceNumber + "," +
                    ValidationContract.OrgAccountBalance + "," +
                    ValidationContract.ThirdPartyTransID + "," +
                    ValidationContract.MSISDN + "," +
                    ValidationContract.FirstName + "," +
                    ValidationContract.MiddleName + "," +
                    ValidationContract.LastName;


                logger(CallBackString);

                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                    RESTAPI.Mpesa_WebService myConnection_Validation = new RESTAPI.Mpesa_WebService();

                    myConnection_Validation.Url = "http://desktop-b5nmsff:7047/DynamicsNAV110/WS/CRONUS%20International%20Ltd./Codeunit/Mpesa_WebService";

                    System.Net.CredentialCache myCredentials = new System.Net.CredentialCache();
                    NetworkCredential netCred = new NetworkCredential("User","soen");
                    myConnection_Validation.PreAuthenticate = true;
                    myCredentials.Add(new Uri(myConnection_Validation.Url), "Negotiate", netCred);
                    myConnection_Validation.Credentials = myCredentials;

                   string LSMessage = myConnection_Validation.SaveTransaction(ValidationContract.TransID,
                        ValidationContract.TransactionType,
                        ValidationContract.TransTime,
                        ValidationContract.TransAmount == "" ? 0 : Convert.ToDecimal(ValidationContract.TransAmount),
                        ValidationContract.BusinessShortCode,
                        ValidationContract.BillRefNumber,
                        ValidationContract.InvoiceNumber,
                        ValidationContract.OrgAccountBalance == "" ? 0 : Convert.ToDecimal(ValidationContract.OrgAccountBalance),
                        ValidationContract.ThirdPartyTransID,
                        ValidationContract.MSISDN, 
                        ValidationContract.FirstName,
                        ValidationContract.MiddleName,
                        ValidationContract.LastName,"Validation");
               

                    if (LSMessage.Equals("TRUE"))
                    {
                        string SuccessResult = "{\"ResultCode\":0,\"ResultDesc\":\"Accepted\"}";
                        HttpResponseMessage response1 = new HttpResponseMessage { Content = new StringContent(SuccessResult, Encoding.UTF8, "application/json") };
                        return response1;
                    }
                    else
                    {
                        string ErrorResult = "{\"ResultCode\":1,\"ResultDesc\":\"Rejected: " + LSMessage + "\"}";
                        HttpResponseMessage response1 = new HttpResponseMessage { Content = new StringContent(ErrorResult, Encoding.UTF8, "application/json") };
                        return response1;
                    }
                }
                catch (Exception ex)
                {
                    logger("Error 1 on Validation" + "=" + ex.Message);
                    string ErrorResult = "{\"ResultCode\":1,\"ResultDesc\":\"Rejected: \"" + "Error on Validation" + "=" + ex.Message + "}";
                    HttpResponseMessage response1 = new HttpResponseMessage { Content = new StringContent(ErrorResult, Encoding.UTF8, "application/json") };
                    return response1;
                }

            }
            catch (Exception ex)
            {
                logger("Error 2 on Validation" + "==" + ex.Message);
                string ErrorResult = "{\"ResultCode\":1,\"ResultDesc\":\"Rejected\"" + "Error on Validation" + " = " + ex.Message + "}";
                HttpResponseMessage response1 = new HttpResponseMessage { Content = new StringContent(ErrorResult, Encoding.UTF8, "application/json") };
                return response1;
            }
        }
        //PUT api/validation/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/validation/5
        public void Delete(int id)
        {
        }
        public void logger(string text)
        {
            try
            {
                string path = "C:\\API\\ValidationFile.txt";
                if (!File.Exists(path))
                    File.Create(path);
                using (StreamWriter streamWriter = new StreamWriter(path, true))
                    streamWriter.WriteLine(DateTime.Now.ToString("MM\\/dd\\/yyyy h\\:mm tt") + ":= " + text);
            }
            catch (Exception ex)
            {



            }
        }

    }
}