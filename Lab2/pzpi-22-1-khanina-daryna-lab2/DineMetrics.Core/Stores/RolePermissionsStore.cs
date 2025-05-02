using DineMetrics.Core.Enums;
using DineMetrics.Core.Models;

namespace DineMetrics.Core.Stores;

public static class RolePermissionsStore
{
    public static readonly List<RolePermission> AllPermissions = new()
    {
        // Admin — повний доступ до всього
        new(PermissionAccess.Full, UserRole.Admin, ManagementName.AnalyticsManagement),
        new(PermissionAccess.Full, UserRole.Admin, ManagementName.EateriesManagement),
        new(PermissionAccess.Full, UserRole.Admin, ManagementName.MetricsManagement),
        new(PermissionAccess.Full, UserRole.Admin, ManagementName.BackupsManagement),
        new(PermissionAccess.Full, UserRole.Admin, ManagementName.DevicesManagement),
        new(PermissionAccess.Full, UserRole.Admin, ManagementName.EmployeesManagement),
        new(PermissionAccess.Full, UserRole.Admin, ManagementName.UsersManagement),

        // FacilityManager — доступ до їдалень, метрік та пристроїв
        new(PermissionAccess.Full, UserRole.FacilityManager, ManagementName.EateriesManagement),
        new(PermissionAccess.Full, UserRole.FacilityManager, ManagementName.MetricsManagement),
        new(PermissionAccess.Full, UserRole.FacilityManager, ManagementName.DevicesManagement),
        new(PermissionAccess.Full, UserRole.FacilityManager, ManagementName.EmployeesManagement),

        // Staff — лише читання
        new(PermissionAccess.Read, UserRole.Staff, ManagementName.EmployeesManagement),
        new(PermissionAccess.Read, UserRole.Staff, ManagementName.MetricsManagement),

        // Analyst — доступ до аналітики
        new(PermissionAccess.Read, UserRole.Analyst, ManagementName.AnalyticsManagement),
        new(PermissionAccess.Read, UserRole.Analyst, ManagementName.EateriesManagement),

        // Viewer — тільки читання до всього
        new(PermissionAccess.Read, UserRole.Viewer, ManagementName.AnalyticsManagement),
        new(PermissionAccess.Read, UserRole.Viewer, ManagementName.EateriesManagement),
        new(PermissionAccess.Read, UserRole.Viewer, ManagementName.MetricsManagement),
        new(PermissionAccess.Read, UserRole.Viewer, ManagementName.DevicesManagement),
    };
}