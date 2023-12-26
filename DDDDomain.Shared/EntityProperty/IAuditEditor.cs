using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDDomain.Shared.EntityProperty
{
    public interface IAuditEditor
    {
        public int LastModifier { get; set; }
    }

    public interface IAuditEditTime
    {
        public DateTime LastUpdateTime { get; set; }
    }
}
