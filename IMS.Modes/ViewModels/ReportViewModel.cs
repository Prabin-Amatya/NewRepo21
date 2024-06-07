using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Modes.ViewModels
{
    public class ReportViewModel { 
        public SearchCriteria search{get; set;}
        public IEnumerable<CustomerReportViewModel> CustomerReportViewModels { get; set;}
        public IEnumerable<CustomerDetailedReportViewModel> CustomerDetailedReportViewModels { get; set; }
    }
}
