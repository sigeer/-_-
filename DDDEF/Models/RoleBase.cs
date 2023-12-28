﻿namespace DDDEF.Models
{
    public class RoleBase
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public int Status { get; set; }
    }
}
