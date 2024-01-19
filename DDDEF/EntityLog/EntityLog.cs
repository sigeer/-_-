using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DDDEF.EntityLog
{
    public class EntityLog
    {
        protected EntityLog()
        {
        }

        public EntityLog(string tableName, int tablePrimaryId, int type, string? oldValuePairs, string? newValuePairs, int userId)
        {
            TableName = tableName;
            TablePrimaryId = tablePrimaryId;
            Type = type;
            OldValuePairs = oldValuePairs;
            NewValuePairs = newValuePairs;
            UserId = userId;
            CreationTime = DateTime.Now;
        }

        public int Id { get; set; }
        public string TableName { get; private set; } = null!;
        public int TablePrimaryId { get; private set; }
        public int Type { get; private set; }
        public string? OldValuePairs { get; private set; }
        public string? NewValuePairs { get; private set; }
        public int UserId { get; private set; }
        public DateTime CreationTime { get; set; }
    }
}
