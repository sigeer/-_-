using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DDDEF.EntityLog
{
    public class EntityLogContext
    {
        public const int TYPE_UPDATE = 1;
        public const int TYPE_ADD = 2;
        public const int TYPE_DELETE = 3;

        public string TableName { get; set; } = null!;
        public int TablePrimaryId { get; set; }
        public object? OldEntity { get; set; }
        public object? NewEntity { get; set; }
        public int Type { get; set; }

        public EntityEntry Ref { get; set; } = null!;
    }
}
