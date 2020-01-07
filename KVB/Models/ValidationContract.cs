using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace KVB.Models
{
    [DataContract]
    public class ValidationContract
    {
        [DataMember]
        public string TransactionType { get; set; }
        [DataMember]
        public string TransID { get; set; }
        [DataMember]
        public string TransTime { get; set; }
        [DataMember]
        public string TransAmount { get; set; }
        [DataMember]
        public string BusinessShortCode { get; set; }
        [DataMember]
        public string BillRefNumber { get; set; }
        [DataMember]
        public string InvoiceNumber { get; set; }
        [DataMember]
        public string OrgAccountBalance { get; set; }
        [DataMember]
        public string ThirdPartyTransID { get; set; }
        [DataMember]
        public string MSISDN { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string LastName { get; set; }

    }
}