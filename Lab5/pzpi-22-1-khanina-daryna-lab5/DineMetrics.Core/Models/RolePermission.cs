using DineMetrics.Core.Enums;

namespace DineMetrics.Core.Models;

public class RolePermission(PermissionAccess access, UserRole role, ManagementName management)
{
    public PermissionAccess Access { get; set; } = access;
    public UserRole Role { get; set; } = role;
    public ManagementName Management { get; set; } = management;
}
