using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace AzureFunction.HotChocolate.Services
{
    using Enums;
    
    public class RequestService : QueryBuilder
    {
        private static string Endpoint => 
            Environment.GetEnvironmentVariable("BeaGovEndpoint");

        private static string BeaGovKey =>
            Environment.GetEnvironmentVariable("BeaGovKey");


        public RequestService()
        {
            base.Add("UserID", BeaGovKey);
            base.Add("method", "GetData");
        }

        public Uri Request
        {
            get
            {
                return new UriBuilder(Endpoint)
                {
                    Query = base.ToString()
                }.Uri;
            }
        }


        public void AddDataSet(DataSet dataset)
        {
            base.Add("DataSetName", dataset.ToString());
        }


        public void AddTableName(TableName tableName)
        {
            base.Add("TableName", tableName.ToString());
        }


        public void AddFrequency(Frequency frequency)
        {
            switch (frequency)
            {
                case Frequency.Yearly:
                    base.Add("Frequency", "A");
                    break;

                case Frequency.Quarterly:
                    base.Add("Frequency", "Q");
                    break;

                case Frequency.Monthly:
                    base.Add("Frequency", "M");
                    break;
            }
        }


        public void AddTimePeriodRange(int From, int To)
        {
            if (From > To)
                throw new Exception("To input mast be less then than the From input");

            string range = string.Empty;

            for(int i = From; i <= To; i++)
            {
                if (i == From)
                    range = From.ToString();
                else
                    range = string.Join(',', range, i.ToString());
            }

            base.Add("Year", range);
        }


    }
}
