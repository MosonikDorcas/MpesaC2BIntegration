using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Text;

namespace KVB.Controllers
{
    public class ConfirmationController : ApiController
    {

        public struct ConfirmationMessage
        {
            public string MobileNo { get; set; }
            public string MPESACode { get; set; }
            public string MPESASender { get; set; }
            public string MPESAAmount { get; set; }
            //public string ShortCode { get; set; }
        }

        // GET api/confirmation
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/confirmation/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/confirmation
       
        //public async Task<HttpResponseMessage> PostRawBufferManual()
        //public async Task<string> PostRawBufferManual()
        public HttpResponseMessage Post(Models.ConfirmationContract confirmationContract)
        {
            try
            {

                string CallBackString = confirmationContract.TransID + "," +
                        confirmationContract.TransactionType + "," +
                        confirmationContract.TransTime + "," +
                        confirmationContract.TransAmount  + "," +
                        confirmationContract.BusinessShortCode + "," +
                        confirmationContract.BillRefNumber + "," +
                        confirmationContract.InvoiceNumber + "," +
                        confirmationContract.OrgAccountBalance + "," +
                        confirmationContract.ThirdPartyTransID + "," +
                        confirmationContract.MSISDN + "," +
                        confirmationContract.FirstName + "," +
                        confirmationContract.MiddleName + "," +
                        confirmationContract.LastName;

                logger(CallBackString);


                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                    RESTAPI.Mpesa_WebService myConnection_Confirmation = new RESTAPI.Mpesa_WebService();

                    myConnection_Confirmation.Url = "http://desktop-b5nmsff:7047/DynamicsNAV110/WS/CRONUS%20International%20Ltd./Codeunit/Mpesa_WebService";

                    System.Net.CredentialCache myCredentials = new System.Net.CredentialCache();
                    NetworkCredential netCred = new NetworkCredential("User","soen");
                    myConnection_Confirmation.PreAuthenticate = true;
                    myCredentials.Add(new Uri(myConnection_Confirmation.Url), "Negotiate", netCred);
                    myConnection_Confirmation.Credentials = myCredentials;

                  
                   string LSMessage = myConnection_Confirmation.SaveTransaction(confirmationContract.TransID,
                        confirmationContract.TransactionType,
                        confirmationContract.TransTime,
                        confirmationContract.TransAmount == "" ? 0 : Convert.ToDecimal(confirmationContract.TransAmount),
                        confirmationContract.BusinessShortCode,
                        confirmationContract.BillRefNumber,
                        confirmationContract.InvoiceNumber,
                        confirmationContract.OrgAccountBalance == "" ? 0 : Convert.ToDecimal(confirmationContract.OrgAccountBalance),
                        confirmationContract.ThirdPartyTransID,
                        confirmationContract.MSISDN,
                        confirmationContract.FirstName,
                        confirmationContract.MiddleName,
                        confirmationContract.LastName, "Confirmation");
                       
                    if(LSMessage.Equals("TRUE"))
                    {
                        string SuccessResult = "{\r\n  \"C2BPaymentConfirmationResult\": \"Success\"\r\n}";
                        HttpResponseMessage response1 = new HttpResponseMessage { Content = new StringContent(SuccessResult, Encoding.UTF8, "application/json") };
                        return response1;
                    }
                    else
                    {
                        string ErrorResult = "{\r\n  \"C2BPaymentConfirmationResult\": \"Failure: " + LSMessage + "\"\r\n}";
                        HttpResponseMessage response1 = new HttpResponseMessage { Content = new StringContent(ErrorResult, Encoding.UTF8, "application/json") };
                        return response1;
                    }
                }
                catch (Exception ex)
                {
                    logger("Error 1 on Confirmation" + "==" + ex.Message);
                    logger(1 + "----" + ex.Message);
                    logger(2 + "----" + ex.InnerException);
                    string ErrorResult = "{\r\n  \"C2BPaymentConfirmationResult\": \"Failure: " + "Error 1 on Confirmation" + "==" + ex.Message + "\"\r\n}";
                    HttpResponseMessage response1 = new HttpResponseMessage { Content = new StringContent(ErrorResult, Encoding.UTF8, "application/json") };
                    return response1;
                }
            }
            catch (Exception ex)
            {
                logger("Error 2 on Confirmation" + "==" + ex.Message);
                string ErrorResult = "{\r\n  \"C2BPaymentConfirmationResult\": \"Failure: " + "Error 2 on Confirmation" + "==" + ex.Message + "\"\r\n}";
                HttpResponseMessage response1 = new HttpResponseMessage { Content = new StringContent(ErrorResult, Encoding.UTF8, "application/json") };
                return response1;
            }
        }




        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public void logger(string text)
        {
            try
            {
                string path = "C:\\API\\ConfirmationLogFile.txt";
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
