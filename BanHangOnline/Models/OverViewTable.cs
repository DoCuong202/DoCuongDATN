using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanHangOnline.Models
{
    public class OverViewTable
    {
        public string day { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string count { get; set; }
        public string note { get; set; }

        public OverViewTable() { }

        public OverViewTable(string day, string starttime, string endtime, string code, string name, string phone, string count, string note)
        {
            this.day = day;
            this.starttime = starttime;
            this.endtime = endtime;
            this.code = code;
            this.name = name;
            this.phone = phone;
            this.count = count;
            this.note = note;
        }
    }
}