using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoiNongDan.Models.Entitys
{
    public class HistoryModel
    {
        [Key]
        public Guid HistoryModifyId { get; set; }
        [MaxLength(100)]
        public string Key { get; set; }
        [MaxLength(100)]
        public string FieldName { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public string TableName { get; set; }
        public Guid ModifiedAccountId { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
