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
        /// ��ǰ̨url�������
        /// </summary>
        public string Key { get; set; } = null!;

        public bool? HiddenInNav { get; set; }
        /// <summary>
        /// ��Ȩ������
        /// </summary>
        public bool? IsPublic { get; set; }
        public string? PermissionKey { get; set; }
    }
}
