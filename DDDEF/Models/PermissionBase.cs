namespace DDDEF.Models
{
    public class PermissionBase
    {
        public int Id { get; set; }
        /// <summary>
        /// 仅能使用字母or数字or下划线，不可重复
        /// </summary>
        public string Key { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        /// <summary>
        /// 哪个page的pageload api
        /// </summary>
        public int? PageId { get; set; }
        public int Status { get; set; }
    }
}
