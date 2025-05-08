import {UserRole} from "../models/user.ts";

export enum PermissionAccess {
   Read = "Read",
   Full = "Full",
}

export enum ManagementName {
   AnalyticsManagement = "AnalyticsManagement",
   EateriesManagement = "EateriesManagement",
   MetricsManagement = "MetricsManagement",
   BackupsManagement = "BackupsManagement",
   DevicesManagement = "DevicesManagement",
   EmployeesManagement = "EmployeesManagement",
   UsersManagement = "UsersManagement",
}

const rolePermissionsMap: Record<UserRole, { access: PermissionAccess, management: ManagementName }[]> = {
   [UserRole.Admin]: [
      { access: PermissionAccess.Full, management: ManagementName.AnalyticsManagement },
      { access: PermissionAccess.Full, management: ManagementName.EateriesManagement },
      { access: PermissionAccess.Full, management: ManagementName.MetricsManagement },
      { access: PermissionAccess.Full, management: ManagementName.BackupsManagement },
      { access: PermissionAccess.Full, management: ManagementName.DevicesManagement },
      { access: PermissionAccess.Full, management: ManagementName.EmployeesManagement },
      { access: PermissionAccess.Full, management: ManagementName.UsersManagement },
   ],
   [UserRole.FacilityManager]: [
      { access: PermissionAccess.Full, management: ManagementName.EateriesManagement },
      { access: PermissionAccess.Full, management: ManagementName.MetricsManagement },
      { access: PermissionAccess.Full, management: ManagementName.DevicesManagement },
      { access: PermissionAccess.Full, management: ManagementName.EmployeesManagement },
   ],
   [UserRole.Analyst]: [
      { access: PermissionAccess.Read, management: ManagementName.AnalyticsManagement },
   ],
   [UserRole.Staff]: [
      { access: PermissionAccess.Read, management: ManagementName.EmployeesManagement },
      { access: PermissionAccess.Read, management: ManagementName.MetricsManagement },
   ],
   [UserRole.Viewer]: [
      { access: PermissionAccess.Read, management: ManagementName.AnalyticsManagement },
      { access: PermissionAccess.Read, management: ManagementName.EateriesManagement },
      { access: PermissionAccess.Read, management: ManagementName.MetricsManagement },
      { access: PermissionAccess.Read, management: ManagementName.DevicesManagement },
   ]
};

export function hasAccess(
   role: UserRole,
   management: ManagementName,
   accessLevel: PermissionAccess
): boolean {
   const numericRole = UserRole[role as unknown as keyof typeof UserRole];
   const permissions = rolePermissionsMap[numericRole] || [];

   return permissions.some(p =>
      p.management === management &&
      (p.access === PermissionAccess.Full || p.access === accessLevel)
   );
}
