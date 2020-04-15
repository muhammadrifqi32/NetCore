using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetCore.ViewModels
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class DepartmentJson
    {
        [JsonProperty("data")]
        public IList<DepartmentVM> data { get; set; }
    }
}
