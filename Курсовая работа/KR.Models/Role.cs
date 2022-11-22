using KR.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Kr.Models
{
    public class Role
    {
        [Key]
        public Guid Id_Role { get; set; } = new Guid();
        [Required]
        [StringLength(30)]
        [MinLength(3)]
        public string Name { get; set; }
        public virtual Account AccountUser { get; set; }

        enum NameRole
        {
            [Description("Администратор")]
            ADMIN,
            [Description("Бухгалтер")]
            ACCOUNTANT,
            [Description("Отдел кадров")]
            CHAR,
            [Description("Отдел продаж")]
            SALESDEPARTMENT,
            [Description("Отдел закупок")]
            PURCHASESDEPARTMENT,
            [Description("Работник склада")]
            STORAGE,
        }

        public static List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();
            var enumType = typeof(NameRole);
            foreach (var name in Enum.GetNames(enumType))
            {
                var memberInfos = enumType.GetMember(name);
                var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
                var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = ((DescriptionAttribute)valueAttributes[0]).Description;
                Role role = new Role();
                role.Name = description;
                roles.Add(role);
            }
            return roles;

        }
    }
}
