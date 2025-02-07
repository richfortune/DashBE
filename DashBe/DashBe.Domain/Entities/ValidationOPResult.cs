using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Domain.Entities
{
    public sealed class ValidationOPResult
    {
        public bool isBalanced { get; }

        public string sMessage { get;  }

        public DateTime dateTime { get; }

        public ValidationOPResult(bool IsBalanced, string Message)
        {
            isBalanced = IsBalanced;
            sMessage = Message ?? throw new ArgumentNullException(nameof(sMessage));
            dateTime = DateTime.Now;    
        }

        public override string ToString() =>
                    $"[ValidationOpResult] Balancedx: {isBalanced}, Message: {sMessage}, Timestamp: {dateTime:O}";
        
    }
}
