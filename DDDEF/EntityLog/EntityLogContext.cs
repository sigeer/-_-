using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel;

namespace DDDEF.EntityLog
{
    public class EntityLogContext
    {
        public string TableName { get; set; } = null!;
        public int TablePrimaryId { get; set; }
        public object? OldEntity { get; set; }
        public object? NewEntity { get; set; }
        public EntityLogType Type { get; set; }

        public EntityEntry Ref { get; set; } = null!;
    }

    public enum EntityLogType
    {
        [Description("none")]
        None = 0,
        [Description("update")]
        Update = 1,
        [Description("add")]
        Add = 2,
        [Description("delete")]
        Delete = 3
    }
}
