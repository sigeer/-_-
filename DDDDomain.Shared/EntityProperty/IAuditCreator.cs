namespace DDDDomain.Shared.EntityProperty
{
    public interface IAuditCreator
    {
        public int Creator { get; set; }

    }

    public interface IAuditCreateTime
    {
        public DateTime CreationTime { get; set; }
    }

    public interface IAuditCreatorCheck : IAuditCreator
    {
    }
}
