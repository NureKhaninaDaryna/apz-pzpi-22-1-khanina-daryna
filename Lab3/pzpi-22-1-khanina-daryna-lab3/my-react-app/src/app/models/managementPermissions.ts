import {UserRole} from "./user.ts";

export enum ManagementName {
   AnalyticsManagement = 'AnalyticsManagement',
   EateriesManagement = 'EateriesManagement',
   MetricsManagement = 'MetricsManagement',
   BackupsManagement = 'BackupsManagement',
   DevicesManagement = 'DevicesManagement',
   EmployeesManagement = 'EmployeesManagement',
   UsersManagement = 'UsersManagement',
}

export enum PermissionAccess {
   Full = 'Full',
   Read = 'Read'
}

export const RolePermissions = {
   [UserRole.Admin]: {
      [ManagementName.AnalyticsManagement]: PermissionAccess.Full,
      [ManagementName.EateriesManagement]: PermissionAccess.Full,
      [ManagementName.MetricsManagement]: PermissionAccess.Full,
      [ManagementName.BackupsManagement]: PermissionAccess.Full,
      [ManagementName.DevicesManagement]: PermissionAccess.Full,
      [ManagementName.EmployeesManagement]: PermissionAccess.Full,
      [ManagementName.UsersManagement]: PermissionAccess.Full,
   },
   [UserRole.FacilityManager]: {
      [ManagementName.EateriesManagement]: PermissionAccess.Full,
      [ManagementName.MetricsManagement]: PermissionAccess.Full,
      [ManagementName.DevicesManagement]: PermissionAccess.Full,
      [ManagementName.EmployeesManagement]: PermissionAccess.Full,
   },
   [UserRole.Staff]: {
      [ManagementName.EmployeesManagement]: PermissionAccess.Read,
      [ManagementName.MetricsManagement]: PermissionAccess.Read,
   },
   [UserRole.Analyst]: {
      [ManagementName.AnalyticsManagement]: PermissionAccess.Read,
   },
   [UserRole.Viewer]: {
      [ManagementName.AnalyticsManagement]: PermissionAccess.Read,
      [ManagementName.EateriesManagement]: PermissionAccess.Read,
      [ManagementName.MetricsManagement]: PermissionAccess.Read,
      [ManagementName.DevicesManagement]: PermissionAccess.Read,
   }
};
