using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.EnterpriseTask
{
    public class Enterprise
    {
        public Guid Guid { get; }

        public Enterprise(Guid guid)
        {
            Guid = guid;
        }

        public string Name { get; set; }

        private string inn;
        public string Inn
        {
            get => inn;
            set
            {
                if (inn.Length != 10 || !inn.All(char.IsDigit))
                    throw new ArgumentException(); 
                inn = value;
            }
        }

        public DateTime EstablishDate { get; set; }

        public TimeSpan ActiveTimeSpan
        {
            get => DateTime.Now - EstablishDate;
        }

        public double GetTotalTransactionsAmount()
        {
            DataBase.OpenConnection();

            return DataBase.Transactions()
                .Where(z => z.EnterpriseGuid == Guid)
                .Sum(t => t.Amount);
        }
    }
}
