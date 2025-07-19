using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ioc.Core.DbModel.Models
{
    public class RandomGenHelp :  PublicBaseEntity
    {
        public int Type { get; set; }
        public string RandomValue { get; set; } = "";
    }

    public enum RandomType {
        Name,
        Username,
        CompanyName,
        ProductName,
        InVoiceNo,
        ContactNo,
        City,
        Street,
        FullAddress,
        RandomString,
        RandomNumber,
        RandomParagraph,
        Password
    }
}
