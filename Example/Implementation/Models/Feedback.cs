using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Example.Implementation.Models
{
    [TableName("Feedback")]
    [PrimaryKey("ContentId", autoIncrement = false)]
    public class Feedback
    {
        [PrimaryKeyColumn(AutoIncrement = false)]
        public int ContentId { get; set; }

        public string Url { get; set; }

        public int TotalYes { get; set; }
        public int TotalNo { get; set; }
    }
}