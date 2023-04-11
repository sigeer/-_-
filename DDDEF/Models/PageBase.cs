namespace DDDEF.Models
{
    public class PageBase
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Icon { get; set; }
        public int? ParentId { get; set; }
        /// <summary>
        /// 与前台url关联标记
        /// </summary>
        public string Key { get; set; } = null!;

        public bool? HiddenInNav { get; set; }
        /// <summary>
        /// 无权限限制
        /// </summary>
        public bool? IsPublic { get; set; }
        public string? PermissionKey { get; set; }
    }
}
